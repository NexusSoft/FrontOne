USE FrontOne;
GO

-- Redefine los 3 SPs de 010_SP_RecepcionFruta_DisponiblesParaLote_Destarado.sql tras el rename
-- Recepcion.RecepcionFruta.CamionDestarado -> DescargaCompleta (ver
-- Database/Recepcion/013_Rename_RecepcionFruta_DescargaCompleta.sql). Solo cambia el nombre de
-- columna referenciado; la regla de negocio (una Recepción solo entra a un Lote si ya se
-- descargó por completo) no cambia. Este archivo es ahora la última palabra de estos 3 SPs.
CREATE OR ALTER PROCEDURE Lotes.sp_RecepcionFruta_ObtenerTop100ParaLote
    @HuertaId               INT = NULL,
    @AcuerdoCorteId         INT = NULL,
    @PagarCorteACardCode    NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        rf.Id, rf.Folio, rf.NumeroTicket, rf.Fecha, rf.PesoNeto, rf.PorcentajeMateriaSeca, rf.CoprefBico,
        rf.DescargaCompleta,
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
      AND rf.DescargaCompleta = 1
      AND (@HuertaId IS NULL OR oc.HuertaId = @HuertaId)
      AND (@AcuerdoCorteId IS NULL OR oc.AcuerdoCorteId = @AcuerdoCorteId)
      AND (@PagarCorteACardCode IS NULL OR oc.PagarCorteACardCode = @PagarCorteACardCode)
    ORDER BY rf.FechaCreacion DESC;
END
GO

-- CamionDestarado -> DescargaCompleta se SELECCIONA pero no se filtra a propósito — el Service
-- necesita distinguir entre "no existe" y "existe pero no se ha descargado por completo" para
-- dar el mensaje correcto (ver LoteService.AgregarLineaAsync).
CREATE OR ALTER PROCEDURE Lotes.sp_RecepcionFruta_ObtenerParaLote
    @RecepcionFrutaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        rf.Id, rf.Folio, rf.NumeroTicket, rf.Fecha, rf.PesoNeto, rf.PorcentajeMateriaSeca, rf.CoprefBico,
        rf.DescargaCompleta,
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
        rf.DescargaCompleta,
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
      AND rf.DescargaCompleta = 1
      AND (@HuertaId IS NULL OR oc.HuertaId = @HuertaId)
      AND (@AcuerdoCorteId IS NULL OR oc.AcuerdoCorteId = @AcuerdoCorteId)
      AND (@PagarCorteACardCode IS NULL OR oc.PagarCorteACardCode = @PagarCorteACardCode)
      AND (rf.Folio LIKE '%' + @Filtro + '%' OR rf.NumeroTicket LIKE '%' + @Filtro + '%' OR h.Nombre LIKE '%' + @Filtro + '%')
    ORDER BY rf.FechaCreacion DESC;
END
GO
