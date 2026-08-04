using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IMovimientoAlmacenRepository
{
    Task<IReadOnlyList<SaldoCajaCampo>> ObtenerSaldosCajaCampoAsync();
    Task<IReadOnlyList<PerdidaCajaCampoMes>> ObtenerPerdidaCajaCampoMesAsync(int anio, int mes);
    Task InsertarMovimientoCajaCampoAsync(MovimientoCajaCampo movimiento);

    // Borra los movimientos ligados a un origen (Orden de Corte o Recepción) — usado antes de
    // volver a insertar en cada Actualizar, y también en Eliminar, para que el saldo del
    // Almacén nunca quede desfasado por una edición o borrado del registro origen.
    Task EliminarMovimientosCajaCampoPorOrigenAsync(string origenModulo, int origenId);
}
