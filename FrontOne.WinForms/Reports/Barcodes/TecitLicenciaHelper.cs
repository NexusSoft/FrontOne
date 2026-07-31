using FrontOne.Domain.DTOs;
using TECIT.TBarCode;

namespace FrontOne.WinForms.Reports.Barcodes;

// Aplica la licencia de TECIT TBarCode.NET a un Barcode recién creado — se llama una vez por
// cada Barcode que se construye (no hay licenciamiento global de proceso en la API de TECIT).
public static class TecitLicenciaHelper
{
    public static void AplicarLicencia(Barcode barcode, LicenciaTecitDto? licencia)
    {
        if (licencia is null || string.IsNullOrWhiteSpace(licencia.ClaveLicencia))
        {
            return;
        }

        var tipoLicencia = Enum.TryParse<LicenseType>(licencia.TipoLicencia, ignoreCase: true, out var tipo)
            ? tipo
            : LicenseType.Single;

        barcode.License(
            licencia.Licenciatario,
            tipoLicencia,
            licencia.NumeroLicencias ?? 1,
            licencia.ClaveLicencia,
            TBarCodeProduct.Barcode1D);
    }
}
