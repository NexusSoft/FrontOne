USE FrontOne;
GO

-- Cajas Perdidas = CajasEntregadas - CajasCortadas - CajasRecibidasVacias (calculado en el
-- servidor, ver RecepcionFrutaService.Validar) — de lo que realmente salió con la cuadrilla,
-- cuánto no volvió en ninguna forma. Es lo que dispara el ajuste de inventario del módulo
-- Almacenes (ver Database/Almacenes/). Separado de CajasDiferencia, que ahora compara
-- CajasPorEntregar (el plan de la Orden de Corte) contra CajasEntregadas (lo que realmente
-- salió) — dos preguntas distintas, ver contexto/almacenes.md.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Recepcion.RecepcionFruta') AND name = 'CajasPerdidas')
BEGIN
    ALTER TABLE Recepcion.RecepcionFruta ADD CajasPerdidas SMALLINT NOT NULL
        CONSTRAINT DF_Recepcion_RecepcionFruta_CajasPerdidas DEFAULT (0);
END
GO
