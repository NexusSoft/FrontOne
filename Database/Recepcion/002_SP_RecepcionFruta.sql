USE FrontOne;
GO

-- sp_RecepcionFruta_Obtener trae el encabezado y, vía STRING_AGG sobre el detalle, una columna
-- "Huertas" con los nombres de huerta de todas sus Órdenes de Corte separados por coma — se usa
-- en la columna "Huerta" del listado (una Recepción puede traer fruta de varias huertas).
-- EstaEnLote (EXISTS contra Lotes.LoteRecepcion) alimenta la columna de candado del listado y el
-- bloqueo de edición en RecepcionFrutaEditarForm — mismo criterio de "una Recepción se puede
-- reconsultar aquí" que ya usa el propio módulo Lotes (Lotes.sp_RecepcionFruta_EstaEnLote), solo
-- que aquí es por-fila para todo el listado en una sola pasada en vez de un SP separado por Id.
CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFruta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        rf.Id, rf.Folio, rf.NoLote, rf.Fecha, rf.Chofer, rf.Placas, rf.Observaciones,
        rf.NumeroTicket, rf.CoprefBico,
        rf.PesoBruto, rf.PesoTara, rf.TaraCajas, rf.PesoMuestra, rf.PesoNeto, rf.PesoProductor,
        rf.PorcentajeMateriaSeca,
        rf.CajasEntregadas, rf.CajasCortadas, rf.CajasRecibidasVacias, rf.CajasDiferencia,
        rf.CamionDestarado, rf.FechaCreacion,
        (
            SELECT STRING_AGG(h.Nombre, ', ')
            FROM Recepcion.RecepcionFrutaOrdenCorte det
            INNER JOIN Acopio.OrdenCorte oc ON oc.Id = det.OrdenCorteId
            INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
            WHERE det.RecepcionFrutaId = rf.Id
        ) AS Huertas,
        (
            SELECT TOP 1 oc.Folio
            FROM Recepcion.RecepcionFrutaOrdenCorte det
            INNER JOIN Acopio.OrdenCorte oc ON oc.Id = det.OrdenCorteId
            WHERE det.RecepcionFrutaId = rf.Id
        ) AS OrdenCorteFolio,
        (
            SELECT TOP 1 ac.Folio
            FROM Recepcion.RecepcionFrutaOrdenCorte det
            INNER JOIN Acopio.OrdenCorte oc ON oc.Id = det.OrdenCorteId
            INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
            WHERE det.RecepcionFrutaId = rf.Id
        ) AS AcuerdoCorteFolio,
        CAST(CASE WHEN EXISTS (
            SELECT 1 FROM Lotes.LoteRecepcion lr WHERE lr.RecepcionFrutaId = rf.Id
        ) THEN 1 ELSE 0 END AS BIT) AS EstaEnLote
    FROM Recepcion.RecepcionFruta rf
    WHERE (@Id IS NULL OR rf.Id = @Id)
    ORDER BY rf.FechaCreacion DESC;
END
GO

CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFruta_Insertar
    @NoLote                 NVARCHAR(20) = NULL,
    @Fecha                  DATE,
    @Chofer                 NVARCHAR(200),
    @Placas                 NVARCHAR(20) = NULL,
    @Observaciones          NVARCHAR(500) = NULL,
    @NumeroTicket           NVARCHAR(50) = NULL,
    @CoprefBico             NVARCHAR(50) = NULL,
    @PesoBruto              DECIMAL(18,2),
    @PesoTara               DECIMAL(18,2),
    @TaraCajas              DECIMAL(18,2),
    @PesoMuestra            DECIMAL(18,2),
    @PesoNeto               DECIMAL(18,2),
    @PesoProductor          DECIMAL(18,2),
    @PorcentajeMateriaSeca  DECIMAL(5,2),
    @CajasEntregadas        SMALLINT,
    @CajasCortadas          SMALLINT,
    @CajasRecibidasVacias   SMALLINT,
    @CajasDiferencia        SMALLINT,
    @CamionDestarado        BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Folio NVARCHAR(7) = RIGHT('0000000' + CAST(NEXT VALUE FOR Recepcion.SeqRecepcionFrutaFolio AS VARCHAR(7)), 7);

    INSERT INTO Recepcion.RecepcionFruta
        (Folio, NoLote, Fecha, Chofer, Placas, Observaciones, NumeroTicket, CoprefBico,
         PesoBruto, PesoTara, TaraCajas, PesoMuestra, PesoNeto, PesoProductor, PorcentajeMateriaSeca,
         CajasEntregadas, CajasCortadas, CajasRecibidasVacias, CajasDiferencia, CamionDestarado)
    VALUES
        (@Folio, @NoLote, @Fecha, @Chofer, @Placas, @Observaciones, @NumeroTicket, @CoprefBico,
         @PesoBruto, @PesoTara, @TaraCajas, @PesoMuestra, @PesoNeto, @PesoProductor, @PorcentajeMateriaSeca,
         @CajasEntregadas, @CajasCortadas, @CajasRecibidasVacias, @CajasDiferencia, @CamionDestarado);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO

CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFruta_Actualizar
    @Id                     INT,
    @NoLote                 NVARCHAR(20) = NULL,
    @Fecha                  DATE,
    @Chofer                 NVARCHAR(200),
    @Placas                 NVARCHAR(20) = NULL,
    @Observaciones          NVARCHAR(500) = NULL,
    @NumeroTicket           NVARCHAR(50) = NULL,
    @CoprefBico             NVARCHAR(50) = NULL,
    @PesoBruto              DECIMAL(18,2),
    @PesoTara               DECIMAL(18,2),
    @TaraCajas              DECIMAL(18,2),
    @PesoMuestra            DECIMAL(18,2),
    @PesoNeto               DECIMAL(18,2),
    @PesoProductor          DECIMAL(18,2),
    @PorcentajeMateriaSeca  DECIMAL(5,2),
    @CajasEntregadas        SMALLINT,
    @CajasCortadas          SMALLINT,
    @CajasRecibidasVacias   SMALLINT,
    @CajasDiferencia        SMALLINT,
    @CamionDestarado        BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Recepcion.RecepcionFruta
    SET NoLote = @NoLote,
        Fecha = @Fecha,
        Chofer = @Chofer,
        Placas = @Placas,
        Observaciones = @Observaciones,
        NumeroTicket = @NumeroTicket,
        CoprefBico = @CoprefBico,
        PesoBruto = @PesoBruto,
        PesoTara = @PesoTara,
        TaraCajas = @TaraCajas,
        PesoMuestra = @PesoMuestra,
        PesoNeto = @PesoNeto,
        PesoProductor = @PesoProductor,
        PorcentajeMateriaSeca = @PorcentajeMateriaSeca,
        CajasEntregadas = @CajasEntregadas,
        CajasCortadas = @CajasCortadas,
        CajasRecibidasVacias = @CajasRecibidasVacias,
        CajasDiferencia = @CajasDiferencia,
        CamionDestarado = @CamionDestarado
    WHERE Id = @Id;
END
GO

-- Borra primero el detalle y luego el encabezado dentro del mismo SP — las FK del proyecto
-- nunca llevan CASCADE (regla dura), así que el control de "borrar hijos antes que el padre"
-- se hace explícito aquí en vez de delegarlo al motor.
CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFruta_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Recepcion.RecepcionFrutaOrdenCorte WHERE RecepcionFrutaId = @Id;
    DELETE FROM Recepcion.RecepcionFruta WHERE Id = @Id;
END
GO
