USE FrontOne;
GO

-- Agrega las columnas que necesita el reporte nuevo "Vale de Recepción" (Folio de la Orden de
-- Corte y datos de registro de la Huerta: SAGARPA, Global GAP/GGN y Municipio) al mismo SP que ya
-- usa "Recepción de Ordenes de Corte" (007_SP_RecepcionFruta_ObtenerParaReporte.sql) — un solo
-- origen de datos para los 2 reportes del módulo, mismo criterio que el resto de reportes del
-- proyecto (un SP por pantalla, no uno por reporte).
CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFruta_ObtenerParaReporte
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        rf.Id, rf.Folio, rf.NoLote, rf.Fecha, rf.Chofer, rf.Placas, rf.Observaciones,
        rf.NumeroTicket, rf.CoprefBico,
        rf.PesoBruto, rf.PesoTara, rf.TaraCajas, rf.PesoMuestra, rf.PesoNeto, rf.PesoProductor,
        rf.PorcentajeMateriaSeca,
        rf.CajasPorEntregar, rf.CajasEntregadas, rf.CajasCortadas, rf.CajasRecibidasVacias, rf.CajasDiferencia,
        rf.DescargaCompleta,
        oc.Folio AS OrdenCorteFolio,
        h.Nombre AS HuertaNombre,
        h.RegistroSagarpa AS HuertaRegistroSagarpa,
        h.NumeroGlobalGap AS HuertaRegistroGgn,
        mun.Nombre AS HuertaMunicipioNombre,
        p.NombreProductor AS ProductorNombre,
        tc.Nombre AS TipoCorteNombre,
        ac.Folio AS AcuerdoCorteFolio,
        oc.TransportistaNombre,
        oc.JefeCuadrillaNombre AS EmpresaCorteNombre,
        oc.NoCandado,
        oc.Observaciones AS OrdenObservaciones,
        prod.Nombre AS ProductoNombre,
        v.Nombre AS VariedadNombre,
        det.Kilogramos
    FROM Recepcion.RecepcionFruta rf
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte det ON det.RecepcionFrutaId = rf.Id
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = det.OrdenCorteId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    LEFT JOIN Catalogos.Municipio mun ON mun.Id = h.MunicipioId
    INNER JOIN Catalogos.Productor p ON p.Id = oc.ProductorId
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Catalogos.Producto prod ON prod.Id = ac.ProductoId
    INNER JOIN Acopio.Variedad v ON v.Id = oc.VariedadId
    INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
    WHERE rf.Id = @Id;
END
GO
