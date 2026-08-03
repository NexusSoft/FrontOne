USE FrontOne;
GO

-- La fórmula de "Referencia" (código de barras GS1-128, AI(10) Batch/Lot) creció de 11 a 16
-- dígitos para incluir el Id de la Huerta: 089 (fijo) + HuertaId (5) + Folio del Lote (5) +
-- día juliano de la Fecha (3) — ver LoteService.CalcularReferencia. Los Lotes ya creados con el
-- formato viejo (11 dígitos) NO se recalculan, se quedan como están. ALTER COLUMN a un ancho
-- mayor sobre una columna existente es seguro/idempotente (nunca trunca datos ya guardados).
ALTER TABLE Lotes.Lote ALTER COLUMN Referencia NVARCHAR(16) NULL;
GO
