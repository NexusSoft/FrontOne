using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class PaisEditarForm : XtraForm
{
    private readonly PaisService _paisService = null!;
    private readonly PaisDto? _paisExistente;

    public PaisEditarForm()
    {
        InitializeComponent();
    }

    public PaisEditarForm(PaisService paisService, PaisDto? paisExistente)
        : this()
    {
        _paisService = paisService;
        _paisExistente = paisExistente;

        Text = paisExistente is null ? "FrontOne - Nuevo país" : "FrontOne - Editar país";

        if (paisExistente is not null)
        {
            _txtClave.Text = paisExistente.Clave;
            _txtNombre.Text = paisExistente.Nombre;
            _chkActivo.Checked = paisExistente.Activo;
        }
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_paisExistente is null)
            {
                await _paisService.CrearAsync(_txtClave.Text, _txtNombre.Text);
            }
            else
            {
                await _paisService.ActualizarAsync(_paisExistente.Id, _txtClave.Text, _txtNombre.Text, _chkActivo.Checked);
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
}
