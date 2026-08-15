using System.Drawing.Printing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraReports.UI;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Shared.Constants;
using FrontOne.WinForms.Forms.Etiquetado;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Reports.Controles;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Pallets;

// Formulario compartido de impresión de etiquetas de trazabilidad, usado desde PalletEditarForm en
// 3 puntos: columna "Etiqueta Caja" (Tipo Caja) y columna "Etiqueta Lote" (Tipo RegistroSagarpa)
// por renglón del detalle — ambas con datos de ESE renglón específico — y botón "Imprimir
// Papeleta" (Tipo Pallet, encabezado + detalle agrupado). A diferencia de EtiquetaVistaPreviaForm
// no pide folio (el Pallet ya se conoce) y en vez de abrir VisorReporteForm imprime directo a la
// impresora elegida por el usuario, sin diálogo adicional de Windows.
public partial class PalletImprimirEtiquetaForm : XtraForm
{
    private readonly EtiquetaService _etiquetaService = null!;
    private readonly PalletService _palletService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly LicenciaTecitService _licenciaTecitService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly TipoEtiqueta _tipo;
    private readonly int _palletId;
    private readonly int? _palletDetalleId;

    private List<EtiquetaDto> _etiquetas = new();
    private XtraReport? _reporteActual;

    // Mismo criterio de wrappers planos que EtiquetaVistaPreviaForm: los nombres de propiedad
    // calcan las ExpressionBindings ([Campo]) que el usuario arma en el Diseñador.
    private sealed record VistaCaja(
        string NoPallet, DateTime FechaPallet, DateTime FechaEmpacado, string? NumeroEmpaque,
        string NoLote, string? CodigoTrazabilidad, DateTime FechaLote, DateTime? FechaOrdenCorteMax,
        decimal MateriaSecaLote, string? Productor, string? Huerta, string? RegistroSagarpa, string? RegistroGgn,
        string? Municipio, string NombreProductoTerminado, string? TipoProducto, string? Categoria, string? CalibreApeam,
        string? MercadoDestino, string? Marca, string? Variedad, decimal? PesoEstandar, string? CodigoCalibreExterno,
        string? CodigoUpc, string? CodigoPlu, string? CodigoGtin, string? CodigoGs1128, string? VoiceCodeLow, string? VoiceCodeHigh,
        byte[]? LogoUsdaOrganic);

    private sealed record VistaPalletEncabezado(
        string NoPallet, DateTime FechaProcesado, string Status, string NombreProducto, decimal? PesoEstandar,
        string? RazonSocial, string? Domicilio, string? Rfc, string? Telefono, string? Correo, byte[]? Logo,
        int TotalCajas, decimal TotalKilogramos);

    public PalletImprimirEtiquetaForm()
    {
        InitializeComponent();
    }

    public PalletImprimirEtiquetaForm(
        EtiquetaService etiquetaService,
        PalletService palletService,
        EmpresaConfiguracionService empresaConfiguracionService,
        LicenciaTecitService licenciaTecitService,
        SessionContext sessionContext,
        TipoEtiqueta tipo,
        int palletId,
        int cantidadPorDefecto,
        int? palletDetalleId = null)   // requerido para Caja y RegistroSagarpa (por renglón); ignorado en Pallet
        : this()
    {
        _etiquetaService = etiquetaService;
        _palletService = palletService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _licenciaTecitService = licenciaTecitService;
        _sessionContext = sessionContext;
        _tipo = tipo;
        _palletId = palletId;
        _palletDetalleId = palletDetalleId;

        _txtCantidad.EditValue = cantidadPorDefecto > 0 ? cantidadPorDefecto : 1;

        Load += async (_, _) => await CargarEtiquetasAsync();

        // XRBarcodeControl lee la licencia de TECIT desde este estático (ver DisenadorReporteForm,
        // el único otro punto que lo hacía) — sin esto el código de barras siempre imprime con la
        // marca de agua "Demo" fuera del Diseñador, aunque haya licencia configurada.
        FormClosed += (_, _) => XRBarcodeControl.LicenciaActual = null;
    }

    private async Task CargarEtiquetasAsync()
    {
        XRBarcodeControl.LicenciaActual = await _licenciaTecitService.ObtenerAsync();

        var todas = await _etiquetaService.ObtenerTodosAsync();
        _etiquetas = todas.Where(e => e.Activo && e.Tipo == _tipo).ToList();

        _cmbEtiqueta.Properties.DataSource = _etiquetas;
        _cmbEtiqueta.Properties.ValueMember = "Id";
        _cmbEtiqueta.Properties.DisplayMember = "Nombre";
        _cmbEtiqueta.Properties.Columns.Clear();
        _cmbEtiqueta.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Nombre de la etiqueta"));
        _cmbEtiqueta.Properties.PopupWidth = 260;

        CargarImpresoras();
    }

    private void CargarImpresoras()
    {
        var impresoras = PrinterSettings.InstalledPrinters
            .Cast<string>()
            .Select(nombre => new ImpresoraItem(nombre))
            .ToList();

        _cmbImpresora.Properties.DataSource = impresoras;
        _cmbImpresora.Properties.ValueMember = "Nombre";
        _cmbImpresora.Properties.DisplayMember = "Nombre";
        _cmbImpresora.Properties.Columns.Clear();
        _cmbImpresora.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 300, "Impresora"));
        _cmbImpresora.Properties.PopupWidth = 320;

        var predeterminada = new PrinterSettings().PrinterName;
        if (impresoras.Any(i => i.Nombre == predeterminada))
        {
            _cmbImpresora.EditValue = predeterminada;
        }
    }

    private async void CmbEtiqueta_EditValueChanged(object? sender, EventArgs e)
    {
        _reporteActual = null;
        _printControl.PrintingSystem = null;

        if (_cmbEtiqueta.EditValue is not int etiquetaId)
        {
            ActualizarBotonImprimir();
            return;
        }

        var etiqueta = _etiquetas.FirstOrDefault(et => et.Id == etiquetaId);
        if (etiqueta is null)
        {
            ActualizarBotonImprimir();
            return;
        }

        var reporte = EtiquetaReporte.Crear(etiqueta);

        var cargado = _tipo switch
        {
            TipoEtiqueta.Caja => await CargarCajaAsync(reporte),
            TipoEtiqueta.Pallet => await CargarPalletAsync(reporte),
            TipoEtiqueta.RegistroSagarpa => await CargarSagarpaAsync(reporte),
            _ => false,
        };

        if (!cargado)
        {
            ActualizarBotonImprimir();
            return;
        }

        reporte.CreateDocument();
        _printControl.PrintingSystem = reporte.PrintingSystem;
        _reporteActual = reporte;

        ActualizarBotonImprimir();
    }

    private async Task<bool> CargarCajaAsync(XtraReport reporte)
    {
        if (_palletDetalleId is null)
        {
            return false;
        }

        var datos = await _palletService.ObtenerDatosEtiquetaCajaPorDetalleAsync(_palletDetalleId.Value);
        if (datos is null)
        {
            XtraMessageBox.Show(this, "El renglón seleccionado ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        var empresa = await _empresaConfiguracionService.ObtenerAsync();

        var vista = new VistaCaja(
            datos.NoPallet, datos.FechaPallet, datos.FechaEmpacado, datos.NumeroEmpaque,
            datos.NoLote, datos.CodigoTrazabilidad, datos.FechaLote, datos.FechaOrdenCorteMax,
            datos.MateriaSecaLote, datos.Productor, datos.Huerta, datos.RegistroSagarpa, datos.RegistroGgn,
            datos.Municipio, datos.NombreProductoTerminado, datos.TipoProducto, datos.Categoria, datos.CalibreApeam,
            datos.MercadoDestino, datos.Marca, datos.Variedad, datos.PesoEstandar, datos.CodigoCalibreExterno,
            datos.CodigoUpc, datos.CodigoPlu, datos.CodigoGtin, datos.CodigoGs1128, datos.VoiceCodeLow, datos.VoiceCodeHigh,
            empresa.LogoUsdaOrganic);

        reporte.DataSource = new List<VistaCaja> { vista };
        reporte.DataMember = null;
        return true;
    }

    private async Task<bool> CargarPalletAsync(XtraReport reporte)
    {
        var encabezado = await _palletService.ObtenerDatosEtiquetaPalletEncabezadoAsync(_palletId);
        if (encabezado is null)
        {
            XtraMessageBox.Show(this, "El pallet ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        var detalle = await _palletService.ObtenerDatosEtiquetaPalletDetalleAsync(_palletId);
        var empresa = await _empresaConfiguracionService.ObtenerAsync();

        var vista = new VistaPalletEncabezado(
            encabezado.NoPallet,
            encabezado.FechaProcesado,
            PalletsForm.NombreEstatus(encabezado.Estatus),
            encabezado.NombreProducto,
            encabezado.PesoEstandar,
            empresa.RazonSocial, empresa.Domicilio, empresa.Rfc, empresa.Telefono, empresa.Correo, empresa.Logo,
            detalle.Sum(d => d.Cajas), detalle.Sum(d => d.Kilogramos));

        reporte.DataSource = new List<VistaPalletEncabezado> { vista };
        reporte.DataMember = null;

        var detailReportBand = reporte.Bands.OfType<DetailReportBand>().FirstOrDefault();
        if (detailReportBand is not null)
        {
            detailReportBand.DataSource = detalle.ToList();
            detailReportBand.DataMember = null;
        }

        return true;
    }

    private async Task<bool> CargarSagarpaAsync(XtraReport reporte)
    {
        if (_palletDetalleId is null)
        {
            return false;
        }

        var datos = await _palletService.ObtenerDatosEtiquetaSagarpaPorDetalleAsync(_palletDetalleId.Value);
        if (datos is null)
        {
            XtraMessageBox.Show(this, "El renglón seleccionado ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        reporte.DataSource = new List<EtiquetaSagarpaDatosDto> { datos };
        reporte.DataMember = null;
        return true;
    }

    private void CmbImpresora_EditValueChanged(object? sender, EventArgs e) => ActualizarBotonImprimir();

    private void TxtCantidad_EditValueChanged(object? sender, EventArgs e) => ActualizarBotonImprimir();

    private void ActualizarBotonImprimir()
    {
        var cantidadValida = _txtCantidad.EditValue is not null && Convert.ToInt32(_txtCantidad.EditValue) > 0;

        _btnImprimir.Enabled = _reporteActual is not null
            && _cmbImpresora.EditValue is string
            && cantidadValida;
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        if (_reporteActual is null || _cmbImpresora.EditValue is not string impresora)
        {
            return;
        }

        var codigoTipo = EtiquetasForm.CodigoTipo(_tipo);
        if (!_sessionContext.TienePermisoReporte(codigoTipo, AccionReporte.Impresion))
        {
            XtraMessageBox.Show(this, "No tienes permiso para imprimir este tipo de etiqueta.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cantidad = Convert.ToInt32(_txtCantidad.EditValue);

        var printTool = new ReportPrintTool(_reporteActual);
        printTool.PrinterSettings.PrinterName = impresora;
        printTool.PrinterSettings.Copies = (short)cantidad;
        printTool.Print();

        XtraMessageBox.Show(this, "Etiqueta enviada a imprimir.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private sealed record ImpresoraItem(string Nombre);
}
