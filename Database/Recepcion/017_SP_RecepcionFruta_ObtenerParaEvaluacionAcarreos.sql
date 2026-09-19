USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFruta_ObtenerParaEvaluacionAcarreos
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        oc.Id AS Id,
        oc.Fecha AS Fecha,
        h.Nombre AS HuertaNombre,
        h.RegistroSagarpa AS RegistroSagarpa,
        pob.Nombre AS PoblacionNombre,
        rf.NombreBascula,
        oc.TransportistaNombre,
        rf.Placas,
        rf.Chofer,
        rf.CajasEntregadas,
        rf.CajasCortadas,
        rf.CajasRecibidasVacias,
        rf.PesoNeto,
        rf.PesoProductor,
        rf.PesoProductor - rf.PesoNeto AS PesoDiferencia
    FROM Recepcion.RecepcionFruta rf
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte det ON det.RecepcionFrutaId = rf.Id
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = det.OrdenCorteId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    LEFT JOIN Catalogos.Poblacion pob ON pob.Id = h.PoblacionId
    WHERE (@FechaInicio IS NULL OR rf.Fecha >= @FechaInicio)
      AND (@FechaFin IS NULL OR rf.Fecha <= @FechaFin)
    ORDER BY rf.Fecha DESC, oc.Id DESC;
END
GO
