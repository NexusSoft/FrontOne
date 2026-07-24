USE FrontOne;
GO

-- Cada vigencia ahora se guarda día por día (FechaFin siempre queda NULL), así que la
-- búsqueda ya no lista fila por fila: lista fechas únicas y desde ahí se entra al detalle.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerFechas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT FechaInicio
    FROM Acopio.ListaPrecioFruta
    WHERE Activo = 1
    ORDER BY FechaInicio DESC;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerPorFecha
    @Fecha DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, ItemCode, ItemName, Lista1, Lista2, Lista3, FechaInicio, FechaFin, Activo, FechaCreacion
    FROM Acopio.ListaPrecioFruta
    WHERE FechaInicio = @Fecha
      AND Activo = 1
    ORDER BY ItemCode;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_EliminarPorFecha
    @Fecha DATE
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Acopio.ListaPrecioFruta WHERE FechaInicio = @Fecha;
END
GO
