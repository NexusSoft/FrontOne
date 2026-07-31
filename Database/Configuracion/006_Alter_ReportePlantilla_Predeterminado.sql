USE FrontOne;
GO

-- Cuando una misma pantalla tiene varios reportes disponibles (ej. Recepción de Fruta con dos
-- diseños distintos), "predeterminado" es el que sale preseleccionado en la ventana de elegir
-- reporte al hacer clic en Vista Previa. Solo uno puede ser predeterminado por pantalla — esa
-- regla se controla desde la Application (ReportePlantillaService.MarcarPredeterminadoAsync),
-- no aquí, porque qué reportes pertenecen a qué pantalla vive en código (CatalogoReportes.cs),
-- no en esta tabla.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Configuracion.ReportePlantilla') AND name = 'EsPredeterminado')
BEGIN
    ALTER TABLE Configuracion.ReportePlantilla ADD
        EsPredeterminado BIT NOT NULL CONSTRAINT DF_Configuracion_ReportePlantilla_EsPredeterminado DEFAULT (0);
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_ReportePlantilla_ObtenerPorCodigo
    @Codigo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Codigo, Nombre, DefinicionXml, EsPredeterminado, FechaModificacion
    FROM Configuracion.ReportePlantilla
    WHERE Codigo = @Codigo;
END
GO

-- Marca este reporte como predeterminado — no toca DefinicionXml si la fila ya existía (marcar
-- predeterminado es independiente de tener o no un diseño personalizado guardado). Crea la fila
-- (sin diseño personalizado) si todavía no existía.
CREATE OR ALTER PROCEDURE Configuracion.sp_ReportePlantilla_MarcarPredeterminado
    @Codigo NVARCHAR(50),
    @Nombre NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Configuracion.ReportePlantilla WHERE Codigo = @Codigo)
    BEGIN
        UPDATE Configuracion.ReportePlantilla SET EsPredeterminado = 1 WHERE Codigo = @Codigo;
    END
    ELSE
    BEGIN
        INSERT INTO Configuracion.ReportePlantilla (Codigo, Nombre, EsPredeterminado)
        VALUES (@Codigo, @Nombre, 1);
    END
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_ReportePlantilla_QuitarPredeterminado
    @Codigo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Configuracion.ReportePlantilla SET EsPredeterminado = 0 WHERE Codigo = @Codigo;
END
GO
