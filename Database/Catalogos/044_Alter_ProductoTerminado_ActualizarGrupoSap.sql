USE FrontOne;
GO

-- La sincronización debe actualizar GrupoSap cuando el artículo cambia de grupo en SAP (ej.
-- PT -> ST), no solo fijarlo una vez al insertar — antes sp_ProductoTerminado_ActualizarDatosSap
-- solo tocaba descripción, dejando el grupo local desactualizado para siempre.
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_ActualizarDatosSap
    @Id                       INT,
    @GrupoSap                 NVARCHAR(2) = NULL,
    @DescripcionSap           NVARCHAR(200),
    @DescripcionExtranjeraSap NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.ProductoTerminado
    SET GrupoSap = @GrupoSap,
        DescripcionSap = @DescripcionSap,
        DescripcionExtranjeraSap = @DescripcionExtranjeraSap
    WHERE Id = @Id;
END
GO
