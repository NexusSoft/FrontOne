USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- =============================================================================
-- SPs para la nueva jerarquía País -> Estado -> Municipio -> Población.
-- =============================================================================

-- Municipio
CREATE OR ALTER PROCEDURE Catalogos.sp_Municipio_Obtener
    @EstadoId INT = NULL,
    @Id       INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, EstadoId, Activo
    FROM Catalogos.Municipio
    WHERE (@EstadoId IS NULL OR EstadoId = @EstadoId)
      AND (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Municipio_Insertar
    @Nombre   NVARCHAR(150),
    @EstadoId INT,
    @Activo   BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.Municipio (Nombre, EstadoId, Activo)
    VALUES (@Nombre, @EstadoId, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Municipio_Actualizar
    @Id       INT,
    @Nombre   NVARCHAR(150),
    @EstadoId INT,
    @Activo   BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Municipio
    SET Nombre = @Nombre,
        EstadoId = @EstadoId,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Municipio_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Catalogos.Municipio WHERE Id = @Id;
END
GO

-- Poblacion (ahora cuelga de Municipio, no de Estado)
CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Obtener
    @MunicipioId INT = NULL,
    @Id          INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, MunicipioId, Activo
    FROM Catalogos.Poblacion
    WHERE (@MunicipioId IS NULL OR MunicipioId = @MunicipioId)
      AND (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Insertar
    @Nombre      NVARCHAR(150),
    @MunicipioId INT,
    @Activo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.Poblacion (Nombre, MunicipioId, Activo)
    VALUES (@Nombre, @MunicipioId, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Actualizar
    @Id          INT,
    @Nombre      NVARCHAR(150),
    @MunicipioId INT,
    @Activo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Poblacion
    SET Nombre = @Nombre,
        MunicipioId = @MunicipioId,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

-- Huerta: @Municipio (texto) -> @MunicipioId (int)
CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_Obtener
    @Id          INT = NULL,
    @ProductorId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id, Nombre, ProductorId, Ubicacion, PoblacionId, MunicipioId, EstadoId, ProductoId,
        EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
        RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
        SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
        Latitud, Longitud,
        Activo,
        CASE WHEN FechaCambioStatus IS NULL THEN NULL ELSE DATEDIFF(DAY, FechaCambioStatus, GETDATE()) / 365.0 END AS AniosEnStatusActual,
        CASE WHEN FechaVencimiento IS NULL THEN NULL ELSE DATEDIFF(DAY, GETDATE(), FechaVencimiento) END AS DiasParaVencimiento
    FROM Catalogos.Huerta
    WHERE (@Id IS NULL OR Id = @Id)
      AND (@ProductorId IS NULL OR ProductorId = @ProductorId)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_Insertar
    @Nombre                 NVARCHAR(150),
    @ProductorId             INT,
    @Ubicacion               NVARCHAR(200) = NULL,
    @PoblacionId             INT = NULL,
    @MunicipioId             INT = NULL,
    @EstadoId                INT = NULL,
    @ProductoId              INT = NULL,
    @EncargadoNombre         NVARCHAR(150) = NULL,
    @EncargadoTelefono       NVARCHAR(30) = NULL,
    @Observaciones           NVARCHAR(500) = NULL,
    @RegistroSagarpa         NVARCHAR(50) = NULL,
    @CertificadoGlobalGap    BIT = 0,
    @RegistroFda             NVARCHAR(50) = NULL,
    @NumeroGlobalGap         NVARCHAR(50) = NULL,
    @Superficie              DECIMAL(10,2) = NULL,
    @Altura                  DECIMAL(10,2) = NULL,
    @NumeroArboles           INT = NULL,
    @EdadArboles             INT = NULL,
    @SistemaRiegoId          INT = NULL,
    @PorcentajeMecanizacion  DECIMAL(5,2) = NULL,
    @StatusHuertaId          INT = NULL,
    @FechaCambioStatus       DATE = NULL,
    @FechaVencimiento        DATE = NULL,
    @Latitud                 DECIMAL(9,6) = NULL,
    @Longitud                DECIMAL(9,6) = NULL,
    @Activo                  BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.Huerta
        (Nombre, ProductorId, Ubicacion, PoblacionId, MunicipioId, EstadoId, ProductoId,
         EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
         RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
         SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
         Latitud, Longitud, Activo)
    VALUES
        (@Nombre, @ProductorId, @Ubicacion, @PoblacionId, @MunicipioId, @EstadoId, @ProductoId,
         @EncargadoNombre, @EncargadoTelefono, @Observaciones, @RegistroSagarpa, @CertificadoGlobalGap,
         @RegistroFda, @NumeroGlobalGap, @Superficie, @Altura, @NumeroArboles, @EdadArboles,
         @SistemaRiegoId, @PorcentajeMecanizacion, @StatusHuertaId, @FechaCambioStatus, @FechaVencimiento,
         @Latitud, @Longitud, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_Actualizar
    @Id                      INT,
    @Nombre                  NVARCHAR(150),
    @ProductorId             INT,
    @Ubicacion               NVARCHAR(200) = NULL,
    @PoblacionId             INT = NULL,
    @MunicipioId             INT = NULL,
    @EstadoId                INT = NULL,
    @ProductoId              INT = NULL,
    @EncargadoNombre         NVARCHAR(150) = NULL,
    @EncargadoTelefono       NVARCHAR(30) = NULL,
    @Observaciones           NVARCHAR(500) = NULL,
    @RegistroSagarpa         NVARCHAR(50) = NULL,
    @CertificadoGlobalGap    BIT = 0,
    @RegistroFda             NVARCHAR(50) = NULL,
    @NumeroGlobalGap         NVARCHAR(50) = NULL,
    @Superficie              DECIMAL(10,2) = NULL,
    @Altura                  DECIMAL(10,2) = NULL,
    @NumeroArboles           INT = NULL,
    @EdadArboles             INT = NULL,
    @SistemaRiegoId          INT = NULL,
    @PorcentajeMecanizacion  DECIMAL(5,2) = NULL,
    @StatusHuertaId          INT = NULL,
    @FechaCambioStatus       DATE = NULL,
    @FechaVencimiento        DATE = NULL,
    @Latitud                 DECIMAL(9,6) = NULL,
    @Longitud                DECIMAL(9,6) = NULL,
    @Activo                  BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Huerta
    SET Nombre                 = @Nombre,
        ProductorId             = @ProductorId,
        Ubicacion               = @Ubicacion,
        PoblacionId             = @PoblacionId,
        MunicipioId             = @MunicipioId,
        EstadoId                = @EstadoId,
        ProductoId              = @ProductoId,
        EncargadoNombre         = @EncargadoNombre,
        EncargadoTelefono       = @EncargadoTelefono,
        Observaciones           = @Observaciones,
        RegistroSagarpa         = @RegistroSagarpa,
        CertificadoGlobalGap    = @CertificadoGlobalGap,
        RegistroFda             = @RegistroFda,
        NumeroGlobalGap         = @NumeroGlobalGap,
        Superficie              = @Superficie,
        Altura                  = @Altura,
        NumeroArboles           = @NumeroArboles,
        EdadArboles             = @EdadArboles,
        SistemaRiegoId          = @SistemaRiegoId,
        PorcentajeMecanizacion  = @PorcentajeMecanizacion,
        StatusHuertaId          = @StatusHuertaId,
        FechaCambioStatus       = @FechaCambioStatus,
        FechaVencimiento        = @FechaVencimiento,
        Latitud                 = @Latitud,
        Longitud                = @Longitud,
        Activo                  = @Activo
    WHERE Id = @Id;
END
GO

-- Productor: @Municipio (texto) -> @MunicipioId (int)
CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Clave;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_Buscar
    @Filtro NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 500
        Id, Clave, FechaRegistro, NombreProductor, Domicilio, Colonia, CodigoPostal,
        PoblacionId, MunicipioId, EstadoId, Rfc, Telefono, Celular, Email,
        Organizacion, Observaciones, Usuario, PasswordEncriptado, DiasCredito, Activo
    FROM Catalogos.Productor
    WHERE Clave LIKE '%' + @Filtro + '%'
       OR NombreProductor LIKE '%' + @Filtro + '%'
    ORDER BY NombreProductor;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Productor_Insertar
    @NombreProductor    NVARCHAR(200),
    @Domicilio          NVARCHAR(200) = NULL,
    @Colonia            NVARCHAR(100) = NULL,
    @CodigoPostal       NVARCHAR(10)  = NULL,
    @PoblacionId        INT           = NULL,
    @MunicipioId        INT           = NULL,
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
        (Clave, NombreProductor, Domicilio, Colonia, CodigoPostal, PoblacionId, MunicipioId,
         EstadoId, Rfc, Telefono, Celular, Email, Organizacion, Observaciones,
         Usuario, PasswordEncriptado, DiasCredito, Activo)
    VALUES
        (@Clave, @NombreProductor, @Domicilio, @Colonia, @CodigoPostal, @PoblacionId, @MunicipioId,
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
    @MunicipioId        INT           = NULL,
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
        MunicipioId        = @MunicipioId,
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
