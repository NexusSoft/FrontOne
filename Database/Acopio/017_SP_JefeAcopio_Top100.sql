-- =============================================================================
-- Carga inicial (Top 100) para el buscador embebido de Jefe de Acopio.
-- No reemplaza sp_JefeAcopio_Buscar (búsqueda por texto) — se usa solo al
-- abrir el picker, para que el grid no se vea vacío desde un inicio.
-- =============================================================================

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_ObtenerTop100
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        Id, Clave, Nombre, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Telefono, Celular, Email,
        Observaciones, Activo
    FROM Acopio.JefeAcopio
    ORDER BY Nombre;
END
GO
