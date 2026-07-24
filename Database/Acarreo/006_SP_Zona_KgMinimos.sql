USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_Zona_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Zona AS Nombre, KgMinimo300, KgMinimo400, KgMinimo500, Activo
    FROM Acarreo.Zona
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Zona;
END
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_Zona_Insertar
    @Nombre NVARCHAR(100),
    @KgMinimo300 DECIMAL(18,2) = 0,
    @KgMinimo400 DECIMAL(18,2) = 0,
    @KgMinimo500 DECIMAL(18,2) = 0,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acarreo.Zona (Zona, KgMinimo300, KgMinimo400, KgMinimo500, Activo)
    VALUES (@Nombre, @KgMinimo300, @KgMinimo400, @KgMinimo500, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_Zona_Actualizar
    @Id INT,
    @Nombre NVARCHAR(100),
    @KgMinimo300 DECIMAL(18,2) = 0,
    @KgMinimo400 DECIMAL(18,2) = 0,
    @KgMinimo500 DECIMAL(18,2) = 0,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acarreo.Zona
    SET Zona = @Nombre,
        KgMinimo300 = @KgMinimo300,
        KgMinimo400 = @KgMinimo400,
        KgMinimo500 = @KgMinimo500,
        Activo = @Activo
    WHERE Id = @Id;
END
GO
