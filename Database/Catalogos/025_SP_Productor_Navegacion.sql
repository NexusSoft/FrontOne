USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- =============================================================================
-- Navegación del catálogo completo de Productor (botones Inicio/Anterior/
-- Siguiente/Fin en ProductorEditarForm) por orden de creación (Id, ya es la
-- PK/clustered index). Mismo criterio que Catalogos/024_SP_Huerta_Navegacion.sql.
-- =============================================================================

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_ObtenerPrimero
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    ORDER BY Id ASC;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_ObtenerUltimo
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    ORDER BY Id DESC;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_ObtenerSiguiente
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    WHERE Id > @Id
    ORDER BY Id ASC;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_ObtenerAnterior
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    WHERE Id < @Id
    ORDER BY Id DESC;
END
GO
