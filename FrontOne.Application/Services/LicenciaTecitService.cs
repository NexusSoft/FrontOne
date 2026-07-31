using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class LicenciaTecitService
{
    private const string Modulo = "Seguridad";

    private readonly ILicenciaTecitRepository _licenciaTecitRepository;
    private readonly ICryptoService _cryptoService;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public LicenciaTecitService(
        ILicenciaTecitRepository licenciaTecitRepository,
        ICryptoService cryptoService,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _licenciaTecitRepository = licenciaTecitRepository;
        _cryptoService = cryptoService;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<LicenciaTecitDto> ObtenerAsync()
    {
        var licencia = await _licenciaTecitRepository.ObtenerAsync();
        return MapearDto(licencia);
    }

    public async Task ActualizarAsync(LicenciaTecitDto datos)
    {
        ValidarCampos(datos);

        var anterior = await _licenciaTecitRepository.ObtenerAsync();

        await _licenciaTecitRepository.ActualizarAsync(new LicenciaTecit
        {
            Licenciatario = datos.Licenciatario.Trim(),
            ClaveLicencia = string.IsNullOrWhiteSpace(datos.ClaveLicencia) ? null : _cryptoService.Encrypt(datos.ClaveLicencia.Trim()),
            TipoLicencia = string.IsNullOrWhiteSpace(datos.TipoLicencia) ? null : datos.TipoLicencia.Trim(),
            NumeroLicencias = datos.NumeroLicencias,
            Producto = string.IsNullOrWhiteSpace(datos.Producto) ? null : datos.Producto.Trim(),
        });

        var nuevo = await _licenciaTecitRepository.ObtenerAsync();
        await RegistrarAuditoriaAsync(anterior, nuevo);
    }

    // La clave de licencia nunca se incluye en el snapshot de auditoría (mismo criterio que el
    // password de Usuario) — solo se registra si cambió, no su contenido cifrado ni descifrado.
    private Task RegistrarAuditoriaAsync(LicenciaTecit anterior, LicenciaTecit nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = JsonSerializer.Serialize(new
        {
            anterior.Licenciatario,
            anterior.TipoLicencia,
            anterior.NumeroLicencias,
            anterior.Producto,
            ClaveModificada = false,
        });
        var valoresNuevos = JsonSerializer.Serialize(new
        {
            nuevo.Licenciatario,
            nuevo.TipoLicencia,
            nuevo.NumeroLicencias,
            nuevo.Producto,
            ClaveModificada = anterior.ClaveLicencia != nuevo.ClaveLicencia,
        });

        return _auditService.RegistrarAsync(usuario, TipoAccionAuditoria.Modificar, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(LicenciaTecitDto datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Licenciatario))
        {
            throw new ValidationException("El nombre del licenciatario es obligatorio");
        }
    }

    private LicenciaTecitDto MapearDto(LicenciaTecit l) => new(
        l.Licenciatario,
        string.IsNullOrWhiteSpace(l.ClaveLicencia) ? null : _cryptoService.Decrypt(l.ClaveLicencia),
        l.TipoLicencia,
        l.NumeroLicencias,
        l.Producto);
}
