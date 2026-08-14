using System.ComponentModel;
using System.Drawing;
using System.IO;
using DevExpress.Utils.Serializing;
using DevExpress.XtraReports.Expressions;
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
    // Resolución fija con la que se genera el bitmap del código de barras (independiente de la
    // resolución de impresión final) — suficiente para que el escáner lea bien a tamaño de
    // etiqueta; WidthF/HeightF del control están en centésimas de pulgada (mismo criterio que
    // PageWidth/PageHeight de EtiquetaReporte), de ahí la conversión a píxeles con este DPI.
    private const double ResolucionImagen = 300;


    // Registra CampoDato ante el End-User Report Designer para que aparezca en la lista de
    // propiedades del Expression Editor (el ícono "f" flotante sobre el control seleccionado)
    // — sin este registro, el Diseñador solo detecta como bindeables las propiedades nativas de
    // XRPictureBox (ImageSource, Tag, etc.), nunca las propiedades custom de la subclase, sin
    // importar los atributos que lleven. EventNames/ScopeName calcan los valores reales que
    // DevExpress usa para XRPictureBox.ImageSource (verificado por reflection contra
    // DevExpress.XtraReports.v26.1.dll vía ExpressionBindingDescriptor.TryGetPropertyDescription).
    static XRBarcodeControl()
    {
        ExpressionBindingDescriptor.SetPropertyDescription(
            typeof(XRBarcodeControl),
            nameof(CampoDato),
            new ExpressionBindingDescription(new[] { "BeforePrint", "PrintOnPage" }, 100, new string[0], string.Empty));
    }

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

    // Texto legible arriba del código en vez de abajo (antes quedaba fijo en false/abajo).
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public bool MostrarTextoArriba { get; set; }

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public Alignment AlineacionTexto { get; set; } = Alignment.Center;

    // Vacío = fuente default de TECIT (Arial). Solo tipografías instaladas en la máquina que
    // imprime — si el nombre no existe, TECIT cae de vuelta a su default sin error.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public string FuenteTexto { get; set; } = string.Empty;

    // Proporción ancho/angosto de barra (ej. "1:2", "1:3") — solo aplica a simbologías que la
    // usan (Code39, Codabar, Interleaved 2 of 5); vacío = default de TECIT según la simbología.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public string Proporcion { get; set; } = string.Empty;

    // Alinea el ancho de barra a la rejilla de píxeles de la resolución de impresión — clave para
    // que el código salga nítido y escaneable en impresoras térmicas de etiquetas (no solo en
    // pantalla/PDF). Default true: mejora la impresión real sin afectar la vista previa.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public bool AjustarAnchoModuloAPixel { get; set; } = true;

    // Color del texto legible, independiente del color de las barras (BarcodeColor).
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public Color ColorTexto { get; set; } = Color.Black;

    // Si el código no cabe en el rectángulo del control, TECIT lo reduce automáticamente en vez
    // de recortarlo o fallar — red de seguridad adicional a AjustarAnchoModuloAPixel/BoundingRectangleF.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public bool ForzarAjuste { get; set; }

    // Recorta el texto legible si no cabe en el ancho disponible, en vez de desbordarse fuera
    // del control (relevante con datos GS1-128 largos).
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public bool RecortarTexto { get; set; }

    // Permite que el texto legible salte de línea si no cabe en una sola — alternativa a
    // RecortarTexto (en vez de cortar, lo acomoda en 2+ líneas).
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public bool AjustarTextoEnLineas { get; set; }

    // Fondo transparente en vez de un color sólido — útil si el código va sobre una etiqueta con
    // color de fondo distinto a ColorFondo.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public bool FondoTransparente { get; set; }

    // Modo de codificación de la simbología (ej. en Code128: automático/A/B/C vía CodePage; en QR:
    // Numérico/Alfanumérico/Byte según el enum de TECIT) — relevante para simbologías 2D.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public EncodingMode ModoCodificacion { get; set; } = EncodingMode.CodePage;

    // Reduce el ancho de barra impreso una cantidad fija, para compensar la "ganancia de tinta"
    // de impresoras térmicas/láser (las barras salen más gruesas de lo digital). 0 = sin reducción.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public double ReduccionAnchoBarra { get; set; }

    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public BarWidthReductionUnit UnidadReduccionAnchoBarra { get; set; } = BarWidthReductionUnit.Percent;

    // Recorta espacios en blanco del dato antes de codificarlo — evita códigos inválidos si el
    // campo de origen trae espacios de más al inicio/final.
    [Category("Apariencia")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [XtraSerializableProperty]
    public TrimWhiteSpaces RecortarEspacios { get; set; } = TrimWhiteSpaces.None;

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
            // SizeMode.FitToBoundingRectangle no hace nada por sí solo — TECIT necesita que se le
            // diga explícitamente el rectángulo destino (en píxeles) para calcular el ancho de
            // barra que llena ese espacio. Sin esto, genera la imagen con su propio tamaño default
            // (independiente de WidthF/HeightF del control) y luego el Sizing=ZoomImage de
            // DevExpress la encoge/centra en vez de llenar el ancho real del control.
            var anchoPixeles = (float)(WidthF / 100f * ResolucionImagen);
            var altoPixeles = (float)(HeightF / 100f * ResolucionImagen);

            using var barcode = new Barcode
            {
                BarcodeType = Simbologia,
                Data = CampoDato,
                IsTextVisible = MostrarTexto,
                IsTextAbove = MostrarTextoArriba,
                HumanReadableText = CampoDato,
                TextAlignment = AlineacionTexto,
                ModuleWidth = ModuleWidth,
                BarcodeColor = BarcodeColor,
                BackgroundColor = ColorFondo,
                Rotation = Rotacion,
                FontHeight = TamanoTexto,
                TextDistance = DistanciaTexto,
                CheckdigitMethod = MetodoDigitoVerificador,
                AdjustModuleWidthToPixelRaster = AjustarAnchoModuloAPixel,
                FontColor = ColorTexto,
                MustFit = ForzarAjuste,
                TextClipping = RecortarTexto,
                WordWrapping = AjustarTextoEnLineas,
                IsBackgroundTransparent = FondoTransparente,
                EncodingMode = ModoCodificacion,
                BarWidthReduction = ReduccionAnchoBarra,
                BarWidthReductionUnit = UnidadReduccionAnchoBarra,
                TrimWhiteSpaces = RecortarEspacios,
                SizeMode = SizeMode.FitToBoundingRectangle,
                Dpi = ResolucionImagen,
                BoundingRectangleF = new RectangleF(0, 0, anchoPixeles, altoPixeles),
            };

            // Vacío = dejar el default de TECIT tal cual — asignar "" a estas dos propiedades sí
            // cambia comportamiento (no es un no-op como en la mayoría de las propiedades string).
            if (!string.IsNullOrWhiteSpace(FuenteTexto))
            {
                barcode.FontName = FuenteTexto;
            }

            if (!string.IsNullOrWhiteSpace(Proporcion))
            {
                barcode.Ratio = Proporcion;
            }

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
