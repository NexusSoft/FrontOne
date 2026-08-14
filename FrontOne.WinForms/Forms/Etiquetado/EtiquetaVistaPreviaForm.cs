using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Reports.Controles;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Etiquetado;

// Pide un No. Pallet real, carga los datos según el Tipo de la etiqueta seleccionada y abre el
// mismo VisorReporteForm que usan los reportes — reutilizado, no se crea un visor aparte.
public partial class EtiquetaVistaPreviaForm : XtraForm
{
    private readonly EtiquetaDto _etiqueta = null!;
    private readonly PalletService _palletService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly LicenciaTecitService _licenciaTecitService = null!;
    private readonly SessionContext _sessionContext = null!;

    // Los 3 wrappers son planos (mismos nombres de campo que el SP de diseño conectado en
    // EtiquetasForm.ConectarOrigenDatosDiseno) para que las ExpressionBindings que el usuario
    // arma en el Diseñador ([Campo]) resuelvan igual en tiempo de diseño y en esta vista previa
    // — regla dura del proyecto (ver bug real ya corregido en ReportePallet/ReporteRecepcionFruta).
    private sealed record VistaCaja(
        string NoPallet, DateTime FechaPallet, DateTime FechaEmpacado, string? NumeroEmpaque,
        string NoLote, string? CodigoTrazabilidad, DateTime FechaLote, DateTime? FechaOrdenCorteMax,
        decimal MateriaSecaLote, string? Productor, string? Huerta, string? RegistroSagarpa, string? RegistroGgn,
        string? Municipio, string NombreProductoTerminado, string? TipoProducto, string? Categoria, string? CalibreApeam,
        string? MercadoDestino, string? Marca, string? Variedad, decimal? PesoEstandar, string? CodigoCalibreExterno,
        string? CodigoUpc, string? CodigoPlu, string? CodigoGtin, string? CodigoGs1128, string? VoiceCodeLow, string? VoiceCodeHigh,
        byte[]? LogoUsdaOrganic);

    private sealed record VistaPalletEncabezado(
        string NoPallet, string Status, string NombreProducto,
        string? RazonSocial, string? Domicilio, string? Rfc, string? Telefono, string? Correo, byte[]? Logo,
        int TotalCajas, decimal TotalKilogramos);

    public EtiquetaVistaPreviaForm()
    {
        InitializeComponent();
    }

    public EtiquetaVistaPreviaForm(
        EtiquetaDto etiqueta,
        PalletService palletService,
        EmpresaConfiguracionService empresaConfiguracionService,
        LicenciaTecitService licenciaTecitService,
        SessionContext sessionContext)
        : this()
    {
        _etiqueta = etiqueta;
        _palletService = palletService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _licenciaTecitService = licenciaTecitService;
        _sessionContext = sessionContext;

        // XRBarcodeControl lee la licencia de TECIT desde este estático (mismo criterio que
        // PalletImprimirEtiquetaForm/DisenadorReporteForm) — sin esto el código de barras siempre
        // imprime con la marca de agua "Demo" aunque haya licencia configurada.
        FormClosed += (_, _) => XRBarcodeControl.LicenciaActual = null;
    }

    private async void BtnVer_Click(object? sender, EventArgs e)
    {
        XRBarcodeControl.LicenciaActual = await _licenciaTecitService.ObtenerAsync();

        var folio = _txtFolio.Text.Trim();
        if (string.IsNullOrWhiteSpace(folio))
        {
            XtraMessageBox.Show(this, "Captura el No. Pallet.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var pallets = await _palletService.ObtenerAsync();
        var pallet = pallets.FirstOrDefault(p => string.Equals(p.Folio, folio, StringComparison.OrdinalIgnoreCase));
        if (pallet is null)
        {
            XtraMessageBox.Show(this, $"No se encontró el pallet '{folio}'.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var reporte = EtiquetaReporte.Crear(_etiqueta);

        switch (_etiqueta.Tipo)
        {
            case TipoEtiqueta.Caja:
                if (!await CargarCajaAsync(reporte, pallet.Id))
                {
                    return;
                }

                break;
            case TipoEtiqueta.Pallet:
                if (!await CargarPalletAsync(reporte, pallet.Id))
                {
                    return;
                }

                break;
            case TipoEtiqueta.RegistroSagarpa:
                if (!await CargarSagarpaAsync(reporte, pallet.Id))
                {
                    return;
                }

                break;
        }

        var codigoTipo = EtiquetasForm.CodigoTipo(_etiqueta.Tipo);
        using var visor = new VisorReporteForm(reporte, codigoTipo, _sessionContext);
        visor.ShowDialog(this);
    }

    private async Task<bool> CargarCajaAsync(DevExpress.XtraReports.UI.XtraReport reporte, int palletId)
    {
        var datos = await _palletService.ObtenerDatosEtiquetaCajaAsync(palletId);
        if (datos is null)
        {
            XtraMessageBox.Show(this, "Este pallet todavía no tiene lotes capturados.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        var empresa = await _empresaConfiguracionService.ObtenerAsync();

        // CodigoGs1128/VoiceCodeLow/VoiceCodeHigh ya vienen calculados y guardados a nivel de
        // línea de detalle del Pallet (Produccion.PalletDetalle) desde que se capturó la caja —
        // ya no se recalculan aquí en cada vista previa.
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

    private async Task<bool> CargarPalletAsync(DevExpress.XtraReports.UI.XtraReport reporte, int palletId)
    {
        var encabezado = await _palletService.ObtenerDatosEtiquetaPalletEncabezadoAsync(palletId);
        if (encabezado is null)
        {
            XtraMessageBox.Show(this, "El pallet ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        var detalle = await _palletService.ObtenerDatosEtiquetaPalletDetalleAsync(palletId);
        var empresa = await _empresaConfiguracionService.ObtenerAsync();

        var vista = new VistaPalletEncabezado(
            encabezado.NoPallet,
            encabezado.Estatus == 3 ? "Completo" : "Incompleto",
            encabezado.NombreProducto,
            empresa.RazonSocial, empresa.Domicilio, empresa.Rfc, empresa.Telefono, empresa.Correo, empresa.Logo,
            detalle.Sum(d => d.Cajas), detalle.Sum(d => d.Kilogramos));

        reporte.DataSource = new List<VistaPalletEncabezado> { vista };
        reporte.DataMember = null;

        // Se busca por TIPO, no por nombre: el usuario arma el layout desde un lienzo en blanco
        // en el Diseñador y le pone el nombre que DevExpress le asigne por default al arrastrar
        // el DetailReportBand del toolbox — no hay forma de asumir un nombre fijo.
        var detailReportBand = reporte.Bands.OfType<DevExpress.XtraReports.UI.DetailReportBand>().FirstOrDefault();
        if (detailReportBand is not null)
        {
            detailReportBand.DataSource = detalle.ToList();
            detailReportBand.DataMember = null;
        }

        return true;
    }

    private async Task<bool> CargarSagarpaAsync(DevExpress.XtraReports.UI.XtraReport reporte, int palletId)
    {
        var datos = await _palletService.ObtenerDatosEtiquetaSagarpaAsync(palletId);
        if (datos is null)
        {
            XtraMessageBox.Show(this, "Este pallet no tiene ninguna huerta asociada.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        reporte.DataSource = new List<EtiquetaSagarpaDatosDto> { datos };
        reporte.DataMember = null;
        return true;
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
