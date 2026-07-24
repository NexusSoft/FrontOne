using System.Security.Cryptography;
using System.Text;

namespace FrontOne.Shared.Security;

public class CryptoService : ICryptoService
{
    private const string Passphrase = "FrontOne-ERP-Empacadora-Aguacate-2026";
    private const string Salt = "FrontOne-Salt-Fijo-v1";
    private const int KeySizeBits = 256;
    private const int Iterations = 100_000;

    public string Encrypt(string plainText)
    {
        using var aes = CreateAes();
        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(cipherBytes);
    }

    public string Decrypt(string cipherText)
    {
        using var aes = CreateAes();
        using var decryptor = aes.CreateDecryptor();
        var cipherBytes = Convert.FromBase64String(cipherText);
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    private static Aes CreateAes()
    {
        var passphraseBytes = Encoding.UTF8.GetBytes(Passphrase);
        var saltBytes = Encoding.UTF8.GetBytes(Salt);
        var derivedBytes = Rfc2898DeriveBytes.Pbkdf2(passphraseBytes, saltBytes, Iterations, HashAlgorithmName.SHA256, (KeySizeBits / 8) + 16);

        var aes = Aes.Create();
        aes.KeySize = KeySizeBits;
        aes.Key = derivedBytes[..(KeySizeBits / 8)];
        aes.IV = derivedBytes[(KeySizeBits / 8)..];
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }
}
