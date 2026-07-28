USE FrontOne;
GO

-- Viven en el schema Acopio (no Recepcion) porque consultan Acopio.OrdenCorte — mismo criterio
-- ya usado en el proyecto para SPs de un módulo que sirven a otro (ej.
-- sp_ListaPrecioFruta_ObtenerFechasPorProductorYRango, usado desde AcuerdoCorte).
-- Alimentan el picker "Seleccionar Orden de Corte" del grid de Recepción de Fruta: solo deben
-- regresar Órdenes de Corte que NO estén ya usadas en otra Recepción (Recepcion.RecepcionFrutaOrdenCorte
-- tiene UNIQUE en OrdenCorteId — una orden solo puede recibirse una vez).

-- Carga inicial (Top 100) al abrir el picker, mismo patrón que sp_JefeAcopio_ObtenerTop100.
CREATE OR ALTER PROCEDURE Acopio.sp_OrdenCorte_ObtenerTop100ParaRecepcion
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        oc.Id, oc.Folio, oc.Fecha, h.Nombre AS HuertaNombre, oc.CajasEntregadas
    FROM Acopio.OrdenCorte oc
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    WHERE NOT EXISTS (
        SELECT 1 FROM Recepcion.RecepcionFrutaOrdenCorte det WHERE det.OrdenCorteId = oc.Id
    )
    ORDER BY oc.FechaCreacion DESC;
END
GO

-- Búsqueda por texto (Folio o Huerta), mismo patrón que sp_JefeAcopio_Buscar.
CREATE OR ALTER PROCEDURE Acopio.sp_OrdenCorte_BuscarParaRecepcion
    @Filtro NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 500
        oc.Id, oc.Folio, oc.Fecha, h.Nombre AS HuertaNombre, oc.CajasEntregadas
    FROM Acopio.OrdenCorte oc
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    WHERE NOT EXISTS (
            SELECT 1 FROM Recepcion.RecepcionFrutaOrdenCorte det WHERE det.OrdenCorteId = oc.Id
          )
      AND (oc.Folio LIKE '%' + @Filtro + '%' OR h.Nombre LIKE '%' + @Filtro + '%')
    ORDER BY oc.FechaCreacion DESC;
END
GO

-- Validación explícita antes de insertar el detalle (Application la usa para dar un mensaje
-- claro) — respaldada de todos modos por el UNIQUE en Recepcion.RecepcionFrutaOrdenCorte.OrdenCorteId.
CREATE OR ALTER PROCEDURE Acopio.sp_OrdenCorte_EstaDisponibleParaRecepcion
    @OrdenCorteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM Recepcion.RecepcionFrutaOrdenCorte WHERE OrdenCorteId = @OrdenCorteId
    ) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS Disponible;
END
GO
