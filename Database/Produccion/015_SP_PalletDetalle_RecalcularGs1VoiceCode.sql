USE FrontOne;
GO

-- Recalcula CodigoGs1128/VoiceCode para las líneas de PalletDetalle. @PalletId NULL = recálculo
-- masivo (mantenimiento manual, ej. líneas que ya estaban en la base ANTES de que
-- 013_Alter_PalletDetalle_Gs1VoiceCode.sql agregara estas columnas); @PalletId con valor = recálculo
-- de un solo pallet, usado automáticamente por PalletService en cada modificación del pallet y al
-- cerrarlo (Bloquear) — así una línea que quedó en NULL porque el Lote/Producto todavía no tenía
-- GTIN/Código de Trazabilidad en el momento de capturarla se corrige sola en cuanto ese catálogo se
-- completa y el usuario vuelve a tocar el pallet, sin tener que correr el SP a mano.
--
-- CodigoGs1128 se recalcula 100% en T-SQL (mismo CASE que sp_PalletDetalle_Insertar/Actualizar).
-- VoiceCode requiere CRC-16 (no práctico en T-SQL) — este SP solo expone las columnas crudas
-- (GTIN/CodigoTrazabilidad/FechaLote) para que el caller en C# calcule Low/High con
-- FrontOne.Shared.Utils.VoicePickCodeCalculator (misma lógica que usa la app en captura normal) y
-- los guarde uno por uno con sp_PalletDetalle_ActualizarVoiceCode, ya existente.
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_RecalcularGs1128Masivo
    @PalletId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE d
    SET d.CodigoGs1128 = CASE
        WHEN pt.CodigoGtin IS NOT NULL AND l.CodigoTrazabilidad IS NOT NULL
        THEN '(01)' + pt.CodigoGtin + '(13)' + FORMAT(l.Fecha, 'yyMMdd') + '(10)' + l.CodigoTrazabilidad
        ELSE NULL
    END
    FROM Produccion.PalletDetalle d
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    WHERE @PalletId IS NULL OR d.PalletId = @PalletId;

    SELECT @@ROWCOUNT AS FilasActualizadas;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_ObtenerParaRecalcularVoiceCode
    @PalletId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.Id,
        pt.CodigoGtin,
        l.CodigoTrazabilidad,
        l.Fecha AS FechaLote
    FROM Produccion.PalletDetalle d
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    WHERE pt.CodigoGtin IS NOT NULL AND l.CodigoTrazabilidad IS NOT NULL
        AND (@PalletId IS NULL OR d.PalletId = @PalletId)
    ORDER BY d.Id;
END
GO
