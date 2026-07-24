using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class MonedaService
{
    private const string Modulo = "Acopio";

    private readonly IMonedaRepository _monedaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public MonedaService(IMonedaRepository monedaRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _monedaRepository = monedaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<MonedaDto>> ObtenerAsync()
    {
        var monedas = await _monedaRepository.ObtenerAsync();
        return monedas.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre, string nomenclatura)
    {
        ValidarCampos(nombre, nomenclatura);

        var id = await _monedaRepository.InsertarAsync(new Moneda { Nombre = nombre.Trim(), Nomenclatura = nomenclatura.Trim().ToUpperInvariant(), Activo = true });

        var creado = (await _monedaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, string nomenclatura, bool activo)
    {
        ValidarCampos(nombre, nomenclatura);

        var anterior = (await _monedaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _monedaRepository.ActualizarAsync(new Moneda { Id = id, Nombre = nombre.Trim(), Nomenclatura = nomenclatura.Trim().ToUpperInvariant(), Activo = activo });

        var nuevo = (await _monedaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _monedaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _monedaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Moneda? anterior, Moneda? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(string nombre, string nomenclatura)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre de la moneda es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(nomenclatura))
        {
            throw new ValidationException("La nomenclatura de la moneda es obligatoria");
        }
    }

    private static MonedaDto MapearDto(Moneda m) => new(m.Id, m.Nombre, m.Nomenclatura, m.Activo);
}
