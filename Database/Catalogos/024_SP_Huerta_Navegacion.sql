USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- =============================================================================
-- Navegación del catálogo completo de Huerta (botones Inicio/Anterior/Siguiente/Fin
-- en HuertaEditarForm) por orden de creación (Id, que ya es la PK/clustered index —
-- no hace falta índice nuevo). Cada SP hace un seek puntual, nunca un recorrido
-- completo, así que el costo es el mismo con 54k filas que con 500k.
-- =============================================================================

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_ObtenerPrimero
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Nombre, ProductorId, Ubicacion, PoblacionId, MunicipioId, EstadoId, ProductoId,
        EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
        RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
        SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
        Latitud, Longitud,
        Activo,
        CASE WHEN FechaCambioStatus IS NULL THEN NULL ELSE DATEDIFF(DAY, FechaCambioStatus, GETDATE()) / 365.0 END AS AniosEnStatusActual,
        CASE WHEN FechaVencimiento IS NULL THEN NULL ELSE DATEDIFF(DAY, GETDATE(), FechaVencimiento) END AS DiasParaVencimiento
    FROM Catalogos.Huerta
    ORDER BY Id ASC;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_ObtenerUltimo
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Nombre, ProductorId, Ubicacion, PoblacionId, MunicipioId, EstadoId, ProductoId,
        EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
        RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
        SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
        Latitud, Longitud,
        Activo,
        CASE WHEN FechaCambioStatus IS NULL THEN NULL ELSE DATEDIFF(DAY, FechaCambioStatus, GETDATE()) / 365.0 END AS AniosEnStatusActual,
        CASE WHEN FechaVencimiento IS NULL THEN NULL ELSE DATEDIFF(DAY, GETDATE(), FechaVencimiento) END AS DiasParaVencimiento
    FROM Catalogos.Huerta
    ORDER BY Id DESC;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_ObtenerSiguiente
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Nombre, ProductorId, Ubicacion, PoblacionId, MunicipioId, EstadoId, ProductoId,
        EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
        RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
        SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
        Latitud, Longitud,
        Activo,
        CASE WHEN FechaCambioStatus IS NULL THEN NULL ELSE DATEDIFF(DAY, FechaCambioStatus, GETDATE()) / 365.0 END AS AniosEnStatusActual,
        CASE WHEN FechaVencimiento IS NULL THEN NULL ELSE DATEDIFF(DAY, GETDATE(), FechaVencimiento) END AS DiasParaVencimiento
    FROM Catalogos.Huerta
    WHERE Id > @Id
    ORDER BY Id ASC;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_ObtenerAnterior
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Nombre, ProductorId, Ubicacion, PoblacionId, MunicipioId, EstadoId, ProductoId,
        EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
        RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
        SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
        Latitud, Longitud,
        Activo,
        CASE WHEN FechaCambioStatus IS NULL THEN NULL ELSE DATEDIFF(DAY, FechaCambioStatus, GETDATE()) / 365.0 END AS AniosEnStatusActual,
        CASE WHEN FechaVencimiento IS NULL THEN NULL ELSE DATEDIFF(DAY, GETDATE(), FechaVencimiento) END AS DiasParaVencimiento
    FROM Catalogos.Huerta
    WHERE Id < @Id
    ORDER BY Id DESC;
END
GO
