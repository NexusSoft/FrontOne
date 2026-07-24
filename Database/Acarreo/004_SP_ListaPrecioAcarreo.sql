USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_ListaPrecioAcarreo_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        l.Id,
        l.MunicipioId,
        m.Nombre AS MunicipioNombre,
        est.Nombre AS EstadoNombre,
        l.ZonaId,
        z.Zona AS ZonaNombre,
        l.Precio300,
        l.Precio400,
        l.Precio500,
        l.ToleranciaKgFaltante,
        l.Activo
    FROM Acarreo.ListaPrecioAcarreo l
    INNER JOIN Catalogos.Municipio m ON m.Id = l.MunicipioId
    INNER JOIN Catalogos.Estado est ON est.Id = m.EstadoId
    INNER JOIN Acarreo.Zona z ON z.Id = l.ZonaId
    WHERE (@Id IS NULL OR l.Id = @Id)
    ORDER BY l.ZonaId, m.Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_ListaPrecioAcarreo_Insertar
    @MunicipioId INT,
    @ZonaId INT,
    @Precio300 DECIMAL(18,2),
    @Precio400 DECIMAL(18,2),
    @Precio500 DECIMAL(18,2),
    @ToleranciaKgFaltante INT,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acarreo.ListaPrecioAcarreo (MunicipioId, ZonaId, Precio300, Precio400, Precio500, ToleranciaKgFaltante, Activo)
    VALUES (@MunicipioId, @ZonaId, @Precio300, @Precio400, @Precio500, @ToleranciaKgFaltante, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_ListaPrecioAcarreo_Actualizar
    @Id INT,
    @MunicipioId INT,
    @ZonaId INT,
    @Precio300 DECIMAL(18,2),
    @Precio400 DECIMAL(18,2),
    @Precio500 DECIMAL(18,2),
    @ToleranciaKgFaltante INT,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acarreo.ListaPrecioAcarreo
    SET MunicipioId = @MunicipioId,
        ZonaId = @ZonaId,
        Precio300 = @Precio300,
        Precio400 = @Precio400,
        Precio500 = @Precio500,
        ToleranciaKgFaltante = @ToleranciaKgFaltante,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_ListaPrecioAcarreo_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acarreo.ListaPrecioAcarreo WHERE Id = @Id;
END
GO
