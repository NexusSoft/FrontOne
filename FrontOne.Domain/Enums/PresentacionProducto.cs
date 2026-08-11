namespace FrontOne.Domain.Enums;

// Presentación del Producto Terminado: Caja (empacado en cajas, Peso Estándar × Cajas = Kilogramos)
// o Granel (sin cajas, se pesa directo). Campo 100% local de FrontOne, no sincroniza con SAP.
public enum PresentacionProducto
{
    Caja = 1,
    Granel = 2,
}
