using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using TECIT.TBarCode;
using FrontOne.WinForms.Reports.Barcodes;

namespace FrontOne.WinForms.Reports.Controles;

// Control de código de barras arrastrable desde el Diseñador de Reportes (ver
// Forms/Sistema/DisenadorReporteForm.cs, registrado en el toolbox vía XRToolboxService).
// CampoDato es un valor literal por defecto, pero como cualquier propiedad pública de un
// XRControl, el usuario puede asociarle un ExpressionBinding ("[Columna]") desde el Property
// Grid del Diseñador — el motor de reportes la resuelve contra la fila actual del DataSource
// antes de OnBeforePrint, así que aquí ya llega con el dato real.
public class XRBarcodeControl : XRPictureBox
{
    // La licencia se inyecta desde fuera (ver ReporteRecepcionFruta/reportes que la usen) —
    // este control no conoce servicios de aplicación, mantiene la separación de capas.
    public static LicenciaTecitDto? LicenciaActual { get; set; }

    [Category("Datos")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string CampoDato { get; set; } = string.Empty;

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public TipoSimbologiaBarcode Simbologia { get; set; } = TipoSimbologiaBarcode.Code128;

    protected override void OnBeforePrint(CancelEventArgs e)
    {
        base.OnBeforePrint(e);

        if (string.IsNullOrWhiteSpace(CampoDato))
        {
            Image = null;
            return;
        }

        using var barcode = new Barcode
        {
            BarcodeType = MapearTipo(Simbologia),
            Data = CampoDato,
        };

        TecitLicenciaHelper.AplicarLicencia(barcode, LicenciaActual);

        var anterior = Image;
        Image = barcode.DrawBitmap();
        anterior?.Dispose();
    }

    private static BarcodeType MapearTipo(TipoSimbologiaBarcode simbologia) => simbologia switch
    {
        TipoSimbologiaBarcode.Code128 => BarcodeType.Code128,
        TipoSimbologiaBarcode.Gs1_128 => BarcodeType.GS1_128,
        TipoSimbologiaBarcode.QRCode => BarcodeType.QRCode,
        TipoSimbologiaBarcode.DataMatrix => BarcodeType.DataMatrix,
        TipoSimbologiaBarcode.Pdf417 => BarcodeType.Pdf417,
        _ => BarcodeType.Code128,
    };
}
