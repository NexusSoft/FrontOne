USE FrontOne;
GO

-- Conformación de Lotes: agrupa una o varias Recepciones de Fruta ya capturadas (mismo
-- proveedor de "Pagar el Corte a", misma Huerta, mismo Acuerdo de Corte — validado en
-- Application) para conformar un embarque. Encabezado = el Lote; detalle (LoteRecepcion) =
-- cada Recepción que lo compone. Mismo criterio de índices/QUOTED_IDENTIFIER que Recepcion.
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Lotes')
BEGIN
    EXEC('CREATE SCHEMA Lotes');
END
GO

-- Folio consecutivo de 7 dígitos (0000001, 0000002...), mismo patrón que Recepcion.SeqRecepcionFrutaFolio.
-- No confundir con el folio embebido dentro de "Referencia" (formato juliano) — son dos
-- formateos independientes del mismo consecutivo, ver Lotes.Lote.Referencia y LoteService.
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqLoteFolio' AND schema_id = SCHEMA_ID('Lotes'))
BEGIN
    CREATE SEQUENCE Lotes.SeqLoteFolio AS INT START WITH 1 INCREMENT BY 1;
END
GO

-- Referencia se calcula en el servidor (Application) al guardar, con la fórmula del folio
-- juliano de 11 dígitos (089 + centena del día juliano + folio a 5 dígitos + decena/unidad del
-- día juliano) — ver LoteService.CalcularReferencia. Como el Folio se genera DENTRO del INSERT
-- (secuencia) y la fórmula lo necesita, Referencia se inserta en NULL y se completa con un
-- UPDATE inmediato después de conocer el Folio — por eso la columna es NULL (SQL Server permite
-- varios NULL bajo un UNIQUE, a diferencia de varias cadenas vacías, que sí chocarían).
-- Kilogramos se recalcula en vivo en la UI como la suma del PesoNeto de las Recepciones
-- agregadas, nunca se captura a mano.
-- Estatus: 0=Pendiente, 1=Procesado, 2=Error en Conformación — sin lógica real todavía, todo
-- Lote nuevo queda en Pendiente (ver contexto/lotes.md).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Lotes.Lote'))
BEGIN
    CREATE TABLE Lotes.Lote
    (
        Id                      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Lotes_Lote PRIMARY KEY,
        Folio                   NVARCHAR(7)         NOT NULL,
        Fecha                   DATE                NOT NULL,
        Referencia              NVARCHAR(11)        NULL,
        Observaciones           NVARCHAR(500)       NULL,
        Kilogramos              DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Lotes_Lote_Kilogramos DEFAULT (0),
        Personalizado           NVARCHAR(200)       NULL,
        LineaProduccionId       INT                 NOT NULL,
        PorcentajeMateriaSeca   DECIMAL(5,2)         NOT NULL CONSTRAINT DF_Lotes_Lote_PctMateriaSeca DEFAULT (0),
        Estatus                 TINYINT             NOT NULL CONSTRAINT DF_Lotes_Lote_Estatus DEFAULT (0),
        FechaCreacion           DATETIME2           NOT NULL CONSTRAINT DF_Lotes_Lote_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Lotes_Lote_Folio UNIQUE (Folio),
        CONSTRAINT UQ_Lotes_Lote_Referencia UNIQUE (Referencia),
        CONSTRAINT FK_Lotes_Lote_LineaProduccion FOREIGN KEY (LineaProduccionId) REFERENCES Catalogos.LineaProduccion (Id)
    );
END
GO

-- Detalle: cada Recepción de Fruta que compone el Lote. RecepcionFrutaId es UNIQUE en toda la
-- tabla (no solo por Lote) para garantizar que una misma Recepción nunca entre a dos Lotes
-- distintos — regla explícita del usuario. FK sin CASCADE/SET NULL (regla dura del proyecto);
-- el borrado del detalle al eliminar el encabezado se controla a mano en sp_Lote_Eliminar.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Lotes.LoteRecepcion'))
BEGIN
    CREATE TABLE Lotes.LoteRecepcion
    (
        Id                  INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Lotes_LoteRecepcion PRIMARY KEY,
        LoteId              INT                 NOT NULL,
        RecepcionFrutaId    INT                 NOT NULL,
        CONSTRAINT UQ_Lotes_LoteRecepcion_RecepcionFrutaId UNIQUE (RecepcionFrutaId),
        CONSTRAINT FK_Lotes_LoteRecepcion_Lote FOREIGN KEY (LoteId) REFERENCES Lotes.Lote (Id),
        CONSTRAINT FK_Lotes_LoteRecepcion_RecepcionFruta FOREIGN KEY (RecepcionFrutaId) REFERENCES Recepcion.RecepcionFruta (Id)
    );
END
GO
