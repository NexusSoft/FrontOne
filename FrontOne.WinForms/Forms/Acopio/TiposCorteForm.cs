using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class TiposCorteForm : XtraForm
{
    private readonly TipoCorteService _tipoCorteService = null!;
    private readonly TipoPagoService _tipoPagoService = null!;

    public TiposCorteForm()
    {
        InitializeComponent();
    }

    public TiposCorteForm(TipoCorteService tipoCorteService, TipoPagoService tipoPagoService)
        : this()
    {
        _tipoCorteService = tipoCorteService;
        _tipoPagoService = tipoPagoService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var tiposCorte = await _tipoCorteService.ObtenerAsync();
        _grid.DataSource = tiposCorte.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "Id", "TipoPagoId" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Nombre"] is { } colNombre)
        {
            colNombre.Caption = "Tipo de corte";
        }

        if (_gridView.Columns["FueraDeNormaGr"] is { } colFueraDeNorma)
        {
            colFueraDeNorma.Caption = "Fuera de norma (gr)";
            colFueraDeNorma.DisplayFormat.FormatType = FormatType.Numeric;
            colFueraDeNorma.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["DanioMinimo"] is { } colDanioMinimo)
        {
            colDanioMinimo.Caption = "Daño mínimo";
        }

        if (_gridView.Columns["TipoPagoNombre"] is { } colTipoPago)
        {
            colTipoPago.Caption = "Tipo de pago";
        }

        _gridView.BestFitColumns();
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new TipoCorteEditarForm(_tipoCorteService, _tipoPagoService, null);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un tipo de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new TipoCorteEditarForm(_tipoCorteService, _tipoPagoService, seleccionado);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un tipo de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el tipo de corte '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _tipoCorteService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private TipoCorteDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as TipoCorteDto;
}
