USE FrontOne;
GO

-- Una vigencia de Acopio.ListaPrecioFruta (fecha + productor-o-general) no se puede eliminar
-- si algún Acuerdo de Corte activo la está usando como referencia de liquidación
-- (Acopio.AcuerdoCorte.ListaPrecioFecha/ListaPrecioProductorId). NULL-safe en ProductorId,
-- mismo criterio que sp_ListaPrecioFruta_ExisteTraslape/ObtenerPorFecha.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ExisteVinculoAcuerdoCorte
    @Fecha       DATE,
    @ProductorId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(CASE WHEN EXISTS (
        SELECT 1 FROM Acopio.AcuerdoCorte
        WHERE Activo = 1
          AND ListaPrecioFecha = @Fecha
          AND ((@ProductorId IS NULL AND ListaPrecioProductorId IS NULL) OR ListaPrecioProductorId = @ProductorId)
    ) THEN 1 ELSE 0 END AS BIT) AS Existe;
END
GO
