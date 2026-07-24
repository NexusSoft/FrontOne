USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Pais_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Clave, Nombre, Activo
    FROM Catalogos.Pais
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Pais_Insertar
    @Clave  NVARCHAR(3),
    @Nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.Pais (Clave, Nombre, Activo)
    VALUES (@Clave, @Nombre, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Pais_Actualizar
    @Id     INT,
    @Clave  NVARCHAR(3),
    @Nombre NVARCHAR(100),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Pais
    SET Clave = @Clave,
        Nombre = @Nombre,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Pais_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Catalogos.Pais
    WHERE Id = @Id;
END
GO
