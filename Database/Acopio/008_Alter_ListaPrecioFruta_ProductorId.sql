USE FrontOne;
GO

-- Productor opcional: si viene NULL es lista general; si viene con valor es una lista
-- especial (override) solo para ese productor, y coexiste con la lista general del mismo
-- ItemCode/día.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'ProductorId')
BEGIN
    ALTER TABLE Acopio.ListaPrecioFruta ADD ProductorId INT NULL;
    ALTER TABLE Acopio.ListaPrecioFruta
        ADD CONSTRAINT FK_Acopio_ListaPrecioFruta_Productor FOREIGN KEY (ProductorId) REFERENCES Catalogos.Productor (Id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Acopio_ListaPrecioFruta_FechaInicio_ProductorId')
BEGIN
    CREATE INDEX IX_Acopio_ListaPrecioFruta_FechaInicio_ProductorId ON Acopio.ListaPrecioFruta (FechaInicio, ProductorId);
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, ProductorId, Activo, FechaCreacion
    FROM Acopio.ListaPrecioFruta
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY FechaInicio DESC, ItemCode;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_Insertar
    @ItemCode    NVARCHAR(50),
    @ItemName    NVARCHAR(200),
    @Lista1      DECIMAL(18,4),
    @Lista2      DECIMAL(18,4),
    @Lista3      DECIMAL(18,4),
    @FechaInicio DATE,
    @FechaFin    DATE = NULL,
    @ProductorId INT = NULL,
    @Activo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Acopio.ListaPrecioFruta (ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, ProductorId, Activo)
    VALUES (@ItemCode, @ItemName, @Lista1, @Lista2, @Lista3, @FechaInicio, @FechaFin, @ProductorId, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Traslape ahora es NULL-safe en ProductorId: una lista general (ProductorId NULL) y una
-- lista especial de un productor para el mismo ItemCode/día NO chocan entre sí — cada
-- combinación (ItemCode, día, productor-o-general) es su propio espacio de unicidad.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ExisteTraslape
    @ItemCode    NVARCHAR(50),
    @FechaInicio DATE,
    @FechaFin    DATE = NULL,
    @ProductorId INT = NULL,
    @IdExcluir   INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(CASE WHEN EXISTS (
        SELECT 1 FROM Acopio.ListaPrecioFruta
        WHERE ItemCode = @ItemCode
          AND Activo = 1
          AND (@IdExcluir IS NULL OR Id <> @IdExcluir)
          AND FechaInicio <= ISNULL(@FechaFin, @FechaInicio)
          AND ISNULL(FechaFin, FechaInicio) >= @FechaInicio
          AND ((@ProductorId IS NULL AND ProductorId IS NULL) OR ProductorId = @ProductorId)
    ) THEN 1 ELSE 0 END AS BIT) AS Existe;
END
GO

-- Cada combinación única de (fecha, productor-o-general) es una "vigencia" navegable desde
-- el buscador. ProductorNombre viene por LEFT JOIN, NULL = lista general.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerFechas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT lpf.FechaInicio, lpf.ProductorId, p.NombreProductor AS ProductorNombre
    FROM Acopio.ListaPrecioFruta lpf
    LEFT JOIN Catalogos.Productor p ON p.Id = lpf.ProductorId
    WHERE lpf.Activo = 1
    ORDER BY lpf.FechaInicio DESC, ProductorNombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerPorFecha
    @Fecha       DATE,
    @ProductorId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, ProductorId, Activo, FechaCreacion
    FROM Acopio.ListaPrecioFruta
    WHERE FechaInicio = @Fecha
      AND Activo = 1
      AND ((@ProductorId IS NULL AND ProductorId IS NULL) OR ProductorId = @ProductorId)
    ORDER BY ItemCode;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_EliminarPorFecha
    @Fecha       DATE,
    @ProductorId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Acopio.ListaPrecioFruta
    WHERE FechaInicio = @Fecha
      AND ((@ProductorId IS NULL AND ProductorId IS NULL) OR ProductorId = @ProductorId);
END
GO
