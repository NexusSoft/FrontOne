using System.Security.Cryptography;

namespace FrontOne.Shared.Security;

/// <summary>
/// Implementación PBKDF2 (SHA256, 210 000 iteraciones, salt de 16 bytes) de <see cref="IPasswordHasher"/>.
/// Formato de almacenamiento: "v1.{iteraciones}.{saltBase64}.{hashBase64}".
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private const int Iteraciones = 210_000;
    private const int TamanioSaltBytes = 16;
    private const int TamanioHashBytes = 32;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(TamanioSaltBytes);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iteraciones, HashAlgorithmName.SHA256, TamanioHashBytes);

        return $"v1.{Iteraciones}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string hash)
    {
        var partes = hash.Split('.');
        if (partes.Length != 4 || partes[0] != "v1")
        {
            return false;
        }

        if (!int.TryParse(partes[1], out var iteraciones))
        {
            return false;
        }

        byte[] salt;
        byte[] hashEsperado;
        try
        {
            salt = Convert.FromBase64String(partes[2]);
            hashEsperado = Convert.FromBase64String(partes[3]);
        }
        catch (FormatException)
        {
            return false;
        }

        var hashCalculado = Rfc2898DeriveBytes.Pbkdf2(password, salt, iteraciones, HashAlgorithmName.SHA256, hashEsperado.Length);

        return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
    }
}
