USE FrontOne;
GO

-- Folio reutilizable: si se captura el Lote 0000026 y luego se elimina, el siguiente Lote debe
-- volver a ser 0000026 y no saltar al 0000027 (petición explícita del usuario). Con `SEQUENCE`
-- eso es imposible: una secuencia nunca devuelve un valor ya entregado, aunque la fila que lo
-- usaba se haya borrado. Se reemplaza por MAX(Folio)+1 leído de la propia tabla.
--
-- Comportamiento resultante:
--   * Se borra el último folio  -> se reutiliza (es lo que pidió el usuario).
--   * Se borra uno intermedio   -> el hueco NO se rellena (MAX no cambia). A propósito: rellenar
--                                  huecos intermedios rompería el orden cronológico del folio.
--   * Se vacía toda la tabla    -> vuelve a arrancar en 0000001 solo, sin resetear nada a mano.
--
-- Concurrencia: el SELECT del MAX y el INSERT van en la MISMA transacción, y el SELECT toma
-- (UPDLOCK, HOLDLOCK) — un lock de rango que serializa a dos capturas simultáneas, así la
-- segunda espera y lee el MAX ya actualizado en vez de calcular el mismo folio. El índice
-- UNIQUE de Folio (UQ_Lotes_Lote_Folio) queda como red de seguridad. XACT_ABORT ON garantiza
-- que cualquier error haga rollback y no deje la transacción abierta.
--
-- Lotes.SeqLoteFolio queda sin uso a partir de aquí. NO se elimina a propósito: archivos viejos
-- del repo (002_SP_Lote.sql) todavía la referencian, y borrarla rompería un replay completo en
-- una base nueva.
CREATE OR ALTER PROCEDURE Lotes.sp_Lote_Insertar
    @Fecha                  DATE,
    @CodigoTrazabilidad     NVARCHAR(16) = NULL,
    @Observaciones          NVARCHAR(500) = NULL,
    @Kilogramos             DECIMAL(18,2),
    @Personalizado          NVARCHAR(200) = NULL,
    @LineaProduccionId      INT,
    @PorcentajeMateriaSeca  DECIMAL(5,2),
    @Estatus                TINYINT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Folio NVARCHAR(7);

    BEGIN TRANSACTION;

    SELECT @Folio = RIGHT('0000000' + CAST(ISNULL(MAX(TRY_CAST(Folio AS INT)), 0) + 1 AS VARCHAR(7)), 7)
    FROM Lotes.Lote WITH (UPDLOCK, HOLDLOCK);

    INSERT INTO Lotes.Lote
        (Folio, Fecha, CodigoTrazabilidad, Observaciones, Kilogramos, Personalizado, LineaProduccionId,
         PorcentajeMateriaSeca, Estatus)
    VALUES
        (@Folio, @Fecha, @CodigoTrazabilidad, @Observaciones, @Kilogramos, @Personalizado, @LineaProduccionId,
         @PorcentajeMateriaSeca, @Estatus);

    COMMIT TRANSACTION;

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO
