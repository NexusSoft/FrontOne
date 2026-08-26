namespace FrontOne.Shared.Security;

/// <summary>
/// Genera y verifica hashes de contraseña con PBKDF2 (salt aleatorio por usuario).
/// Reemplaza gradualmente al cifrado AES reversible (<see cref="ICryptoService"/>), que
/// se mantiene solo como ruta legacy de compatibilidad durante la migración.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>Genera un hash nuevo con salt aleatorio para <paramref name="password"/>.</summary>
    string Hash(string password);

    /// <summary>Verifica que <paramref name="password"/> corresponda al <paramref name="hash"/> almacenado.</summary>
    bool Verify(string password, string hash);
}
