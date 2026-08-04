namespace FrontOne.Domain.Enums;

// Las tres "cuentas" (ubicaciones) por las que pasa una caja de campo: empieza en Existencia
// (almacén/empaque), sale a EnCampo cuando se crea la Orden de Corte, y al recibirse la fruta se
// reparte entre Produccion (cajas que volvieron con fruta) y otra vez Existencia (cajas vacías).
public enum CuentaAlmacen
{
    Existencia,
    EnCampo,
    Produccion,
}
