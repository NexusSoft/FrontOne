using System.ComponentModel;
using System.Drawing;
using System.IO;
using DevExpress.Utils.Serializing;
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

    // [XtraSerializableProperty] es indispensable en TODAS las propiedades de este control —
    // sin él, DevExpress persiste el valor en el Property Grid del Diseñador (WinForms designer
    // serialization) pero NO lo lleva al árbol de reporte que realmente se usa para generar
    // Vista Previa/impresión (XtraSerializer interno, un mecanismo aparte). Confirmado en
    // pruebas: sin este atributo, OnBeforePrint sí se ejecuta pero recibe los valores default.
    [Category("Datos")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public string CampoDato { get; set; } = string.Empty;

    // Se expone el enum completo de TECIT (BarcodeType, ~120 simbologías: Code128, GS1_128,
    // Ean13, QRCode, DataMatrix, Pdf417, etc.) en vez de un subconjunto propio — la licencia del
    // usuario (Barcode2D u otro producto) puede cubrir cualquiera de ellas.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public BarcodeType Simbologia { get; set; } = BarcodeType.Code128;

    // Texto legible (el mismo CampoDato) debajo del código — comportamiento default de TECIT
    // cuando IsTextVisible=true e IsTextAbove=false.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public bool MostrarTexto { get; set; } = true;

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public double ModuleWidth { get; set; } = 0.33;

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public Color BarcodeColor { get; set; } = Color.Black;

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public Color ColorFondo { get; set; } = Color.White;

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public Rotation Rotacion { get; set; } = Rotation.Degree0;

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public int TamanoTexto { get; set; } = 10;

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public float DistanciaTexto { get; set; }

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public CheckdigitMethod MetodoDigitoVerificador { get; set; } = CheckdigitMethod.None;

    // Tamaño por defecto al arrastrarlo del toolbox — sin esto el control cae con el tamaño
    // default de XRPictureBox (muy chico), y TECIT dibuja "Control's boundaries are too small
    // for the barcode" en vez del código. El usuario puede redimensionarlo libremente después.
    public XRBarcodeControl()
    {
        SizeF = new System.Drawing.SizeF(300, 80);
    }

    protected override void OnBeforePrint(CancelEventArgs e)
    {
        base.OnBeforePrint(e);

        if (string.IsNullOrWhiteSpace(CampoDato))
        {
            Image = null;
            return;
        }

        var anterior = Image;
        try
        {
            using var barcode = new Barcode
            {
                BarcodeType = Simbologia,
                Data = CampoDato,
                IsTextVisible = MostrarTexto,
                IsTextAbove = false,
                HumanReadableText = CampoDato,
                ModuleWidth = ModuleWidth,
                BarcodeColor = BarcodeColor,
                BackgroundColor = ColorFondo,
                Rotation = Rotacion,
                FontHeight = TamanoTexto,
                TextDistance = DistanciaTexto,
                CheckdigitMethod = MetodoDigitoVerificador,
                SizeMode = SizeMode.FitToBoundingRectangle,
            };

            TecitLicenciaHelper.AplicarLicencia(barcode, LicenciaActual);

            Image = GenerarImagen(barcode);
        }
        catch (Exception ex)
        {
            // Cualquier falla (licencia no cargada, dígito verificador incorrecto, dato
            // inválido para la simbología, etc.) queda visible en rojo — sin este catch el
            // control queda en blanco sin ninguna pista de qué pasó.
            Image = CrearImagenError(ex.GetType().Name + ": " + ex.Message);
        }

        anterior?.Dispose();
    }

    // TECIT solo incluye el texto legible cuando se dibuja a un ARCHIVO (Draw(ruta, ImageType))
    // — ni DrawBitmap(ancho, alto) ni Draw(Graphics) en memoria lo muestran (confirmado con
    // pruebas aisladas). Se dibuja a un archivo temporal y se carga a un Bitmap independiente
    // (mismo patrón que ConfiguracionEmpresaForm.ImagenDesdeBytes) para poder borrar el archivo
    // de inmediato sin que GDI+ truene por el lock.
    private Bitmap GenerarImagen(Barcode barcode)
    {
        var rutaTemporal = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.png");
        try
        {
            barcode.Draw(rutaTemporal, ImageType.Png);
            using var original = Image.FromFile(rutaTemporal);
            return new Bitmap(original);
        }
        finally
        {
            if (File.Exists(rutaTemporal))
            {
                File.Delete(rutaTemporal);
            }
        }
    }

    private Bitmap CrearImagenError(string mensaje)
    {
        var ancho = Math.Max((int)WidthF, 1);
        var alto = Math.Max((int)HeightF, 1);
        var bitmap = new Bitmap(ancho, alto);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        using var font = new Font("Arial", 8);
        using var brush = new SolidBrush(Color.Red);
        graphics.DrawString(mensaje, font, brush, new RectangleF(2, 2, ancho - 4, alto - 4));
        return bitmap;
    }
}
