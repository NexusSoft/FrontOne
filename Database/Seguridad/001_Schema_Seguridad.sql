USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Seguridad')
    EXEC('CREATE SCHEMA Seguridad');
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Seguridad.Usuario'))
BEGIN
    CREATE TABLE Seguridad.Usuario
    (
        Id                  INT IDENTITY(1,1)      NOT NULL CONSTRAINT PK_Seguridad_Usuario PRIMARY KEY,
        NombreUsuario       NVARCHAR(50)            NOT NULL,
        NombreCompleto      NVARCHAR(150)           NOT NULL,
        Email               NVARCHAR(150)           NULL,
        PasswordEncriptado  NVARCHAR(500)           NOT NULL,
        Activo              BIT                     NOT NULL CONSTRAINT DF_Seguridad_Usuario_Activo DEFAULT (1),
        FechaCreacion       DATETIME2               NOT NULL CONSTRAINT DF_Seguridad_Usuario_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Seguridad_Usuario_NombreUsuario UNIQUE (NombreUsuario)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Seguridad.Rol'))
BEGIN
    CREATE TABLE Seguridad.Rol
    (
        Id          INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Seguridad_Rol PRIMARY KEY,
        Nombre      NVARCHAR(100)      NOT NULL,
        Descripcion NVARCHAR(300)      NULL,
        Activo      BIT                NOT NULL CONSTRAINT DF_Seguridad_Rol_Activo DEFAULT (1),
        CONSTRAINT UQ_Seguridad_Rol_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Seguridad.Modulo'))
BEGIN
    CREATE TABLE Seguridad.Modulo
    (
        Id          INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Seguridad_Modulo PRIMARY KEY,
        Nombre      NVARCHAR(100)      NOT NULL,
        Descripcion NVARCHAR(300)      NULL,
        CONSTRAINT UQ_Seguridad_Modulo_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Seguridad.Pantalla'))
BEGIN
    CREATE TABLE Seguridad.Pantalla
    (
        Id          INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Seguridad_Pantalla PRIMARY KEY,
        ModuloId    INT                NOT NULL,
        Nombre      NVARCHAR(100)      NOT NULL,
        Descripcion NVARCHAR(300)      NULL,
        CONSTRAINT FK_Seguridad_Pantalla_Modulo FOREIGN KEY (ModuloId) REFERENCES Seguridad.Modulo (Id),
        CONSTRAINT UQ_Seguridad_Pantalla_Modulo_Nombre UNIQUE (ModuloId, Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Seguridad.Accion'))
BEGIN
    CREATE TABLE Seguridad.Accion
    (
        Id     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Seguridad_Accion PRIMARY KEY,
        Nombre NVARCHAR(50)      NOT NULL,
        CONSTRAINT UQ_Seguridad_Accion_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Seguridad.Permiso'))
BEGIN
    CREATE TABLE Seguridad.Permiso
    (
        Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Seguridad_Permiso PRIMARY KEY,
        RolId      INT               NOT NULL,
        PantallaId INT               NOT NULL,
        AccionId   INT               NOT NULL,
        CONSTRAINT FK_Seguridad_Permiso_Rol FOREIGN KEY (RolId) REFERENCES Seguridad.Rol (Id),
        CONSTRAINT FK_Seguridad_Permiso_Pantalla FOREIGN KEY (PantallaId) REFERENCES Seguridad.Pantalla (Id),
        CONSTRAINT FK_Seguridad_Permiso_Accion FOREIGN KEY (AccionId) REFERENCES Seguridad.Accion (Id),
        CONSTRAINT UQ_Seguridad_Permiso UNIQUE (RolId, PantallaId, AccionId)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Seguridad.UsuarioRol'))
BEGIN
    CREATE TABLE Seguridad.UsuarioRol
    (
        UsuarioId INT NOT NULL,
        RolId     INT NOT NULL,
        CONSTRAINT PK_Seguridad_UsuarioRol PRIMARY KEY (UsuarioId, RolId),
        CONSTRAINT FK_Seguridad_UsuarioRol_Usuario FOREIGN KEY (UsuarioId) REFERENCES Seguridad.Usuario (Id),
        CONSTRAINT FK_Seguridad_UsuarioRol_Rol FOREIGN KEY (RolId) REFERENCES Seguridad.Rol (Id)
    );
END
GO
