using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.WinForms.Reports;

namespace FrontOne.WinForms.Forms.Sistema;

public partial class ReportesForm : XtraForm
{
    private readonly ReportePlantillaService _reportePlantillaService = null!;

    private record FilaReporte(string Codigo, string Nombre, string Pantalla, bool Personalizado, bool Predeterminado, DateTime? FechaModificacion);

    public ReportesForm()
    {
        InitializeComponent();
    }

    public ReportesForm(ReportePlantillaService reportePlantillaService)
        : this()
    {
        _reportePlantillaService = reportePlantillaService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var filas = new List<FilaReporte>();
        foreach (var reporte in CatalogoReportes.Todos)
        {
            var plantilla = await _reportePlantillaService.ObtenerPorCodigoAsync(reporte.Codigo);
            var personalizado = !string.IsNullOrWhiteSpace(plantilla?.DefinicionXml);
            filas.Add(new FilaReporte(reporte.Codigo, reporte.Nombre, reporte.Pantalla, personalizado, plantilla?.EsPredeterminado ?? false,
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

        var descriptor = CatalogoReportes.Todos.First(r => r.Codigo == seleccionado.Codigo);
        var reporte = descriptor.CrearReporteDefault();
        await DisenadorReporteForm.MostrarAsync(this, _reportePlantillaService, descriptor.Codigo, descriptor.Nombre, reporte);

        await CargarDatosAsync();
    }

    private async void BtnRestablecer_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            XtraMessageBox.Show(this, "Ese reporte ya es el predeterminado de su pantalla.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var otrosCodigos = CatalogoReportes.ObtenerPorPantalla(seleccionado.Pantalla)
            .Where(r => r.Codigo != seleccionado.Codigo)
            .Select(r => r.Codigo)
            .ToList();

        await _reportePlantillaService.MarcarPredeterminadoAsync(seleccionado.Codigo, seleccionado.Nombre, otrosCodigos);
        await CargarDatosAsync();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private FilaReporte? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as FilaReporte;
}
