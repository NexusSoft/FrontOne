USE FrontOne;
GO

-- CajaCampoId (color de caja que sale al campo con la cuadrilla, ver módulo Almacenes) —
-- NULLABLE para no romper las Órdenes de Corte ya existentes sin este dato; Application exige
-- el campo para órdenes nuevas vía validación, no vía NOT NULL de BD. Sin CASCADE (regla dura).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.OrdenCorte') AND name = 'CajaCampoId')
BEGIN
    ALTER TABLE Acopio.OrdenCorte ADD CajaCampoId INT NULL
        CONSTRAINT FK_Acopio_OrdenCorte_CajaCampo FOREIGN KEY REFERENCES Catalogos.CajaCampo (Id);
END
GO
