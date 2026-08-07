USE FrontOne;
GO

-- El Código de Peso Estándar deja de capturarse a mano: se genera server-side con una SEQUENCE
-- al insertar (mismo criterio de folio ya usado en Lote/OrdenCorte/AcuerdoCorte/Pallet), formato
-- de 7 dígitos consecutivos. Ya no se puede editar después de creado.
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqPesoEstandarCodigo' AND schema_id = SCHEMA_ID('Catalogos'))
BEGIN
    CREATE SEQUENCE Catalogos.SeqPesoEstandarCodigo AS INT START WITH 1 INCREMENT BY 1;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_PesoEstandar_Insertar
    @Descripcion  NVARCHAR(200),
    @PesoNeto     DECIMAL(10,3),
    @PesoPromedio DECIMAL(10,3)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Codigo NVARCHAR(50) =
        RIGHT('0000000' + CAST(NEXT VALUE FOR Catalogos.SeqPesoEstandarCodigo AS VARCHAR(7)), 7);

    INSERT INTO Catalogos.PesoEstandar (Codigo, Descripcion, PesoNeto, PesoPromedio, Activo)
    VALUES (@Codigo, @Descripcion, @PesoNeto, @PesoPromedio, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Codigo AS Codigo;
END
GO

-- Código ya no es parámetro: nunca se vuelve a tocar después de creado.
CREATE OR ALTER PROCEDURE Catalogos.sp_PesoEstandar_Actualizar
    @Id           INT,
    @Descripcion  NVARCHAR(200),
    @PesoNeto     DECIMAL(10,3),
    @PesoPromedio DECIMAL(10,3),
    @Activo       BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.PesoEstandar
    SET Descripcion = @Descripcion,
        PesoNeto = @PesoNeto,
        PesoPromedio = @PesoPromedio,
        Activo = @Activo
    WHERE Id = @Id;
END
GO
