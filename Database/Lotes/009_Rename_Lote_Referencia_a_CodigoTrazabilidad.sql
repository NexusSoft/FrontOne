USE FrontOne;
GO

-- Renombra Lotes.Lote.Referencia a CodigoTrazabilidad (nombre nuevo elegido por el usuario, ver
-- contexto/lotes.md) — el ancho de la columna ya se amplió a 16 en 008_Alter_Lote_Referencia_Huerta.sql,
-- este script solo cambia el nombre de la columna y de su constraint UNIQUE. Los datos existentes
-- no se tocan, sp_rename es un rename in-place.
IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('Lotes.Lote') AND name = 'Referencia'
)
BEGIN
    EXEC sp_rename 'Lotes.Lote.Referencia', 'CodigoTrazabilidad', 'COLUMN';
END
GO

IF EXISTS (
    SELECT 1 FROM sys.objects
    WHERE object_id = OBJECT_ID('Lotes.UQ_Lotes_Lote_Referencia') AND type = 'UQ'
)
BEGIN
    EXEC sp_rename 'Lotes.UQ_Lotes_Lote_Referencia', 'UQ_Lotes_Lote_CodigoTrazabilidad', 'OBJECT';
END
GO
