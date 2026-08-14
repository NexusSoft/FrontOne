USE FrontOne;
GO

-- Datos reales de un Pallet para imprimir/previsualizar sus 3 tipos de etiqueta (módulo
-- Etiquetado). Misma cadena de joins que ya usa Produccion.sp_Pallet_ObtenerLotesEnProceso para
-- llegar de un Lote a su Huerta/Productor/Sagarpa: PalletDetalle -> Lote -> LoteRecepcion ->
-- RecepcionFrutaOrdenCorte -> OrdenCorte -> Huerta/Productor.
SET QUOTED_IDENTIFIER ON;
GO

-- Etiqueta de Caja: un solo registro, tomado del PRIMER lote capturado en el pallet (TOP 1 por
-- Id de PalletDetalle) — así lo pidió el usuario para pallets con varios lotes. Fecha Juliana,
-- VoiceCode Low/High e Imagen USDA Organic NO van aquí: se resuelven en C# (VoicePickCodeCalculator
-- y EmpresaConfiguracionService) justo antes de imprimir.
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
        pt.CodigoGtin                      AS CodigoGtin
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

-- Etiqueta de Pallet, encabezado: un solo registro. Logo/datos de empresa se resuelven aparte
-- (EmpresaConfiguracionService), igual que en ReportePallet. Status se regresa crudo (Estatus del
-- Pallet) — el mapeo a "Completo"/"Incompleto" lo hace PalletService (simplificación pedida por
-- el usuario: cualquier Estatus distinto de 3=Completo se muestra como "Incompleto").
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerEtiquetaPalletEncabezado
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pa.Folio    AS NoPallet,
        pa.Estatus  AS Estatus,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) > 1 THEN 'Mixto'
            ELSE ISNULL(prim.DescripcionSap, '')
        END AS NombreProducto
    FROM Produccion.Pallet pa
    OUTER APPLY (
        SELECT COUNT(DISTINCT d.ProductoTerminadoId) AS ProductosDistintos
        FROM Produccion.PalletDetalle d
        WHERE d.PalletId = pa.Id
    ) AS tot
    OUTER APPLY (
        SELECT TOP 1 pt.DescripcionSap
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = pa.Id
        ORDER BY d.Id
    ) AS prim
    WHERE pa.Id = @PalletId;
END
GO

-- Etiqueta de Pallet, detalle: agrupado por Huerta+Lote (una fila por combinación, como pide el
-- formato de referencia con varias huertas mezcladas en un mismo pallet). El pie de página
-- (Total/TotalCajas/TotalKg) se calcula en C# sobre esta misma lista, no hace falta un SP aparte.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerEtiquetaPalletDetalle
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Folio             AS NoLote,
        h.RegistroSagarpa   AS RegistroSagarpa,
        h.Nombre            AS Huerta,
        SUM(d.Cajas)        AS Cajas,
        SUM(d.Kilogramos)   AS Kilogramos
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
    WHERE d.PalletId = @PalletId
    GROUP BY l.Folio, h.Nombre, h.RegistroSagarpa
    ORDER BY l.Folio;
END
GO

-- Etiqueta de Registro Sagarpa: TOP 1 (solo la primera huerta que encuentre en el pallet, según
-- lo pedido explícitamente por el usuario).
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerEtiquetaSagarpa
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
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
    WHERE d.PalletId = @PalletId
    ORDER BY d.Id;
END
GO
