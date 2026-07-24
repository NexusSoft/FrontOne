USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, Municipio, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Clave;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_Insertar
    @NombreProductor    NVARCHAR(200),
    @Domicilio          NVARCHAR(200) = NULL,
    @Colonia            NVARCHAR(100) = NULL,
    @CodigoPostal       NVARCHAR(10)  = NULL,
    @PoblacionId        INT           = NULL,
    @Municipio          NVARCHAR(100) = NULL,
    @EstadoId           INT           = NULL,
    @Rfc                NVARCHAR(20)  = NULL,
    @Telefono           NVARCHAR(30)  = NULL,
    @Celular            NVARCHAR(30)  = NULL,
    @Email              NVARCHAR(150) = NULL,
    @Organizacion       NVARCHAR(150) = NULL,
    @Observaciones      NVARCHAR(500) = NULL,
    @Usuario            NVARCHAR(50)  = NULL,
    @PasswordEncriptado NVARCHAR(500) = NULL,
    @DiasCredito        INT           = 0,
    @Activo             BIT           = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    DECLARE @SiguienteNumero INT;

    SELECT @SiguienteNumero = ISNULL(MAX(CAST(Clave AS INT)), 0) + 1
    FROM Catalogos.Productor WITH (TABLOCKX, HOLDLOCK);

    DECLARE @Clave NVARCHAR(6) = RIGHT('000000' + CAST(@SiguienteNumero AS VARCHAR(6)), 6);

    INSERT INTO Catalogos.Productor
        (Clave, NombreProductor, Domicilio, Colonia, CodigoPostal, PoblacionId, Municipio,
         EstadoId, Rfc, Telefono, Celular, Email, Organizacion, Observaciones,
         Usuario, PasswordEncriptado, DiasCredito, Activo)
    VALUES
        (@Clave, @NombreProductor, @Domicilio, @Colonia, @CodigoPostal, @PoblacionId, @Municipio,
         @EstadoId, @Rfc, @Telefono, @Celular, @Email, @Organizacion, @Observaciones,
         @Usuario, @PasswordEncriptado, @DiasCredito, @Activo);

    DECLARE @Id INT = SCOPE_IDENTITY();

    COMMIT TRANSACTION;

    SELECT @Id AS Id, @Clave AS Clave;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_Actualizar
    @Id                 INT,
    @NombreProductor    NVARCHAR(200),
    @Domicilio          NVARCHAR(200) = NULL,
    @Colonia            NVARCHAR(100) = NULL,
    @CodigoPostal       NVARCHAR(10)  = NULL,
    @PoblacionId        INT           = NULL,
    @Municipio          NVARCHAR(100) = NULL,
    @EstadoId           INT           = NULL,
    @Rfc                NVARCHAR(20)  = NULL,
    @Telefono           NVARCHAR(30)  = NULL,
    @Celular            NVARCHAR(30)  = NULL,
    @Email              NVARCHAR(150) = NULL,
    @Organizacion       NVARCHAR(150) = NULL,
    @Observaciones      NVARCHAR(500) = NULL,
    @Usuario            NVARCHAR(50)  = NULL,
    @PasswordEncriptado NVARCHAR(500) = NULL,
    @DiasCredito        INT           = 0,
    @Activo             BIT           = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Productor
    SET NombreProductor    = @NombreProductor,
        Domicilio          = @Domicilio,
        Colonia            = @Colonia,
        CodigoPostal       = @CodigoPostal,
        PoblacionId        = @PoblacionId,
        Municipio          = @Municipio,
        EstadoId           = @EstadoId,
        Rfc                = @Rfc,
        Telefono           = @Telefono,
        Celular            = @Celular,
        Email              = @Email,
        Organizacion       = @Organizacion,
        Observaciones      = @Observaciones,
        Usuario            = @Usuario,
        PasswordEncriptado = @PasswordEncriptado,
        DiasCredito        = @DiasCredito,
        Activo             = @Activo
    WHERE Id = @Id;
END
GO
