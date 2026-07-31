USE FrontOne;
GO

-- Al guardar un Lote, cada Recepción que queda incluida recibe en su campo "No. de Lote"
-- (Recepcion.RecepcionFruta.NoLote, columna que ya existía sin uso) el Folio del Lote que la
-- contiene. Al quitar una Recepción del Lote (o borrar el Lote completo), se limpia de vuelta a
-- NULL — ya no le corresponde ese folio. Update ligero dedicado (no se usa
-- sp_RecepcionFruta_Actualizar completo) porque LoteService no tiene ni necesita el resto del
-- encabezado de la Recepción para este cambio puntual.
CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFruta_ActualizarNoLote
    @Id     INT,
    @NoLote NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Recepcion.RecepcionFruta
    SET NoLote = @NoLote
    WHERE Id = @Id;
END
GO
