USE FrontOne;
GO

-- Estatus del Pallet, actualizado: 1 Vacío, 2 Incompleto, 3 Completo, 4 Excedido, 5 Empacado,
-- 6 Reempacado, 7 En Proceso (granel), 8 Embarcado (nuevo — lo fija Embarques.sp_Contenedor_
-- AgregarPallet cuando el pallet entra a un Contenedor; sp_Contenedor_QuitarPallet lo regresa a 5).
-- Un pallet Embarcado ya no debe poder entrar a un reempaque como origen.
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_ObtenerPalletsOrigenDisponibles
    @Folio NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        p.Id, p.Folio, p.FechaCreacion, p.Estatus,
        tot.TotalCajas, tot.TotalKilogramos,
        CAST(NULL AS INT) AS CajasObjetivo, -- no aplica en modo Origen (el pallet ya está Bloqueado)
        p.EsMixto,
        CASE WHEN tot.ProductosDistintos > 1 THEN 'Mixto' ELSE ISNULL(prim.DescripcionSap, '') END AS ProductoDescripcion
    FROM Produccion.Pallet p
    OUTER APPLY (
        SELECT SUM(d.Cajas) AS TotalCajas, SUM(d.Kilogramos) AS TotalKilogramos,
               COUNT(DISTINCT d.ProductoTerminadoId) AS ProductosDistintos
        FROM Produccion.PalletDetalle d WHERE d.PalletId = p.Id
    ) AS tot
    OUTER APPLY (
        SELECT TOP 1 pt.DescripcionSap
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = p.Id ORDER BY d.Id
    ) AS prim
    WHERE p.EsNeutro = 0
      AND p.Estatus NOT IN (6, 8)
      AND p.Bloqueado = 1
      AND NOT EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle rd WHERE rd.PalletOrigenId = p.Id)
      AND (@Folio IS NULL OR @Folio = '' OR p.Folio LIKE '%' + @Folio + '%')
    ORDER BY p.FechaCreacionRegistro DESC;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_AgregarPalletOrigen
    @ReempaqueId INT,
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Reempaque WHERE Id = @ReempaqueId AND Estatus <> 1)
    BEGIN
        THROW 50000, 'Este reempaque ya está cerrado.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND EsNeutro = 1)
    BEGIN
        THROW 50000, 'Un Pallet Neutro (Merma/Diferencia a Favor) no puede entrar a un reempaque.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Estatus = 6)
    BEGIN
        THROW 50000, 'Este pallet ya fue reempacado antes: no puede volver a entrar.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Estatus = 8)
    BEGIN
        THROW 50000, 'Este pallet ya está embarcado en un contenedor.', 1;
    END

    IF NOT EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Solo se pueden reempacar pallets ya armados (bloqueados).', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle WHERE PalletOrigenId = @PalletId)
    BEGIN
        THROW 50000, 'Este pallet ya está reservado en otro reempaque.', 1;
    END

    BEGIN TRANSACTION;

    INSERT INTO Produccion.ReempaqueDetalle
        (ReempaqueId, PalletOrigenId, PalletDetalleOrigenId, LoteId, ProductoTerminadoOrigenId,
         CajasEntrada, KilosEntrada, KilosDisponibles)
    SELECT
        @ReempaqueId, @PalletId, d.Id, d.LoteId, d.ProductoTerminadoId,
        d.Cajas, d.Kilogramos, d.Kilogramos
    FROM Produccion.PalletDetalle d
    WHERE d.PalletId = @PalletId;

    UPDATE Produccion.Reempaque
    SET KilosAProcesar = KilosAProcesar + (SELECT SUM(Kilogramos) FROM Produccion.PalletDetalle WHERE PalletId = @PalletId)
    WHERE Id = @ReempaqueId;

    UPDATE Produccion.Pallet
    SET Estatus = 6, NoReempaque = (SELECT Folio FROM Produccion.Reempaque WHERE Id = @ReempaqueId)
    WHERE Id = @PalletId;

    COMMIT TRANSACTION;
END
GO
