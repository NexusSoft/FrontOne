using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class PesoEstandarEditarForm : XtraForm
{
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly PesoEstandarDto? _pesoEstandarExistente;

    public PesoEstandarEditarForm()
    {
        InitializeComponent();
    }

    public PesoEstandarEditarForm(PesoEstandarService pesoEstandarService, PesoEstandarDto? pesoEstandarExistente)
        : this()
    {
        _pesoEstandarService = pesoEstandarService;
        _pesoEstandarExistente = pesoEstandarExistente;

        Text = pesoEstandarExistente is null ? "FrontOne - Nuevo peso estándar" : "FrontOne - Editar peso estándar";

        if (pesoEstandarExistente is not null)
        {
            _txtCodigo.Text = pesoEstandarExistente.Codigo;
            _txtDescripcion.Text = pesoEstandarExistente.Descripcion;
            _spnPesoNeto.Value = pesoEstandarExistente.PesoNeto;
            _spnPesoPromedio.Value = pesoEstandarExistente.PesoPromedio;
            _chkActivo.Checked = pesoEstandarExistente.Activo;
        }
        else
        {
            _chkActivo.Checked = true;
        }
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var datos = new PesoEstandarDto(
            _pesoEstandarExistente?.Id ?? 0,
            _txtCodigo.Text,
            _txtDescripcion.Text,
            _spnPesoNeto.Value,
            _spnPesoPromedio.Value,
            _chkActivo.Checked);

        try
        {
            if (_pesoEstandarExistente is null)
            {
                await _pesoEstandarService.CrearAsync(datos);
            }
            else
            {
                await _pesoEstandarService.ActualizarAsync(datos);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
