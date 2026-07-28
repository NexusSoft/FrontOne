USE FrontOne;
GO

-- Detalle del grid de Recepción: cada renglón es una Orden de Corte que trae el camión.
-- "CajasCortadas" del grid se toma de Acopio.OrdenCorte.CajasEntregadas (esa orden no tiene un
-- campo separado de "cajas cortadas") — se trae por join, no se duplica en la tabla de detalle.
CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFrutaOrdenCorte_Obtener
    @RecepcionFrutaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        det.Id, det.RecepcionFrutaId, det.OrdenCorteId,
        oc.Folio AS OrdenCorteFolio, h.Nombre AS HuertaNombre, oc.CajasEntregadas AS CajasCortadas,
        det.Kilogramos
    FROM Recepcion.RecepcionFrutaOrdenCorte det
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = det.OrdenCorteId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    WHERE det.RecepcionFrutaId = @RecepcionFrutaId
    ORDER BY det.Id;
END
GO

CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFrutaOrdenCorte_Insertar
    @RecepcionFrutaId  INT,
    @OrdenCorteId       INT,
    @Kilogramos         DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Recepcion.RecepcionFrutaOrdenCorte (RecepcionFrutaId, OrdenCorteId, Kilogramos)
    VALUES (@RecepcionFrutaId, @OrdenCorteId, @Kilogramos);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- OrdenCorteId es inmutable una vez creada la línea (mismo criterio que otros campos "de
-- origen" del proyecto) — solo Kilogramos es editable.
CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFrutaOrdenCorte_Actualizar
    @Id             INT,
    @Kilogramos     DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Recepcion.RecepcionFrutaOrdenCorte
    SET Kilogramos = @Kilogramos
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Recepcion.sp_RecepcionFrutaOrdenCorte_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Recepcion.RecepcionFrutaOrdenCorte WHERE Id = @Id;
END
GO
