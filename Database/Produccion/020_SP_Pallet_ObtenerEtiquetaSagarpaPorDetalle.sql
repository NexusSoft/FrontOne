USE FrontOne;
GO

-- Variante de Produccion.sp_Pallet_ObtenerEtiquetaSagarpa (ver 012_Schema_SP_EtiquetaDatos.sql)
-- que en vez de traer la primera huerta del pallet completo (TOP 1 por PalletId), trae exactamente
-- el renglón de detalle indicado — usada por PalletEditarForm para imprimir la etiqueta de
-- Registro Sagarpa de un renglón específico del grid de detalle (columna "Etiqueta Lote").
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerEtiquetaSagarpaPorDetalle
    @PalletDetalleId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        h.Nombre            AS NombreHuerta,
        h.RegistroSagarpa   AS RegistroSagarpa
    FROM Produccion.PalletDetalle d
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    OUTER APPLY (
        SELECT TOP 1 h2.Nombre, h2.RegistroSagarpa
        FROM Lotes.LoteRecepcion lr
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = lr.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h2 ON h2.Id = oc.HuertaId
        WHERE lr.LoteId = l.Id
        ORDER BY lr.Id
    ) AS h
    WHERE d.Id = @PalletDetalleId;
END
GO
