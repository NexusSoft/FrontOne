USE FrontOne;
GO

-- Solo aparecen Lotes cuya Corrida ya está Finalizada (Estatus = 2) — antes de eso no se
-- puede costear (regla dura del módulo Gastos). Mismo criterio OUTER APPLY que
-- Produccion.sp_Corrida_Obtener para resolver Huerta/Productor de la primera Recepción.
CREATE OR ALTER PROCEDURE Gastos.sp_GastoLote_ObtenerLotesCosteables
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Id AS LoteId, l.Folio, l.Fecha, l.Kilogramos,
        primera.HuertaNombre, primera.ProductorNombre
    FROM Lotes.Lote l
    INNER JOIN Produccion.Corrida c ON c.LoteId = l.Id AND c.Estatus = 2
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, pr.NombreProductor AS ProductorNombre
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    ORDER BY c.FechaHoraFin DESC;
END
GO

-- Encabezado de Gastos: todo lo que no es explícitamente capturado por el usuario (vigencia de
-- Costo Estimado) se deriva en vivo de Lote/Corrida/OrdenCorte/AcuerdoCorte de la primera
-- Recepción del Lote, mismo criterio que Produccion.sp_Corrida_Obtener. La fila de
-- Gastos.GastoLote se trae con LEFT JOIN porque puede no existir todavía (se crea con
-- sp_GastoLote_ObtenerOCrear la primera vez que se abre el Lote).
CREATE OR ALTER PROCEDURE Gastos.sp_GastoLote_ObtenerEncabezado
    @LoteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Id AS LoteId, l.Folio AS LoteFolio, ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad,
        c.FechaHoraFin AS FechaCorrida, l.Kilogramos,
        primera.HuertaNombre, primera.RegistroSagarpa, primera.VariedadId, primera.VariedadNombre,
        primera.TipoCorteNombre, primera.TipoPagoNombre, primera.NecesitaListaPrecios,
        primera.Precio, primera.ListaPrecioFecha, primera.ListaPrecioProductorNombre, primera.ListaPrecioNumero,
        gl.Id AS GastoLoteId,
        gl.CostoEstimadoListaPrecioFecha, gl.CostoEstimadoListaPrecioProductorId,
        cep.NombreProductor AS CostoEstimadoListaPrecioProductorNombre, gl.CostoEstimadoListaPrecioNumero
    FROM Lotes.Lote l
    INNER JOIN Produccion.Corrida c ON c.LoteId = l.Id
    LEFT JOIN Gastos.GastoLote gl ON gl.LoteId = l.Id
    LEFT JOIN Catalogos.Productor cep ON cep.Id = gl.CostoEstimadoListaPrecioProductorId
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, h.RegistroSagarpa, oc.VariedadId, v.Nombre AS VariedadNombre,
            tc.Nombre AS TipoCorteNombre, tp.Nombre AS TipoPagoNombre, tp.NecesitaListaPrecios,
            ac.Precio, ac.ListaPrecioFecha, lpp.NombreProductor AS ListaPrecioProductorNombre, ac.ListaPrecioNumero
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Acopio.Variedad v ON v.Id = oc.VariedadId
        INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
        INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
        LEFT JOIN Catalogos.Productor lpp ON lpp.Id = ac.ListaPrecioProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    WHERE l.Id = @LoteId;
END
GO

-- Se llama al entrar por primera vez a un Lote en Gastos: si no existe el encabezado
-- Gastos.GastoLote todavía, lo crea en blanco (sin vigencia de Costo Estimado elegida).
CREATE OR ALTER PROCEDURE Gastos.sp_GastoLote_ObtenerOCrear
    @LoteId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Gastos.GastoLote WHERE LoteId = @LoteId)
    BEGIN
        INSERT INTO Gastos.GastoLote (LoteId) VALUES (@LoteId);
    END

    SELECT Id FROM Gastos.GastoLote WHERE LoteId = @LoteId;
END
GO

-- Fila cruda de Gastos.GastoLote (sin joins), usada por GastoLoteService para armar el JSON de
-- auditoría antes/después de ActualizarVigenciaEstimadoAsync.
CREATE OR ALTER PROCEDURE Gastos.sp_GastoLote_ObtenerPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LoteId, CostoEstimadoListaPrecioFecha, CostoEstimadoListaPrecioProductorId,
           CostoEstimadoListaPrecioNumero, FechaCreacion
    FROM Gastos.GastoLote
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Gastos.sp_GastoLote_ActualizarVigenciaEstimado
    @Id                                  INT,
    @CostoEstimadoListaPrecioFecha       DATE = NULL,
    @CostoEstimadoListaPrecioProductorId INT = NULL,
    @CostoEstimadoListaPrecioNumero      TINYINT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Gastos.GastoLote
    SET CostoEstimadoListaPrecioFecha = @CostoEstimadoListaPrecioFecha,
        CostoEstimadoListaPrecioProductorId = @CostoEstimadoListaPrecioProductorId,
        CostoEstimadoListaPrecioNumero = @CostoEstimadoListaPrecioNumero
    WHERE Id = @Id;
END
GO
