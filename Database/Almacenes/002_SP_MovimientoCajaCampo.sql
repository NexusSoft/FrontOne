USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Almacenes.sp_MovimientoCajaCampo_Insertar
    @Fecha          DATE,
    @CajaCampoId    INT,
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
        (Fecha, CajaCampoId, TipoMovimiento, Cantidad, OrigenModulo, OrigenId, Observaciones, Usuario)
    VALUES
        (@Fecha, @CajaCampoId, @TipoMovimiento, @Cantidad, @OrigenModulo, @OrigenId, @Observaciones, @Usuario);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Borra los movimientos ligados a un origen (Orden de Corte o Recepción) — se usa antes de volver
-- a insertar en cada Actualizar/Eliminar, patrón "borra y reinserta" para que el saldo nunca quede
-- desfasado por una edición.
CREATE OR ALTER PROCEDURE Almacenes.sp_MovimientoCajaCampo_EliminarPorOrigen
    @OrigenModulo NVARCHAR(20),
    @OrigenId     INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Almacenes.MovimientoCajaCampo
    WHERE OrigenModulo = @OrigenModulo AND OrigenId = @OrigenId;
END
GO

CREATE OR ALTER PROCEDURE Almacenes.sp_MovimientoCajaCampo_ObtenerExistencias
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cc.Id,
        cc.Nombre,
        ISNULL(SUM(CASE WHEN m.TipoMovimiento = 'Entrada' THEN m.Cantidad
                        WHEN m.TipoMovimiento = 'Salida' THEN -m.Cantidad
                        ELSE 0 END), 0) AS Existencia
    FROM Catalogos.CajaCampo cc
    LEFT JOIN Almacenes.MovimientoCajaCampo m ON m.CajaCampoId = cc.Id
    WHERE cc.Activo = 1
    GROUP BY cc.Id, cc.Nombre
    ORDER BY cc.Nombre;
END
GO

CREATE OR ALTER PROCEDURE Almacenes.sp_MovimientoCajaCampo_ObtenerPerdidaMes
    @Anio INT,
    @Mes  INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        oc.CajaCampoId,
        SUM(rf.CajasPerdidas) AS CajasPerdidas
    FROM Recepcion.RecepcionFruta rf
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = rf.Id
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
    WHERE oc.CajaCampoId IS NOT NULL
        AND YEAR(rf.Fecha) = @Anio
        AND MONTH(rf.Fecha) = @Mes
    GROUP BY oc.CajaCampoId;
END
GO
