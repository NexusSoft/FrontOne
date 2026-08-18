USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Gastos.sp_TipoAjuste_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, TipoGasto, Signo, Activo
    FROM Gastos.TipoAjuste
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Gastos.sp_TipoAjuste_Insertar
    @Nombre    NVARCHAR(100),
    @TipoGasto TINYINT,
    @Signo     TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Gastos.TipoAjuste (Nombre, TipoGasto, Signo, Activo)
    VALUES (@Nombre, @TipoGasto, @Signo, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Gastos.sp_TipoAjuste_Actualizar
    @Id        INT,
    @Nombre    NVARCHAR(100),
    @TipoGasto TINYINT,
    @Signo     TINYINT,
    @Activo    BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Gastos.TipoAjuste
    SET Nombre = @Nombre, TipoGasto = @TipoGasto, Signo = @Signo, Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Gastos.sp_TipoAjuste_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Gastos.TipoAjuste WHERE Id = @Id;
END
GO
