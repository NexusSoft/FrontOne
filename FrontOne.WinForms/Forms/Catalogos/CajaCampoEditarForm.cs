using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class CajaCampoEditarForm : XtraForm
{
    private readonly CajaCampoService _cajaCampoService = null!;
    private readonly CajaCampoDto? _cajaCampoExistente;

    public CajaCampoEditarForm()
    {
        InitializeComponent();
    }

    public CajaCampoEditarForm(CajaCampoService cajaCampoService, CajaCampoDto? cajaCampoExistente)
        : this()
    {
        _cajaCampoService = cajaCampoService;
        _cajaCampoExistente = cajaCampoExistente;

        Text = cajaCampoExistente is null ? "FrontOne - Nueva caja de campo" : "FrontOne - Editar caja de campo";

        if (cajaCampoExistente is not null)
        {
            _txtNombre.Text = cajaCampoExistente.Nombre;
            _chkActivo.Checked = cajaCampoExistente.Activo;
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
            if (_cajaCampoExistente is null)
            {
                await _cajaCampoService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _cajaCampoService.ActualizarAsync(_cajaCampoExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
