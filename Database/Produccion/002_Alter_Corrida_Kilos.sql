USE FrontOne;
GO

-- Kilos a Procesar (snapshot de Lote.Kilogramos al Iniciar, informativo) y Kilos Procesados
-- (arranca en 0, se irá acumulando con los Pallets del módulo futuro — nada lo actualiza todavía).
-- Además: una Corrida ya Procesada, o con Kilos Procesados > 0, ya no se puede eliminar — implica
-- que ya se generó producción real contra ella.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.Corrida') AND name = 'KilosAProcesar')
BEGIN
    ALTER TABLE Produccion.Corrida ADD KilosAProcesar DECIMAL(10,2) NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.Corrida') AND name = 'KilosProcesados')
BEGIN
    ALTER TABLE Produccion.Corrida ADD KilosProcesados DECIMAL(10,2) NOT NULL DEFAULT 0;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id, c.LoteId, l.Folio AS LoteFolio, ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad,
        l.Kilogramos,
        primera.HuertaNombre, primera.RegistroSagarpa, primera.ProductorNombre, primera.Beneficiario,
        c.FechaHoraInicio, c.FechaHoraFin, c.Estatus, c.KilosAProcesar, c.KilosProcesados, c.FechaCreacion
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

-- Kilos a Procesar se fija al Iniciar como snapshot de Lote.Kilogramos (no se vuelve a tocar).
CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Insertar
    @LoteId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Corrida WHERE LoteId = @LoteId)
    BEGIN
        THROW 50000, 'Este lote ya fue capturado en una corrida.', 1;
    END

    INSERT INTO Produccion.Corrida (LoteId, FechaHoraInicio, Estatus, KilosAProcesar)
    SELECT @LoteId, SYSDATETIME(), 1, l.Kilogramos
    FROM Lotes.Lote l
    WHERE l.Id = @LoteId;

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- No se puede eliminar una Corrida ya Procesada, ni una con Kilos Procesados > 0 (ya hay
-- producción real registrada contra ella vía Pallets, aunque siga "En Proceso").
CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Corrida WHERE Id = @Id AND (Estatus = 2 OR KilosProcesados > 0))
    BEGIN
        THROW 50000, 'No se puede eliminar esta corrida: ya está procesada o ya tiene kilos procesados registrados.', 1;
    END

    DELETE FROM Produccion.Corrida WHERE Id = @Id;
END
GO
