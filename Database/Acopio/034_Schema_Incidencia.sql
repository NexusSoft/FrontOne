USE FrontOne;
GO

-- Captura de campo por Orden de Corte (llegada a huerta, cajas cosechadas, incidencias, ajustes,
-- datos SAP de flete/cosecha) — relación 1 a 1 con Acopio.OrdenCorte. Sin CASCADE (regla dura del
-- proyecto): mientras una Orden de Corte tenga Incidencia capturada, no se puede eliminar.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.Incidencia'))
BEGIN
    CREATE TABLE Acopio.Incidencia
    (
        Id                  INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Acopio_Incidencia PRIMARY KEY,
        OrdenCorteId        INT                 NOT NULL,
        SupervisorHuertaId  INT                 NULL,
        OdcSapCosecha       NVARCHAR(20)        NULL,
        NumeroTelefono      NVARCHAR(20)        NULL,
        Placas              NVARCHAR(20)        NULL,
        OdcSapFlete         NVARCHAR(20)        NULL,
        Bascula             NVARCHAR(100)       NULL,
        PuntoReunion        NVARCHAR(200)       NULL,
        HoraLlegadaHuerta   TIME                NULL,
        CajasCosechadas     INT                 NULL,
        CajaPorCuadrilla    NVARCHAR(100)       NULL,
        Observaciones       NVARCHAR(MAX)       NULL,
        Incidencias         NVARCHAR(MAX)       NULL,
        Ajuste              NVARCHAR(MAX)       NULL,
        FechaCreacion       DATETIME2           NOT NULL CONSTRAINT DF_Acopio_Incidencia_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Acopio_Incidencia_OrdenCorteId UNIQUE (OrdenCorteId),
        CONSTRAINT FK_Acopio_Incidencia_OrdenCorte FOREIGN KEY (OrdenCorteId) REFERENCES Acopio.OrdenCorte (Id),
        CONSTRAINT FK_Acopio_Incidencia_SupervisorHuerta FOREIGN KEY (SupervisorHuertaId) REFERENCES Acopio.SupervisorHuerta (Id)
    );
END
GO
