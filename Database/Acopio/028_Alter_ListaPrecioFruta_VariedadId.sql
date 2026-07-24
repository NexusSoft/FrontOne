USE FrontOne;
GO

-- Variedad se agrega como columna opcional a nivel de base (la tabla ya tiene 1,356 filas
-- reales capturadas antes de este cambio, sin dato de variedad posible para backfill) — pero
-- la aplicación (ListaPrecioFrutaService.ValidarFila) la exige siempre para captura nueva.
-- Mismo criterio de "opcional en BD, obligatorio en Application" no aplica aquí como en
-- ProductorId (que sí es legítimamente opcional siempre) — es solo para no romper datos
-- históricos ya guardados.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'VariedadId')
BEGIN
    ALTER TABLE Acopio.ListaPrecioFruta ADD VariedadId INT NULL;
    ALTER TABLE Acopio.ListaPrecioFruta
        ADD CONSTRAINT FK_Acopio_ListaPrecioFruta_Variedad FOREIGN KEY (VariedadId) REFERENCES Acopio.Variedad (Id);
END
GO
