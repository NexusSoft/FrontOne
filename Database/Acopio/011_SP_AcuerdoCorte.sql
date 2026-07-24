USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acopio.sp_AcuerdoCorte_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ac.Id, ac.Folio, ac.ProductorId, pr.NombreProductor AS ProductorNombre,
        ac.FechaInicio, ac.FechaFin,
        ac.ProductoId, prod.Nombre AS ProductoNombre,
        ac.VariedadId, v.Nombre AS VariedadNombre,
        ac.TipoComercializacionId, tcom.Nombre AS TipoComercializacionNombre,
        ac.TipoCorteId, tc.Nombre AS TipoCorteNombre,
        ac.Precio,
        ac.ListaPrecioFecha, ac.ListaPrecioProductorId, lpp.NombreProductor AS ListaPrecioProductorNombre,
        ac.MonedaId, m.Nombre AS MonedaNombre,
        ac.Observaciones, ac.Activo, ac.FechaCreacion
    FROM Acopio.AcuerdoCorte ac
    INNER JOIN Catalogos.Productor pr ON pr.Id = ac.ProductorId
    INNER JOIN Catalogos.Producto prod ON prod.Id = ac.ProductoId
    INNER JOIN Acopio.Variedad v ON v.Id = ac.VariedadId
    INNER JOIN Acopio.TipoComercializacion tcom ON tcom.Id = ac.TipoComercializacionId
    INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
    INNER JOIN Acopio.Moneda m ON m.Id = ac.MonedaId
    LEFT JOIN Catalogos.Productor lpp ON lpp.Id = ac.ListaPrecioProductorId
    WHERE (@Id IS NULL OR ac.Id = @Id)
    ORDER BY ac.FechaCreacion DESC;
END
GO

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
    @MonedaId                INT,
    @Observaciones           NVARCHAR(500) = NULL,
    @Activo                  BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Folio NVARCHAR(7) = RIGHT('0000000' + CAST(NEXT VALUE FOR Acopio.SeqAcuerdoCorteFolio AS VARCHAR(7)), 7);

    INSERT INTO Acopio.AcuerdoCorte
        (Folio, ProductorId, FechaInicio, FechaFin, ProductoId, VariedadId, TipoComercializacionId, TipoCorteId,
         Precio, ListaPrecioFecha, ListaPrecioProductorId, MonedaId, Observaciones, Activo)
    VALUES
        (@Folio, @ProductorId, @FechaInicio, @FechaFin, @ProductoId, @VariedadId, @TipoComercializacionId, @TipoCorteId,
         @Precio, @ListaPrecioFecha, @ListaPrecioProductorId, @MonedaId, @Observaciones, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_AcuerdoCorte_Actualizar
    @Id                      INT,
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
    @MonedaId                INT,
    @Observaciones           NVARCHAR(500) = NULL,
    @Activo                  BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Acopio.AcuerdoCorte
    SET ProductorId = @ProductorId,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        ProductoId = @ProductoId,
        VariedadId = @VariedadId,
        TipoComercializacionId = @TipoComercializacionId,
        TipoCorteId = @TipoCorteId,
        Precio = @Precio,
        ListaPrecioFecha = @ListaPrecioFecha,
        ListaPrecioProductorId = @ListaPrecioProductorId,
        MonedaId = @MonedaId,
        Observaciones = @Observaciones,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_AcuerdoCorte_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.AcuerdoCorte WHERE Id = @Id;
END
GO

-- Vigencias de lista de precios disponibles para armar un Acuerdo de Corte: solo las que
-- caen dentro del rango de vigencia del acuerdo, y solo generales o del productor elegido
-- (nunca las especiales de otro productor).
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerFechasPorProductorYRango
    @ProductorId INT,
    @FechaInicio DATE,
    @FechaFin    DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT lpf.FechaInicio, lpf.ProductorId, p.NombreProductor AS ProductorNombre
    FROM Acopio.ListaPrecioFruta lpf
    LEFT JOIN Catalogos.Productor p ON p.Id = lpf.ProductorId
    WHERE lpf.Activo = 1
      AND lpf.FechaInicio BETWEEN @FechaInicio AND @FechaFin
      AND (lpf.ProductorId IS NULL OR lpf.ProductorId = @ProductorId)
    ORDER BY lpf.FechaInicio DESC, ProductorNombre;
END
GO
