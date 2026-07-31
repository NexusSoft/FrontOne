USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Configuracion.Empresa') AND name = 'NumeroEmpaque')
BEGIN
    ALTER TABLE Configuracion.Empresa ADD NumeroEmpaque NVARCHAR(3) NULL;
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_Empresa_Obtener
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, RazonSocial, Domicilio, Rfc, Telefono, Correo, Logo, NumeroEmpaque, FechaModificacion
    FROM Configuracion.Empresa
    WHERE Id = 1;
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_Empresa_Actualizar
    @RazonSocial   NVARCHAR(200),
    @Domicilio     NVARCHAR(300) = NULL,
    @Rfc           NVARCHAR(20)  = NULL,
    @Telefono      NVARCHAR(30)  = NULL,
    @Correo        NVARCHAR(150) = NULL,
    @Logo          VARBINARY(MAX) = NULL,
    @NumeroEmpaque NVARCHAR(3)   = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Configuracion.Empresa
    SET RazonSocial = @RazonSocial,
        Domicilio = @Domicilio,
        Rfc = @Rfc,
        Telefono = @Telefono,
        Correo = @Correo,
        Logo = @Logo,
        NumeroEmpaque = @NumeroEmpaque,
        FechaModificacion = SYSDATETIME()
    WHERE Id = 1;
END
GO
