USE FrontOne;
GO

-- Navegación del catálogo completo de JefeAcopio (botones Inicio/Anterior/Siguiente/Fin
-- en JefeAcopioEditarForm) por orden de creación (Id, ya es la PK/clustered index).
-- Mismo criterio que Catalogos.sp_Productor_Obtener{Primero,Ultimo,Siguiente,Anterior}.

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_ObtenerPrimero
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Clave, Nombre, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Telefono, Celular, Email, Observaciones, Activo
    FROM Acopio.JefeAcopio
    ORDER BY Id ASC;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_ObtenerUltimo
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Clave, Nombre, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Telefono, Celular, Email, Observaciones, Activo
    FROM Acopio.JefeAcopio
    ORDER BY Id DESC;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_ObtenerSiguiente
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Clave, Nombre, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Telefono, Celular, Email, Observaciones, Activo
    FROM Acopio.JefeAcopio
    WHERE Id > @Id
    ORDER BY Id ASC;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_ObtenerAnterior
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id, Clave, Nombre, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Telefono, Celular, Email, Observaciones, Activo
    FROM Acopio.JefeAcopio
    WHERE Id < @Id
    ORDER BY Id DESC;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_Buscar
    @Filtro NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 500
        Id, Clave, Nombre, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Telefono, Celular, Email, Observaciones, Activo
    FROM Acopio.JefeAcopio
    WHERE Clave LIKE '%' + @Filtro + '%'
       OR Nombre LIKE '%' + @Filtro + '%'
    ORDER BY Nombre;
END
GO
