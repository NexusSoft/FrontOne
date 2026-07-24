USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, ProductorId, VariedadId, Activo, FechaCreacion
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
    @VariedadId  INT,
    @Activo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Acopio.ListaPrecioFruta (ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, ProductorId, VariedadId, Activo)
    VALUES (@ItemCode, @ItemName, @Lista1, @Lista2, @Lista3, @FechaInicio, @FechaFin, @ProductorId, @VariedadId, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Ahora también NULL-safe en VariedadId (mismo criterio que ya tenía ProductorId): las 1,356
-- filas históricas sin variedad no chocan contra ninguna captura nueva, porque nunca se les
-- va a pasar @VariedadId = NULL desde la aplicación (siempre viene con valor).
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ExisteTraslape
    @ItemCode    NVARCHAR(50),
    @FechaInicio DATE,
    @FechaFin    DATE = NULL,
    @ProductorId INT = NULL,
    @VariedadId  INT = NULL,
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
          AND ((@VariedadId IS NULL AND VariedadId IS NULL) OR VariedadId = @VariedadId)
    ) THEN 1 ELSE 0 END AS BIT) AS Existe;
END
GO

-- Ahora también expone Variedad — cada combinación (fecha, productor-o-general, variedad) es
-- su propia vigencia navegable desde el buscador.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerFechas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT lpf.FechaInicio, lpf.ProductorId, p.NombreProductor AS ProductorNombre, lpf.VariedadId, v.Nombre AS VariedadNombre
    FROM Acopio.ListaPrecioFruta lpf
    LEFT JOIN Catalogos.Productor p ON p.Id = lpf.ProductorId
    LEFT JOIN Acopio.Variedad v ON v.Id = lpf.VariedadId
    WHERE lpf.Activo = 1
    ORDER BY lpf.FechaInicio DESC, ProductorNombre, VariedadNombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerPorFecha
    @Fecha       DATE,
    @ProductorId INT = NULL,
    @VariedadId  INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, ProductorId, VariedadId, Activo, FechaCreacion
    FROM Acopio.ListaPrecioFruta
    WHERE FechaInicio = @Fecha
      AND Activo = 1
      AND ((@ProductorId IS NULL AND ProductorId IS NULL) OR ProductorId = @ProductorId)
      AND ((@VariedadId IS NULL AND VariedadId IS NULL) OR VariedadId = @VariedadId)
    ORDER BY ItemCode;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_EliminarPorFecha
    @Fecha       DATE,
    @ProductorId INT = NULL,
    @VariedadId  INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Acopio.ListaPrecioFruta
    WHERE FechaInicio = @Fecha
      AND ((@ProductorId IS NULL AND ProductorId IS NULL) OR ProductorId = @ProductorId)
      AND ((@VariedadId IS NULL AND VariedadId IS NULL) OR VariedadId = @VariedadId);
END
GO
