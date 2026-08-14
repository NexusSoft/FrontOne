USE FrontOne;
GO

-- Indicador de "hay cambios nuevos" para el listado de Pallets (PalletsForm.cs): agrega
-- FechaModificacion a Produccion.Pallet y la actualiza en cada punto de escritura que afecta el
-- encabezado (directo o vía sp_Pallet_Recalcular, que corre en todo alta/edición/eliminación de
-- línea de detalle). Un pallet insertado nuevo ya la trae con el DEFAULT; un pallet eliminado no
-- necesita tocarla, se detecta por el cambio en Total (ver sp_Pallet_ObtenerUltimaModificacion).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.Pallet') AND name = 'FechaModificacion')
BEGIN
    ALTER TABLE Produccion.Pallet ADD FechaModificacion DATETIME2 NOT NULL
        CONSTRAINT DF_Produccion_Pallet_FechaModificacion DEFAULT (SYSDATETIME());
END
GO

-- Mismo cuerpo vigente de 005_Alter_Pallet_ProductoEncabezado.sql, solo se agrega
-- FechaModificacion al UPDATE.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ActualizarEncabezado
    @Id INT,
    @LineaProduccionId INT,
    @EsMixto BIT,
    @ProductoTerminadoId INT = NULL,
    @PesoReal DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @Id AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se puede modificar.', 1;
    END

    IF @EsMixto = 1
    BEGIN
        SET @ProductoTerminadoId = NULL;
    END
    ELSE IF @ProductoTerminadoId IS NULL
    BEGIN
        THROW 50000, 'Selecciona un Producto para este pallet.', 1;
    END

    IF EXISTS (
        SELECT 1
        FROM Produccion.Pallet p
        WHERE p.Id = @Id
          AND EXISTS (SELECT 1 FROM Produccion.PalletDetalle WHERE PalletId = @Id)
          AND ISNULL(p.ProductoTerminadoId, -1) <> ISNULL(@ProductoTerminadoId, -1)
    )
    BEGIN
        THROW 50000, 'No se puede cambiar el producto del encabezado: ya hay líneas de detalle capturadas.', 1;
    END

    UPDATE Produccion.Pallet
    SET LineaProduccionId = @LineaProduccionId,
        EsMixto = @EsMixto,
        ProductoTerminadoId = @ProductoTerminadoId,
        PesoReal = @PesoReal,
        FechaModificacion = SYSDATETIME()
    WHERE Id = @Id;
END
GO

-- Mismo cuerpo vigente de 009_Alter_PalletDetalle_Granel.sql, solo se agrega FechaModificacion al
-- UPDATE. Corre en todo alta/edición/eliminación de línea de detalle (sp_PalletDetalle_*), así que
-- basta con tocarla aquí para cubrir todos esos casos sin repetir la lógica en cada SP.
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
                      WHEN @EsMixto = 0 AND @Presentacion = 2 THEN 3      -- Granel no mixto: algo capturado ya es Completo
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

-- Mismo cuerpo vigente de 006_Alter_Pallet_BloquearSoloCompleto.sql, solo se agrega
-- FechaModificacion al UPDATE.
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

    IF @EsMixto = 0 AND @Estatus <> 3
    BEGIN
        THROW 50000, 'No se puede bloquear: el pallet todavía no está Completo (las cajas capturadas no igualan el objetivo del producto).', 1;
    END

    UPDATE Produccion.Pallet
    SET Estatus = 5, Bloqueado = 1, FechaBloqueo = SYSDATETIME(), FechaModificacion = SYSDATETIME()
    WHERE Id = @Id;
END
GO

-- Huella ligera para detectar "hay cambios nuevos" en el listado sin traer todo el grid otra vez:
-- Total (COUNT) cambia con cualquier alta/baja; UltimaModificacion cambia con cualquier edición de
-- encabezado o de una línea de detalle (vía sp_Pallet_Recalcular) o al bloquear. Comparando ambos
-- valores contra la última huella conocida se detecta cualquier tipo de cambio, sin columna
-- "EsNeutro"/estado adicional ni traer filas.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerUltimaModificacion
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS Total, MAX(FechaModificacion) AS UltimaModificacion
    FROM Produccion.Pallet;
END
GO
