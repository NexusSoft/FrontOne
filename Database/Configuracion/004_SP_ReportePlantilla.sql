USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_ReportePlantilla_ObtenerPorCodigo
    @Codigo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Codigo, Nombre, DefinicionXml, FechaModificacion
    FROM Configuracion.ReportePlantilla
    WHERE Codigo = @Codigo;
END
GO

-- Upsert por Codigo: si ya existe una plantilla para ese reporte se actualiza, si no se crea.
CREATE OR ALTER PROCEDURE Configuracion.sp_ReportePlantilla_Guardar
    @Codigo         NVARCHAR(50),
    @Nombre         NVARCHAR(200),
    @DefinicionXml  NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Configuracion.ReportePlantilla WHERE Codigo = @Codigo)
    BEGIN
        UPDATE Configuracion.ReportePlantilla
        SET Nombre = @Nombre,
            DefinicionXml = @DefinicionXml,
            FechaModificacion = SYSUTCDATETIME()
        WHERE Codigo = @Codigo;
    END
    ELSE
    BEGIN
        INSERT INTO Configuracion.ReportePlantilla (Codigo, Nombre, DefinicionXml)
        VALUES (@Codigo, @Nombre, @DefinicionXml);
    END
END
GO

-- "Restablecer" desde la pantalla Reportes: borra el diseño personalizado, el reporte vuelve a
-- usar el layout default embebido en el proyecto.
CREATE OR ALTER PROCEDURE Configuracion.sp_ReportePlantilla_Eliminar
    @Codigo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Configuracion.ReportePlantilla WHERE Codigo = @Codigo;
END
GO
