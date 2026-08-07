using System.Text.Json;
using System.Text.RegularExpressions;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

// Singleton de configuración, mismo patrón que EmpresaConfiguracionService: solo Obtener y
// Actualizar (nunca Crear ni Eliminar — la fila Id = 1 la crea el script de esquema).
public class ConfiguracionBasculaService
{
    private const string Modulo = "Seguridad";

    private readonly IConfiguracionBasculaRepository _configuracionBasculaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ConfiguracionBasculaService(
        IConfiguracionBasculaRepository configuracionBasculaRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _configuracionBasculaRepository = configuracionBasculaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<ConfiguracionBasculaDto> ObtenerAsync()
        => MapearDto(await _configuracionBasculaRepository.ObtenerAsync());

    public async Task ActualizarAsync(ConfiguracionBasculaDto datos)
    {
        ValidarCampos(datos);

        var anterior = await _configuracionBasculaRepository.ObtenerAsync();

        await _configuracionBasculaRepository.ActualizarAsync(new ConfiguracionBascula
        {
            Puerto = datos.Puerto.Trim(),
            BaudRate = datos.BaudRate,
            Parity = datos.Parity,
            DataBits = datos.DataBits,
            StopBits = datos.StopBits,
            PatronLectura = string.IsNullOrWhiteSpace(datos.PatronLectura) ? null : datos.PatronLectura.Trim(),
        });

        var nuevo = await _configuracionBasculaRepository.ObtenerAsync();
        await RegistrarAuditoriaAsync(anterior, nuevo);
    }

    private Task RegistrarAuditoriaAsync(ConfiguracionBascula anterior, ConfiguracionBascula nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        return _auditService.RegistrarAsync(
            usuario,
            TipoAccionAuditoria.Modificar,
            Modulo,
            JsonSerializer.Serialize(anterior),
            JsonSerializer.Serialize(nuevo));
    }

    private static void ValidarCampos(ConfiguracionBasculaDto datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Puerto))
        {
            throw new ValidationException("El puerto de la báscula es obligatorio");
        }

        if (datos.BaudRate <= 0)
        {
            throw new ValidationException("El baud rate debe ser mayor a cero");
        }

        if (datos.DataBits is < 5 or > 8)
        {
            throw new ValidationException("Los bits de datos deben estar entre 5 y 8");
        }

        // El patrón se valida acá y no al leer la báscula: si es una expresión regular inválida,
        // más vale que el usuario se entere al guardar la configuración y no en medio del pesaje.
        if (!string.IsNullOrWhiteSpace(datos.PatronLectura))
        {
            try
            {
                _ = Regex.Match(string.Empty, datos.PatronLectura);
            }
            catch (ArgumentException)
            {
                throw new ValidationException("El patrón de lectura no es una expresión regular válida");
            }
        }
    }

    private static ConfiguracionBasculaDto MapearDto(ConfiguracionBascula b) => new(
        b.Puerto, b.BaudRate, b.Parity, b.DataBits, b.StopBits, b.PatronLectura);
}
