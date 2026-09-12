USE FrontOne;
GO

-- Campo simple true/false para marcar la Orden de Corte como confirmada desde WinForms; se
-- consume después desde una pantalla nueva en FrontOne.Web (Acopio). Default 0 para que las
-- órdenes ya existentes queden como no confirmadas.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.OrdenCorte') AND name = 'OrdenConfirmada')
BEGIN
    ALTER TABLE Acopio.OrdenCorte ADD OrdenConfirmada BIT NOT NULL CONSTRAINT DF_Acopio_OrdenCorte_OrdenConfirmada DEFAULT (0);
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_OrdenCorte_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        oc.Id, oc.Folio, oc.Fecha,
        oc.AcuerdoCorteId, ac.Folio AS AcuerdoCorteFolio,
        oc.ProductorId, pr.NombreProductor AS ProductorNombre,
        oc.HuertaId, h.Nombre AS HuertaNombre,
        oc.FloracionId, fl.Floracion AS FloracionNombre,
        oc.RegistroSagarpa,
        oc.VariedadId, v.Nombre AS VariedadNombre,
        tc.Nombre AS TipoCorteNombre, tp.Nombre AS TipoPagoNombre,
        oc.PagarCorteACardCode, oc.PagarCorteANombre,
        oc.TransportistaCardCode, oc.TransportistaNombre,
        oc.PrecioAcarreo,
        oc.NoCandado,
        oc.CajasEntregadas,
        oc.JefeCuadrillaCardCode, oc.JefeCuadrillaNombre,
        oc.CostoKg, oc.PagoDia, oc.CuadrillaApoyo, oc.KgMinimo,
        oc.JefeAcopioId, oc.JefeAcopioNombre,
        oc.PuntoReunion, oc.Observaciones, oc.Cancelado,
        oc.CajaCampoId, cc.Nombre AS CajaCampoNombre,
        oc.EstimacionId, est.Folio AS EstimacionFolio,
        oc.OrdenConfirmada,
        CAST(CASE WHEN EXISTS (
            SELECT 1 FROM Recepcion.RecepcionFrutaOrdenCorte roc WHERE roc.OrdenCorteId = oc.Id
        ) THEN 1 ELSE 0 END AS BIT) AS EstaEnRecepcion,
        oc.FechaCreacion
    FROM Acopio.OrdenCorte oc
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Catalogos.Productor pr ON pr.Id = oc.ProductorId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    INNER JOIN Acopio.Floracion fl ON fl.Id = oc.FloracionId
    INNER JOIN Acopio.Variedad v ON v.Id = oc.VariedadId
    INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
    INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
    LEFT JOIN Catalogos.CajaCampo cc ON cc.Id = oc.CajaCampoId
    LEFT JOIN Acopio.Estimacion est ON est.Id = oc.EstimacionId
    WHERE (@Id IS NULL OR oc.Id = @Id)
    ORDER BY oc.FechaCreacion DESC;
END
GO

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
    @CajaCampoId            INT = NULL,
    @EstimacionId           INT = NULL,
    @OrdenConfirmada        BIT = 0
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
         PuntoReunion, Observaciones, Cancelado, CajaCampoId, EstimacionId, OrdenConfirmada)
    VALUES
        (@Folio, @Fecha, @AcuerdoCorteId, @ProductorId, @HuertaId, @FloracionId, @RegistroSagarpa, @VariedadId,
         @PagarCorteACardCode, @PagarCorteANombre, @TransportistaCardCode, @TransportistaNombre, @PrecioAcarreo,
         @NoCandado, @CajasEntregadas, @JefeCuadrillaCardCode, @JefeCuadrillaNombre,
         @CostoKg, @PagoDia, @CuadrillaApoyo, @KgMinimo, @JefeAcopioId, @JefeAcopioNombre,
         @PuntoReunion, @Observaciones, @Cancelado, @CajaCampoId, @EstimacionId, @OrdenConfirmada);

    IF @EstimacionId IS NOT NULL
    BEGIN
        EXEC Acopio.sp_Estimacion_MarcarCerrada @Id = @EstimacionId;
    END

    COMMIT TRANSACTION;

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_OrdenCorte_Actualizar
    @Id                     INT,
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
    @CajaCampoId            INT = NULL,
    @EstimacionId           INT = NULL,
    @OrdenConfirmada        BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    UPDATE Acopio.OrdenCorte
    SET Fecha = @Fecha,
        AcuerdoCorteId = @AcuerdoCorteId,
        ProductorId = @ProductorId,
        HuertaId = @HuertaId,
        FloracionId = @FloracionId,
        RegistroSagarpa = @RegistroSagarpa,
        VariedadId = @VariedadId,
        PagarCorteACardCode = @PagarCorteACardCode,
        PagarCorteANombre = @PagarCorteANombre,
        TransportistaCardCode = @TransportistaCardCode,
        TransportistaNombre = @TransportistaNombre,
        PrecioAcarreo = @PrecioAcarreo,
        NoCandado = @NoCandado,
        CajasEntregadas = @CajasEntregadas,
        JefeCuadrillaCardCode = @JefeCuadrillaCardCode,
        JefeCuadrillaNombre = @JefeCuadrillaNombre,
        CostoKg = @CostoKg,
        PagoDia = @PagoDia,
        CuadrillaApoyo = @CuadrillaApoyo,
        KgMinimo = @KgMinimo,
        JefeAcopioId = @JefeAcopioId,
        JefeAcopioNombre = @JefeAcopioNombre,
        PuntoReunion = @PuntoReunion,
        Observaciones = @Observaciones,
        Cancelado = @Cancelado,
        CajaCampoId = @CajaCampoId,
        EstimacionId = @EstimacionId,
        OrdenConfirmada = @OrdenConfirmada
    WHERE Id = @Id;

    IF @EstimacionId IS NOT NULL
    BEGIN
        EXEC Acopio.sp_Estimacion_MarcarCerrada @Id = @EstimacionId;
    END

    COMMIT TRANSACTION;
END
GO
