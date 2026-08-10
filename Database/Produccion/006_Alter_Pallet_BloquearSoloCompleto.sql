USE FrontOne;
GO

-- Un pallet NO mixto solo se puede bloquear si ya está Completo (Estatus = 3, cajas = objetivo
-- exacto del producto). Un pallet mixto se puede bloquear sin esa restricción — no tiene un
-- objetivo único de cajas, así que "Completo" no aplica ahí.
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
    SET Estatus = 5, Bloqueado = 1, FechaBloqueo = SYSDATETIME()
    WHERE Id = @Id;
END
GO
