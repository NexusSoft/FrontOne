USE FrontOne;
GO

-- Grid principal del módulo Incidencias: TODAS las Órdenes de Corte del rango de fecha elegido,
-- tengan o no Incidencia capturada ya (Revisada distingue ambos casos).
CREATE OR ALTER PROCEDURE Acopio.sp_Incidencia_ObtenerOrdenesConEstatus
    @FechaDesde DATE,
    @FechaHasta DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        oc.Id AS OrdenCorteId,
        inc.Id AS IncidenciaId,
        CAST(CASE WHEN inc.Id IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS Revisada,
        oc.Folio,
        oc.Fecha,
        h.Nombre AS HuertaNombre,
        pr.NombreProductor AS ProductorNombre,
        oc.JefeAcopioNombre AS Acopiador,
        sup.Nombre AS SupervisorNombre,
        oc.CajasEntregadas AS Cajas,
        inc.Incidencias,
        inc.Ajuste
    FROM Acopio.OrdenCorte oc
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    INNER JOIN Catalogos.Productor pr ON pr.Id = oc.ProductorId
    LEFT JOIN Acopio.Incidencia inc ON inc.OrdenCorteId = oc.Id
    LEFT JOIN Acopio.SupervisorHuerta sup ON sup.Id = inc.SupervisorHuertaId
    WHERE oc.Fecha BETWEEN @FechaDesde AND @FechaHasta
    ORDER BY oc.Fecha DESC, oc.Folio DESC;
END
GO

-- Usado al abrir el formulario de captura: siempre regresa una fila con los campos derivados de
-- la Orden de Corte (Municipio/Población incluidos, vía Huerta) resueltos, aunque la Incidencia
-- todavía no exista para esa orden (los campos propios de Incidencia llegan NULL en ese caso).
CREATE OR ALTER PROCEDURE Acopio.sp_Incidencia_ObtenerPorOrdenCorteId
    @OrdenCorteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        inc.Id,
        oc.Id AS OrdenCorteId,
        oc.Folio,
        oc.Fecha,
        oc.JefeAcopioNombre AS Acopiador,
        oc.RegistroSagarpa,
        h.Nombre AS HuertaNombre,
        oc.CajasEntregadas,
        pr.NombreProductor AS ProductorNombre,
        oc.PagarCorteANombre AS Beneficiario,
        tc.Nombre AS TipoCorteNombre,
        tp.Nombre AS TipoPagoNombre,
        oc.CostoKg,
        oc.JefeCuadrillaNombre,
        oc.TransportistaNombre,
        fl.Floracion AS FloracionNombre,
        mun.Nombre AS MunicipioNombre,
        pob.Nombre AS PoblacionNombre,
        inc.SupervisorHuertaId,
        sup.Nombre AS SupervisorHuertaNombre,
        inc.OdcSapCosecha,
        inc.NumeroTelefono,
        inc.Placas,
        inc.OdcSapFlete,
        inc.Bascula,
        inc.PuntoReunion,
        inc.HoraLlegadaHuerta,
        inc.CajasCosechadas,
        inc.CajaPorCuadrilla,
        inc.Observaciones,
        inc.Incidencias,
        inc.Ajuste
    FROM Acopio.OrdenCorte oc
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    INNER JOIN Catalogos.Productor pr ON pr.Id = oc.ProductorId
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
    INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
    INNER JOIN Acopio.Floracion fl ON fl.Id = oc.FloracionId
    LEFT JOIN Catalogos.Municipio mun ON mun.Id = h.MunicipioId
    LEFT JOIN Catalogos.Poblacion pob ON pob.Id = h.PoblacionId
    LEFT JOIN Acopio.Incidencia inc ON inc.OrdenCorteId = oc.Id
    LEFT JOIN Acopio.SupervisorHuerta sup ON sup.Id = inc.SupervisorHuertaId
    WHERE oc.Id = @OrdenCorteId;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Incidencia_Insertar
    @OrdenCorteId       INT,
    @SupervisorHuertaId INT = NULL,
    @OdcSapCosecha      NVARCHAR(20) = NULL,
    @NumeroTelefono     NVARCHAR(20) = NULL,
    @Placas             NVARCHAR(20) = NULL,
    @OdcSapFlete        NVARCHAR(20) = NULL,
    @Bascula            NVARCHAR(100) = NULL,
    @PuntoReunion       NVARCHAR(200) = NULL,
    @HoraLlegadaHuerta  TIME = NULL,
    @CajasCosechadas    INT = NULL,
    @CajaPorCuadrilla   NVARCHAR(100) = NULL,
    @Observaciones      NVARCHAR(MAX) = NULL,
    @Incidencias        NVARCHAR(MAX) = NULL,
    @Ajuste             NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Acopio.Incidencia
        (OrdenCorteId, SupervisorHuertaId, OdcSapCosecha, NumeroTelefono, Placas, OdcSapFlete, Bascula,
         PuntoReunion, HoraLlegadaHuerta, CajasCosechadas, CajaPorCuadrilla, Observaciones, Incidencias, Ajuste)
    VALUES
        (@OrdenCorteId, @SupervisorHuertaId, @OdcSapCosecha, @NumeroTelefono, @Placas, @OdcSapFlete, @Bascula,
         @PuntoReunion, @HoraLlegadaHuerta, @CajasCosechadas, @CajaPorCuadrilla, @Observaciones, @Incidencias, @Ajuste);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Incidencia_Actualizar
    @Id                 INT,
    @SupervisorHuertaId INT = NULL,
    @OdcSapCosecha      NVARCHAR(20) = NULL,
    @NumeroTelefono     NVARCHAR(20) = NULL,
    @Placas             NVARCHAR(20) = NULL,
    @OdcSapFlete        NVARCHAR(20) = NULL,
    @Bascula            NVARCHAR(100) = NULL,
    @PuntoReunion       NVARCHAR(200) = NULL,
    @HoraLlegadaHuerta  TIME = NULL,
    @CajasCosechadas    INT = NULL,
    @CajaPorCuadrilla   NVARCHAR(100) = NULL,
    @Observaciones      NVARCHAR(MAX) = NULL,
    @Incidencias        NVARCHAR(MAX) = NULL,
    @Ajuste             NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Acopio.Incidencia
    SET SupervisorHuertaId = @SupervisorHuertaId,
        OdcSapCosecha = @OdcSapCosecha,
        NumeroTelefono = @NumeroTelefono,
        Placas = @Placas,
        OdcSapFlete = @OdcSapFlete,
        Bascula = @Bascula,
        PuntoReunion = @PuntoReunion,
        HoraLlegadaHuerta = @HoraLlegadaHuerta,
        CajasCosechadas = @CajasCosechadas,
        CajaPorCuadrilla = @CajaPorCuadrilla,
        Observaciones = @Observaciones,
        Incidencias = @Incidencias,
        Ajuste = @Ajuste
    WHERE Id = @Id;
END
GO

-- Fuente de datos del PDF de Incidencias del rango de fecha elegido — solo Órdenes de Corte que
-- ya tienen Incidencia capturada.
CREATE OR ALTER PROCEDURE Acopio.sp_Incidencia_ObtenerParaReporte
    @FechaDesde DATE,
    @FechaHasta DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        oc.Folio,
        oc.Fecha,
        oc.JefeAcopioNombre AS Acopiador,
        oc.RegistroSagarpa,
        h.Nombre AS HuertaNombre,
        oc.CajasEntregadas,
        pr.NombreProductor AS ProductorNombre,
        oc.PagarCorteANombre AS Beneficiario,
        tc.Nombre AS TipoCorteNombre,
        tp.Nombre AS TipoPagoNombre,
        oc.CostoKg,
        oc.JefeCuadrillaNombre,
        oc.TransportistaNombre,
        fl.Floracion AS FloracionNombre,
        mun.Nombre AS MunicipioNombre,
        pob.Nombre AS PoblacionNombre,
        sup.Nombre AS SupervisorHuertaNombre,
        inc.OdcSapCosecha,
        inc.NumeroTelefono,
        inc.Placas,
        inc.OdcSapFlete,
        inc.Bascula,
        inc.PuntoReunion,
        inc.HoraLlegadaHuerta,
        inc.CajasCosechadas,
        inc.CajaPorCuadrilla,
        inc.Observaciones,
        inc.Incidencias,
        inc.Ajuste
    FROM Acopio.Incidencia inc
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = inc.OrdenCorteId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    INNER JOIN Catalogos.Productor pr ON pr.Id = oc.ProductorId
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
    INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
    INNER JOIN Acopio.Floracion fl ON fl.Id = oc.FloracionId
    LEFT JOIN Catalogos.Municipio mun ON mun.Id = h.MunicipioId
    LEFT JOIN Catalogos.Poblacion pob ON pob.Id = h.PoblacionId
    LEFT JOIN Acopio.SupervisorHuerta sup ON sup.Id = inc.SupervisorHuertaId
    WHERE oc.Fecha BETWEEN @FechaDesde AND @FechaHasta
    ORDER BY oc.Fecha, oc.Folio;
END
GO
