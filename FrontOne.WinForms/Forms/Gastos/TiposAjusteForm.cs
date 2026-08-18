using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Gastos;

public partial class TiposAjusteForm : XtraForm
{
    private readonly TipoAjusteService _tipoAjusteService = null!;

    public TiposAjusteForm()
    {
        InitializeComponent();
    }

    public TiposAjusteForm(TipoAjusteService tipoAjusteService)
        : this()
    {
        _tipoAjusteService = tipoAjusteService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var tipos = await _tipoAjusteService.ObtenerAsync();
        _grid.DataSource = tipos.ToList();
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new TipoAjusteEditarForm(_tipoAjusteService, null);
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
            XtraMessageBox.Show(this, "Selecciona un tipo de ajuste.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new TipoAjusteEditarForm(_tipoAjusteService, seleccionado);
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
            XtraMessageBox.Show(this, "Selecciona un tipo de ajuste.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el tipo de ajuste '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _tipoAjusteService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private TipoAjusteDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as TipoAjusteDto;
}
