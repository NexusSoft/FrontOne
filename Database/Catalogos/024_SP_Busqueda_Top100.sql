-- =============================================================================
-- Carga inicial (Top 100) para los buscadores embebidos de Productor y Huerta.
-- No reemplaza sp_Productor_Buscar / sp_Huerta_Buscar (búsqueda por texto,
-- TOP 500) — se usa solo al abrir el picker, para que el grid no se vea
-- vacío desde un inicio.
-- =============================================================================

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_ObtenerTop100
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    ORDER BY NombreProductor;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_ObtenerTop100
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        h.Id,
        h.Nombre,
        h.RegistroSagarpa,
        h.ProductorId,
        p.NombreProductor,
        h.Activo
    FROM Catalogos.Huerta h
    INNER JOIN Catalogos.Productor p ON p.Id = h.ProductorId
    ORDER BY h.Nombre;
END
GO
