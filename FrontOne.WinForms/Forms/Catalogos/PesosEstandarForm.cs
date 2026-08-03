using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class PesosEstandarForm : XtraForm
{
    private readonly PesoEstandarService _pesoEstandarService = null!;

    public PesosEstandarForm()
    {
        InitializeComponent();
    }

    public PesosEstandarForm(PesoEstandarService pesoEstandarService)
        : this()
    {
        _pesoEstandarService = pesoEstandarService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var pesosEstandar = await _pesoEstandarService.ObtenerAsync();
        _grid.DataSource = pesosEstandar.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["Id"] is { } colId)
        {
            colId.Visible = false;
        }

        if (_gridView.Columns["Codigo"] is { } colCodigo)
        {
            colCodigo.Caption = "Código";
        }

        if (_gridView.Columns["Descripcion"] is { } colDescripcion)
        {
            colDescripcion.Caption = "Descripción";
        }

        if (_gridView.Columns["PesoNeto"] is { } colPesoNeto)
        {
            colPesoNeto.Caption = "Peso neto";
        }

        if (_gridView.Columns["PesoPromedio"] is { } colPesoPromedio)
        {
            colPesoPromedio.Caption = "Peso promedio";
        }
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new PesoEstandarEditarForm(_pesoEstandarService, null);
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
            XtraMessageBox.Show(this, "Selecciona un peso estándar.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new PesoEstandarEditarForm(_pesoEstandarService, seleccionado);
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
            XtraMessageBox.Show(this, "Selecciona un peso estándar.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el peso estándar '{seleccionado.Codigo}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _pesoEstandarService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private PesoEstandarDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as PesoEstandarDto;
}
