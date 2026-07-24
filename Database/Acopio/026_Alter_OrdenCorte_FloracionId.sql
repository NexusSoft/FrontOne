USE FrontOne;
GO

-- Floración de la huerta al momento del corte — campo obligatorio, se coloca justo después de
-- Huerta en la captura. La tabla está vacía en este punto (módulo recién construido), así que
-- se puede agregar directo como NOT NULL sin backfill.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.OrdenCorte') AND name = 'FloracionId')
BEGIN
    ALTER TABLE Acopio.OrdenCorte ADD FloracionId INT NOT NULL
        CONSTRAINT FK_Acopio_OrdenCorte_Floracion REFERENCES Acopio.Floracion (Id);
END
GO
