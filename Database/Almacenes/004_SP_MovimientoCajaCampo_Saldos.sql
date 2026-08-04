USE FrontOne;
GO

DROP PROCEDURE IF EXISTS Almacenes.sp_MovimientoCajaCampo_ObtenerExistencias;
GO

CREATE OR ALTER PROCEDURE Almacenes.sp_MovimientoCajaCampo_Insertar
    @Fecha          DATE,
    @CajaCampoId    INT,
    @Cuenta         NVARCHAR(20),
    @TipoMovimiento NVARCHAR(20),
    @Cantidad       SMALLINT,
    @OrigenModulo   NVARCHAR(20),
    @OrigenId       INT = NULL,
    @Observaciones  NVARCHAR(500) = NULL,
    @Usuario        NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Almacenes.MovimientoCajaCampo
        (Fecha, CajaCampoId, Cuenta, TipoMovimiento, Cantidad, OrigenModulo, OrigenId, Observaciones, Usuario)
    VALUES
        (@Fecha, @CajaCampoId, @Cuenta, @TipoMovimiento, @Cantidad, @OrigenModulo, @OrigenId, @Observaciones, @Usuario);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Saldo de cada una de las 3 cuentas (Existencia/EnCampo/Produccion), por color, pivoteado en
-- columnas para que el dashboard las muestre lado a lado.
CREATE OR ALTER PROCEDURE Almacenes.sp_MovimientoCajaCampo_ObtenerSaldos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cc.Id,
        cc.Nombre,
        ISNULL(SUM(CASE WHEN m.Cuenta = 'Existencia' AND m.TipoMovimiento = 'Entrada' THEN m.Cantidad
                        WHEN m.Cuenta = 'Existencia' AND m.TipoMovimiento = 'Salida' THEN -m.Cantidad
                        ELSE 0 END), 0) AS Existencia,
        ISNULL(SUM(CASE WHEN m.Cuenta = 'EnCampo' AND m.TipoMovimiento = 'Entrada' THEN m.Cantidad
                        WHEN m.Cuenta = 'EnCampo' AND m.TipoMovimiento = 'Salida' THEN -m.Cantidad
                        ELSE 0 END), 0) AS EnCampo,
        ISNULL(SUM(CASE WHEN m.Cuenta = 'Produccion' AND m.TipoMovimiento = 'Entrada' THEN m.Cantidad
                        WHEN m.Cuenta = 'Produccion' AND m.TipoMovimiento = 'Salida' THEN -m.Cantidad
                        ELSE 0 END), 0) AS Produccion
    FROM Catalogos.CajaCampo cc
    LEFT JOIN Almacenes.MovimientoCajaCampo m ON m.CajaCampoId = cc.Id
    WHERE cc.Activo = 1
    GROUP BY cc.Id, cc.Nombre
    ORDER BY cc.Nombre;
END
GO
