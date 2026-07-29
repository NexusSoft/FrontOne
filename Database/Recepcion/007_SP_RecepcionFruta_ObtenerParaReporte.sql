USE FrontOne;
GO

-- Un solo SP con todos los joins necesarios para el reporte "Recepción de Fruta" — evita
-- encadenar varias llamadas a servicios distintos (Recepción + Orden de Corte + Acuerdo +
-- Producto). Solo trae una fila porque la regla de negocio ya limita a una Orden de Corte por
-- Recepción (ver RecepcionFrutaEditarForm.BtnDetalleNuevo_Click).
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
        rf.CamionDestarado,
        h.Nombre AS HuertaNombre,
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
    INNER JOIN Catalogos.Productor p ON p.Id = oc.ProductorId
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Catalogos.Producto prod ON prod.Id = ac.ProductoId
    INNER JOIN Acopio.Variedad v ON v.Id = oc.VariedadId
    INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
    WHERE rf.Id = @Id;
END
GO
