USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_MarcarAutorizada
    @Id INT,
    @Autorizada BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Acopio.Estimacion
    SET Autorizada = @Autorizada
    WHERE Id = @Id AND Cerrada = 0;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_ObtenerParaAutorizacion
    @Fecha DATE = NULL,
    @SoloAutorizadas BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Fecha IS NULL
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
        WHERE e.Fecha = @Fecha
          AND (@SoloAutorizadas IS NULL OR e.Autorizada = @SoloAutorizadas)
        ORDER BY e.Fecha DESC, e.Id DESC;
    END
END
GO
