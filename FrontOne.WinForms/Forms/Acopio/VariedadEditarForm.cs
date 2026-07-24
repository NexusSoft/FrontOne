using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class VariedadEditarForm : XtraForm
{
    private readonly VariedadService _variedadService = null!;
    private readonly VariedadDto? _variedadExistente;

    public VariedadEditarForm()
    {
        InitializeComponent();
    }

    public VariedadEditarForm(VariedadService variedadService, VariedadDto? variedadExistente)
        : this()
    {
        _variedadService = variedadService;
        _variedadExistente = variedadExistente;

        Text = variedadExistente is null ? "FrontOne - Nueva variedad" : "FrontOne - Editar variedad";

        if (variedadExistente is not null)
        {
            _txtNombre.Text = variedadExistente.Nombre;
            _chkActivo.Checked = variedadExistente.Activo;
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
            if (_variedadExistente is null)
            {
                await _variedadService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _variedadService.ActualizarAsync(_variedadExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
