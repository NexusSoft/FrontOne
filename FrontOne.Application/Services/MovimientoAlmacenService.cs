using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class MovimientoAlmacenService
{
    private readonly IMovimientoAlmacenRepository _movimientoAlmacenRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public MovimientoAlmacenService(
        IMovimientoAlmacenRepository movimientoAlmacenRepository,
        ICurrentUserProvider currentUserProvider)
    {
        _movimientoAlmacenRepository = movimientoAlmacenRepository;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<AlmacenCajaCampoDto>> ObtenerDashboardAsync()
    {
        var hoy = DateTime.Today;
        var saldos = await _movimientoAlmacenRepository.ObtenerSaldosCajaCampoAsync();
        var perdidas = await _movimientoAlmacenRepository.ObtenerPerdidaCajaCampoMesAsync(hoy.Year, hoy.Month);

        return saldos
            .Select(s => new AlmacenCajaCampoDto(
                s.Id,
                s.Nombre,
                s.Existencia,
                s.EnCampo,
                s.Produccion,
                perdidas.FirstOrDefault(p => p.CajaCampoId == s.Id)?.CajasPerdidas ?? 0))
            .ToList();
    }

    // Registra una compra o un ajuste manual de inventario desde el dashboard — siempre afecta la
    // cuenta Existencia (es la única que el usuario corrige a mano; EnCampo/Produccion solo se
    // mueven automáticamente desde Orden de Corte/Recepción). Cantidad siempre positiva, el signo
    // lo da TipoMovimiento (Entrada para compra, Salida para ajuste a la baja).
    public Task RegistrarMovimientoManualAsync(int cajaCampoId, TipoMovimientoAlmacen tipo, short cantidad, string? observaciones)
    {
        if (cajaCampoId <= 0)
        {
            throw new ValidationException("Selecciona el color de caja de campo.");
        }

        if (cantidad <= 0)
        {
            throw new ValidationException("La cantidad debe ser mayor a cero.");
        }

        return _movimientoAlmacenRepository.InsertarMovimientoCajaCampoAsync(new MovimientoCajaCampo
        {
            Fecha = DateTime.Today,
            CajaCampoId = cajaCampoId,
            Cuenta = CuentaAlmacen.Existencia.ToString(),
            TipoMovimiento = tipo.ToString(),
            Cantidad = cantidad,
            OrigenModulo = OrigenMovimientoAlmacen.Manual.ToString(),
            OrigenId = null,
            Observaciones = observaciones,
            Usuario = _currentUserProvider.NombreUsuario ?? "desconocido",
        });
    }
}
