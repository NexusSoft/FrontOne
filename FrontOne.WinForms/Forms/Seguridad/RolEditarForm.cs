using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Seguridad;

public partial class RolEditarForm : XtraForm
{
    private readonly RolService _rolService = null!;
    private readonly RolDto? _rolExistente;

    public RolEditarForm()
    {
        InitializeComponent();
    }

    public RolEditarForm(RolService rolService, RolDto? rolExistente)
        : this()
    {
        _rolService = rolService;
        _rolExistente = rolExistente;

        Text = rolExistente is null ? "FrontOne - Nuevo rol" : "FrontOne - Editar rol";

        if (rolExistente is not null)
        {
            _txtNombre.Text = rolExistente.Nombre;
            _txtDescripcion.Text = rolExistente.Descripcion ?? string.Empty;
            _chkActivo.Checked = rolExistente.Activo;
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
            if (_rolExistente is null)
            {
                await _rolService.CrearAsync(_txtNombre.Text, _txtDescripcion.Text);
            }
            else
            {
                await _rolService.ActualizarAsync(_rolExistente.Id, _txtNombre.Text, _txtDescripcion.Text, _chkActivo.Checked);
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
