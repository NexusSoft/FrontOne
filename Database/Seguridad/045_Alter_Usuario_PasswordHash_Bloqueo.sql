USE FrontOne;
GO

-- Migración a PBKDF2 (hash+salt) con fallback transparente al cifrado AES actual
-- (PasswordEncriptado). Mientras PasswordHash sea NULL, AuthService.LoginAsync valida por la
-- ruta legacy AES y graba el hash nuevo en el primer login exitoso (rehash transparente) — nadie
-- tiene que volver a capturar su contraseña. IntentosFallidos/BloqueadoHasta soportan el bloqueo
-- de cuenta del sitio web tras 5 fallos consecutivos (FrontOne.Web, rate limiter en /login).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Seguridad.Usuario') AND name = 'PasswordHash')
BEGIN
    ALTER TABLE Seguridad.Usuario ADD PasswordHash NVARCHAR(200) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Seguridad.Usuario') AND name = 'IntentosFallidos')
BEGIN
    ALTER TABLE Seguridad.Usuario ADD IntentosFallidos INT NOT NULL CONSTRAINT DF_Seguridad_Usuario_IntentosFallidos DEFAULT (0);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Seguridad.Usuario') AND name = 'BloqueadoHasta')
BEGIN
    ALTER TABLE Seguridad.Usuario ADD BloqueadoHasta DATETIME2 NULL;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ObtenerPorNombreUsuario
    @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        NombreUsuario,
        NombreCompleto,
        Email,
        PasswordEncriptado,
        PasswordHash,
        Activo,
        FechaCreacion,
        IntentosFallidos,
        BloqueadoHasta
    FROM Seguridad.Usuario
    WHERE NombreUsuario = @NombreUsuario;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, NombreUsuario, NombreCompleto, Email, PasswordEncriptado, PasswordHash, Activo, FechaCreacion
    FROM Seguridad.Usuario
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY NombreUsuario;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_Insertar
    @NombreUsuario      NVARCHAR(50),
    @NombreCompleto     NVARCHAR(150),
    @Email              NVARCHAR(150) = NULL,
    @PasswordEncriptado NVARCHAR(500),
    @PasswordHash       NVARCHAR(200) = NULL,
    @Activo             BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.Usuario (NombreUsuario, NombreCompleto, Email, PasswordEncriptado, PasswordHash, Activo)
    VALUES (@NombreUsuario, @NombreCompleto, @Email, @PasswordEncriptado, @PasswordHash, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_Actualizar
    @Id                 INT,
    @NombreUsuario      NVARCHAR(50),
    @NombreCompleto     NVARCHAR(150),
    @Email              NVARCHAR(150) = NULL,
    @PasswordEncriptado NVARCHAR(500),
    @PasswordHash       NVARCHAR(200) = NULL,
    @Activo             BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Seguridad.Usuario
    SET NombreUsuario      = @NombreUsuario,
        NombreCompleto     = @NombreCompleto,
        Email              = @Email,
        PasswordEncriptado = @PasswordEncriptado,
        PasswordHash       = @PasswordHash,
        Activo             = @Activo
    WHERE Id = @Id;
END
GO

-- Se llama solo desde AuthService.LoginAsync (rehash transparente) y desde UsuarioService al
-- capturar/cambiar contraseña — no toca PasswordEncriptado.
CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ActualizarPasswordHash
    @Id           INT,
    @PasswordHash NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Seguridad.Usuario
    SET PasswordHash = @PasswordHash
    WHERE Id = @Id;
END
GO

-- Soporte de bloqueo de cuenta (FrontOne.Web): se llama tras cada intento fallido de login;
-- al llegar a 5 fallos consecutivos bloquea la cuenta 15 minutos.
CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_RegistrarIntentoFallido
    @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Seguridad.Usuario
    SET IntentosFallidos = IntentosFallidos + 1,
        BloqueadoHasta = CASE WHEN IntentosFallidos + 1 >= 5 THEN DATEADD(MINUTE, 15, SYSUTCDATETIME()) ELSE BloqueadoHasta END
    WHERE NombreUsuario = @NombreUsuario;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ResetearIntentosFallidos
    @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Seguridad.Usuario
    SET IntentosFallidos = 0,
        BloqueadoHasta = NULL
    WHERE NombreUsuario = @NombreUsuario;
END
GO
