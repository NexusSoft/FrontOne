USE FrontOne;
GO

-- Usado por RecepcionFrutaService.ActualizarAsync para bloquear la edición de una Recepción
-- que ya forma parte de un Lote — respaldado de todos modos por el UNIQUE en
-- Lotes.LoteRecepcion.RecepcionFrutaId.
CREATE OR ALTER PROCEDURE Lotes.sp_RecepcionFruta_EstaEnLote
    @RecepcionFrutaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM Lotes.LoteRecepcion WHERE RecepcionFrutaId = @RecepcionFrutaId
    ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS EstaEnLote;
END
GO
