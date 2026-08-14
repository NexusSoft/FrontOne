USE FrontOne;
GO

-- CodigoGs1128/VoiceCodeLow/VoiceCodeHigh ya no se calculan al vuelo (ver
-- 013_Alter_PalletDetalle_Gs1VoiceCode.sql) — se leen directo de la línea de detalle del pallet
-- (Produccion.PalletDetalle, alias d, ya está en el FROM de este SP).
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerEtiquetaCaja
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        pa.Folio                           AS NoPallet,
        pa.FechaCreacion                   AS FechaPallet,
        pa.FechaCreacion                   AS FechaEmpacado,
        emp.NumeroEmpaque                  AS NumeroEmpaque,
        l.Folio                            AS NoLote,
        l.CodigoTrazabilidad               AS CodigoTrazabilidad,
        l.Fecha                            AS FechaLote,
        ocMax.FechaOrdenCorteMax           AS FechaOrdenCorteMax,
        l.PorcentajeMateriaSeca            AS MateriaSecaLote,
        pr.NombreProductor                 AS Productor,
        h.Nombre                           AS Huerta,
        h.RegistroSagarpa                  AS RegistroSagarpa,
        h.NumeroGlobalGap                  AS RegistroGgn,
        mun.Nombre                         AS Municipio,
        pt.DescripcionSap                  AS NombreProductoTerminado,
        tp.Nombre                          AS TipoProducto,
        cat.Nombre                         AS Categoria,
        ca.Nombre                          AS CalibreApeam,
        pais.Nombre                        AS MercadoDestino,
        mar.Nombre                         AS Marca,
        var.Nombre                         AS Variedad,
        pt.PesoNeto                        AS PesoEstandar,
        pt.CalibreCodigoExterno            AS CodigoCalibreExterno,
        pt.CodigoUpc                       AS CodigoUpc,
        pt.CodigoPlu                       AS CodigoPlu,
        pt.CodigoGtin                      AS CodigoGtin,
        d.CodigoGs1128                     AS CodigoGs1128,
        d.VoiceCodeLow                     AS VoiceCodeLow,
        d.VoiceCodeHigh                    AS VoiceCodeHigh
    FROM Produccion.PalletDetalle d
    INNER JOIN Produccion.Pallet pa ON pa.Id = d.PalletId
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    LEFT JOIN Catalogos.TipoProducto tp ON tp.Id = pt.TipoProductoId
    LEFT JOIN Catalogos.Categoria cat ON cat.Id = pt.CategoriaId
    LEFT JOIN Catalogos.CalibreApeam ca ON ca.Id = pt.CalibreApeamId
    LEFT JOIN Catalogos.Pais pais ON pais.Id = pt.MercadoDestinoPaisId
    LEFT JOIN Catalogos.Marca mar ON mar.Id = pt.MarcaId
    LEFT JOIN Acopio.Variedad var ON var.Id = pt.VariedadId
    CROSS JOIN Configuracion.Empresa emp
    OUTER APPLY (
        SELECT TOP 1 h2.Id, h2.Nombre, h2.RegistroSagarpa, h2.NumeroGlobalGap, h2.MunicipioId, h2.ProductorId
        FROM Lotes.LoteRecepcion lr
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = lr.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h2 ON h2.Id = oc.HuertaId
        WHERE lr.LoteId = l.Id
        ORDER BY lr.Id
    ) AS h
    LEFT JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
    LEFT JOIN Catalogos.Municipio mun ON mun.Id = h.MunicipioId
    OUTER APPLY (
        SELECT MAX(oc2.Fecha) AS FechaOrdenCorteMax
        FROM Lotes.LoteRecepcion lr2
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc2 ON roc2.RecepcionFrutaId = lr2.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc2 ON oc2.Id = roc2.OrdenCorteId
        WHERE lr2.LoteId = l.Id
    ) AS ocMax
    WHERE d.PalletId = @PalletId
    ORDER BY d.Id;
END
GO
