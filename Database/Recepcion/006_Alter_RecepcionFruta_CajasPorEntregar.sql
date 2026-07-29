USE FrontOne;
GO

-- "Por Entregar" separa lo que la Orden de Corte indicaba que debía traer el camión (automático,
-- solo lectura) de "Entregadas" (ahora captura manual: lo que realmente llegó). La Diferencia
-- pasa a comparar ambos: Por Entregar - (Entregadas - Cortadas - Recibidas Vacías). Mismo patrón
-- de ALTER + regenerar SPs que 005_Alter_RecepcionFruta_TicketPesada.sql.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Recepcion.RecepcionFruta') AND name = 'CajasPorEntregar')
BEGIN
    ALTER TABLE Recepcion.RecepcionFruta ADD
        CajasPorEntregar SMALLINT NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_CajasPorEntregar DEFAULT (0);
END
GO

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
        rf.CajasPorEntregar, rf.CajasEntregadas, rf.CajasCortadas, rf.CajasRecibidasVacias, rf.CajasDiferencia,
        rf.CamionDestarado, rf.TicketPesadaArchivo, rf.TicketPesadaNombreArchivo, rf.FechaCreacion,
        (
            SELECT STRING_AGG(h.Nombre, ', ')
            FROM Recepcion.RecepcionFrutaOrdenCorte det
            INNER JOIN Acopio.OrdenCorte oc ON oc.Id = det.OrdenCorteId
            INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
            WHERE det.RecepcionFrutaId = rf.Id
        ) AS Huertas
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
    @CajasPorEntregar       SMALLINT = 0,
    @CajasEntregadas        SMALLINT,
    @CajasCortadas          SMALLINT,
    @CajasRecibidasVacias   SMALLINT,
    @CajasDiferencia        SMALLINT,
    @CamionDestarado        BIT = 0,
    @TicketPesadaArchivo        VARBINARY(MAX) = NULL,
    @TicketPesadaNombreArchivo  NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Folio NVARCHAR(7) = RIGHT('0000000' + CAST(NEXT VALUE FOR Recepcion.SeqRecepcionFrutaFolio AS VARCHAR(7)), 7);

    INSERT INTO Recepcion.RecepcionFruta
        (Folio, NoLote, Fecha, Chofer, Placas, Observaciones, NumeroTicket, CoprefBico,
         PesoBruto, PesoTara, TaraCajas, PesoMuestra, PesoNeto, PesoProductor, PorcentajeMateriaSeca,
         CajasPorEntregar, CajasEntregadas, CajasCortadas, CajasRecibidasVacias, CajasDiferencia, CamionDestarado,
         TicketPesadaArchivo, TicketPesadaNombreArchivo)
    VALUES
        (@Folio, @NoLote, @Fecha, @Chofer, @Placas, @Observaciones, @NumeroTicket, @CoprefBico,
         @PesoBruto, @PesoTara, @TaraCajas, @PesoMuestra, @PesoNeto, @PesoProductor, @PorcentajeMateriaSeca,
         @CajasPorEntregar, @CajasEntregadas, @CajasCortadas, @CajasRecibidasVacias, @CajasDiferencia, @CamionDestarado,
         @TicketPesadaArchivo, @TicketPesadaNombreArchivo);

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
    @CajasPorEntregar       SMALLINT = 0,
    @CajasEntregadas        SMALLINT,
    @CajasCortadas          SMALLINT,
    @CajasRecibidasVacias   SMALLINT,
    @CajasDiferencia        SMALLINT,
    @CamionDestarado        BIT = 0,
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
        PorcentajeMateriaSeca = @PorcentajeMateriaSeca,
        CajasPorEntregar = @CajasPorEntregar,
        CajasEntregadas = @CajasEntregadas,
        CajasCortadas = @CajasCortadas,
        CajasRecibidasVacias = @CajasRecibidasVacias,
        CajasDiferencia = @CajasDiferencia,
        CamionDestarado = @CamionDestarado,
        TicketPesadaArchivo = @TicketPesadaArchivo,
        TicketPesadaNombreArchivo = @TicketPesadaNombreArchivo
    WHERE Id = @Id;
END
GO
