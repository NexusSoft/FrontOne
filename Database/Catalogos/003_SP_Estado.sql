USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Estado_Obtener
    @PaisId INT = NULL,
    @Id     INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, PaisId, Clave, Nombre, Activo
    FROM Catalogos.Estado
    WHERE (@PaisId IS NULL OR PaisId = @PaisId)
      AND (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Estado_Insertar
    @PaisId INT,
    @Clave  NVARCHAR(10),
    @Nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.Estado (PaisId, Clave, Nombre, Activo)
    VALUES (@PaisId, @Clave, @Nombre, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Estado_Actualizar
    @Id     INT,
    @PaisId INT,
    @Clave  NVARCHAR(10),
    @Nombre NVARCHAR(100),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Estado
    SET PaisId = @PaisId,
        Clave = @Clave,
        Nombre = @Nombre,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Estado_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Catalogos.Estado
    WHERE Id = @Id;
END
GO
