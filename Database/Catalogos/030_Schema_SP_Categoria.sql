USE FrontOne;
GO

-- Catálogo Categoria: solo Id/Nombre/Activo, sin clave corta (a diferencia de Pais).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.Categoria'))
BEGIN
    CREATE TABLE Catalogos.Categoria
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_Categoria PRIMARY KEY,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_Categoria_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_Categoria_Nombre UNIQUE (Nombre)
    );
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Categoria_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, Activo
    FROM Catalogos.Categoria
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Categoria_Insertar
    @Nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.Categoria (Nombre, Activo)
    VALUES (@Nombre, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Categoria_Actualizar
    @Id     INT,
    @Nombre NVARCHAR(100),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Categoria
    SET Nombre = @Nombre,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Categoria_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Catalogos.Categoria
    WHERE Id = @Id;
END
GO
