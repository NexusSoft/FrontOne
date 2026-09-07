USE FrontOne;
GO

-- Permite editar Posición/Temperatura de un pallet ya agregado al contenedor directo desde el
-- grid (ContenedorEditarForm), sin tener que quitarlo y volver a agregarlo. Misma validación de
-- posición única que sp_Contenedor_AgregarPallet, excluyendo el propio renglón.
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_ActualizarPallet
    @ContenedorPalletId INT,
    @Posicion INT,
    @Temperatura DECIMAL(5,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ContenedorId INT = (SELECT ContenedorId FROM Embarques.ContenedorPallet WHERE Id = @ContenedorPalletId);
    IF @ContenedorId IS NULL
    BEGIN
        THROW 50000, 'El renglón ya no existe.', 1;
    END

    IF EXISTS (
        SELECT 1 FROM Embarques.ContenedorPallet
        WHERE ContenedorId = @ContenedorId AND Posicion = @Posicion AND Id <> @ContenedorPalletId)
    BEGIN
        THROW 50000, 'Ya existe un pallet en esa posición dentro del contenedor.', 1;
    END

    UPDATE Embarques.ContenedorPallet
    SET Posicion = @Posicion, Temperatura = @Temperatura
    WHERE Id = @ContenedorPalletId;
END
GO
