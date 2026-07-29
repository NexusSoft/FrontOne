USE FrontOne;
GO

-- Plantillas de reportes editadas por el usuario desde el Diseñador de Reportes (dentro de la
-- app, sin Visual Studio). Cada reporte tiene un Codigo fijo (ej. "RecepcionFruta") definido en
-- código C# (FrontOne.WinForms/Reports/); DefinicionXml es el layout serializado
-- (XtraReport.SaveLayoutToXml — XML de texto plano, por eso NVARCHAR(MAX) y no VARBINARY).
-- Si no hay fila para un Codigo (o DefinicionXml es NULL), se usa el layout default embebido en
-- el proyecto — el reporte nunca se rompe por falta de plantilla guardada.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Configuracion.ReportePlantilla'))
BEGIN
    CREATE TABLE Configuracion.ReportePlantilla
    (
        Id                  INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Configuracion_ReportePlantilla PRIMARY KEY,
        Codigo              NVARCHAR(50)        NOT NULL,
        Nombre              NVARCHAR(200)       NOT NULL,
        DefinicionXml       NVARCHAR(MAX)       NULL,
        FechaModificacion   DATETIME2           NOT NULL CONSTRAINT DF_Configuracion_ReportePlantilla_FechaModificacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Configuracion_ReportePlantilla_Codigo UNIQUE (Codigo)
    );
END
GO
