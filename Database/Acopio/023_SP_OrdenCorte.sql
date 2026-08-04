USE FrontOne;
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
    @CajaCampoId            INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Folio NVARCHAR(7) = RIGHT('0000000' + CAST(NEXT VALUE FOR Acopio.SeqOrdenCorteFolio AS VARCHAR(7)), 7);

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
    @CajaCampoId            INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

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
        CajaCampoId = @CajaCampoId
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_OrdenCorte_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.OrdenCorte WHERE Id = @Id;
END
GO
