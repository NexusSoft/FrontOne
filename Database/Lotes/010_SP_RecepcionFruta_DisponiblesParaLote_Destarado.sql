USE FrontOne;
GO

-- Regla nueva: una Recepción solo puede entrar a un Lote si su camión ya fue destarado
-- (CamionDestarado = 1). Mientras el camión no se destara, la recepción todavía no está cerrada
-- —falta la pesada en vacío— así que ni sus kilos ni sus cajas son definitivos y no tiene caso
-- conformar el Lote con ella. El mismo flag dispara el paso de la caja de campo a la cuenta
-- Produccion del Almacén (ver RecepcionFrutaService.RegistrarMovimientoEntradaAsync).
--
-- Este archivo redefine los 3 SPs completos (no solo los 2 que cambian de filtro) para que sea
-- la única "última palabra" de 004_SP_RecepcionFruta_DisponiblesParaLote.sql — mismo criterio
-- que Database/Recepcion/011, y así un replay en orden numérico en una BD nueva termina con la
-- versión correcta sin depender de cuál corrió al final.

CREATE OR ALTER PROCEDURE Lotes.sp_RecepcionFruta_ObtenerTop100ParaLote
    @HuertaId               INT = NULL,
    @AcuerdoCorteId         INT = NULL,
    @PagarCorteACardCode    NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        rf.Id, rf.Folio, rf.NumeroTicket, rf.Fecha, rf.PesoNeto, rf.PorcentajeMateriaSeca, rf.CoprefBico,
        rf.CamionDestarado,
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
      AND rf.CamionDestarado = 1
      AND (@HuertaId IS NULL OR oc.HuertaId = @HuertaId)
      AND (@AcuerdoCorteId IS NULL OR oc.AcuerdoCorteId = @AcuerdoCorteId)
      AND (@PagarCorteACardCode IS NULL OR oc.PagarCorteACardCode = @PagarCorteACardCode)
    ORDER BY rf.FechaCreacion DESC;
END
GO

-- Trae la Huerta/Acuerdo/Proveedor de UNA Recepción puntual (por Id), sin filtrar por
-- disponibilidad ni por destarado — usado por LoteService.AgregarLineaAsync para validar
-- compatibilidad y destarado contra lo ya agregado al Lote (defensa en profundidad: el picker
-- ya filtra en SQL, pero el Service no confía ciegamente en lo que mande la UI). Por eso aquí
-- CamionDestarado se SELECCIONA pero no se filtra: el Service necesita poder distinguir entre
-- "no existe" y "existe pero no está destarada" para dar el mensaje correcto.
CREATE OR ALTER PROCEDURE Lotes.sp_RecepcionFruta_ObtenerParaLote
    @RecepcionFrutaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        rf.Id, rf.Folio, rf.NumeroTicket, rf.Fecha, rf.PesoNeto, rf.PorcentajeMateriaSeca, rf.CoprefBico,
        rf.CamionDestarado,
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
        rf.CamionDestarado,
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
      AND rf.CamionDestarado = 1
      AND (@HuertaId IS NULL OR oc.HuertaId = @HuertaId)
      AND (@AcuerdoCorteId IS NULL OR oc.AcuerdoCorteId = @AcuerdoCorteId)
      AND (@PagarCorteACardCode IS NULL OR oc.PagarCorteACardCode = @PagarCorteACardCode)
      AND (rf.Folio LIKE '%' + @Filtro + '%' OR rf.NumeroTicket LIKE '%' + @Filtro + '%' OR h.Nombre LIKE '%' + @Filtro + '%')
    ORDER BY rf.FechaCreacion DESC;
END
GO
