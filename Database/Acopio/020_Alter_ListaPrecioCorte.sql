USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- El renglón "Otros" ya no aplica (el catálogo lo controla SAP por completo, sin precio de
-- respaldo manual) — se quita la columna EsOtros y su índice único.
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Acopio.ListaPrecioCorte') AND name = 'UQ_Acopio_ListaPrecioCorte_EsOtros')
BEGIN
    DROP INDEX UQ_Acopio_ListaPrecioCorte_EsOtros ON Acopio.ListaPrecioCorte;
END
GO

IF EXISTS (SELECT 1 FROM Acopio.ListaPrecioCorte WHERE EsOtros = 1)
BEGIN
    DELETE FROM Acopio.ListaPrecioCorte WHERE EsOtros = 1;
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioCorte') AND name = 'EsOtros')
BEGIN
    ALTER TABLE Acopio.ListaPrecioCorte DROP CONSTRAINT DF_Acopio_ListaPrecioCorte_EsOtros;
    ALTER TABLE Acopio.ListaPrecioCorte DROP COLUMN EsOtros;
END
GO

-- Kg mínimos: se captura junto con los demás precios, mismo criterio de formato (2 decimales).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioCorte') AND name = 'KgMinimo')
BEGIN
    ALTER TABLE Acopio.ListaPrecioCorte ADD KgMinimo DECIMAL(18,2) NOT NULL CONSTRAINT DF_Acopio_ListaPrecioCorte_KgMinimo DEFAULT (0);
END
GO

-- Ya no se puede eliminar un renglón desde FrontOne — el catálogo lo controla SAP por
-- completo (deshabilitar/habilitar en SAP mueve Activo en FrontOne vía la sincronización).
DROP PROCEDURE IF EXISTS Acopio.sp_ListaPrecioCorte_Eliminar;
GO
