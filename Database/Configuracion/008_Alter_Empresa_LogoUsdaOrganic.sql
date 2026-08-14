USE FrontOne;
GO

-- Segundo logo de la empresa, usado por la etiqueta de Caja cuando el producto es orgánico
-- certificado USDA (imagen opcional, se muestra vacía si no se ha cargado).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Configuracion.Empresa') AND name = 'LogoUsdaOrganic')
BEGIN
    ALTER TABLE Configuracion.Empresa ADD LogoUsdaOrganic VARBINARY(MAX) NULL;
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_Empresa_Obtener
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, RazonSocial, Domicilio, Rfc, Telefono, Correo, Logo, NumeroEmpaque, LogoUsdaOrganic, FechaModificacion
    FROM Configuracion.Empresa
    WHERE Id = 1;
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_Empresa_Actualizar
    @RazonSocial      NVARCHAR(200),
    @Domicilio        NVARCHAR(300) = NULL,
    @Rfc              NVARCHAR(20)  = NULL,
    @Telefono         NVARCHAR(30)  = NULL,
    @Correo           NVARCHAR(150) = NULL,
    @Logo             VARBINARY(MAX) = NULL,
    @NumeroEmpaque    NVARCHAR(3)   = NULL,
    @LogoUsdaOrganic  VARBINARY(MAX) = NULL
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
        LogoUsdaOrganic = @LogoUsdaOrganic,
        FechaModificacion = SYSDATETIME()
    WHERE Id = 1;
END
GO
