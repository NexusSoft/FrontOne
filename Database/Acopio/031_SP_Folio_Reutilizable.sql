USE FrontOne;
GO

-- Folio reutilizable — ver el detalle completo del criterio en
-- Database/Lotes/011_SP_Lote_Folio_Reutilizable.sql. Mismo cambio en los dos folios del schema
-- Acopio: MAX(Folio)+1 dentro de la misma transacción del INSERT con (UPDLOCK, HOLDLOCK), en vez
-- de NEXT VALUE FOR.
--
-- Acopio.SeqAcuerdoCorteFolio y Acopio.SeqOrdenCorteFolio quedan sin uso; no se eliminan porque
-- archivos viejos del repo (011/013/023) todavía las referencian.

-- Cuerpo idéntico a Database/Acopio/013_Alter_AcuerdoCorte_ListaPrecioNumero.sql, solo cambia el folio.
CREATE OR ALTER PROCEDURE Acopio.sp_AcuerdoCorte_Insertar
    @ProductorId             INT,
    @FechaInicio             DATE,
    @FechaFin                DATE,
    @ProductoId              INT,
    @VariedadId              INT,
    @TipoComercializacionId  INT,
    @TipoCorteId             INT,
    @Precio                  DECIMAL(18,4) = NULL,
    @ListaPrecioFecha        DATE = NULL,
    @ListaPrecioProductorId  INT = NULL,
    @ListaPrecioNumero       TINYINT = NULL,
    @MonedaId                INT,
    @Observaciones           NVARCHAR(500) = NULL,
    @Activo                  BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Folio NVARCHAR(7);

    BEGIN TRANSACTION;

    SELECT @Folio = RIGHT('0000000' + CAST(ISNULL(MAX(TRY_CAST(Folio AS INT)), 0) + 1 AS VARCHAR(7)), 7)
    FROM Acopio.AcuerdoCorte WITH (UPDLOCK, HOLDLOCK);

    INSERT INTO Acopio.AcuerdoCorte
        (Folio, ProductorId, FechaInicio, FechaFin, ProductoId, VariedadId, TipoComercializacionId, TipoCorteId,
         Precio, ListaPrecioFecha, ListaPrecioProductorId, ListaPrecioNumero, MonedaId, Observaciones, Activo)
    VALUES
        (@Folio, @ProductorId, @FechaInicio, @FechaFin, @ProductoId, @VariedadId, @TipoComercializacionId, @TipoCorteId,
         @Precio, @ListaPrecioFecha, @ListaPrecioProductorId, @ListaPrecioNumero, @MonedaId, @Observaciones, @Activo);

    COMMIT TRANSACTION;

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO

-- Cuerpo idéntico a Database/Acopio/023_SP_OrdenCorte.sql, solo cambia el folio.
CREATE OR ALTER PROCEDURE Acopio.sp_OrdenCorte_Insertar
    @Fecha                  DATE,
    @AcuerdoCorteId         INT,
    @ProductorId            INT,
    @HuertaId               INT,
    @FloracionId            INT,
    @RegistroSagarpa        NVARCHAR(50) = NULL,
    @VariedadId             INT,
    @PagarCorteACardCode    NVARCHAR(20),
    @PagarCorteANombre      NVARCHAR(200),
    @TransportistaCardCode  NVARCHAR(20),
    @TransportistaNombre    NVARCHAR(200),
    @PrecioAcarreo          DECIMAL(18,2),
    @NoCandado              NVARCHAR(50) = NULL,
    @CajasEntregadas        SMALLINT,
    @JefeCuadrillaCardCode  NVARCHAR(20),
    @JefeCuadrillaNombre    NVARCHAR(200),
    @CostoKg                DECIMAL(18,2),
    @PagoDia                DECIMAL(18,2),
    @CuadrillaApoyo         DECIMAL(18,2),
    @KgMinimo               DECIMAL(18,2),
    @JefeAcopioId           INT,
    @JefeAcopioNombre       NVARCHAR(200),
    @PuntoReunion           NVARCHAR(200) = NULL,
    @Observaciones          NVARCHAR(500) = NULL,
    @Cancelado              BIT = 0,
    @CajaCampoId            INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Folio NVARCHAR(7);

    BEGIN TRANSACTION;

    SELECT @Folio = RIGHT('0000000' + CAST(ISNULL(MAX(TRY_CAST(Folio AS INT)), 0) + 1 AS VARCHAR(7)), 7)
    FROM Acopio.OrdenCorte WITH (UPDLOCK, HOLDLOCK);

    INSERT INTO Acopio.OrdenCorte
        (Folio, Fecha, AcuerdoCorteId, ProductorId, HuertaId, FloracionId, RegistroSagarpa, VariedadId,
         PagarCorteACardCode, PagarCorteANombre, TransportistaCardCode, TransportistaNombre, PrecioAcarreo,
         NoCandado, CajasEntregadas, JefeCuadrillaCardCode, JefeCuadrillaNombre,
         CostoKg, PagoDia, CuadrillaApoyo, KgMinimo, JefeAcopioId, JefeAcopioNombre,
         PuntoReunion, Observaciones, Cancelado, CajaCampoId)
    VALUES
        (@Folio, @Fecha, @AcuerdoCorteId, @ProductorId, @HuertaId, @FloracionId, @RegistroSagarpa, @VariedadId,
         @PagarCorteACardCode, @PagarCorteANombre, @TransportistaCardCode, @TransportistaNombre, @PrecioAcarreo,
         @NoCandado, @CajasEntregadas, @JefeCuadrillaCardCode, @JefeCuadrillaNombre,
         @CostoKg, @PagoDia, @CuadrillaApoyo, @KgMinimo, @JefeAcopioId, @JefeAcopioNombre,
         @PuntoReunion, @Observaciones, @Cancelado, @CajaCampoId);

    COMMIT TRANSACTION;

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO
