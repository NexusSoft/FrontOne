USE FrontOne;
GO

-- Consulta web de órdenes de corte; conserva todos los datos para la vista previa.
CREATE OR ALTER PROCEDURE Acopio.sp_OrdenCorte_ObtenerParaConsultaWeb
    @Fecha DATE = NULL,
    @Confirmada BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Fecha IS NULL
    BEGIN
        SELECT TOP (50)
            oc.Id, oc.Folio, oc.Fecha,
            oc.AcuerdoCorteId, ac.Folio AS AcuerdoCorteFolio,
            oc.ProductorId, pr.NombreProductor AS ProductorNombre,
            oc.HuertaId, h.Nombre AS HuertaNombre,
            oc.FloracionId, fl.Floracion AS FloracionNombre,
            oc.RegistroSagarpa,
            oc.VariedadId, v.Nombre AS VariedadNombre,
            tc.Nombre AS TipoCorteNombre, tp.Nombre AS TipoPagoNombre,
            oc.PagarCorteACardCode, oc.PagarCorteANombre,
            oc.TransportistaCardCode, oc.TransportistaNombre,
            oc.PrecioAcarreo,
            oc.NoCandado,
            oc.CajasEntregadas,
            oc.JefeCuadrillaCardCode, oc.JefeCuadrillaNombre,
            oc.CostoKg, oc.PagoDia, oc.CuadrillaApoyo, oc.KgMinimo,
            oc.JefeAcopioId, oc.JefeAcopioNombre,
            oc.PuntoReunion, oc.Observaciones, oc.Cancelado,
            oc.CajaCampoId, cc.Nombre AS CajaCampoNombre,
            oc.EstimacionId, est.Folio AS EstimacionFolio,
            oc.OrdenConfirmada,
            CAST(CASE WHEN EXISTS (
                SELECT 1 FROM Recepcion.RecepcionFrutaOrdenCorte roc WHERE roc.OrdenCorteId = oc.Id
            ) THEN 1 ELSE 0 END AS BIT) AS EstaEnRecepcion,
            oc.FechaCreacion
        FROM Acopio.OrdenCorte oc
        INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
        INNER JOIN Catalogos.Productor pr ON pr.Id = oc.ProductorId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Acopio.Floracion fl ON fl.Id = oc.FloracionId
        INNER JOIN Acopio.Variedad v ON v.Id = oc.VariedadId
        INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
        INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
        LEFT JOIN Catalogos.CajaCampo cc ON cc.Id = oc.CajaCampoId
        LEFT JOIN Acopio.Estimacion est ON est.Id = oc.EstimacionId
        WHERE (@Confirmada IS NULL OR oc.OrdenConfirmada = @Confirmada)
        ORDER BY oc.Fecha DESC, oc.Id DESC;
    END
    ELSE
    BEGIN
        SELECT
            oc.Id, oc.Folio, oc.Fecha,
            oc.AcuerdoCorteId, ac.Folio AS AcuerdoCorteFolio,
            oc.ProductorId, pr.NombreProductor AS ProductorNombre,
            oc.HuertaId, h.Nombre AS HuertaNombre,
            oc.FloracionId, fl.Floracion AS FloracionNombre,
            oc.RegistroSagarpa,
            oc.VariedadId, v.Nombre AS VariedadNombre,
            tc.Nombre AS TipoCorteNombre, tp.Nombre AS TipoPagoNombre,
            oc.PagarCorteACardCode, oc.PagarCorteANombre,
            oc.TransportistaCardCode, oc.TransportistaNombre,
            oc.PrecioAcarreo,
            oc.NoCandado,
            oc.CajasEntregadas,
            oc.JefeCuadrillaCardCode, oc.JefeCuadrillaNombre,
            oc.CostoKg, oc.PagoDia, oc.CuadrillaApoyo, oc.KgMinimo,
            oc.JefeAcopioId, oc.JefeAcopioNombre,
            oc.PuntoReunion, oc.Observaciones, oc.Cancelado,
            oc.CajaCampoId, cc.Nombre AS CajaCampoNombre,
            oc.EstimacionId, est.Folio AS EstimacionFolio,
            oc.OrdenConfirmada,
            CAST(CASE WHEN EXISTS (
                SELECT 1 FROM Recepcion.RecepcionFrutaOrdenCorte roc WHERE roc.OrdenCorteId = oc.Id
            ) THEN 1 ELSE 0 END AS BIT) AS EstaEnRecepcion,
            oc.FechaCreacion
        FROM Acopio.OrdenCorte oc
        INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
        INNER JOIN Catalogos.Productor pr ON pr.Id = oc.ProductorId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Acopio.Floracion fl ON fl.Id = oc.FloracionId
        INNER JOIN Acopio.Variedad v ON v.Id = oc.VariedadId
        INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
        INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
        LEFT JOIN Catalogos.CajaCampo cc ON cc.Id = oc.CajaCampoId
        LEFT JOIN Acopio.Estimacion est ON est.Id = oc.EstimacionId
        WHERE oc.Fecha = @Fecha
          AND (@Confirmada IS NULL OR oc.OrdenConfirmada = @Confirmada)
        ORDER BY oc.Fecha DESC, oc.Id DESC;
    END
END
GO
