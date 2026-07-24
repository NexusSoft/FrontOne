USE FrontOne;
GO

-- ItemCode/ItemName no se tocan aquí (vienen de SAP y son inmutables); solo se puede
-- corregir la vigencia (fecha inicio/fin) y, de paso, los precios/estado activo.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_Actualizar
    @Id          INT,
    @Lista1      DECIMAL(18,4),
    @Lista2      DECIMAL(18,4),
    @Lista3      DECIMAL(18,4),
    @FechaInicio DATE,
    @FechaFin    DATE = NULL,
    @Activo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Acopio.ListaPrecioFruta
    SET Lista1 = @Lista1,
        Lista2 = @Lista2,
        Lista3 = @Lista3,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        Activo = @Activo
    WHERE Id = @Id;
END
GO
