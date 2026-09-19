USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFruta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        rf.Id, rf.Folio, rf.NoLote, rf.Fecha, rf.Chofer, rf.Placas, rf.Observaciones,
        rf.NumeroTicket, rf.CoprefBico,
        rf.PesoBruto, rf.PesoTara, rf.TaraCajas, rf.PesoMuestra, rf.PesoNeto, rf.PesoProductor, rf.NombreBascula,
        rf.PorcentajeMateriaSeca,
        rf.CajasPorEntregar, rf.CajasEntregadas, rf.CajasCortadas, rf.CajasRecibidasVacias,
        rf.CajasDiferencia, rf.CajasPerdidas,
        rf.DescargaCompleta, rf.TicketPesadaArchivo, rf.TicketPesadaNombreArchivo, rf.FechaCreacion,
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
    @NombreBascula          NVARCHAR(100) = NULL,
    @PorcentajeMateriaSeca  DECIMAL(5,2),
    @CajasPorEntregar       SMALLINT = 0,
    @CajasEntregadas        SMALLINT,
    @CajasCortadas          SMALLINT,
    @CajasRecibidasVacias   SMALLINT,
    @CajasDiferencia        SMALLINT,
    @CajasPerdidas          SMALLINT = 0,
    @DescargaCompleta       BIT = 0,
    @TicketPesadaArchivo        VARBINARY(MAX) = NULL,
    @TicketPesadaNombreArchivo  NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Folio NVARCHAR(7);

    BEGIN TRANSACTION;

    SELECT @Folio = RIGHT('0000000' + CAST(ISNULL(MAX(TRY_CAST(Folio AS INT)), 0) + 1 AS VARCHAR(7)), 7)
    FROM Recepcion.RecepcionFruta WITH (UPDLOCK, HOLDLOCK);

    INSERT INTO Recepcion.RecepcionFruta
        (Folio, NoLote, Fecha, Chofer, Placas, Observaciones, NumeroTicket, CoprefBico,
         PesoBruto, PesoTara, TaraCajas, PesoMuestra, PesoNeto, PesoProductor, NombreBascula, PorcentajeMateriaSeca,
         CajasPorEntregar, CajasEntregadas, CajasCortadas, CajasRecibidasVacias, CajasDiferencia, CajasPerdidas,
         DescargaCompleta, TicketPesadaArchivo, TicketPesadaNombreArchivo)
    VALUES
        (@Folio, @NoLote, @Fecha, @Chofer, @Placas, @Observaciones, @NumeroTicket, @CoprefBico,
         @PesoBruto, @PesoTara, @TaraCajas, @PesoMuestra, @PesoNeto, @PesoProductor, @NombreBascula, @PorcentajeMateriaSeca,
         @CajasPorEntregar, @CajasEntregadas, @CajasCortadas, @CajasRecibidasVacias, @CajasDiferencia, @CajasPerdidas,
         @DescargaCompleta, @TicketPesadaArchivo, @TicketPesadaNombreArchivo);

    COMMIT TRANSACTION;

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
    @NombreBascula          NVARCHAR(100) = NULL,
    @PorcentajeMateriaSeca  DECIMAL(5,2),
    @CajasPorEntregar       SMALLINT = 0,
    @CajasEntregadas        SMALLINT,
    @CajasCortadas          SMALLINT,
    @CajasRecibidasVacias   SMALLINT,
    @CajasDiferencia        SMALLINT,
    @CajasPerdidas          SMALLINT = 0,
    @DescargaCompleta       BIT = 0,
    @TicketPesadaArchivo        VARBINARY(MAX) = NULL,
    @TicketPesadaNombreArchivo  NVARCHAR(255) = NULL
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
        NombreBascula = @NombreBascula,
        PorcentajeMateriaSeca = @PorcentajeMateriaSeca,
        CajasPorEntregar = @CajasPorEntregar,
        CajasEntregadas = @CajasEntregadas,
        CajasCortadas = @CajasCortadas,
        CajasRecibidasVacias = @CajasRecibidasVacias,
        CajasDiferencia = @CajasDiferencia,
        CajasPerdidas = @CajasPerdidas,
        DescargaCompleta = @DescargaCompleta,
        TicketPesadaArchivo = @TicketPesadaArchivo,
        TicketPesadaNombreArchivo = @TicketPesadaNombreArchivo
    WHERE Id = @Id;
END
GO
