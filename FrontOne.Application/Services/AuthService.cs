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
    private readonly IPasswordHasher _passwordHasher;
    private readonly AuditService _auditService;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        ICryptoService cryptoService,
        IPasswordHasher passwordHasher,
        AuditService auditService)
    {
        _usuarioRepository = usuarioRepository;
        _cryptoService = cryptoService;
        _passwordHasher = passwordHasher;
        _auditService = auditService;
    }

    public async Task<Result<UsuarioDto>> LoginAsync(string nombreUsuario, string password)
    {
        var usuario = await _usuarioRepository.ObtenerPorNombreUsuarioAsync(nombreUsuario);

        if (usuario is null || !usuario.Activo)
        {
            return Result.Failure<UsuarioDto>("Usuario o contraseña incorrectos");
        }

        if (!string.IsNullOrEmpty(usuario.PasswordHash))
        {
            // Ruta nueva: ya tiene hash PBKDF2, se verifica directo.
            if (!_passwordHasher.Verify(password, usuario.PasswordHash))
            {
                return Result.Failure<UsuarioDto>("Usuario o contraseña incorrectos");
            }
        }
        else
        {
            // Ruta legacy: todavía solo tiene el cifrado AES reversible. Se valida por ahí y,
            // si la contraseña es correcta, se aprovecha para grabar el hash nuevo de una vez
            // (rehash transparente) — así la próxima vez ya entra por la ruta nueva.
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

            var hashNuevo = _passwordHasher.Hash(password);
            await _usuarioRepository.ActualizarPasswordHashAsync(usuario.Id, hashNuevo);
        }

        await _auditService.RegistrarAsync(usuario.NombreUsuario, TipoAccionAuditoria.Login, "Seguridad");

        return Result.Success(new UsuarioDto(usuario.Id, usuario.NombreUsuario, usuario.NombreCompleto, usuario.Email, usuario.Activo));
    }
}
