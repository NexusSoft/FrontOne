USE FrontOne;
GO

-- Permisos de reportes: modelo separado del genérico Pantalla/Accion/Permiso, porque un
-- reporte no es una "pantalla" con CRUD — tiene 4 acciones propias (Vista Previa, Impresión,
-- Exportación, Diseño). ReporteCodigo no lleva FK a ninguna tabla: los reportes están
-- definidos en código (FrontOne.Domain.Constants.ReportesDisponibles), mismo criterio que
-- Configuracion.ReportePlantilla.Codigo.
IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id
               WHERE s.name = 'Seguridad' AND t.name = 'ReportePermiso')
BEGIN
    CREATE TABLE Seguridad.ReportePermiso
    (
        Id            INT           NOT NULL CONSTRAINT PK_Seguridad_ReportePermiso PRIMARY KEY IDENTITY(1,1),
        RolId         INT           NOT NULL CONSTRAINT FK_Seguridad_ReportePermiso_Rol REFERENCES Seguridad.Rol (Id),
        ReporteCodigo NVARCHAR(50)  NOT NULL,
        VistaPrevia   BIT           NOT NULL CONSTRAINT DF_Seguridad_ReportePermiso_VistaPrevia DEFAULT (0),
        Impresion     BIT           NOT NULL CONSTRAINT DF_Seguridad_ReportePermiso_Impresion DEFAULT (0),
        Exportacion   BIT           NOT NULL CONSTRAINT DF_Seguridad_ReportePermiso_Exportacion DEFAULT (0),
        Diseno        BIT           NOT NULL CONSTRAINT DF_Seguridad_ReportePermiso_Diseno DEFAULT (0),
        CONSTRAINT UQ_Seguridad_ReportePermiso UNIQUE (RolId, ReporteCodigo)
    );
END
GO
