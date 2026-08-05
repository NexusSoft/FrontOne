USE FrontOne;
GO

-- Folio reutilizable — ver el detalle completo del criterio en
-- Database/Lotes/011_SP_Lote_Folio_Reutilizable.sql. Mismo cambio: MAX(Folio)+1 dentro de la
-- misma transacción del INSERT con (UPDLOCK, HOLDLOCK), en vez de NEXT VALUE FOR.
--
-- El resto del cuerpo es idéntico a Database/Recepcion/011_SP_RecepcionFruta_CajasPerdidas.sql
-- (que sigue siendo la última palabra de _Obtener y _Actualizar) — aquí solo se redefine
-- _Insertar, que es el único que genera folio.
--
-- Recepcion.SeqRecepcionFrutaFolio queda sin uso; no se elimina porque archivos viejos del repo
-- (002/005/006) todavía la referencian y un replay en base nueva tronaría.
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
    @CajasPerdidas          SMALLINT = 0,
    @CamionDestarado        BIT = 0,
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
         PesoBruto, PesoTara, TaraCajas, PesoMuestra, PesoNeto, PesoProductor, PorcentajeMateriaSeca,
         CajasPorEntregar, CajasEntregadas, CajasCortadas, CajasRecibidasVacias, CajasDiferencia, CajasPerdidas,
         CamionDestarado, TicketPesadaArchivo, TicketPesadaNombreArchivo)
    VALUES
        (@Folio, @NoLote, @Fecha, @Chofer, @Placas, @Observaciones, @NumeroTicket, @CoprefBico,
         @PesoBruto, @PesoTara, @TaraCajas, @PesoMuestra, @PesoNeto, @PesoProductor, @PorcentajeMateriaSeca,
         @CajasPorEntregar, @CajasEntregadas, @CajasCortadas, @CajasRecibidasVacias, @CajasDiferencia, @CajasPerdidas,
         @CamionDestarado, @TicketPesadaArchivo, @TicketPesadaNombreArchivo);

    COMMIT TRANSACTION;

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO
