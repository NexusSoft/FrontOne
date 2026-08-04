namespace FrontOne.Domain.Entities;

// Proyección agregada para el dashboard del Almacén — saldo actual (Entradas - Salidas) de las
// 3 cuentas por las que pasa una caja de campo (ver CuentaAlmacen), por color. Se calcula en el
// SP, nunca se guarda un total mutable en la BD.
public class SaldoCajaCampo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Existencia { get; set; }
    public int EnCampo { get; set; }
    public int Produccion { get; set; }
}
