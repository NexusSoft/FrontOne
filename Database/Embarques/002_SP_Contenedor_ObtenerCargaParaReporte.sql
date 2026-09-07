USE FrontOne;
GO

-- Detalle plano (una fila por combinación pallet+línea de lote) para los 7 reportes de carga de
-- contenedor (Carga, Detalle por Lote, Detalle por Huerta, Resúmenes por Calibre/Lote/Huerta).
-- Cada reporte agrupa o pre-agrega esta misma lista según su layout — ver
-- FrontOne.WinForms/Reports/ReporteContenedorComun.cs. La Huerta no cuelga directo del Lote:
-- se llega vía Lotes.LoteRecepcion -> Recepcion.RecepcionFrutaOrdenCorte -> Acopio.OrdenCorte ->
-- Catalogos.Huerta, mismo patrón que Produccion.sp_Pallet_ObtenerEtiquetaSagarpaPorDetalle
-- (se toma la primera huerta encontrada por lote, TOP 1, mismo criterio que esa SP).
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_ObtenerCargaParaReporte
    @ContenedorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cp.Posicion,
        p.Folio                                            AS PalletFolio,
        p.FechaCreacion                                     AS PalletFechaCreacion,
        l.Folio                                             AS LoteFolio,
        l.Fecha                                             AS LoteFecha,
        pt.DescripcionSap                                   AS ProductoDescripcion,
        CASE WHEN pt.PesoNeto IS NULL THEN ''
             ELSE FORMAT(pt.PesoNeto, 'N1') + ' KG' END      AS ProductoPresentacionTexto,
        pt.CalibreCodigoExterno,
        mc.Nombre                                           AS MarcaNombre,
        d.Cajas,
        d.Kilogramos,
        h.Nombre                                            AS HuertaNombre,
        h.RegistroSagarpa,
        h.NumeroGlobalGap,
        h.CertificadoGlobalGap,
        pob.Nombre                                          AS PoblacionNombre,
        mun.Nombre                                           AS Municipio,
        prod.NombreProductor                                AS ProductorNombre,
        cp.Temperatura
    FROM Embarques.ContenedorPallet cp
    INNER JOIN Produccion.Pallet p ON p.Id = cp.PalletId
    INNER JOIN Produccion.PalletDetalle d ON d.PalletId = cp.PalletId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    LEFT JOIN Catalogos.Marca mc ON mc.Id = pt.MarcaId
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    OUTER APPLY (
        SELECT TOP 1 h2.Nombre, h2.RegistroSagarpa, h2.NumeroGlobalGap, h2.CertificadoGlobalGap,
               h2.MunicipioId, h2.ProductorId, h2.PoblacionId
        FROM Lotes.LoteRecepcion lr
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = lr.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h2 ON h2.Id = oc.HuertaId
        WHERE lr.LoteId = l.Id
        ORDER BY lr.Id
    ) AS h
    LEFT JOIN Catalogos.Poblacion pob ON pob.Id = h.PoblacionId
    LEFT JOIN Catalogos.Municipio mun ON mun.Id = h.MunicipioId
    LEFT JOIN Catalogos.Productor prod ON prod.Id = h.ProductorId
    WHERE cp.ContenedorId = @ContenedorId
    ORDER BY cp.Posicion, l.Folio;
END
GO
