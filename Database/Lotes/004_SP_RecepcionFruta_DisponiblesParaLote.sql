USE FrontOne;
GO

-- Viven en el schema Lotes (aunque consultan Recepcion/Acopio) porque son específicos de este
-- flujo — mismo criterio que Recepcion.sp_OrdenCorte_ObtenerTop100ParaRecepcion (vive en Acopio
-- porque consulta Acopio.OrdenCorte). Alimentan el picker "Seleccionar Recepción" de
-- LoteEditarForm: solo regresan Recepciones que NO estén ya en otro Lote (Lotes.LoteRecepcion
-- tiene UNIQUE en RecepcionFrutaId) y, si el Lote ya tiene líneas, solo las compatibles
-- (misma Huerta/Acuerdo/Proveedor de "Pagar el Corte a") — la Huerta/Acuerdo/Proveedor de cada
-- Recepción se toma de su primera Orden de Corte (CROSS APPLY TOP 1), ver LoteService.

CREATE OR ALTER PROCEDURE Lotes.sp_RecepcionFruta_ObtenerTop100ParaLote
    @HuertaId               INT = NULL,
    @AcuerdoCorteId         INT = NULL,
    @PagarCorteACardCode    NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        rf.Id, rf.Folio, rf.NumeroTicket, rf.Fecha, rf.PesoNeto, rf.PorcentajeMateriaSeca, rf.CoprefBico,
        oc.HuertaId, h.Nombre AS HuertaNombre, oc.AcuerdoCorteId,
        oc.PagarCorteACardCode, oc.PagarCorteANombre,
        oc.Id AS OrdenCorteId, oc.Folio AS OrdenCorteFolio, ac.Folio AS AcuerdoCorteFolio
    FROM Recepcion.RecepcionFruta rf
    CROSS APPLY (
        SELECT TOP 1 roc.OrdenCorteId
        FROM Recepcion.RecepcionFrutaOrdenCorte roc
        WHERE roc.RecepcionFrutaId = rf.Id
        ORDER BY roc.Id
    ) AS primera
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = primera.OrdenCorteId
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    WHERE NOT EXISTS (SELECT 1 FROM Lotes.LoteRecepcion det WHERE det.RecepcionFrutaId = rf.Id)
      AND (@HuertaId IS NULL OR oc.HuertaId = @HuertaId)
      AND (@AcuerdoCorteId IS NULL OR oc.AcuerdoCorteId = @AcuerdoCorteId)
      AND (@PagarCorteACardCode IS NULL OR oc.PagarCorteACardCode = @PagarCorteACardCode)
    ORDER BY rf.FechaCreacion DESC;
END
GO

-- Trae la Huerta/Acuerdo/Proveedor de UNA Recepción puntual (por Id), sin filtrar por
-- disponibilidad — usado por LoteService.AgregarLineaAsync para validar compatibilidad contra
-- las líneas ya agregadas al Lote (defensa en profundidad: el picker ya filtra en SQL, pero el
-- Service no confía ciegamente en lo que mande la UI).
CREATE OR ALTER PROCEDURE Lotes.sp_RecepcionFruta_ObtenerParaLote
    @RecepcionFrutaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        rf.Id, rf.Folio, rf.NumeroTicket, rf.Fecha, rf.PesoNeto, rf.PorcentajeMateriaSeca, rf.CoprefBico,
        oc.HuertaId, h.Nombre AS HuertaNombre, oc.AcuerdoCorteId,
        oc.PagarCorteACardCode, oc.PagarCorteANombre,
        oc.Id AS OrdenCorteId, oc.Folio AS OrdenCorteFolio, ac.Folio AS AcuerdoCorteFolio
    FROM Recepcion.RecepcionFruta rf
    CROSS APPLY (
        SELECT TOP 1 roc.OrdenCorteId
        FROM Recepcion.RecepcionFrutaOrdenCorte roc
        WHERE roc.RecepcionFrutaId = rf.Id
        ORDER BY roc.Id
    ) AS primera
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = primera.OrdenCorteId
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    WHERE rf.Id = @RecepcionFrutaId;
END
GO

CREATE OR ALTER PROCEDURE Lotes.sp_RecepcionFruta_BuscarParaLote
    @Filtro                 NVARCHAR(100),
    @HuertaId               INT = NULL,
    @AcuerdoCorteId         INT = NULL,
    @PagarCorteACardCode    NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 500
        rf.Id, rf.Folio, rf.NumeroTicket, rf.Fecha, rf.PesoNeto, rf.PorcentajeMateriaSeca, rf.CoprefBico,
        oc.HuertaId, h.Nombre AS HuertaNombre, oc.AcuerdoCorteId,
        oc.PagarCorteACardCode, oc.PagarCorteANombre,
        oc.Id AS OrdenCorteId, oc.Folio AS OrdenCorteFolio, ac.Folio AS AcuerdoCorteFolio
    FROM Recepcion.RecepcionFruta rf
    CROSS APPLY (
        SELECT TOP 1 roc.OrdenCorteId
        FROM Recepcion.RecepcionFrutaOrdenCorte roc
        WHERE roc.RecepcionFrutaId = rf.Id
        ORDER BY roc.Id
    ) AS primera
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = primera.OrdenCorteId
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    WHERE NOT EXISTS (SELECT 1 FROM Lotes.LoteRecepcion det WHERE det.RecepcionFrutaId = rf.Id)
      AND (@HuertaId IS NULL OR oc.HuertaId = @HuertaId)
      AND (@AcuerdoCorteId IS NULL OR oc.AcuerdoCorteId = @AcuerdoCorteId)
      AND (@PagarCorteACardCode IS NULL OR oc.PagarCorteACardCode = @PagarCorteACardCode)
      AND (rf.Folio LIKE '%' + @Filtro + '%' OR rf.NumeroTicket LIKE '%' + @Filtro + '%' OR h.Nombre LIKE '%' + @Filtro + '%')
    ORDER BY rf.FechaCreacion DESC;
END
GO
