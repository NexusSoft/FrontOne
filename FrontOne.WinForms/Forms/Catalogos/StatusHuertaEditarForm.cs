using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class StatusHuertaEditarForm : XtraForm
{
    private readonly StatusHuertaService _statusHuertaService = null!;
    private readonly StatusHuertaDto? _statusExistente;

    public StatusHuertaEditarForm()
    {
        InitializeComponent();
    }

    public StatusHuertaEditarForm(StatusHuertaService statusHuertaService, StatusHuertaDto? statusExistente)
        : this()
    {
        _statusHuertaService = statusHuertaService;
        _statusExistente = statusExistente;

        Text = statusExistente is null ? "FrontOne - Nuevo estatus de huerta" : "FrontOne - Editar estatus de huerta";

        if (statusExistente is not null)
        {
            _txtNombre.Text = statusExistente.Nombre;
            _chkActivo.Checked = statusExistente.Activo;
        }
        else
        {
            _chkActivo.Checked = true;
        }
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_statusExistente is null)
            {
                await _statusHuertaService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _statusHuertaService.ActualizarAsync(_statusExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
