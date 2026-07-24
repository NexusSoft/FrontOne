using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class ListaPrecioAcarreoService
{
    private const string Modulo = "Acarreo";

    private readonly IListaPrecioAcarreoRepository _listaPrecioAcarreoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ListaPrecioAcarreoService(
        IListaPrecioAcarreoRepository listaPrecioAcarreoRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _listaPrecioAcarreoRepository = listaPrecioAcarreoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<ListaPrecioAcarreoDto>> ObtenerAsync()
    {
        var lista = await _listaPrecioAcarreoRepository.ObtenerAsync();
        return lista.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(
        int municipioId, int zonaId, decimal precio300, decimal precio400, decimal precio500, int toleranciaKgFaltante)
    {
        await ValidarAsync(null, municipioId, zonaId, precio300, precio400, precio500);

        var id = await _listaPrecioAcarreoRepository.InsertarAsync(new ListaPrecioAcarreo
        {
            MunicipioId = municipioId,
            ZonaId = zonaId,
            Precio300 = precio300,
            Precio400 = precio400,
            Precio500 = precio500,
            ToleranciaKgFaltante = toleranciaKgFaltante,
            Activo = true,
        });

        var creado = (await _listaPrecioAcarreoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(
        int id, int municipioId, int zonaId, decimal precio300, decimal precio400, decimal precio500, int toleranciaKgFaltante, bool activo)
    {
        await ValidarAsync(id, municipioId, zonaId, precio300, precio400, precio500);

        var anterior = (await _listaPrecioAcarreoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _listaPrecioAcarreoRepository.ActualizarAsync(new ListaPrecioAcarreo
        {
            Id = id,
            MunicipioId = municipioId,
            ZonaId = zonaId,
            Precio300 = precio300,
            Precio400 = precio400,
            Precio500 = precio500,
            ToleranciaKgFaltante = toleranciaKgFaltante,
            Activo = activo,
        });

        var nuevo = (await _listaPrecioAcarreoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _listaPrecioAcarreoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _listaPrecioAcarreoRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    // Un municipio solo puede tener un renglón de precios (nunca dos zonas/precios distintos
    // para el mismo municipio) — se valida acá para dar un mensaje claro en vez del error crudo
    // del índice único de la tabla.
    private async Task ValidarAsync(int? id, int municipioId, int zonaId, decimal precio300, decimal precio400, decimal precio500)
    {
        if (municipioId <= 0)
        {
            throw new ValidationException("Selecciona el municipio");
        }

        if (zonaId <= 0)
        {
            throw new ValidationException("Selecciona la zona");
        }

        if (precio300 < 0 || precio400 < 0 || precio500 < 0)
        {
            throw new ValidationException("Los precios no pueden ser negativos");
        }

        var existente = (await _listaPrecioAcarreoRepository.ObtenerAsync())
            .FirstOrDefault(l => l.MunicipioId == municipioId && l.Id != (id ?? 0));
        if (existente is not null)
        {
            throw new ValidationException($"El municipio '{existente.MunicipioNombre}' ya tiene un renglón de precios capturado");
        }
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, ListaPrecioAcarreo? anterior, ListaPrecioAcarreo? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static ListaPrecioAcarreoDto MapearDto(ListaPrecioAcarreo l) => new(
        l.Id,
        l.MunicipioId,
        l.MunicipioNombre,
        l.EstadoNombre,
        l.ZonaId,
        l.ZonaNombre,
        l.Precio300,
        l.Precio400,
        l.Precio500,
        l.ToleranciaKgFaltante,
        l.Activo);
}
