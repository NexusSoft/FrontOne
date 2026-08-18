USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcionAjuste_Obtener
    @GastoLoteId INT,
    @TipoGasto   TINYINT -- 1 = Cosecha, 2 = Acarreo
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        gra.Id, gra.GastoLoteId, gra.LoteRecepcionId,
        rf.Id AS RecepcionFrutaId, rf.Folio AS RecepcionFolio, rf.PesoNeto, rf.PesoProductor,
        oc.Id AS OrdenCorteId, oc.Folio AS OrdenCorteFolio,
        gra.TipoAjusteId, ta.Nombre AS TipoAjusteNombre, ta.Signo,
        gra.Monto, gra.CargoA
    FROM Gastos.GastoRecepcionAjuste gra
    INNER JOIN Gastos.TipoAjuste ta ON ta.Id = gra.TipoAjusteId
    INNER JOIN Lotes.LoteRecepcion det ON det.Id = gra.LoteRecepcionId
    INNER JOIN Recepcion.RecepcionFruta rf ON rf.Id = det.RecepcionFrutaId
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = rf.Id
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
    WHERE gra.GastoLoteId = @GastoLoteId AND ta.TipoGasto = @TipoGasto
    ORDER BY gra.FechaCreacion;
END
GO

CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcionAjuste_Insertar
    @GastoLoteId     INT,
    @LoteRecepcionId INT,
    @TipoAjusteId    INT,
    @Monto           DECIMAL(18,2),
    @CargoA          TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Gastos.GastoRecepcionAjuste (GastoLoteId, LoteRecepcionId, TipoAjusteId, Monto, CargoA)
    VALUES (@GastoLoteId, @LoteRecepcionId, @TipoAjusteId, @Monto, @CargoA);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcionAjuste_Actualizar
    @Id           INT,
    @TipoAjusteId INT,
    @Monto        DECIMAL(18,2),
    @CargoA       TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Gastos.GastoRecepcionAjuste
    SET TipoAjusteId = @TipoAjusteId, Monto = @Monto, CargoA = @CargoA
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcionAjuste_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Gastos.GastoRecepcionAjuste WHERE Id = @Id;
END
GO
