USE FrontOne;
GO

-- Recepción de Fruta: cuando el camión regresa de campo a la empacadora, se pesa en báscula,
-- se cuenta cajas y se registra contra una o varias Órdenes de Corte ya capturadas (Acopio).
-- Encabezado = un camión/entrega; detalle (RecepcionFrutaOrdenCorte) = cada Orden de Corte que
-- trae ese camión, con los kilogramos que le corresponden. Igual patrón de índices filtrados
-- que ListaPrecioCorte/OrdenCorte, necesita QUOTED_IDENTIFIER ON en la sesión que los crea.
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Recepcion')
BEGIN
    EXEC('CREATE SCHEMA Recepcion');
END
GO

-- Folio consecutivo de 7 dígitos (0000001, 0000002...), mismo patrón que
-- Acopio.SeqAcuerdoCorteFolio / Acopio.SeqOrdenCorteFolio.
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqRecepcionFrutaFolio' AND schema_id = SCHEMA_ID('Recepcion'))
BEGIN
    CREATE SEQUENCE Recepcion.SeqRecepcionFrutaFolio AS INT START WITH 1 INCREMENT BY 1;
END
GO

-- PesoNeto y CajasDiferencia se calculan en el servidor (Application) antes de guardar, nunca
-- se confía en lo que mande la UI — se persisten ya calculados, mismo criterio que otros campos
-- "resueltos server-side" del proyecto (ver OrdenCorteService.ResolverYValidarAsync).
-- NoLote pertenece a un módulo futuro (aún no construido) — columna presente pero sin uso todavía.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Recepcion.RecepcionFruta'))
BEGIN
    CREATE TABLE Recepcion.RecepcionFruta
    (
        Id                      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Recepcion_RecepcionFruta PRIMARY KEY,
        Folio                   NVARCHAR(7)         NOT NULL,
        NoLote                  NVARCHAR(20)        NULL,
        Fecha                   DATE                NOT NULL,
        Chofer                  NVARCHAR(200)       NOT NULL,
        Placas                  NVARCHAR(20)        NULL,
        Observaciones           NVARCHAR(500)       NULL,
        NumeroTicket            NVARCHAR(50)        NULL,
        CoprefBico              NVARCHAR(50)        NULL,
        PesoBruto               DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_PesoBruto DEFAULT (0),
        PesoTara                DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_PesoTara DEFAULT (0),
        TaraCajas               DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_TaraCajas DEFAULT (0),
        PesoMuestra             DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_PesoMuestra DEFAULT (0),
        PesoNeto                DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_PesoNeto DEFAULT (0),
        PesoProductor           DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_PesoProductor DEFAULT (0),
        PorcentajeMateriaSeca   DECIMAL(5,2)         NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_PctMateriaSeca DEFAULT (0),
        CajasEntregadas         SMALLINT            NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_CajasEntregadas DEFAULT (0),
        CajasCortadas           SMALLINT            NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_CajasCortadas DEFAULT (0),
        CajasRecibidasVacias    SMALLINT            NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_CajasVacias DEFAULT (0),
        CajasDiferencia         SMALLINT            NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_CajasDiferencia DEFAULT (0),
        CamionDestarado         BIT                 NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_CamionDestarado DEFAULT (0),
        FechaCreacion           DATETIME2           NOT NULL CONSTRAINT DF_Recepcion_RecepcionFruta_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Recepcion_RecepcionFruta_Folio UNIQUE (Folio)
    );
END
GO

-- Detalle: cada Orden de Corte que trae ese camión. OrdenCorteId es UNIQUE en toda la tabla
-- (no solo por Recepción) para garantizar que una misma Orden de Corte nunca se reciba dos
-- veces en Recepciones distintas — decisión explícita del usuario. Kilogramos se captura a
-- mano por renglón, no se calcula. FK sin CASCADE/SET NULL (regla dura del proyecto); el
-- borrado del detalle al eliminar el encabezado se controla a mano dentro del propio SP
-- sp_RecepcionFruta_Eliminar.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Recepcion.RecepcionFrutaOrdenCorte'))
BEGIN
    CREATE TABLE Recepcion.RecepcionFrutaOrdenCorte
    (
        Id                  INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Recepcion_RecepcionFrutaOrdenCorte PRIMARY KEY,
        RecepcionFrutaId    INT                 NOT NULL,
        OrdenCorteId        INT                 NOT NULL,
        Kilogramos          DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Recepcion_RecepcionFrutaOrdenCorte_Kilogramos DEFAULT (0),
        CONSTRAINT UQ_Recepcion_RecepcionFrutaOrdenCorte_OrdenCorteId UNIQUE (OrdenCorteId),
        CONSTRAINT FK_Recepcion_RecepcionFrutaOrdenCorte_RecepcionFruta FOREIGN KEY (RecepcionFrutaId) REFERENCES Recepcion.RecepcionFruta (Id),
        CONSTRAINT FK_Recepcion_RecepcionFrutaOrdenCorte_OrdenCorte FOREIGN KEY (OrdenCorteId) REFERENCES Acopio.OrdenCorte (Id)
    );
END
GO
