using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class SupervisoresHuertaForm : XtraForm
{
    private readonly SupervisorHuertaService _supervisorHuertaService = null!;

    public SupervisoresHuertaForm()
    {
        InitializeComponent();
    }

    public SupervisoresHuertaForm(SupervisorHuertaService supervisorHuertaService)
        : this()
    {
        _supervisorHuertaService = supervisorHuertaService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var supervisores = await _supervisorHuertaService.ObtenerAsync();
        _grid.DataSource = supervisores.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["Id"] is { } colId)
        {
            colId.Visible = false;
        }
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new SupervisorHuertaEditarForm(_supervisorHuertaService, null);
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
            XtraMessageBox.Show(this, "Selecciona un supervisor de huerta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new SupervisorHuertaEditarForm(_supervisorHuertaService, seleccionado);
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
            XtraMessageBox.Show(this, "Selecciona un supervisor de huerta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el supervisor de huerta '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _supervisorHuertaService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => BtnEditar_Click(sender, e);

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private SupervisorHuertaDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as SupervisorHuertaDto;
}
