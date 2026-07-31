USE FrontOne;
GO

-- Detalle del grid de Lote: cada renglón es una Recepción de Fruta agregada al Lote. No lleva
-- Actualizar — nada es editable por línea (mismo criterio que Recepcion.RecepcionFrutaOrdenCorte,
-- solo Nuevo/Borrar en la UI).
CREATE OR ALTER PROCEDURE Lotes.sp_LoteRecepcion_Obtener
    @LoteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        det.Id, det.LoteId, det.RecepcionFrutaId,
        rf.Folio AS RecepcionFrutaFolio, rf.NumeroTicket, rf.Fecha, rf.CoprefBico,
        rf.PesoNeto, rf.PorcentajeMateriaSeca
    FROM Lotes.LoteRecepcion det
    INNER JOIN Recepcion.RecepcionFruta rf ON rf.Id = det.RecepcionFrutaId
    WHERE det.LoteId = @LoteId
    ORDER BY det.Id;
END
GO

CREATE OR ALTER PROCEDURE Lotes.sp_LoteRecepcion_Insertar
    @LoteId             INT,
    @RecepcionFrutaId   INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Lotes.LoteRecepcion (LoteId, RecepcionFrutaId)
    VALUES (@LoteId, @RecepcionFrutaId);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Regresa el RecepcionFrutaId de la línea borrada (OUTPUT) para que LoteService pueda limpiar
-- el "No. de Lote" de esa Recepción (ver 006_SP_RecepcionFruta_ActualizarNoLote.sql) — al salir
-- del Lote, ya no le corresponde ese folio.
CREATE OR ALTER PROCEDURE Lotes.sp_LoteRecepcion_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Lotes.LoteRecepcion
    OUTPUT DELETED.RecepcionFrutaId
    WHERE Id = @Id;
END
GO
