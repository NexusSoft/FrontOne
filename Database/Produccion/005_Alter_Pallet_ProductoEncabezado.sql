USE FrontOne;
GO

-- Producto de referencia del encabezado del Pallet. NULL cuando EsMixto = 1 (varios productos
-- por línea, sin objetivo único) o mientras no se haya elegido nada todavía. Para pallets NO
-- mixtos, toda línea de detalle queda forzada a este mismo producto (ver sp_PalletDetalle_*) y
-- el objetivo de cajas (Completo/Incompleto/Excedido) sale de aquí, no de la primera línea
-- capturada como antes.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.Pallet') AND name = 'ProductoTerminadoId')
BEGIN
    ALTER TABLE Produccion.Pallet ADD ProductoTerminadoId INT NULL
        CONSTRAINT FK_Produccion_Pallet_ProductoTerminado REFERENCES Catalogos.ProductoTerminado (Id);
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id,
        p.Folio,
        p.FechaCreacion,
        p.HoraCreacion,
        p.Estatus,
        p.LineaProduccionId,
        lp.Nombre AS LineaProduccionNombre,
        p.EsMixto,
        p.ProductoTerminadoId,
        p.PorcentajeMateriaSeca,
        p.PesoReal,
        p.Bloqueado,
        p.FechaBloqueo,
        p.NoReempaque,
        p.PrimeraCorrida,
        ISNULL(tot.TotalCajas, 0) AS TotalCajas,
        ISNULL(tot.TotalKilogramos, 0) AS TotalKilogramos,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) = 0 THEN ''
            WHEN tot.ProductosDistintos > 1 THEN 'Mixto'
            ELSE ISNULL(prim.DescripcionSap, '')
        END AS ProductoDescripcion,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) = 0 THEN ''
            WHEN tot.ProductosDistintos > 1 THEN ''
            ELSE ISNULL(prim.CodigoSap, '')
        END AS ProductoCodigoSap,
        p.FechaCreacionRegistro
    FROM Produccion.Pallet p
    INNER JOIN Catalogos.LineaProduccion lp ON lp.Id = p.LineaProduccionId
    OUTER APPLY (
        SELECT SUM(d.Cajas) AS TotalCajas,
               SUM(d.Kilogramos) AS TotalKilogramos,
               COUNT(DISTINCT d.ProductoTerminadoId) AS ProductosDistintos
        FROM Produccion.PalletDetalle d
        WHERE d.PalletId = p.Id
    ) AS tot
    OUTER APPLY (
        SELECT TOP 1 pt.DescripcionSap, pt.CodigoSap
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = p.Id
        ORDER BY d.Id
    ) AS prim
    WHERE (@Id IS NULL OR p.Id = @Id)
    ORDER BY p.FechaCreacion DESC, p.Id DESC;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Insertar
    @LineaProduccionId INT,
    @EsMixto BIT = 0,
    @ProductoTerminadoId INT = NULL,
    @PesoReal DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @EsMixto = 1
    BEGIN
        SET @ProductoTerminadoId = NULL;
    END
    ELSE IF @ProductoTerminadoId IS NULL
    BEGIN
        THROW 50000, 'Selecciona un Producto para este pallet.', 1;
    END

    DECLARE @Folio NVARCHAR(7) =
        RIGHT('0000000' + CAST(NEXT VALUE FOR Produccion.SeqPalletFolio AS VARCHAR(7)), 7);

    INSERT INTO Produccion.Pallet
        (Folio, FechaCreacion, HoraCreacion, Estatus, LineaProduccionId, EsMixto, ProductoTerminadoId, PesoReal, PrimeraCorrida)
    VALUES
        (@Folio, CAST(SYSDATETIME() AS DATE), CAST(SYSDATETIME() AS TIME(0)), 1, @LineaProduccionId, @EsMixto, @ProductoTerminadoId, @PesoReal, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

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
        PesoReal = @PesoReal
    WHERE Id = @Id;
END
GO

-- Objetivo de cajas: pallets NO mixtos lo toman directo del Producto del encabezado (ya no de la
-- primera línea capturada) — así el objetivo existe incluso antes de la primera línea. Pallets
-- mixtos conservan el criterio anterior (snapshot de la primera línea), sin objetivo único real.
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
    DECLARE @SumaPonderada DECIMAL(18,4) = 0;
    DECLARE @Objetivo INT = 0;
    DECLARE @EsMixto BIT, @ProductoTerminadoId INT;

    SELECT @EsMixto = EsMixto, @ProductoTerminadoId = ProductoTerminadoId
    FROM Produccion.Pallet
    WHERE Id = @PalletId;

    SELECT @TotalCajas = ISNULL(SUM(Cajas), 0),
           @SumaPonderada = ISNULL(SUM(CAST(Cajas AS DECIMAL(18,4)) * PorcentajeMateriaSeca), 0)
    FROM Produccion.PalletDetalle
    WHERE PalletId = @PalletId;

    IF @EsMixto = 0
    BEGIN
        SELECT @Objetivo = ISNULL(CajasPorPallet, 0)
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
                      WHEN @TotalCajas = 0 THEN 1                        -- Vacío
                      WHEN @Objetivo <= 0 THEN 2                         -- sin objetivo válido: se queda Incompleto
                      WHEN @TotalCajas < @Objetivo THEN 2                -- Incompleto
                      WHEN @TotalCajas = @Objetivo THEN 3                -- Completo
                      ELSE 4                                             -- Excedido
                  END,
        PorcentajeMateriaSeca = CASE WHEN @TotalCajas = 0 THEN 0
                                     ELSE CAST(@SumaPonderada / @TotalCajas AS DECIMAL(5,2))
                                END
    WHERE Id = @PalletId;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerLotesEnProceso
    @LineaProduccionId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id AS CorridaId,
        l.Id AS LoteId,
        l.Folio AS LoteFolio,
        ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad,
        l.LineaProduccionId,
        lp.Nombre AS LineaProduccionNombre,
        l.PorcentajeMateriaSeca,
        c.KilosAProcesar,
        c.KilosProcesados,
        c.KilosAProcesar - c.KilosProcesados AS KilosDisponibles,
        primera.HuertaNombre,
        primera.RegistroSagarpa,
        primera.ProductorNombre
    FROM Produccion.Corrida c
    INNER JOIN Lotes.Lote l ON l.Id = c.LoteId
    INNER JOIN Catalogos.LineaProduccion lp ON lp.Id = l.LineaProduccionId
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, h.RegistroSagarpa, pr.NombreProductor AS ProductorNombre
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    WHERE c.Estatus = 1
      AND (@LineaProduccionId IS NULL OR l.LineaProduccionId = @LineaProduccionId)
    ORDER BY l.Folio;
END
GO

-- Agrega una línea: para pallets NO mixtos se IGNORA el @ProductoTerminadoId que mande el
-- cliente y se sustituye por el del encabezado (defensa en profundidad — ninguna línea de un
-- pallet no mixto puede terminar con un producto distinto, pase lo que pase del lado de la UI).
-- Nueva validación: la suma de cajas del pallet (incluida esta línea) no puede exceder el
-- CajasPorPallet objetivo cuando el pallet no es mixto.
CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_Insertar
    @PalletId INT,
    @CorridaId INT,
    @ProductoTerminadoId INT,
    @Cajas INT,
    @PorcentajeMateriaSeca DECIMAL(5,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Cajas IS NULL OR @Cajas <= 0
    BEGIN
        THROW 50000, 'Las cajas deben ser mayores a cero.', 1;
    END

    DECLARE @EsMixto BIT, @ProductoEncabezadoId INT, @PalletBloqueado BIT;
    SELECT @EsMixto = EsMixto, @ProductoEncabezadoId = ProductoTerminadoId, @PalletBloqueado = Bloqueado
    FROM Produccion.Pallet
    WHERE Id = @PalletId;

    IF @PalletBloqueado = 1
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se le pueden agregar líneas.', 1;
    END

    IF @EsMixto = 0
    BEGIN
        SET @ProductoTerminadoId = @ProductoEncabezadoId;
    END

    DECLARE @PesoNeto DECIMAL(10,2), @CajasPorPallet INT;
    SELECT @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet
    FROM Catalogos.ProductoTerminado
    WHERE Id = @ProductoTerminadoId;

    IF @PesoNeto IS NULL OR @PesoNeto <= 0
    BEGIN
        THROW 50000, 'El producto terminado seleccionado no tiene Peso Neto configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.', 1;
    END

    IF @CajasPorPallet IS NULL OR @CajasPorPallet <= 0
    BEGIN
        THROW 50000, 'El producto terminado seleccionado no tiene Cajas por Pallet configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.', 1;
    END

    IF @EsMixto = 0
    BEGIN
        DECLARE @CajasExistentes INT = ISNULL((SELECT SUM(Cajas) FROM Produccion.PalletDetalle WHERE PalletId = @PalletId), 0);
        IF (@CajasExistentes + @Cajas) > @CajasPorPallet
        BEGIN
            THROW 50000, 'No se puede exceder el objetivo de cajas del producto: ya hay cajas capturadas que, sumadas a esta línea, superan el total configurado en Cajas por Pallet.', 1;
        END
    END

    BEGIN TRANSACTION;

    DECLARE @LoteId INT, @KilosAProcesar DECIMAL(10,2), @KilosProcesados DECIMAL(10,2), @Estatus TINYINT;
    SELECT @LoteId = LoteId, @KilosAProcesar = KilosAProcesar, @KilosProcesados = KilosProcesados, @Estatus = Estatus
    FROM Produccion.Corrida WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @CorridaId;

    IF @LoteId IS NULL
    BEGIN
        THROW 50000, 'El lote seleccionado ya no tiene una corrida abierta.', 1;
    END

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'La corrida de este lote ya fue finalizada: ya no se le pueden cargar cajas.', 1;
    END

    DECLARE @Kilogramos DECIMAL(10,2) = CAST(@PesoNeto * @Cajas AS DECIMAL(10,2));

    IF @KilosAProcesar - @KilosProcesados < @Kilogramos
    BEGIN
        THROW 50000, 'Saldo insuficiente en el lote: las cajas capturadas superan los kilogramos disponibles de la corrida.', 1;
    END

    INSERT INTO Produccion.PalletDetalle
        (PalletId, CorridaId, LoteId, ProductoTerminadoId, Cajas, Kilogramos, PorcentajeMateriaSeca, CajasPorPallet)
    VALUES
        (@PalletId, @CorridaId, @LoteId, @ProductoTerminadoId, @Cajas, @Kilogramos, ISNULL(@PorcentajeMateriaSeca, 0), @CajasPorPallet);

    DECLARE @NuevoId INT = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Kilogramos
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;

    SELECT @NuevoId AS Id;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_Actualizar
    @Id INT,
    @ProductoTerminadoId INT,
    @Cajas INT,
    @PorcentajeMateriaSeca DECIMAL(5,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Cajas IS NULL OR @Cajas <= 0
    BEGIN
        THROW 50000, 'Las cajas deben ser mayores a cero.', 1;
    END

    DECLARE @PalletId INT, @CorridaId INT, @KilosAnteriores DECIMAL(10,2), @CajasAnteriores INT;
    SELECT @PalletId = PalletId, @CorridaId = CorridaId, @KilosAnteriores = Kilogramos, @CajasAnteriores = Cajas
    FROM Produccion.PalletDetalle
    WHERE Id = @Id;

    IF @PalletId IS NULL
    BEGIN
        THROW 50000, 'La línea de detalle ya no existe.', 1;
    END

    DECLARE @EsMixto BIT, @ProductoEncabezadoId INT, @PalletBloqueado BIT;
    SELECT @EsMixto = EsMixto, @ProductoEncabezadoId = ProductoTerminadoId, @PalletBloqueado = Bloqueado
    FROM Produccion.Pallet
    WHERE Id = @PalletId;

    IF @PalletBloqueado = 1
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se pueden modificar sus líneas.', 1;
    END

    IF @EsMixto = 0
    BEGIN
        SET @ProductoTerminadoId = @ProductoEncabezadoId;
    END

    DECLARE @PesoNeto DECIMAL(10,2), @CajasPorPallet INT;
    SELECT @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet
    FROM Catalogos.ProductoTerminado
    WHERE Id = @ProductoTerminadoId;

    IF @PesoNeto IS NULL OR @PesoNeto <= 0
    BEGIN
        THROW 50000, 'El producto terminado seleccionado no tiene Peso Neto configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.', 1;
    END

    IF @CajasPorPallet IS NULL OR @CajasPorPallet <= 0
    BEGIN
        THROW 50000, 'El producto terminado seleccionado no tiene Cajas por Pallet configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.', 1;
    END

    IF @EsMixto = 0
    BEGIN
        DECLARE @CajasExistentes INT = ISNULL(
            (SELECT SUM(Cajas) FROM Produccion.PalletDetalle WHERE PalletId = @PalletId AND Id <> @Id), 0);
        IF (@CajasExistentes + @Cajas) > @CajasPorPallet
        BEGIN
            THROW 50000, 'No se puede exceder el objetivo de cajas del producto: ya hay cajas capturadas que, sumadas a esta línea, superan el total configurado en Cajas por Pallet.', 1;
        END
    END

    BEGIN TRANSACTION;

    DECLARE @KilosAProcesar DECIMAL(10,2), @KilosProcesados DECIMAL(10,2), @Estatus TINYINT;
    SELECT @KilosAProcesar = KilosAProcesar, @KilosProcesados = KilosProcesados, @Estatus = Estatus
    FROM Produccion.Corrida WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @CorridaId;

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'La corrida de este lote ya fue finalizada: esta línea ya no se puede modificar.', 1;
    END

    DECLARE @Kilogramos DECIMAL(10,2) = CAST(@PesoNeto * @Cajas AS DECIMAL(10,2));
    DECLARE @Delta DECIMAL(10,2) = @Kilogramos - @KilosAnteriores;

    IF @KilosAProcesar - @KilosProcesados < @Delta
    BEGIN
        THROW 50000, 'Saldo insuficiente en el lote: las cajas capturadas superan los kilogramos disponibles de la corrida.', 1;
    END

    UPDATE Produccion.PalletDetalle
    SET ProductoTerminadoId = @ProductoTerminadoId,
        Cajas = @Cajas,
        Kilogramos = @Kilogramos,
        PorcentajeMateriaSeca = ISNULL(@PorcentajeMateriaSeca, 0),
        CajasPorPallet = @CajasPorPallet
    WHERE Id = @Id;

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Delta
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;
END
GO
