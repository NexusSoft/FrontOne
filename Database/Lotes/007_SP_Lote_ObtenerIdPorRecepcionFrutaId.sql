USE FrontOne;
GO

-- Resuelve el Lote real que contiene una Recepción de Fruta, vía la relación real
-- Lotes.LoteRecepcion (NoLote de Recepcion.RecepcionFruta es texto libre, no confiable como FK).
-- Usado desde RecepcionesFrutaForm para abrir el Lote correcto al hacer clic en la columna F. Lote.
CREATE OR ALTER PROCEDURE Lotes.sp_Lote_ObtenerIdPorRecepcionFrutaId
    @RecepcionFrutaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT lr.LoteId
    FROM Lotes.LoteRecepcion lr
    WHERE lr.RecepcionFrutaId = @RecepcionFrutaId;
END
GO
