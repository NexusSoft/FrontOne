USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_ReportePermiso_ObtenerPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno
    FROM Seguridad.ReportePermiso
    WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_ReportePermiso_EliminarPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Seguridad.ReportePermiso WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_ReportePermiso_Insertar
    @RolId         INT,
    @ReporteCodigo NVARCHAR(50),
    @VistaPrevia   BIT,
    @Impresion     BIT,
    @Exportacion   BIT,
    @Diseno        BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.ReportePermiso (RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno)
    VALUES (@RolId, @ReporteCodigo, @VistaPrevia, @Impresion, @Exportacion, @Diseno);
END
GO

-- Se agrega junto al resto de sp_Usuario_* (002_SP_Usuario.sql) en cuanto a semántica
-- (se llama desde el mismo flujo de login), pero vive en este archivo para no reabrir un
-- script ya desplegado. Un usuario puede tener el permiso vía más de un rol — se agrupa por
-- ReporteCodigo y MAX() por bit, "el rol más permisivo gana", mismo criterio implícito que ya
-- aplica el modelo de Permiso genérico al unir roles.
CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ObtenerPermisosReporte
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        rp.ReporteCodigo,
        CAST(MAX(CAST(rp.VistaPrevia AS INT)) AS BIT) AS VistaPrevia,
        CAST(MAX(CAST(rp.Impresion AS INT)) AS BIT) AS Impresion,
        CAST(MAX(CAST(rp.Exportacion AS INT)) AS BIT) AS Exportacion,
        CAST(MAX(CAST(rp.Diseno AS INT)) AS BIT) AS Diseno
    FROM Seguridad.UsuarioRol ur
    INNER JOIN Seguridad.ReportePermiso rp ON rp.RolId = ur.RolId
    WHERE ur.UsuarioId = @UsuarioId
    GROUP BY rp.ReporteCodigo;
END
GO
