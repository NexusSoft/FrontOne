USE FrontOne;
GO

-- Kg mínimos no aplica por proveedor de corte — aplica por Zona según el número de cajas
-- (ver Acarreo.Zona / Database/Acarreo/005_Alter_Zona_KgMinimos.sql). Se quita de aquí.
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioCorte') AND name = 'KgMinimo')
BEGIN
    ALTER TABLE Acopio.ListaPrecioCorte DROP CONSTRAINT DF_Acopio_ListaPrecioCorte_KgMinimo;
    ALTER TABLE Acopio.ListaPrecioCorte DROP COLUMN KgMinimo;
END
GO
