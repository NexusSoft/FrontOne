USE FrontOne;
GO

-- Un pallet Granel no mixto no tiene tope de kilogramos — antes se marcaba Estatus 3 Completo con
-- solo tener algo capturado, lo cual es engañoso: "Completo" implica que ya no cabe más, y a un
-- Granel siempre le cabe más. Estatus nuevo 7 "En Proceso" identifica ese caso sin pisar el
-- significado real de Completo (que sigue siendo exclusivo de pallets con tope real de cajas).
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Recalcular
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Bloqueado = 1)
    BEGIN
        RETURN;
    END

    DECLARE @TotalCajas INT = 0;
    DECLARE @TotalKilogramos DECIMAL(18,2) = 0;
    DECLARE @SumaPonderadaKilos DECIMAL(18,4) = 0;
    DECLARE @Objetivo INT = 0;
    DECLARE @EsMixto BIT, @ProductoTerminadoId INT;

    SELECT @EsMixto = EsMixto, @ProductoTerminadoId = ProductoTerminadoId
    FROM Produccion.Pallet
    WHERE Id = @PalletId;

    SELECT @TotalCajas = ISNULL(SUM(Cajas), 0),
           @TotalKilogramos = ISNULL(SUM(Kilogramos), 0),
           @SumaPonderadaKilos = ISNULL(SUM(Kilogramos * PorcentajeMateriaSeca), 0)
    FROM Produccion.PalletDetalle
    WHERE PalletId = @PalletId;

    DECLARE @Presentacion TINYINT = NULL;

    IF @EsMixto = 0
    BEGIN
        SELECT @Objetivo = ISNULL(CajasPorPallet, 0), @Presentacion = Presentacion
        FROM Catalogos.ProductoTerminado
        WHERE Id = @ProductoTerminadoId;
    END
    ELSE
    BEGIN
        SELECT TOP 1 @Objetivo = CajasPorPallet
        FROM Produccion.PalletDetalle
        WHERE PalletId = @PalletId
        ORDER BY Id;
    END

    UPDATE Produccion.Pallet
    SET Estatus = CASE
                      WHEN @TotalKilogramos = 0 THEN 1                    -- Vacío
                      WHEN @EsMixto = 0 AND @Presentacion = 2 THEN 7      -- Granel no mixto: En Proceso, sin tope de kilogramos
                      WHEN @Objetivo <= 0 THEN 2                         -- sin objetivo válido: se queda Incompleto
                      WHEN @TotalCajas < @Objetivo THEN 2                -- Incompleto
                      WHEN @TotalCajas = @Objetivo THEN 3                -- Completo
                      ELSE 4                                             -- Excedido
                  END,
        PorcentajeMateriaSeca = CASE WHEN @TotalKilogramos = 0 THEN 0
                                     ELSE CAST(@SumaPonderadaKilos / @TotalKilogramos AS DECIMAL(5,2))
                                END,
        FechaModificacion = SYSDATETIME()
    WHERE Id = @PalletId;
END
GO

-- Un Granel no mixto ya no llega nunca a Estatus 3 — se bloquea desde 7 En Proceso, cuando el
-- usuario decida que ya no le agrega más kilos. Caja no mixto sigue exigiendo Completo (3), sin
-- cambio de comportamiento.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Bloquear
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EsMixto BIT, @Estatus TINYINT, @Bloqueado BIT;
    SELECT @EsMixto = EsMixto, @Estatus = Estatus, @Bloqueado = Bloqueado
    FROM Produccion.Pallet
    WHERE Id = @Id;

    IF @Bloqueado = 1
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado.', 1;
    END

    IF NOT EXISTS (SELECT 1 FROM Produccion.PalletDetalle WHERE PalletId = @Id)
    BEGIN
        THROW 50000, 'No se puede bloquear un pallet vacío: agrega al menos una línea de detalle.', 1;
    END

    IF @EsMixto = 0 AND @Estatus NOT IN (3, 7)
    BEGIN
        THROW 50000, 'No se puede bloquear: el pallet todavía no está Completo (las cajas capturadas no igualan el objetivo del producto).', 1;
    END

    UPDATE Produccion.Pallet
    SET Estatus = 5, Bloqueado = 1, FechaBloqueo = SYSDATETIME(), FechaModificacion = SYSDATETIME()
    WHERE Id = @Id;
END
GO
