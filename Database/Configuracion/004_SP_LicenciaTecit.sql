USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_LicenciaTecit_Obtener
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Licenciatario, ClaveLicencia, TipoLicencia, NumeroLicencias, Producto, FechaModificacion
    FROM Configuracion.LicenciaTecit
    WHERE Id = 1;
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_LicenciaTecit_Actualizar
    @Licenciatario   NVARCHAR(200),
    @ClaveLicencia   NVARCHAR(400) = NULL,
    @TipoLicencia    NVARCHAR(50)  = NULL,
    @NumeroLicencias INT           = NULL,
    @Producto        NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Configuracion.LicenciaTecit
    SET Licenciatario = @Licenciatario,
        ClaveLicencia = @ClaveLicencia,
        TipoLicencia = @TipoLicencia,
        NumeroLicencias = @NumeroLicencias,
        Producto = @Producto,
        FechaModificacion = SYSDATETIME()
    WHERE Id = 1;
END
GO
