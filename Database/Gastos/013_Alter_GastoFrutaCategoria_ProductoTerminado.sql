USE FrontOne;
GO

-- Corrección: el grid de Fruta se agrupa por Producto Terminado (cada SKU/calibre real, ej.
-- "A) EXP CAL 14 CAT 1"), no por Catalogos.Categoria (catálogo genérico de 2-3 valores,
-- demasiado grueso — se vio al probar el grid en vivo). La Materia Prima
-- (ProductoTerminado.MateriaPrimaItemCode) sigue siendo la que resuelve el precio por match
-- contra Acopio.ListaPrecioFruta — eso no cambia, solo la columna de agrupación/override.
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gastos.GastoFrutaCategoria') AND name = 'CategoriaId')
BEGIN
    DECLARE @uq NVARCHAR(128) = (
        SELECT kc.name FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID('Gastos.GastoFrutaCategoria') AND kc.type = 'UQ'
    );
    IF @uq IS NOT NULL
        EXEC('ALTER TABLE Gastos.GastoFrutaCategoria DROP CONSTRAINT [' + @uq + ']');

    DECLARE @fk NVARCHAR(128) = (
        SELECT fk.name FROM sys.foreign_keys fk
        WHERE fk.parent_object_id = OBJECT_ID('Gastos.GastoFrutaCategoria')
          AND fk.referenced_object_id = OBJECT_ID('Catalogos.Categoria')
    );
    IF @fk IS NOT NULL
        EXEC('ALTER TABLE Gastos.GastoFrutaCategoria DROP CONSTRAINT [' + @fk + ']');

    EXEC sp_rename 'Gastos.GastoFrutaCategoria.CategoriaId', 'ProductoTerminadoId', 'COLUMN';

    ALTER TABLE Gastos.GastoFrutaCategoria
        ADD CONSTRAINT FK_Gastos_GastoFrutaCategoria_ProductoTerminado FOREIGN KEY (ProductoTerminadoId) REFERENCES Catalogos.ProductoTerminado (Id);
    ALTER TABLE Gastos.GastoFrutaCategoria
        ADD CONSTRAINT UQ_Gastos_GastoFrutaCategoria UNIQUE (GastoLoteId, ProductoTerminadoId);
END
GO
