USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, Activo, FechaCreacion
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
    @Activo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Acopio.ListaPrecioFruta (ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, Activo)
    VALUES (@ItemCode, @ItemName, @Lista1, @Lista2, @Lista3, @FechaInicio, @FechaFin, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Traslape: dos vigencias del mismo ItemCode se cruzan si el fin de una (o su propio
-- inicio, cuando no tiene rango) cae dentro del inicio/fin de la otra.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ExisteTraslape
    @ItemCode    NVARCHAR(50),
    @FechaInicio DATE,
    @FechaFin    DATE = NULL,
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
    ) THEN 1 ELSE 0 END AS BIT) AS Existe;
END
GO
