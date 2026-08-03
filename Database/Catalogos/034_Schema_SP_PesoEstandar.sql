USE FrontOne;
GO

-- Catálogo de pesos estándar (código, descripción, peso neto y peso promedio esperados por producto/empaque).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.PesoEstandar'))
BEGIN
    CREATE TABLE Catalogos.PesoEstandar
    (
        Id            INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_PesoEstandar PRIMARY KEY,
        Codigo        NVARCHAR(50)       NOT NULL,
        Descripcion   NVARCHAR(200)      NOT NULL,
        PesoNeto      DECIMAL(10,3)      NOT NULL,
        PesoPromedio  DECIMAL(10,3)      NOT NULL,
        Activo        BIT                NOT NULL CONSTRAINT DF_Catalogos_PesoEstandar_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_PesoEstandar_Codigo UNIQUE (Codigo)
    );
END
GO

-- Obtiene uno (por Id) o todos los pesos estándar, ordenados por código.
CREATE OR ALTER PROCEDURE Catalogos.sp_PesoEstandar_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Codigo, Descripcion, PesoNeto, PesoPromedio, Activo
    FROM Catalogos.PesoEstandar
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Codigo;
END
GO

-- Inserta un peso estándar nuevo, siempre Activo = 1.
CREATE OR ALTER PROCEDURE Catalogos.sp_PesoEstandar_Insertar
    @Codigo       NVARCHAR(50),
    @Descripcion  NVARCHAR(200),
    @PesoNeto     DECIMAL(10,3),
    @PesoPromedio DECIMAL(10,3)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.PesoEstandar (Codigo, Descripcion, PesoNeto, PesoPromedio, Activo)
    VALUES (@Codigo, @Descripcion, @PesoNeto, @PesoPromedio, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Actualiza los datos de un peso estándar existente.
CREATE OR ALTER PROCEDURE Catalogos.sp_PesoEstandar_Actualizar
    @Id           INT,
    @Codigo       NVARCHAR(50),
    @Descripcion  NVARCHAR(200),
    @PesoNeto     DECIMAL(10,3),
    @PesoPromedio DECIMAL(10,3),
    @Activo       BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.PesoEstandar
    SET Codigo = @Codigo,
        Descripcion = @Descripcion,
        PesoNeto = @PesoNeto,
        PesoPromedio = @PesoPromedio,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

-- Elimina un peso estándar (SQL Server rechaza el borrado si está referenciado en otra pantalla, por FK sin CASCADE).
CREATE OR ALTER PROCEDURE Catalogos.sp_PesoEstandar_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Catalogos.PesoEstandar
    WHERE Id = @Id;
END
GO
