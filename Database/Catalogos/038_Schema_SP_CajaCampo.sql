USE FrontOne;
GO

-- Catálogo Caja de Campo: tipos de caja de campo identificados por color (Roja, Azul, Blanca,
-- Amarilla, etc.) — solo Id/Nombre/Activo, mismo patrón que Categoria/LineaProduccion.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.CajaCampo'))
BEGIN
    CREATE TABLE Catalogos.CajaCampo
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_CajaCampo PRIMARY KEY,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_CajaCampo_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_CajaCampo_Nombre UNIQUE (Nombre)
    );
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_CajaCampo_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, Activo
    FROM Catalogos.CajaCampo
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_CajaCampo_Insertar
    @Nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.CajaCampo (Nombre, Activo)
    VALUES (@Nombre, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_CajaCampo_Actualizar
    @Id     INT,
    @Nombre NVARCHAR(100),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.CajaCampo
    SET Nombre = @Nombre,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_CajaCampo_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Catalogos.CajaCampo
    WHERE Id = @Id;
END
GO
