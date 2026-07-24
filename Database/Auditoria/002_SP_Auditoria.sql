USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Auditoria.sp_Auditoria_Registrar
    @Usuario            NVARCHAR(50),
    @Fecha              DATETIME2,
    @Equipo             NVARCHAR(100),
    @Ip                 NVARCHAR(45),
    @Accion             NVARCHAR(50),
    @Modulo             NVARCHAR(100),
    @ValoresAnteriores  NVARCHAR(MAX) = NULL,
    @ValoresNuevos      NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria.Registro
        (Usuario, Fecha, Equipo, Ip, Accion, Modulo, ValoresAnteriores, ValoresNuevos)
    VALUES
        (@Usuario, @Fecha, @Equipo, @Ip, @Accion, @Modulo, @ValoresAnteriores, @ValoresNuevos);
END
GO
