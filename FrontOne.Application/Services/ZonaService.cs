using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class ZonaService
{
    private const string Modulo = "Acarreo";

    private readonly IZonaRepository _zonaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ZonaService(IZonaRepository zonaRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _zonaRepository = zonaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<ZonaDto>> ObtenerAsync()
    {
        var zonas = await _zonaRepository.ObtenerAsync();
        return zonas.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre, decimal kgMinimo300, decimal kgMinimo400, decimal kgMinimo500)
    {
        ValidarCampos(nombre, kgMinimo300, kgMinimo400, kgMinimo500);

        var id = await _zonaRepository.InsertarAsync(new Zona
        {
            Nombre = nombre.Trim(),
            KgMinimo300 = kgMinimo300,
            KgMinimo400 = kgMinimo400,
            KgMinimo500 = kgMinimo500,
            Activo = true,
        });

        var creado = (await _zonaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, decimal kgMinimo300, decimal kgMinimo400, decimal kgMinimo500, bool activo)
    {
        ValidarCampos(nombre, kgMinimo300, kgMinimo400, kgMinimo500);

        var anterior = (await _zonaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _zonaRepository.ActualizarAsync(new Zona
        {
            Id = id,
            Nombre = nombre.Trim(),
            KgMinimo300 = kgMinimo300,
            KgMinimo400 = kgMinimo400,
            KgMinimo500 = kgMinimo500,
            Activo = activo,
        });

        var nuevo = (await _zonaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _zonaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _zonaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Zona? anterior, Zona? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(string nombre, decimal kgMinimo300, decimal kgMinimo400, decimal kgMinimo500)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre de la zona es obligatorio");
        }

        if (kgMinimo300 < 0 || kgMinimo400 < 0 || kgMinimo500 < 0)
        {
            throw new ValidationException("Los kg mínimos no pueden ser negativos");
        }
    }

    private static ZonaDto MapearDto(Zona z) => new(z.Id, z.Nombre, z.KgMinimo300, z.KgMinimo400, z.KgMinimo500, z.Activo);
}
