USE FrontOne;
GO

-- Fila base (no borrable en la UI) por cada Recepción del Lote, para Cosecha o Acarreo.
-- Cantidad/Precio Unitario/Importe NO se persisten: se calculan en vivo (Cosecha desde
-- Acopio.OrdenCorte.CostoKg/PagoDia/CuadrillaApoyo + RecepcionFruta.PesoNeto vs. umbral de
-- 4000 kg; Acarreo desde Acopio.OrdenCorte.PrecioAcarreo + PesoNeto). Esta tabla solo persiste
-- la decisión CargoA (Empresa/Productor) capturada por el usuario para esa fila.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Gastos.GastoRecepcion'))
BEGIN
    CREATE TABLE Gastos.GastoRecepcion (
        Id              INT IDENTITY PRIMARY KEY,
        GastoLoteId     INT NOT NULL REFERENCES Gastos.GastoLote(Id),
        LoteRecepcionId INT NOT NULL REFERENCES Lotes.LoteRecepcion(Id),
        TipoGasto       TINYINT NOT NULL, -- 1 = Cosecha, 2 = Acarreo
        CargoA          TINYINT NOT NULL, -- 1 = Empresa, 2 = Productor
        CONSTRAINT UQ_Gastos_GastoRecepcion UNIQUE (LoteRecepcionId, TipoGasto)
    );
END
GO
