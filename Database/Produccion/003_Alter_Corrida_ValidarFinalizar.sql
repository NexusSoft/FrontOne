USE FrontOne;
GO

-- No se puede Finalizar una Corrida si Kilos Procesados todavía no iguala a Kilos a Procesar —
-- significa que aún faltan Pallets por generar contra ella (módulo futuro).
CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Finalizar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Produccion.Corrida WHERE Id = @Id AND Estatus = 1)
    BEGIN
        THROW 50000, 'Esta corrida ya fue finalizada.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.Corrida WHERE Id = @Id AND KilosAProcesar <> KilosProcesados)
    BEGIN
        THROW 50000, 'No se puede finalizar la corrida: los Kilos Procesados todavía no igualan a los Kilos a Procesar.', 1;
    END

    UPDATE Produccion.Corrida
    SET FechaHoraFin = SYSDATETIME(), Estatus = 2
    WHERE Id = @Id;
END
GO
