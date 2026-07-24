using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class TiposPagoForm : XtraForm
{
    private readonly TipoPagoService _tipoPagoService = null!;

    public TiposPagoForm()
    {
        InitializeComponent();
    }

    public TiposPagoForm(TipoPagoService tipoPagoService)
        : this()
    {
        _tipoPagoService = tipoPagoService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var tiposPago = await _tipoPagoService.ObtenerAsync();
        _grid.DataSource = tiposPago.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["Id"] is { } colId)
        {
            colId.Visible = false;
        }

        if (_gridView.Columns["NecesitaListaPrecios"] is { } colNecesitaLista)
        {
            colNecesitaLista.Caption = "Necesita lista";
        }
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new TipoPagoEditarForm(_tipoPagoService, null);
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
            XtraMessageBox.Show(this, "Selecciona un tipo de pago.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new TipoPagoEditarForm(_tipoPagoService, seleccionado);
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
            XtraMessageBox.Show(this, "Selecciona un tipo de pago.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el tipo de pago '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _tipoPagoService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private TipoPagoDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as TipoPagoDto;
}
