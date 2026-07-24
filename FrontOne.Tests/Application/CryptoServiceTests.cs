using FrontOne.Shared.Security;

namespace FrontOne.Tests.Application;

public class CryptoServiceTests
{
    private readonly ICryptoService _cryptoService = new CryptoService();

    [Fact]
    public void Encrypt_Decrypt_DevuelveElTextoOriginal()
    {
        const string original = "Contraseña-Prueba-123!";

        var cifrado = _cryptoService.Encrypt(original);
        var descifrado = _cryptoService.Decrypt(cifrado);

        Assert.Equal(original, descifrado);
        Assert.NotEqual(original, cifrado);
    }

    [Fact]
    public void Encrypt_MismoTexto_DevuelveMismoCifrado_EnCualquierInstancia()
    {
        const string original = "otra-clave";
        var otraInstancia = new CryptoService();

        var cifrado1 = _cryptoService.Encrypt(original);
        var cifrado2 = otraInstancia.Encrypt(original);

        Assert.Equal(cifrado1, cifrado2);
    }
}
