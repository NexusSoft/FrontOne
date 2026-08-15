USE FrontOne;
GO

-- Exclusivo del Diseñador de Reportes: une en una sola fila los campos de encabezado del Pallet
-- (Produccion.sp_Pallet_ObtenerEtiquetaPalletEncabezado, que NO se toca) + el membrete de empresa,
-- con los mismos alias exactos que PalletImprimirEtiquetaForm.VistaPalletEncabezado (record plano
-- que combina ambas fuentes en runtime). Así el Field List del Diseñador queda con un solo origen
-- para todo el encabezado+empresa, y el DataMember que lo conecta (EtiquetasForm.
-- ConectarOrigenDatosDiseno) hace que arrastrar cualquiera de estos campos genere una expresión
-- plana ([Campo]), igual a como el runtime realmente los expone — antes generaba
-- [EtiquetaPalletEncabezado].[Campo]/[EtiquetaPalletEmpresa].[Campo], que nunca resolvía en la
-- vista previa/impresión real. TotalCajas/TotalKilogramos van como columnas dummy (se calculan en
-- C# sobre el detalle, no existen en ningún SP) solo para que el Field List tenga esas 2 columnas
-- disponibles para arrastrar.
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerEtiquetaPalletEncabezadoDiseno
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pa.Folio AS NoPallet,
        pa.FechaCreacion AS FechaProcesado,
        CASE pa.Estatus
            WHEN 1 THEN 'Vacío' WHEN 2 THEN 'Incompleto' WHEN 3 THEN 'Completo'
            WHEN 4 THEN 'Excedido' WHEN 5 THEN 'Empacado' WHEN 6 THEN 'Reempacado'
            ELSE ''
        END AS Status,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) > 1 THEN 'Mixto'
            ELSE ISNULL(prim.DescripcionSap, '')
        END AS NombreProducto,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) > 1 THEN NULL
            ELSE prim.PesoNeto
        END AS PesoEstandar,
        emp.RazonSocial AS RazonSocial,
        emp.Domicilio AS Domicilio,
        emp.Rfc AS Rfc,
        emp.Telefono AS Telefono,
        emp.Correo AS Correo,
        emp.Logo AS Logo,
        CAST(0 AS INT) AS TotalCajas,
        CAST(0 AS DECIMAL(18, 2)) AS TotalKilogramos
    FROM Produccion.Pallet pa
    CROSS JOIN Configuracion.Empresa emp
    OUTER APPLY (
        SELECT COUNT(DISTINCT d.ProductoTerminadoId) AS ProductosDistintos
        FROM Produccion.PalletDetalle d
        WHERE d.PalletId = pa.Id
    ) AS tot
    OUTER APPLY (
        SELECT TOP 1 pt.DescripcionSap, pt.PesoNeto
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = pa.Id
        ORDER BY d.Id
    ) AS prim
    WHERE pa.Id = @PalletId;
END
GO
