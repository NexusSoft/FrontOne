USE FrontOne;
GO

-- Catálogo de plantillas de etiqueta (Caja/Pallet/Registro Sagarpa). Primer catálogo del proyecto
-- con soft-delete real en Eliminar: Activo=0 en vez de DELETE físico, para poder recuperar una
-- etiqueta borrada por error (recuperación restringida por permiso especial, ver EtiquetaService).
-- DefinicionXml guarda el layout armado en el Diseñador de Reportes (mismo mecanismo que
-- Configuracion.ReportePlantilla.DefinicionXml), pero aquí vive en la misma fila del catálogo
-- porque cada Etiqueta es un registro dinámico creado por el usuario (Id autogenerado), no un
-- Código fijo hardcodeado como los reportes.
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Etiquetado')
BEGIN
    EXEC('CREATE SCHEMA Etiquetado');
END
GO

-- Tipo: 1=Caja, 2=Pallet, 3=RegistroSagarpa (Domain.Enums.TipoEtiqueta).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Etiquetado.Etiqueta'))
BEGIN
    CREATE TABLE Etiquetado.Etiqueta
    (
        Id                  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Etiquetado_Etiqueta PRIMARY KEY,
        Nombre              NVARCHAR(200)      NOT NULL,
        AnchoPulgadas       DECIMAL(5,2)       NOT NULL,
        AltoPulgadas        DECIMAL(5,2)       NOT NULL,
        Tipo                TINYINT            NOT NULL,
        DefinicionXml       NVARCHAR(MAX)      NULL,
        Activo              BIT                NOT NULL CONSTRAINT DF_Etiquetado_Etiqueta_Activo DEFAULT (1),
        FechaCreacion       DATETIME2          NOT NULL CONSTRAINT DF_Etiquetado_Etiqueta_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion   DATETIME2          NOT NULL CONSTRAINT DF_Etiquetado_Etiqueta_FechaModificacion DEFAULT (SYSUTCDATETIME())
    );

    -- Único solo entre las activas: permite reusar el nombre de una etiqueta ya eliminada.
    CREATE UNIQUE INDEX UQ_Etiquetado_Etiqueta_Nombre ON Etiquetado.Etiqueta (Nombre) WHERE Activo = 1;
END
GO

CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, AnchoPulgadas, AltoPulgadas, Tipo, DefinicionXml, Activo, FechaCreacion, FechaModificacion
    FROM Etiquetado.Etiqueta
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_ObtenerEliminados
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, AnchoPulgadas, AltoPulgadas, Tipo, DefinicionXml, Activo, FechaCreacion, FechaModificacion
    FROM Etiquetado.Etiqueta
    WHERE Activo = 0
    ORDER BY FechaModificacion DESC;
END
GO

CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_Obtener
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, AnchoPulgadas, AltoPulgadas, Tipo, DefinicionXml, Activo, FechaCreacion, FechaModificacion
    FROM Etiquetado.Etiqueta
    WHERE Id = @Id;
END
GO

-- @IdExcluir se usa al editar/duplicar, para no chocar contra el propio registro que se está
-- guardando. Devuelve 1 si el nombre YA está en uso por otra etiqueta activa.
CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_ValidarNombre
    @Nombre     NVARCHAR(200),
    @IdExcluir  INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(CASE WHEN EXISTS (
        SELECT 1 FROM Etiquetado.Etiqueta
        WHERE Nombre = @Nombre
          AND Activo = 1
          AND (@IdExcluir IS NULL OR Id <> @IdExcluir)
    ) THEN 1 ELSE 0 END AS BIT) AS EnUso;
END
GO

CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_Insertar
    @Nombre         NVARCHAR(200),
    @AnchoPulgadas  DECIMAL(5,2),
    @AltoPulgadas   DECIMAL(5,2),
    @Tipo           TINYINT,
    @DefinicionXml  NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Etiquetado.Etiqueta (Nombre, AnchoPulgadas, AltoPulgadas, Tipo, DefinicionXml)
    VALUES (@Nombre, @AnchoPulgadas, @AltoPulgadas, @Tipo, @DefinicionXml);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_Actualizar
    @Id             INT,
    @Nombre         NVARCHAR(200),
    @AnchoPulgadas  DECIMAL(5,2),
    @AltoPulgadas   DECIMAL(5,2),
    @Tipo           TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Etiquetado.Etiqueta
    SET Nombre = @Nombre,
        AnchoPulgadas = @AnchoPulgadas,
        AltoPulgadas = @AltoPulgadas,
        Tipo = @Tipo,
        FechaModificacion = SYSUTCDATETIME()
    WHERE Id = @Id;
END
GO

-- Update ligero, usado exclusivamente por el Diseñador de Reportes al guardar el layout — no
-- toca Nombre/Ancho/Alto/Tipo (mismo espíritu que Configuracion.sp_ReportePlantilla_Guardar).
CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_GuardarLayout
    @Id             INT,
    @DefinicionXml  NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Etiquetado.Etiqueta
    SET DefinicionXml = @DefinicionXml,
        FechaModificacion = SYSUTCDATETIME()
    WHERE Id = @Id;
END
GO

-- Soft-delete: nunca DELETE físico, para que la etiqueta se pueda recuperar.
CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Etiquetado.Etiqueta
    SET Activo = 0,
        FechaModificacion = SYSUTCDATETIME()
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Etiquetado.sp_Etiqueta_Recuperar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Etiquetado.Etiqueta
    SET Activo = 1,
        FechaModificacion = SYSUTCDATETIME()
    WHERE Id = @Id;
END
GO
