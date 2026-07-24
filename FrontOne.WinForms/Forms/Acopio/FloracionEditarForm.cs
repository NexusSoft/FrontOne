using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class FloracionEditarForm : XtraForm
{
    private readonly FloracionService _floracionService = null!;
    private readonly FloracionDto? _floracionExistente;

    public FloracionEditarForm()
    {
        InitializeComponent();
    }

    public FloracionEditarForm(FloracionService floracionService, FloracionDto? floracionExistente)
        : this()
    {
        _floracionService = floracionService;
        _floracionExistente = floracionExistente;

        Text = floracionExistente is null ? "FrontOne - Nueva floración" : "FrontOne - Editar floración";

        if (floracionExistente is not null)
        {
            _txtNombre.Text = floracionExistente.Nombre;
            _chkActivo.Checked = floracionExistente.Activo;
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
            if (_floracionExistente is null)
            {
                await _floracionService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _floracionService.ActualizarAsync(_floracionExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
