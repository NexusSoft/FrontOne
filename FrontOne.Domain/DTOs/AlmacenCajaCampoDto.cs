namespace FrontOne.Domain.DTOs;

// Una fila del dashboard del Almacén de Caja de Campo: saldo de las 3 cuentas (Existencia/EnCampo/
// Produccion, ver CuentaAlmacen) y pérdida acumulada en el mes en curso, por color.
public record AlmacenCajaCampoDto(int CajaCampoId, string CajaCampoNombre, int Existencia, int EnCampo, int Produccion, int PerdidaMes);
