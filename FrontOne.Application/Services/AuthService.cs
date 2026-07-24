using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Common;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class AuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICryptoService _cryptoService;
    private readonly AuditService _auditService;

    public AuthService(IUsuarioRepository usuarioRepository, ICryptoService cryptoService, AuditService auditService)
    {
        _usuarioRepository = usuarioRepository;
        _cryptoService = cryptoService;
        _auditService = auditService;
    }

    public async Task<Result<UsuarioDto>> LoginAsync(string nombreUsuario, string password)
    {
        var usuario = await _usuarioRepository.ObtenerPorNombreUsuarioAsync(nombreUsuario);

        if (usuario is null || !usuario.Activo)
        {
            return Result.Failure<UsuarioDto>("Usuario o contraseña incorrectos");
        }

        string passwordDesencriptado;
        try
        {
            passwordDesencriptado = _cryptoService.Decrypt(usuario.PasswordEncriptado);
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            return Result.Failure<UsuarioDto>("Usuario o contraseña incorrectos");
        }

        if (!string.Equals(passwordDesencriptado, password, StringComparison.Ordinal))
        {
            return Result.Failure<UsuarioDto>("Usuario o contraseña incorrectos");
        }

        await _auditService.RegistrarAsync(usuario.NombreUsuario, TipoAccionAuditoria.Login, "Seguridad");

        return Result.Success(new UsuarioDto(usuario.Id, usuario.NombreUsuario, usuario.NombreCompleto, usuario.Email, usuario.Activo));
    }
}
