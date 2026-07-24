USE FrontOne;
GO

-- Los índices filtrados/columnas nuevas de este script necesitan QUOTED_IDENTIFIER ON en la
-- sesión que los crea (mismo gotcha ya documentado en ListaPrecioCorte).
SET QUOTED_IDENTIFIER ON;
GO

-- Folio consecutivo de 7 dígitos (0000001, 0000002...), independiente del Id — mismo patrón
-- que Acopio.SeqAcuerdoCorteFolio.
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqOrdenCorteFolio' AND schema_id = SCHEMA_ID('Acopio'))
BEGIN
    CREATE SEQUENCE Acopio.SeqOrdenCorteFolio AS INT START WITH 1 INCREMENT BY 1;
END
GO

-- Orden de Corte ejecuta un Acuerdo de Corte vigente: la mayoría de los campos de costo se
-- guardan como snapshot (copiados al momento de capturar) porque los catálogos que los
-- originan (ListaPrecioCorte, ListaPrecioAcarreo/Zona, JefeAcopio, Huerta) pueden cambiar
-- después y no se quiere que eso altere órdenes ya capturadas — decisión explícita del
-- usuario. Los proveedores de "Pagar el Corte a" (SAP grupo Mercancia=108) y "Transportista"
-- (SAP grupo Acarreo=101) no tienen tabla local (a diferencia de Cosecha=104/ListaPrecioCorte):
-- solo se consulta SAP en vivo para elegirlos, por eso solo guardan CardCode/CardName sueltos,
-- sin FK.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.OrdenCorte'))
BEGIN
    CREATE TABLE Acopio.OrdenCorte
    (
        Id                      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Acopio_OrdenCorte PRIMARY KEY,
        Folio                   NVARCHAR(7)         NOT NULL,
        Fecha                   DATE                NOT NULL,
        AcuerdoCorteId          INT                 NOT NULL,
        ProductorId             INT                 NOT NULL,
        HuertaId                INT                 NOT NULL,
        RegistroSagarpa         NVARCHAR(50)        NULL,
        VariedadId              INT                 NOT NULL,
        PagarCorteACardCode     NVARCHAR(20)        NOT NULL,
        PagarCorteANombre       NVARCHAR(200)       NOT NULL,
        TransportistaCardCode   NVARCHAR(20)        NOT NULL,
        TransportistaNombre     NVARCHAR(200)       NOT NULL,
        PrecioAcarreo           DECIMAL(18,2)       NOT NULL,
        NoCandado               NVARCHAR(50)        NULL,
        CajasEntregadas         SMALLINT            NOT NULL,
        JefeCuadrillaCardCode   NVARCHAR(20)        NOT NULL,
        JefeCuadrillaNombre     NVARCHAR(200)       NOT NULL,
        CostoKg                 DECIMAL(18,2)       NOT NULL,
        PagoDia                 DECIMAL(18,2)       NOT NULL,
        CuadrillaApoyo          DECIMAL(18,2)       NOT NULL,
        KgMinimo                DECIMAL(18,2)       NOT NULL,
        JefeAcopioId            INT                 NOT NULL,
        JefeAcopioNombre        NVARCHAR(200)       NOT NULL,
        PuntoReunion            NVARCHAR(200)       NULL,
        Observaciones           NVARCHAR(500)       NULL,
        Cancelado               BIT                 NOT NULL CONSTRAINT DF_Acopio_OrdenCorte_Cancelado DEFAULT (0),
        FechaCreacion           DATETIME2           NOT NULL CONSTRAINT DF_Acopio_OrdenCorte_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Acopio_OrdenCorte_Folio UNIQUE (Folio),
        CONSTRAINT CK_Acopio_OrdenCorte_CajasEntregadas CHECK (CajasEntregadas IN (300, 400, 500)),
        CONSTRAINT FK_Acopio_OrdenCorte_AcuerdoCorte FOREIGN KEY (AcuerdoCorteId) REFERENCES Acopio.AcuerdoCorte (Id),
        CONSTRAINT FK_Acopio_OrdenCorte_Productor FOREIGN KEY (ProductorId) REFERENCES Catalogos.Productor (Id),
        CONSTRAINT FK_Acopio_OrdenCorte_Huerta FOREIGN KEY (HuertaId) REFERENCES Catalogos.Huerta (Id),
        CONSTRAINT FK_Acopio_OrdenCorte_Variedad FOREIGN KEY (VariedadId) REFERENCES Acopio.Variedad (Id),
        CONSTRAINT FK_Acopio_OrdenCorte_JefeAcopio FOREIGN KEY (JefeAcopioId) REFERENCES Acopio.JefeAcopio (Id)
    );
END
GO
