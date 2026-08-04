using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class MovimientoAlmacenRepository : SqlRepositoryBase, IMovimientoAlmacenRepository
{
    public MovimientoAlmacenRepository(IConnectionFactory connectionFactory, ILogger<MovimientoAlmacenRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<SaldoCajaCampo>> ObtenerSaldosCajaCampoAsync()
        => QueryAsync<SaldoCajaCampo>("Almacenes.sp_MovimientoCajaCampo_ObtenerSaldos");

    public Task<IReadOnlyList<PerdidaCajaCampoMes>> ObtenerPerdidaCajaCampoMesAsync(int anio, int mes)
        => QueryAsync<PerdidaCajaCampoMes>("Almacenes.sp_MovimientoCajaCampo_ObtenerPerdidaMes", new { Anio = anio, Mes = mes });

    public Task InsertarMovimientoCajaCampoAsync(MovimientoCajaCampo movimiento)
        => ExecuteAsync("Almacenes.sp_MovimientoCajaCampo_Insertar", new
        {
            movimiento.Fecha,
            movimiento.CajaCampoId,
            movimiento.Cuenta,
            movimiento.TipoMovimiento,
            movimiento.Cantidad,
            movimiento.OrigenModulo,
            movimiento.OrigenId,
            movimiento.Observaciones,
            movimiento.Usuario,
        });

    public Task EliminarMovimientosCajaCampoPorOrigenAsync(string origenModulo, int origenId)
        => ExecuteAsync("Almacenes.sp_MovimientoCajaCampo_EliminarPorOrigen", new { OrigenModulo = origenModulo, OrigenId = origenId });
}
