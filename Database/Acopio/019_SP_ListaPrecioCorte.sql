USE FrontOne;
GO

-- La tabla tiene un índice filtrado (CardCode único cuando no es NULL): cualquier SP que
-- inserte/actualice sobre ella debe compilarse con QUOTED_IDENTIFIER ON (si no, SQL Server
-- rechaza el INSERT/UPDATE en tiempo de ejecución con el error 1934, aunque el SP se haya
-- creado sin error).
SET QUOTED_IDENTIFIER ON;
GO

-- Trae todos los renglones, activos e inactivos — la sincronización con SAP necesita ver
-- también los inactivos para detectar reactivaciones. El filtro "solo activos" para el grid
-- se aplica en Application (ListaPrecioCorteService.ObtenerAsync).
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioCorte_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        Id,
        CardCode,
        CardName,
        PrecioKg,
        PrecioDia,
        CuadrillaApoyo,
        Activo
    FROM Acopio.ListaPrecioCorte
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY CardName;
END
GO

-- Solo la usa la sincronización con SAP (proveedores nuevos del grupo Cosecha) — nunca se
-- captura un CardCode/CardName a mano desde el grid.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioCorte_Insertar
    @CardCode       NVARCHAR(20),
    @CardName       NVARCHAR(200),
    @PrecioKg       DECIMAL(18,2) = 0,
    @PrecioDia      DECIMAL(18,2) = 0,
    @CuadrillaApoyo DECIMAL(18,2) = 0,
    @Activo         BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.ListaPrecioCorte (CardCode, CardName, PrecioKg, PrecioDia, CuadrillaApoyo, Activo)
    VALUES (@CardCode, @CardName, @PrecioKg, @PrecioDia, @CuadrillaApoyo, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- CardCode/CardName/Activo no se tocan aquí (vienen de SAP) — solo los precios, que es lo
-- único que se captura desde FrontOne.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioCorte_Actualizar
    @Id             INT,
    @PrecioKg       DECIMAL(18,2),
    @PrecioDia      DECIMAL(18,2),
    @CuadrillaApoyo DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.ListaPrecioCorte
    SET PrecioKg = @PrecioKg,
        PrecioDia = @PrecioDia,
        CuadrillaApoyo = @CuadrillaApoyo
    WHERE Id = @Id;
END
GO

-- Solo la usa la sincronización con SAP, cuando el CardName de un proveedor ya capturado
-- cambió — nunca toca los precios.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioCorte_ActualizarNombre
    @Id       INT,
    @CardName NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.ListaPrecioCorte
    SET CardName = @CardName
    WHERE Id = @Id;
END
GO

-- Solo la usa la sincronización con SAP: si el proveedor se deshabilita en SAP, el renglón
-- se marca inactivo (deja de mostrarse en el grid, sin perder el precio capturado); si se
-- vuelve a habilitar en SAP, se reactiva.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioCorte_ActualizarActivo
    @Id     INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.ListaPrecioCorte
    SET Activo = @Activo
    WHERE Id = @Id;
END
GO
