using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class CajasCampoForm : XtraForm
{
    private readonly CajaCampoService _cajaCampoService = null!;

    public CajasCampoForm()
    {
        InitializeComponent();
    }

    public CajasCampoForm(CajaCampoService cajaCampoService)
        : this()
    {
        _cajaCampoService = cajaCampoService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var cajas = await _cajaCampoService.ObtenerAsync();
        _grid.DataSource = cajas.ToList();
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new CajaCampoEditarForm(_cajaCampoService, null);
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
            XtraMessageBox.Show(this, "Selecciona una caja de campo.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new CajaCampoEditarForm(_cajaCampoService, seleccionado);
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
            XtraMessageBox.Show(this, "Selecciona una caja de campo.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar la caja de campo '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _cajaCampoService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private CajaCampoDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as CajaCampoDto;
}
