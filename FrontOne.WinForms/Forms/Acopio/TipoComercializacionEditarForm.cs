using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class TipoComercializacionEditarForm : XtraForm
{
    private readonly TipoComercializacionService _tipoComercializacionService = null!;
    private readonly TipoComercializacionDto? _tipoComercializacionExistente;

    public TipoComercializacionEditarForm()
    {
        InitializeComponent();
    }

    public TipoComercializacionEditarForm(TipoComercializacionService tipoComercializacionService, TipoComercializacionDto? tipoComercializacionExistente)
        : this()
    {
        _tipoComercializacionService = tipoComercializacionService;
        _tipoComercializacionExistente = tipoComercializacionExistente;

        Text = tipoComercializacionExistente is null ? "FrontOne - Nuevo tipo de comercialización" : "FrontOne - Editar tipo de comercialización";

        if (tipoComercializacionExistente is not null)
        {
            _txtNombre.Text = tipoComercializacionExistente.Nombre;
            _chkActivo.Checked = tipoComercializacionExistente.Activo;
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
            if (_tipoComercializacionExistente is null)
            {
                await _tipoComercializacionService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _tipoComercializacionService.ActualizarAsync(_tipoComercializacionExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
