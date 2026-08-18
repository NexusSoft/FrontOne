USE FrontOne;
GO

-- Filas de ajuste adicionales de Cosecha/Acarreo, capturadas con el catálogo Gastos.TipoAjuste.
-- El TipoGasto (Cosecha/Acarreo) del ajuste se obtiene por join a TipoAjuste.TipoGasto, así un
-- ajuste no puede quedar capturado en la pestaña que no le corresponde.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Gastos.GastoRecepcionAjuste'))
BEGIN
    CREATE TABLE Gastos.GastoRecepcionAjuste (
        Id              INT IDENTITY PRIMARY KEY,
        GastoLoteId     INT NOT NULL REFERENCES Gastos.GastoLote(Id),
        LoteRecepcionId INT NOT NULL REFERENCES Lotes.LoteRecepcion(Id),
        TipoAjusteId    INT NOT NULL REFERENCES Gastos.TipoAjuste(Id),
        Monto           DECIMAL(18,2) NOT NULL,
        CargoA          TINYINT NOT NULL, -- 1 = Empresa, 2 = Productor
        FechaCreacion   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO
