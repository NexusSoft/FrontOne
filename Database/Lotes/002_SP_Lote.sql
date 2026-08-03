USE FrontOne;
GO

-- sp_Lote_Obtener trae el encabezado y la columna "Recepciones" con el NÚMERO de Recepciones que
-- componen el Lote (COUNT, no la lista de folios de ticket — pedido explícito del usuario tras
-- probar en la app real, ver contexto/lotes.md). HuertaNombre/ProductorNombre se toman de la
-- Orden de Corte de la primera Recepción del Lote (CROSS APPLY TOP 1) — son iguales en todas
-- las líneas por la regla de validación de LoteService.AgregarLineaAsync.
CREATE OR ALTER PROCEDURE Lotes.sp_Lote_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Id, l.Folio, l.Fecha, ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad, l.Observaciones, l.Kilogramos, l.Personalizado,
        l.LineaProduccionId, lp.Nombre AS LineaProduccionNombre,
        l.PorcentajeMateriaSeca, l.Estatus, l.FechaCreacion,
        (
            SELECT COUNT(*)
            FROM Lotes.LoteRecepcion det
            WHERE det.LoteId = l.Id
        ) AS Recepciones,
        primera.HuertaNombre,
        primera.ProductorNombre
    FROM Lotes.Lote l
    INNER JOIN Catalogos.LineaProduccion lp ON lp.Id = l.LineaProduccionId
    OUTER APPLY (
        SELECT TOP 1 h.Nombre AS HuertaNombre, pr.NombreProductor AS ProductorNombre
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    WHERE (@Id IS NULL OR l.Id = @Id)
    ORDER BY l.FechaCreacion DESC;
END
GO

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

    DECLARE @Folio NVARCHAR(7) = RIGHT('0000000' + CAST(NEXT VALUE FOR Lotes.SeqLoteFolio AS VARCHAR(7)), 7);

    INSERT INTO Lotes.Lote
        (Folio, Fecha, CodigoTrazabilidad, Observaciones, Kilogramos, Personalizado, LineaProduccionId,
         PorcentajeMateriaSeca, Estatus)
    VALUES
        (@Folio, @Fecha, @CodigoTrazabilidad, @Observaciones, @Kilogramos, @Personalizado, @LineaProduccionId,
         @PorcentajeMateriaSeca, @Estatus);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO

CREATE OR ALTER PROCEDURE Lotes.sp_Lote_Actualizar
    @Id                     INT,
    @Fecha                  DATE,
    @CodigoTrazabilidad     NVARCHAR(16),
    @Observaciones          NVARCHAR(500) = NULL,
    @Kilogramos             DECIMAL(18,2),
    @Personalizado          NVARCHAR(200) = NULL,
    @LineaProduccionId      INT,
    @PorcentajeMateriaSeca  DECIMAL(5,2),
    @Estatus                TINYINT = 0
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Lotes.Lote
    SET Fecha = @Fecha,
        CodigoTrazabilidad = @CodigoTrazabilidad,
        Observaciones = @Observaciones,
        Kilogramos = @Kilogramos,
        Personalizado = @Personalizado,
        LineaProduccionId = @LineaProduccionId,
        PorcentajeMateriaSeca = @PorcentajeMateriaSeca,
        Estatus = @Estatus
    WHERE Id = @Id;
END
GO

-- Borra primero el detalle y luego el encabezado dentro del mismo SP (mismo criterio que
-- Recepcion.sp_RecepcionFruta_Eliminar) — libera las Recepciones de vuelta para poder usarse
-- en otro Lote o volver a editarse, y limpia el "No. de Lote" que se les había puesto al
-- agregarlas (ver 006_SP_RecepcionFruta_ActualizarNoLote.sql).
CREATE OR ALTER PROCEDURE Lotes.sp_Lote_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE rf
    SET rf.NoLote = NULL
    FROM Recepcion.RecepcionFruta rf
    INNER JOIN Lotes.LoteRecepcion det ON det.RecepcionFrutaId = rf.Id
    WHERE det.LoteId = @Id;

    DELETE FROM Lotes.LoteRecepcion WHERE LoteId = @Id;
    DELETE FROM Lotes.Lote WHERE Id = @Id;
END
GO
