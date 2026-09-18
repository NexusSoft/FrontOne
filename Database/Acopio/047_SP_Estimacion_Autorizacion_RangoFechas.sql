USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_ObtenerParaAutorizacion
    @Fecha DATE = NULL,
    @SoloAutorizadas BIT = NULL,
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Conserva compatibilidad con clientes que envían una fecha individual.
    IF @Fecha IS NOT NULL
    BEGIN
        SET @FechaInicio = COALESCE(@FechaInicio, @Fecha);
        SET @FechaFin = COALESCE(@FechaFin, @Fecha);
    END;

    IF @FechaInicio IS NULL AND @FechaFin IS NULL
    BEGIN
        SELECT TOP (50)
            e.Id, e.Folio, e.Fecha, h.Nombre AS HuertaNombre,
            e.PrecioSugerido, e.Cerrada, e.Autorizada,
            e.PorcentajeCat1,
            e.PorcentajeCat2,
            e.PorcentajeNal,
            e.PorcentajeCalibre32,
            e.PorcentajeCalibre36,
            e.PorcentajeCalibre40,
            e.PorcentajeCalibre48,
            e.PorcentajeCalibre60,
            e.PorcentajeCalibre70,
            e.PorcentajeCalibre84,
            e.PorcentajeCalibre90,
            e.PorcentajeBorona,
            e.PorcentajeCanica,
            e.PorcentajeCuarta,
            e.PorcentajeDesecho,
            e.PorcentajeProceso
        FROM Acopio.Estimacion e
        INNER JOIN Catalogos.Huerta h ON h.Id = e.HuertaId
        WHERE (@SoloAutorizadas IS NULL OR e.Autorizada = @SoloAutorizadas)
        ORDER BY e.Fecha DESC, e.Id DESC;
    END
    ELSE
    BEGIN
        SELECT
            e.Id, e.Folio, e.Fecha, h.Nombre AS HuertaNombre,
            e.PrecioSugerido, e.Cerrada, e.Autorizada,
            e.PorcentajeCat1,
            e.PorcentajeCat2,
            e.PorcentajeNal,
            e.PorcentajeCalibre32,
            e.PorcentajeCalibre36,
            e.PorcentajeCalibre40,
            e.PorcentajeCalibre48,
            e.PorcentajeCalibre60,
            e.PorcentajeCalibre70,
            e.PorcentajeCalibre84,
            e.PorcentajeCalibre90,
            e.PorcentajeBorona,
            e.PorcentajeCanica,
            e.PorcentajeCuarta,
            e.PorcentajeDesecho,
            e.PorcentajeProceso
        FROM Acopio.Estimacion e
        INNER JOIN Catalogos.Huerta h ON h.Id = e.HuertaId
        WHERE (@FechaInicio IS NULL OR e.Fecha >= @FechaInicio)
          AND (@FechaFin IS NULL OR e.Fecha <= @FechaFin)
          AND (@SoloAutorizadas IS NULL OR e.Autorizada = @SoloAutorizadas)
        ORDER BY e.Fecha DESC, e.Id DESC;
    END
END
GO
