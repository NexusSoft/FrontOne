USE FrontOne;
GO

-- Catálogo TipoProducto: solo Id/Nombre/Activo, sin clave corta (a diferencia de Pais).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.TipoProducto'))
BEGIN
    CREATE TABLE Catalogos.TipoProducto
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_TipoProducto PRIMARY KEY,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_TipoProducto_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_TipoProducto_Nombre UNIQUE (Nombre)
    );
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, Activo
    FROM Catalogos.TipoProducto
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_Insertar
    @Nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.TipoProducto (Nombre, Activo)
    VALUES (@Nombre, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_Actualizar
    @Id     INT,
    @Nombre NVARCHAR(100),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.TipoProducto
    SET Nombre = @Nombre,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Catalogos.TipoProducto
    WHERE Id = @Id;
END
GO
