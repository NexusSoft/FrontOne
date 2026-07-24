USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_Empresa_Obtener
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, RazonSocial, Domicilio, Rfc, Telefono, Correo, Logo, FechaModificacion
    FROM Configuracion.Empresa
    WHERE Id = 1;
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_Empresa_Actualizar
    @RazonSocial NVARCHAR(200),
    @Domicilio   NVARCHAR(300) = NULL,
    @Rfc         NVARCHAR(20)  = NULL,
    @Telefono    NVARCHAR(30)  = NULL,
    @Correo      NVARCHAR(150) = NULL,
    @Logo        VARBINARY(MAX) = NULL
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
        FechaModificacion = SYSDATETIME()
    WHERE Id = 1;
END
GO
