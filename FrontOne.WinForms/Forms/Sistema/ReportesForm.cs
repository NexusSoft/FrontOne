using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using FrontOne.Application.Services;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Constants;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Sistema;

public partial class ReportesForm : XtraForm
{
    private readonly ReportePlantillaService _reportePlantillaService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly SqlOptions _sqlOptions = null!;
    private readonly LicenciaTecitService _licenciaTecitService = null!;

    private record FilaReporte(string Codigo, string Nombre, bool Personalizado, bool Predeterminado, DateTime? FechaModificacion);

    public ReportesForm()
    {
        InitializeComponent();
    }

    public ReportesForm(
        ReportePlantillaService reportePlantillaService,
        SessionContext sessionContext,
        SqlOptions sqlOptions,
        LicenciaTecitService licenciaTecitService)
        : this()
    {
        _reportePlantillaService = reportePlantillaService;
        _sessionContext = sessionContext;
        _sqlOptions = sqlOptions;
        _licenciaTecitService = licenciaTecitService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var filas = new List<FilaReporte>();
        foreach (var reporte in CatalogoReportes.Todos)
        {
            var plantilla = await _reportePlantillaService.ObtenerPorCodigoAsync(reporte.Codigo);
            var personalizado = !string.IsNullOrWhiteSpace(plantilla?.DefinicionXml);
            filas.Add(new FilaReporte(reporte.Codigo, reporte.Nombre, personalizado, plantilla?.EsPredeterminado ?? false,
                personalizado ? plantilla!.FechaModificacion : null));
        }

        _grid.DataSource = filas;
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["Codigo"] is { } colCodigo)
        {
            colCodigo.Visible = false;
        }

        if (_gridView.Columns["Personalizado"] is { } colPersonalizado)
        {
            colPersonalizado.Caption = "Personalizado";
        }

        if (_gridView.Columns["Predeterminado"] is { } colPredeterminado)
        {
            colPredeterminado.Caption = "Predeterminado";
        }

        if (_gridView.Columns["FechaModificacion"] is { } colFecha)
        {
            colFecha.Caption = "Última Modificación";
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
        }

        _gridView.BestFitColumns();
    }

    private async void BtnDisenar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!_sessionContext.TienePermisoReporte(seleccionado.Codigo, AccionReporte.Diseno))
        {
            XtraMessageBox.Show(this, "No tienes permiso para diseñar este reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var descriptor = CatalogoReportes.Todos.First(r => r.Codigo == seleccionado.Codigo);
        var reporte = descriptor.CrearReporteDefault();
        await DisenadorReporteForm.MostrarAsync(
            this,
            async () => (await _reportePlantillaService.ObtenerPorCodigoAsync(descriptor.Codigo))?.DefinicionXml,
            xml => _reportePlantillaService.GuardarAsync(descriptor.Codigo, descriptor.Nombre, xml),
            reporte,
            r => ConectarOrigenDatos(r, descriptor.Codigo),
            DesconectarOrigenDatos,
            _licenciaTecitService);

        await CargarDatosAsync();
    }

    // No hay un registro específico seleccionado desde este catálogo (es por tipo de reporte,
    // no por dato de negocio) — se conecta con un Id de referencia (0) solo para que el
    // Diseñador arme el Field List a partir de la forma del resultado del SP, sin depender de
    // que exista una fila real.
    private void ConectarOrigenDatos(XtraReport reporte, string codigo)
    {
        switch (codigo)
        {
            case "RecepcionFruta":
                ((ReporteRecepcionFruta)reporte).ConectarOrigenDatos(_sqlOptions, 0);
                break;
            case "Pallet":
                ((ReportePallet)reporte).ConectarOrigenDatos(_sqlOptions, 0);
                break;
            case "Incidencias":
                ((ReporteIncidencias)reporte).ConectarOrigenDatos(_sqlOptions, DateTime.Today, DateTime.Today);
                break;
            case "ProcesoLote":
                ((ReporteProcesoLote)reporte).ConectarOrigenDatos(_sqlOptions, 0);
                break;
            case "LiquidacionProductor":
                ((ReporteLiquidacionProductor)reporte).ConectarOrigenDatos(_sqlOptions, 0);
                break;
        }
    }

    private static void DesconectarOrigenDatos(XtraReport reporte)
    {
        switch (reporte)
        {
            case ReporteRecepcionFruta reporteRecepcionFruta:
                reporteRecepcionFruta.DesconectarOrigenDatos();
                break;
            case ReportePallet reportePallet:
                reportePallet.DesconectarOrigenDatos();
                break;
            case ReporteIncidencias reporteIncidencias:
                reporteIncidencias.DesconectarOrigenDatos();
                break;
            case ReporteProcesoLote reporteProcesoLote:
                reporteProcesoLote.DesconectarOrigenDatos();
                break;
            case ReporteLiquidacionProductor reporteLiquidacionProductor:
                reporteLiquidacionProductor.DesconectarOrigenDatos();
                break;
        }
    }

    private async void BtnRestablecer_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!_sessionContext.TienePermisoReporte(seleccionado.Codigo, AccionReporte.Diseno))
        {
            XtraMessageBox.Show(this, "No tienes permiso para diseñar este reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!seleccionado.Personalizado)
        {
            XtraMessageBox.Show(this, "Este reporte todavía usa el diseño default, no tiene nada que restablecer.",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Restablecer '{seleccionado.Nombre}' a su diseño original? Se perderá el diseño personalizado.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        await _reportePlantillaService.EliminarAsync(seleccionado.Codigo);
        await CargarDatosAsync();
    }

    private async void BtnMarcarPredeterminado_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (seleccionado.Predeterminado)
        {
            XtraMessageBox.Show(this, "Ese reporte ya es el predeterminado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // El catálogo de reportes ya no agrupa por pantalla (cada pantalla usa un único Código
        // de reporte fijo, ver RecepcionesFrutaForm.CodigoReporte) — no hay "hermanos" que
        // desmarcar como predeterminado.
        await _reportePlantillaService.MarcarPredeterminadoAsync(seleccionado.Codigo, seleccionado.Nombre, []);
        await CargarDatosAsync();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private FilaReporte? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as FilaReporte;
}
