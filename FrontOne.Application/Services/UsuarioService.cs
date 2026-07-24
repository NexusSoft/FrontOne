using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class UsuarioService
{
    private const string Modulo = "Seguridad";

    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICryptoService _cryptoService;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        ICryptoService cryptoService,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _usuarioRepository = usuarioRepository;
        _cryptoService = cryptoService;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<UsuarioAdminDto>> ObtenerAsync()
    {
        var usuarios = await _usuarioRepository.ObtenerAsync();
        var resultado = new List<UsuarioAdminDto>();

        foreach (var usuario in usuarios)
        {
            resultado.Add(await MapearDtoAsync(usuario));
        }

        return resultado;
    }

    public async Task<int> CrearAsync(UsuarioAdminDto datos)
    {
        ValidarCampos(datos, esNuevo: true);

        var entidad = MapearEntidad(datos);
        var id = await _usuarioRepository.InsertarAsync(entidad);
        await _usuarioRepository.AsignarRolesAsync(id, datos.RolIds);

        var creado = (await _usuarioRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(UsuarioAdminDto datos)
    {
        ValidarCampos(datos, esNuevo: false);

        var anterior = (await _usuarioRepository.ObtenerAsync(datos.Id)).FirstOrDefault();

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;

        if (string.IsNullOrEmpty(datos.Password))
        {
            entidad.PasswordEncriptado = anterior?.PasswordEncriptado ?? string.Empty;
        }

        await _usuarioRepository.ActualizarAsync(entidad);
        await _usuarioRepository.AsignarRolesAsync(datos.Id, datos.RolIds);

        var nuevo = (await _usuarioRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _usuarioRepository.ObtenerAsync(id)).FirstOrDefault();

        await _usuarioRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Usuario? anterior, Usuario? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(UsuarioAdminDto datos, bool esNuevo)
    {
        if (string.IsNullOrWhiteSpace(datos.NombreUsuario))
        {
            throw new ValidationException("El nombre de usuario es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(datos.NombreCompleto))
        {
            throw new ValidationException("El nombre completo es obligatorio");
        }

        if (esNuevo && string.IsNullOrWhiteSpace(datos.Password))
        {
            throw new ValidationException("La contraseña es obligatoria para un usuario nuevo");
        }
    }

    private Usuario MapearEntidad(UsuarioAdminDto datos) => new()
    {
        NombreUsuario = datos.NombreUsuario.Trim(),
        NombreCompleto = datos.NombreCompleto.Trim(),
        Email = datos.Email,
        PasswordEncriptado = string.IsNullOrEmpty(datos.Password) ? string.Empty : _cryptoService.Encrypt(datos.Password),
        Activo = datos.Activo,
    };

    private async Task<UsuarioAdminDto> MapearDtoAsync(Usuario usuario)
    {
        var rolIds = await _usuarioRepository.ObtenerRolesAsync(usuario.Id);

        return new UsuarioAdminDto(
            usuario.Id,
            usuario.NombreUsuario,
            usuario.NombreCompleto,
            usuario.Email,
            null,
            usuario.Activo,
            rolIds);
    }
}
