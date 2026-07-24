USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id, Clave, Nombre, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Telefono, Celular, Email, Observaciones, Activo
    FROM Acopio.JefeAcopio
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Clave;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_Insertar
    @Nombre         NVARCHAR(200),
    @Domicilio      NVARCHAR(200) = NULL,
    @Colonia        NVARCHAR(100) = NULL,
    @CodigoPostal   NVARCHAR(10)  = NULL,
    @PoblacionId    INT           = NULL,
    @MunicipioId    INT           = NULL,
    @EstadoId       INT           = NULL,
    @Telefono       NVARCHAR(30)  = NULL,
    @Celular        NVARCHAR(30)  = NULL,
    @Email          NVARCHAR(150) = NULL,
    @Observaciones  NVARCHAR(500) = NULL,
    @Activo         BIT           = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    DECLARE @SiguienteNumero INT;

    SELECT @SiguienteNumero = ISNULL(MAX(CAST(Clave AS INT)), 0) + 1
    FROM Acopio.JefeAcopio WITH (TABLOCKX, HOLDLOCK);

    DECLARE @Clave NVARCHAR(6) = RIGHT('000000' + CAST(@SiguienteNumero AS VARCHAR(6)), 6);

    INSERT INTO Acopio.JefeAcopio
        (Clave, Nombre, Domicilio, Colonia, CodigoPostal, PoblacionId, MunicipioId,
         EstadoId, Telefono, Celular, Email, Observaciones, Activo)
    VALUES
        (@Clave, @Nombre, @Domicilio, @Colonia, @CodigoPostal, @PoblacionId, @MunicipioId,
         @EstadoId, @Telefono, @Celular, @Email, @Observaciones, @Activo);

    DECLARE @Id INT = SCOPE_IDENTITY();

    COMMIT TRANSACTION;

    SELECT @Id AS Id, @Clave AS Clave;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_Actualizar
    @Id             INT,
    @Nombre         NVARCHAR(200),
    @Domicilio      NVARCHAR(200) = NULL,
    @Colonia        NVARCHAR(100) = NULL,
    @CodigoPostal   NVARCHAR(10)  = NULL,
    @PoblacionId    INT           = NULL,
    @MunicipioId    INT           = NULL,
    @EstadoId       INT           = NULL,
    @Telefono       NVARCHAR(30)  = NULL,
    @Celular        NVARCHAR(30)  = NULL,
    @Email          NVARCHAR(150) = NULL,
    @Observaciones  NVARCHAR(500) = NULL,
    @Activo         BIT           = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Acopio.JefeAcopio
    SET Nombre        = @Nombre,
        Domicilio      = @Domicilio,
        Colonia        = @Colonia,
        CodigoPostal   = @CodigoPostal,
        PoblacionId    = @PoblacionId,
        MunicipioId    = @MunicipioId,
        EstadoId       = @EstadoId,
        Telefono       = @Telefono,
        Celular        = @Celular,
        Email          = @Email,
        Observaciones  = @Observaciones,
        Activo         = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_JefeAcopio_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Acopio.JefeAcopio WHERE Id = @Id;
END
GO
