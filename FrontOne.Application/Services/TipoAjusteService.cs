using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class TipoAjusteService
{
    private const string Modulo = "Gastos";

    private readonly ITipoAjusteRepository _tipoAjusteRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public TipoAjusteService(ITipoAjusteRepository tipoAjusteRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _tipoAjusteRepository = tipoAjusteRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<TipoAjusteDto>> ObtenerAsync()
    {
        var tipos = await _tipoAjusteRepository.ObtenerAsync();
        return tipos.Select(MapearDto).ToList();
    }

    public async Task<IReadOnlyList<TipoAjusteDto>> ObtenerPorTipoGastoAsync(byte tipoGasto)
    {
        var tipos = await _tipoAjusteRepository.ObtenerAsync();
        return tipos.Where(t => t.TipoGasto == tipoGasto && t.Activo).Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre, byte tipoGasto, byte signo)
    {
        ValidarCampos(nombre, tipoGasto, signo);

        var id = await _tipoAjusteRepository.InsertarAsync(new TipoAjuste
        {
            Nombre = nombre.Trim(),
            TipoGasto = tipoGasto,
            Signo = signo,
            Activo = true,
        });

        var creado = (await _tipoAjusteRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, byte tipoGasto, byte signo, bool activo)
    {
        ValidarCampos(nombre, tipoGasto, signo);

        var anterior = (await _tipoAjusteRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoAjusteRepository.ActualizarAsync(new TipoAjuste
        {
            Id = id,
            Nombre = nombre.Trim(),
            TipoGasto = tipoGasto,
            Signo = signo,
            Activo = activo,
        });

        var nuevo = (await _tipoAjusteRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _tipoAjusteRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoAjusteRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, TipoAjuste? anterior, TipoAjuste? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(string nombre, byte tipoGasto, byte signo)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre del tipo de ajuste es obligatorio");
        }

        if (tipoGasto is not (1 or 2))
        {
            throw new ValidationException("Selecciona el tipo de gasto (Cosecha o Acarreo)");
        }

        if (signo is not (1 or 2))
        {
            throw new ValidationException("Selecciona el signo del ajuste (A Favor o En Contra)");
        }
    }

    private static TipoAjusteDto MapearDto(TipoAjuste t) => new(t.Id, t.Nombre, t.TipoGasto, t.Signo, t.Activo);
}
