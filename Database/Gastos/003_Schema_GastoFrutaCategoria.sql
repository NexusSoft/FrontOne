USE FrontOne;
GO

-- Override manual del costo unitario por Categoría en la pestaña Fruta. Kilogramos/% siempre
-- se calculan en vivo (Produccion.PalletDetalle / Recepcion.RecepcionFruta.PesoNeto) — nunca
-- se persisten aquí. Cuando CostoRealUnitario/CostoEstimadoUnitario es NULL, el SP de
-- obtención aplica la fórmula calculada (Fijo/Banda); si no es NULL, gana el valor capturado
-- a mano por el usuario.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Gastos.GastoFrutaCategoria'))
BEGIN
    CREATE TABLE Gastos.GastoFrutaCategoria (
        Id                     INT IDENTITY PRIMARY KEY,
        GastoLoteId            INT NOT NULL REFERENCES Gastos.GastoLote(Id),
        CategoriaId            INT NOT NULL REFERENCES Catalogos.Categoria(Id),
        CostoRealUnitario      DECIMAL(18,4) NULL,
        CostoEstimadoUnitario  DECIMAL(18,4) NULL,
        CONSTRAINT UQ_Gastos_GastoFrutaCategoria UNIQUE (GastoLoteId, CategoriaId)
    );
END
GO
