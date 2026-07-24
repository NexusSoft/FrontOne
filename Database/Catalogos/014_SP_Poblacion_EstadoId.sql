USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Obtener
    @EstadoId INT = NULL,
    @Id       INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, EstadoId, Activo
    FROM Catalogos.Poblacion
    WHERE (@EstadoId IS NULL OR EstadoId = @EstadoId)
      AND (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Insertar
    @Nombre   NVARCHAR(100),
    @EstadoId INT = NULL,
    @Activo   BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.Poblacion (Nombre, EstadoId, Activo)
    VALUES (@Nombre, @EstadoId, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Actualizar
    @Id       INT,
    @Nombre   NVARCHAR(100),
    @EstadoId INT = NULL,
    @Activo   BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Poblacion
    SET Nombre = @Nombre,
        EstadoId = @EstadoId,
        Activo = @Activo
    WHERE Id = @Id;
END
GO
