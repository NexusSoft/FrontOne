using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class TipoPagoService
{
    private const string Modulo = "Acopio";

    private readonly ITipoPagoRepository _tipoPagoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public TipoPagoService(ITipoPagoRepository tipoPagoRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _tipoPagoRepository = tipoPagoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<TipoPagoDto>> ObtenerAsync()
    {
        var tiposPago = await _tipoPagoRepository.ObtenerAsync();
        return tiposPago.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre, bool necesitaListaPrecios)
    {
        ValidarCampos(nombre);

        var id = await _tipoPagoRepository.InsertarAsync(new TipoPago { Nombre = nombre.Trim(), NecesitaListaPrecios = necesitaListaPrecios, Activo = true });

        var creado = (await _tipoPagoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool necesitaListaPrecios, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _tipoPagoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoPagoRepository.ActualizarAsync(new TipoPago { Id = id, Nombre = nombre.Trim(), NecesitaListaPrecios = necesitaListaPrecios, Activo = activo });

        var nuevo = (await _tipoPagoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _tipoPagoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoPagoRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, TipoPago? anterior, TipoPago? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre del tipo de pago es obligatorio");
        }
    }

    private static TipoPagoDto MapearDto(TipoPago tipoPago) => new(tipoPago.Id, tipoPago.Nombre, tipoPago.NecesitaListaPrecios, tipoPago.Activo);
}
