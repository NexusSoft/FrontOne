USE FrontOne;
GO

-- Segunda corrección (la primera, 013, agrupó por Producto Terminado — todavía demasiado
-- granular: cada calibre/empaque distinto es un Producto Terminado propio). El agrupador real
-- es la Materia Prima (Catalogos.ProductoTerminado.MateriaPrimaItemCode, mismo campo que se ve
-- en el combo "Materia Prima" de ProductoTerminadoEditarForm) — variantes de empaque que
-- comparten Materia Prima ahora se suman en una sola fila. MateriaPrimaItemCode es texto libre
-- (código de artículo SAP), sin FK, mismo criterio que Acopio.ListaPrecioFruta.ItemCode.
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gastos.GastoFrutaCategoria') AND name = 'ProductoTerminadoId')
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
          AND fk.referenced_object_id = OBJECT_ID('Catalogos.ProductoTerminado')
    );
    IF @fk IS NOT NULL
        EXEC('ALTER TABLE Gastos.GastoFrutaCategoria DROP CONSTRAINT [' + @fk + ']');

    -- Módulo todavía en pruebas — se limpia lo capturado en vez de migrarlo, para no dejar la
    -- columna nueva NOT NULL sin valor en filas viejas referenciando el esquema anterior.
    DELETE FROM Gastos.GastoFrutaCategoria;

    ALTER TABLE Gastos.GastoFrutaCategoria ADD MateriaPrimaItemCode NVARCHAR(50) NULL;
    ALTER TABLE Gastos.GastoFrutaCategoria DROP COLUMN ProductoTerminadoId;
    ALTER TABLE Gastos.GastoFrutaCategoria ALTER COLUMN MateriaPrimaItemCode NVARCHAR(50) NOT NULL;

    ALTER TABLE Gastos.GastoFrutaCategoria
        ADD CONSTRAINT UQ_Gastos_GastoFrutaCategoria UNIQUE (GastoLoteId, MateriaPrimaItemCode);
END
GO
