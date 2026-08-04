USE FrontOne;
GO

-- Módulo Corridas: registra que un Lote entró a proceso de producción (Inicio/Fin) antes de que,
-- en un módulo futuro, se generen Pallets contra él. Un Lote solo entra a UNA Corrida (LoteId
-- UNIQUE) — mientras esa fila exista no puede volver a capturarse. No lleva folio propio, se
-- identifica por el Folio del Lote.
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Produccion')
BEGIN
    EXEC('CREATE SCHEMA Produccion');
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Produccion.Corrida'))
BEGIN
    CREATE TABLE Produccion.Corrida (
        Id              INT IDENTITY PRIMARY KEY,
        LoteId          INT NOT NULL UNIQUE REFERENCES Lotes.Lote(Id),
        FechaHoraInicio DATETIME2 NOT NULL,
        FechaHoraFin    DATETIME2 NULL,
        Estatus         TINYINT NOT NULL DEFAULT 1, -- 1 = En Proceso, 2 = Procesado
        KilosAProcesar  DECIMAL(10,2) NOT NULL DEFAULT 0, -- snapshot de Lote.Kilogramos al Iniciar
        KilosProcesados DECIMAL(10,2) NOT NULL DEFAULT 0, -- se va acumulando con los Pallets (módulo futuro)
        FechaCreacion   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

-- sp_Corrida_Obtener resuelve Huerta/Registro Sagarpa/Productor/Beneficiario del Lote con el mismo
-- criterio que ya usa Lotes.sp_Lote_Obtener: se toman de la Orden de Corte de la PRIMERA Recepción
-- del Lote (son iguales en todas las líneas por la regla de validación de LoteService.AgregarLineaAsync).
CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id, c.LoteId, l.Folio AS LoteFolio, ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad,
        l.Kilogramos,
        primera.HuertaNombre, primera.RegistroSagarpa, primera.ProductorNombre, primera.Beneficiario,
        c.FechaHoraInicio, c.FechaHoraFin, c.Estatus, c.FechaCreacion
    FROM Produccion.Corrida c
    INNER JOIN Lotes.Lote l ON l.Id = c.LoteId
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, h.RegistroSagarpa, pr.NombreProductor AS ProductorNombre,
            oc.PagarCorteANombre AS Beneficiario
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    WHERE (@Id IS NULL OR c.Id = @Id)
    ORDER BY c.FechaCreacion DESC;
END
GO

-- sp_Corrida_ObtenerLotesDisponibles: mismo shape de columnas que sp_Corrida_Obtener (Huerta/
-- Registro Sagarpa/Productor/Beneficiario) para que el picker de "Nuevo" pueda autocompletar esos
-- campos sin otra llamada al servidor. Excluye los Lotes que ya tienen una Corrida.
CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_ObtenerLotesDisponibles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Id AS LoteId, l.Folio AS LoteFolio, ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad,
        l.Fecha, l.Kilogramos,
        primera.HuertaNombre, primera.RegistroSagarpa, primera.ProductorNombre, primera.Beneficiario
    FROM Lotes.Lote l
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, h.RegistroSagarpa, pr.NombreProductor AS ProductorNombre,
            oc.PagarCorteANombre AS Beneficiario
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    WHERE NOT EXISTS (SELECT 1 FROM Produccion.Corrida c WHERE c.LoteId = l.Id)
    ORDER BY l.FechaCreacion DESC;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Insertar
    @LoteId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Corrida WHERE LoteId = @LoteId)
    BEGIN
        THROW 50000, 'Este lote ya fue capturado en una corrida.', 1;
    END

    INSERT INTO Produccion.Corrida (LoteId, FechaHoraInicio, Estatus)
    VALUES (@LoteId, SYSDATETIME(), 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Finalizar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Produccion.Corrida WHERE Id = @Id AND Estatus = 1)
    BEGIN
        THROW 50000, 'Esta corrida ya fue finalizada.', 1;
    END

    UPDATE Produccion.Corrida
    SET FechaHoraFin = SYSDATETIME(), Estatus = 2
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Produccion.Corrida WHERE Id = @Id;
END
GO
