USE FrontOne;
GO

-- Antes, el ledger solo tenía dos estados (Entrada/Salida de "el almacén" como bloque único).
-- El usuario aclaró que una caja de campo pasa por TRES ubicaciones reales: Existencia (almacén/
-- empaque), EnCampo (salió con la cuadrilla vía Orden de Corte, todavía no regresa) y Produccion
-- (volvió con fruta en una Recepción, está en la línea de empaque). Cuenta identifica en cuál de
-- las tres está parada cada movimiento; el saldo de cada una sigue siendo SUM(Entrada)-SUM(Salida)
-- pero ahora agrupado también por Cuenta. Los movimientos manuales (compra/ajuste) siempre afectan
-- Existencia, es la única cuenta que el usuario ajusta a mano.
--
-- DEFAULT ('Existencia') backfillea correctamente la única fila real que ya existía (una compra
-- manual capturada por el usuario antes de este cambio) sin necesidad de UPDATE aparte.
-- CHECK va inline en el mismo ADD (no en un ALTER TABLE ... ADD CONSTRAINT separado) para que no
-- truene por resolución de nombre de columna dentro del mismo batch sin GO de por medio.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Almacenes.MovimientoCajaCampo') AND name = 'Cuenta')
BEGIN
    ALTER TABLE Almacenes.MovimientoCajaCampo ADD
        Cuenta NVARCHAR(20) NOT NULL
            CONSTRAINT DF_Almacenes_MovimientoCajaCampo_Cuenta DEFAULT ('Existencia')
            CONSTRAINT CK_Almacenes_MovimientoCajaCampo_Cuenta CHECK (Cuenta IN ('Existencia', 'EnCampo', 'Produccion'));
END
GO
