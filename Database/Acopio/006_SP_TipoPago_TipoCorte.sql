USE FrontOne;
GO

-- TipoPago
CREATE OR ALTER PROCEDURE Acopio.sp_TipoPago_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Acopio.TipoPago WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoPago_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.TipoPago (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoPago_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.TipoPago SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoPago_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.TipoPago WHERE Id = @Id;
END
GO

-- TipoCorte
CREATE OR ALTER PROCEDURE Acopio.sp_TipoCorte_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT tc.Id, tc.Nombre, tc.FueraDeNormaGr, tc.DanioMinimo, tc.TipoPagoId, tc.Activo,
           tp.Nombre AS TipoPagoNombre
    FROM Acopio.TipoCorte tc
    INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
    WHERE (@Id IS NULL OR tc.Id = @Id)
    ORDER BY tc.Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoCorte_Insertar
    @Nombre NVARCHAR(100), @FueraDeNormaGr DECIMAL(10,2), @DanioMinimo BIT, @TipoPagoId INT, @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.TipoCorte (Nombre, FueraDeNormaGr, DanioMinimo, TipoPagoId, Activo)
    VALUES (@Nombre, @FueraDeNormaGr, @DanioMinimo, @TipoPagoId, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoCorte_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @FueraDeNormaGr DECIMAL(10,2), @DanioMinimo BIT, @TipoPagoId INT, @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.TipoCorte
    SET Nombre = @Nombre, FueraDeNormaGr = @FueraDeNormaGr, DanioMinimo = @DanioMinimo,
        TipoPagoId = @TipoPagoId, Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoCorte_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.TipoCorte WHERE Id = @Id;
END
GO
