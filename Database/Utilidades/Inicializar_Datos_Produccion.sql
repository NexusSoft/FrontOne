USE FrontOne;
GO

-- ============================================================================
-- ADVERTENCIA: este script BORRA todos los datos de las tablas operativas
-- (catálogos + Acopio + Auditoría) y reinicia sus folios/Id para que el
-- próximo registro capturado arranque en 1.
--
-- Ejecutar UNA SOLA VEZ, justo antes de poner la base en producción con datos
-- reales — nunca contra una base con información que se quiera conservar.
--
-- Seguridad.Usuario, Rol, Permiso, UsuarioRol, Modulo, Pantalla y Accion
-- (usuarios, perfiles y la estructura de pantallas/permisos) NO se tocan: la
-- base recién puesta en blanco necesita seguir teniendo con qué iniciar sesión.
-- ============================================================================

SET NOCOUNT ON;

DECLARE @Tablas TABLE (Tabla NVARCHAR(261));
INSERT INTO @Tablas (Tabla) VALUES
    ('Acopio.AcuerdoCorte'),
    ('Acopio.OrdenCorte'),
    ('Acopio.JefeAcopio'),
    ('Acopio.ListaPrecioFruta'),
    ('Acopio.ListaPrecioCorte'),
    ('Acopio.TipoCorte'),
    ('Acopio.TipoPago'),
    ('Acopio.Variedad'),
    ('Acopio.Floracion'),
    ('Acopio.TipoComercializacion'),
    ('Acopio.Moneda'),
    ('Catalogos.Huerta'),
    ('Catalogos.Productor'),
    ('Catalogos.Poblacion'),
    ('Catalogos.Municipio'),
    ('Catalogos.Estado'),
    ('Catalogos.Pais'),
    ('Catalogos.Producto'),
    ('Catalogos.StatusHuerta'),
    ('Catalogos.SistemaRiego'),
    ('Acarreo.ListaPrecioAcarreo'),
    ('Acarreo.Zona'),
    ('Recepcion.RecepcionFrutaOrdenCorte'),
    ('Recepcion.RecepcionFruta'),
    ('Produccion.Corrida'),
    ('Lotes.LoteRecepcion'),
    ('Lotes.Lote'),
    ('Catalogos.LineaProduccion'),
    ('Catalogos.CajaCampo'),
    ('Catalogos.PesoEstandar'),
    ('Catalogos.Categoria'),
    ('Catalogos.TipoProducto'),
    ('Catalogos.CalibreApeam'),
    ('Catalogos.Marca'),
    ('Catalogos.ProductoTerminado'),
    ('Configuracion.ReportePlantilla'),
    ('Almacenes.MovimientoCajaCampo'),
    ('Auditoria.Registro');

-- TRUNCATE exige que ninguna FK apunte a la tabla, sin importar en qué sentido —
-- se desactivan todas las FK del proyecto antes de vaciar, y se reactivan al final
-- con WITH CHECK para no dejar ninguna constraint deshabilitada.
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' NOCHECK CONSTRAINT ALL;' + CHAR(10)
FROM sys.foreign_keys fk
INNER JOIN sys.tables t ON t.object_id = fk.parent_object_id
INNER JOIN sys.schemas s ON s.schema_id = t.schema_id;
EXEC sp_executesql @sql;

-- TRUNCATE también reinicia el IDENTITY al seed (0), así el próximo INSERT de cada
-- tabla arranca en Id = 1 sin necesidad de DBCC CHECKIDENT aparte.
DECLARE @Tabla NVARCHAR(261);
DECLARE cur CURSOR LOCAL FAST_FORWARD FOR SELECT Tabla FROM @Tablas;
OPEN cur;
FETCH NEXT FROM cur INTO @Tabla;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC('TRUNCATE TABLE ' + @Tabla);
    PRINT @Tabla + ': vaciada, próximo Id = 1';
    FETCH NEXT FROM cur INTO @Tabla;
END
CLOSE cur;
DEALLOCATE cur;

SET @sql = N'';
SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' WITH CHECK CHECK CONSTRAINT ALL;' + CHAR(10)
FROM sys.foreign_keys fk
INNER JOIN sys.tables t ON t.object_id = fk.parent_object_id
INNER JOIN sys.schemas s ON s.schema_id = t.schema_id;
EXEC sp_executesql @sql;

-- Folio consecutivo de Acuerdo de Corte también arranca en 1.
IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqAcuerdoCorteFolio' AND schema_id = SCHEMA_ID('Acopio'))
BEGIN
    ALTER SEQUENCE Acopio.SeqAcuerdoCorteFolio RESTART WITH 1;
    PRINT 'Acopio.SeqAcuerdoCorteFolio: reiniciada, próximo folio = 0000001';
END
GO

-- Folio consecutivo de Orden de Corte también arranca en 1.
IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqOrdenCorteFolio' AND schema_id = SCHEMA_ID('Acopio'))
BEGIN
    ALTER SEQUENCE Acopio.SeqOrdenCorteFolio RESTART WITH 1;
    PRINT 'Acopio.SeqOrdenCorteFolio: reiniciada, próximo folio = 0000001';
END
GO

-- Folio consecutivo de Recepción de Fruta también arranca en 1.
IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqRecepcionFrutaFolio' AND schema_id = SCHEMA_ID('Recepcion'))
BEGIN
    ALTER SEQUENCE Recepcion.SeqRecepcionFrutaFolio RESTART WITH 1;
    PRINT 'Recepcion.SeqRecepcionFrutaFolio: reiniciada, próximo folio = 0000001';
END
GO

-- Folio consecutivo de Lote también arranca en 1.
IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqLoteFolio' AND schema_id = SCHEMA_ID('Lotes'))
BEGIN
    ALTER SEQUENCE Lotes.SeqLoteFolio RESTART WITH 1;
    PRINT 'Lotes.SeqLoteFolio: reiniciada, próximo folio = 0000001';
END
GO
