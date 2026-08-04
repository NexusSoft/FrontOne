using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Almacenes;

public partial class AlmacenCajaCampoDashboardForm : XtraForm
{
    private readonly MovimientoAlmacenService _movimientoAlmacenService = null!;
    private readonly CajaCampoService _cajaCampoService = null!;

    public AlmacenCajaCampoDashboardForm()
    {
        InitializeComponent();
    }

    public AlmacenCajaCampoDashboardForm(MovimientoAlmacenService movimientoAlmacenService, CajaCampoService cajaCampoService)
        : this()
    {
        _movimientoAlmacenService = movimientoAlmacenService;
        _cajaCampoService = cajaCampoService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var dashboard = await _movimientoAlmacenService.ObtenerDashboardAsync();
        _grid.DataSource = dashboard.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["CajaCampoId"] is { } colId)
        {
            colId.Visible = false;
        }

        if (_gridView.Columns["CajaCampoNombre"] is { } colNombre)
        {
            colNombre.Caption = "Color de Caja";
        }

        if (_gridView.Columns["Existencia"] is { } colExistencia)
        {
            colExistencia.Caption = "Existencia";
        }

        if (_gridView.Columns["EnCampo"] is { } colEnCampo)
        {
            colEnCampo.Caption = "En Campo";
        }

        if (_gridView.Columns["Produccion"] is { } colProduccion)
        {
            colProduccion.Caption = "En Producción";
        }

        if (_gridView.Columns["PerdidaMes"] is { } colPerdida)
        {
            colPerdida.Caption = "Pérdida del Mes";
        }

        _gridView.BestFitColumns();
    }

    private AlmacenCajaCampoDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as AlmacenCajaCampoDto;

    private async void BtnCompra_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        using var form = new MovimientoCajaCampoEditarForm(_cajaCampoService, _movimientoAlmacenService, TipoMovimientoAlmacen.Entrada, seleccionado?.CajaCampoId);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnAjuste_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        using var form = new MovimientoCajaCampoEditarForm(_cajaCampoService, _movimientoAlmacenService, TipoMovimientoAlmacen.Salida, seleccionado?.CajaCampoId);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
