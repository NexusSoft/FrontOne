USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- =============================================================================
-- Rendimiento con catálogo grande (54k huertas / 33k productores):
--   - sp_Huerta_Obtener gana @ProductorId para filtrar server-side (pestaña
--     Huertas de ProductorEditarForm ya no baja todo el catálogo por navegación).
--   - sp_Huerta_Buscar / sp_Productor_Buscar: búsqueda por texto con TOP 500,
--     para los pickers (HuertasForm / ProductoresForm) que antes cargaban todo.
-- =============================================================================

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_Obtener
    @Id          INT = NULL,
    @ProductorId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id, Nombre, ProductorId, Ubicacion, PoblacionId, Municipio, EstadoId, ProductoId,
        EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
        RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
        SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
        Latitud, Longitud,
        Activo,
        CASE WHEN FechaCambioStatus IS NULL THEN NULL ELSE DATEDIFF(DAY, FechaCambioStatus, GETDATE()) / 365.0 END AS AniosEnStatusActual,
        CASE WHEN FechaVencimiento IS NULL THEN NULL ELSE DATEDIFF(DAY, GETDATE(), FechaVencimiento) END AS DiasParaVencimiento
    FROM Catalogos.Huerta
    WHERE (@Id IS NULL OR Id = @Id)
      AND (@ProductorId IS NULL OR ProductorId = @ProductorId)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_Buscar
    @Filtro NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 500
        h.Id,
        h.Nombre,
        h.RegistroSagarpa,
        h.ProductorId,
        p.NombreProductor,
        h.Activo
    FROM Catalogos.Huerta h
    INNER JOIN Catalogos.Productor p ON p.Id = h.ProductorId
    WHERE h.Nombre LIKE '%' + @Filtro + '%'
       OR h.RegistroSagarpa LIKE '%' + @Filtro + '%'
       OR p.NombreProductor LIKE '%' + @Filtro + '%'
    ORDER BY h.Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_Buscar
    @Filtro NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 500
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, Municipio, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    WHERE Clave LIKE '%' + @Filtro + '%'
       OR NombreProductor LIKE '%' + @Filtro + '%'
    ORDER BY NombreProductor;
END
GO
