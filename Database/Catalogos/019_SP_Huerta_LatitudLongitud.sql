USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id, Nombre, ProductorId, Ubicacion, PoblacionId, Municipio, EstadoId, ProductoId,
        EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
        RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
        SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
        Latitud, Longitud,
        Activo,
        CASE WHEN FechaCambioStatus IS NULL THEN NULL ELSE DATEDIFF(DAY, FechaCambioStatus, GETDATE()) / 365.0 END AS AniosEnStatusActual,
        CASE WHEN FechaVencimiento IS NULL THEN NULL ELSE DATEDIFF(DAY, GETDATE(), FechaVencimiento) END AS DiasParaVencimiento
    FROM Catalogos.Huerta
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_Insertar
    @Nombre                 NVARCHAR(150),
    @ProductorId             INT,
    @Ubicacion               NVARCHAR(200) = NULL,
    @PoblacionId             INT = NULL,
    @Municipio               NVARCHAR(100) = NULL,
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
        (Nombre, ProductorId, Ubicacion, PoblacionId, Municipio, EstadoId, ProductoId,
         EncargadoNombre, EncargadoTelefono, Observaciones, RegistroSagarpa, CertificadoGlobalGap,
         RegistroFda, NumeroGlobalGap, Superficie, Altura, NumeroArboles, EdadArboles,
         SistemaRiegoId, PorcentajeMecanizacion, StatusHuertaId, FechaCambioStatus, FechaVencimiento,
         Latitud, Longitud, Activo)
    VALUES
        (@Nombre, @ProductorId, @Ubicacion, @PoblacionId, @Municipio, @EstadoId, @ProductoId,
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
    @Municipio               NVARCHAR(100) = NULL,
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
        Municipio               = @Municipio,
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
