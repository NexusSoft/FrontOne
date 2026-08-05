USE FrontOne;
GO

-- ============================================================================
-- Deja en CERO todo el movimiento operativo (Acuerdos, Órdenes de Corte,
-- Recepciones, Lotes, Corridas y movimientos de Almacén), CONSERVANDO
-- catálogos, listas de precios, configuración y seguridad.
--
-- Diferencia con Inicializar_Datos_Produccion.sql: aquel vacía TODO (incluidos
-- catálogos) y es de un solo uso antes de salir a producción. Éste está pensado
-- para limpiar pruebas durante el desarrollo sin tener que volver a capturar
-- productores, huertas, precios, etc.
--
-- LO QUE SE BORRA:
--   Acopio.AcuerdoCorte, Acopio.OrdenCorte
--   Recepcion.RecepcionFruta (+ su detalle RecepcionFrutaOrdenCorte)
--   Lotes.Lote (+ su detalle LoteRecepcion)
--   Produccion.Corrida
--   Almacenes.MovimientoCajaCampo
--
-- LO QUE SE CONSERVA:
--   Auditoria.Registro — decisión explícita del usuario: la bitácora se conserva
--     como historial (incluye las cargas masivas de catálogos, no solo pruebas)
--   Todo Catalogos.* (Productor, Huerta, País/Estado/Municipio/Población,
--     Producto, CajaCampo, LineaProduccion, ProductoTerminado, Marca, etc.)
--   Todo lo de precios: Acopio.ListaPrecioFruta, Acopio.ListaPrecioCorte,
--     Acarreo.ListaPrecioAcarreo, Acarreo.Zona
--   Catálogos de apoyo de Acopio: TipoCorte, TipoPago, Variedad, Floracion,
--     TipoComercializacion, Moneda, JefeAcopio
--   Configuracion.* y TODO Seguridad.* (usuarios, roles, permisos)
--
-- Los folios (Acuerdo/Orden/Recepción/Lote) vuelven solos a 0000001 porque
-- desde Database/*/0xx_*Folio_Reutilizable.sql se calculan como MAX(Folio)+1
-- sobre la propia tabla — al quedar vacía, el siguiente arranca en 1. Por eso
-- este script NO necesita tocar ninguna SEQUENCE.
--
-- Se puede ejecutar las veces que haga falta (es idempotente).
-- ============================================================================

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Orden dictado por las FK: primero los hijos, luego los padres.
    -- (Produccion.Corrida -> Lotes.Lote; Lotes.LoteRecepcion -> Lote + RecepcionFruta;
    --  Recepcion.RecepcionFrutaOrdenCorte -> RecepcionFruta + OrdenCorte;
    --  Acopio.OrdenCorte -> Acopio.AcuerdoCorte)
    DELETE FROM Almacenes.MovimientoCajaCampo;
    PRINT 'Almacenes.MovimientoCajaCampo: vaciada';

    DELETE FROM Produccion.Corrida;
    PRINT 'Produccion.Corrida: vaciada';

    DELETE FROM Lotes.LoteRecepcion;
    PRINT 'Lotes.LoteRecepcion: vaciada';

    DELETE FROM Lotes.Lote;
    PRINT 'Lotes.Lote: vaciada';

    DELETE FROM Recepcion.RecepcionFrutaOrdenCorte;
    PRINT 'Recepcion.RecepcionFrutaOrdenCorte: vaciada';

    DELETE FROM Recepcion.RecepcionFruta;
    PRINT 'Recepcion.RecepcionFruta: vaciada';

    DELETE FROM Acopio.OrdenCorte;
    PRINT 'Acopio.OrdenCorte: vaciada';

    DELETE FROM Acopio.AcuerdoCorte;
    PRINT 'Acopio.AcuerdoCorte: vaciada';

    -- Auditoria.Registro NO se toca a propósito (ver encabezado).

    -- RESEED a 0 para que el próximo Id de cada tabla vuelva a ser 1. Se usa DELETE +
    -- CHECKIDENT en vez de TRUNCATE porque TRUNCATE no se permite en tablas referenciadas
    -- por una FK, y aquí no queremos desactivar constraints (menos riesgo).
    DBCC CHECKIDENT ('Almacenes.MovimientoCajaCampo', RESEED, 0) WITH NO_INFOMSGS;
    DBCC CHECKIDENT ('Produccion.Corrida',            RESEED, 0) WITH NO_INFOMSGS;
    DBCC CHECKIDENT ('Lotes.LoteRecepcion',           RESEED, 0) WITH NO_INFOMSGS;
    DBCC CHECKIDENT ('Lotes.Lote',                    RESEED, 0) WITH NO_INFOMSGS;
    DBCC CHECKIDENT ('Recepcion.RecepcionFrutaOrdenCorte', RESEED, 0) WITH NO_INFOMSGS;
    DBCC CHECKIDENT ('Recepcion.RecepcionFruta',      RESEED, 0) WITH NO_INFOMSGS;
    DBCC CHECKIDENT ('Acopio.OrdenCorte',             RESEED, 0) WITH NO_INFOMSGS;
    DBCC CHECKIDENT ('Acopio.AcuerdoCorte',           RESEED, 0) WITH NO_INFOMSGS;
    PRINT 'IDENTITY de las 8 tablas reiniciado: proximo Id = 1';

    COMMIT TRANSACTION;
    PRINT '';
    PRINT '>> LISTO: datos operativos en cero. Catalogos, precios y seguridad intactos.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT '';
    PRINT '>> ERROR: se hizo ROLLBACK, no se borro nada.';
    THROW;
END CATCH
GO
