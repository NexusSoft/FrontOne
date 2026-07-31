using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class EmpresaConfiguracionService
{
    private const string Modulo = "Seguridad";

    private readonly IEmpresaConfiguracionRepository _empresaConfiguracionRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public EmpresaConfiguracionService(
        IEmpresaConfiguracionRepository empresaConfiguracionRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _empresaConfiguracionRepository = empresaConfiguracionRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<EmpresaConfiguracionDto> ObtenerAsync()
    {
        var empresa = await _empresaConfiguracionRepository.ObtenerAsync();
        return MapearDto(empresa);
    }

    public async Task ActualizarAsync(EmpresaConfiguracionDto datos)
    {
        ValidarCampos(datos);

        var anterior = await _empresaConfiguracionRepository.ObtenerAsync();

        await _empresaConfiguracionRepository.ActualizarAsync(new EmpresaConfiguracion
        {
            RazonSocial = datos.RazonSocial.Trim(),
            Domicilio = string.IsNullOrWhiteSpace(datos.Domicilio) ? null : datos.Domicilio.Trim(),
            Rfc = string.IsNullOrWhiteSpace(datos.Rfc) ? null : datos.Rfc.Trim(),
            Telefono = string.IsNullOrWhiteSpace(datos.Telefono) ? null : datos.Telefono.Trim(),
            Correo = string.IsNullOrWhiteSpace(datos.Correo) ? null : datos.Correo.Trim(),
            Logo = datos.Logo,
            NumeroEmpaque = string.IsNullOrWhiteSpace(datos.NumeroEmpaque) ? null : datos.NumeroEmpaque.Trim(),
        });

        var nuevo = await _empresaConfiguracionRepository.ObtenerAsync();
        await RegistrarAuditoriaAsync(anterior, nuevo);
    }

    // El Logo no se incluye en el snapshot de auditoría (evita inflar Auditoria.Registro con
    // binarios) — solo se registra si cambió, no su contenido.
    private Task RegistrarAuditoriaAsync(EmpresaConfiguracion anterior, EmpresaConfiguracion nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = JsonSerializer.Serialize(new
        {
            anterior.RazonSocial,
            anterior.Domicilio,
            anterior.Rfc,
            anterior.Telefono,
            anterior.Correo,
            anterior.NumeroEmpaque,
            LogoModificado = false,
        });
        var valoresNuevos = JsonSerializer.Serialize(new
        {
            nuevo.RazonSocial,
            nuevo.Domicilio,
            nuevo.Rfc,
            nuevo.Telefono,
            nuevo.Correo,
            nuevo.NumeroEmpaque,
            LogoModificado = !LogosIguales(anterior.Logo, nuevo.Logo),
        });

        return _auditService.RegistrarAsync(usuario, TipoAccionAuditoria.Modificar, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static bool LogosIguales(byte[]? a, byte[]? b) => (a, b) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        _ => a.AsSpan().SequenceEqual(b),
    };

    private static void ValidarCampos(EmpresaConfiguracionDto datos)
    {
        if (string.IsNullOrWhiteSpace(datos.RazonSocial))
        {
            throw new ValidationException("La razón social es obligatoria");
        }

        if (!string.IsNullOrWhiteSpace(datos.NumeroEmpaque) && datos.NumeroEmpaque.Trim().Length != 3)
        {
            throw new ValidationException("El número de empaque debe tener exactamente 3 caracteres");
        }
    }

    private static EmpresaConfiguracionDto MapearDto(EmpresaConfiguracion e) => new(
        e.RazonSocial, e.Domicilio, e.Rfc, e.Telefono, e.Correo, e.Logo, e.NumeroEmpaque);
}
