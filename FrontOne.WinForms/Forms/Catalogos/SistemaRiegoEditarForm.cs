using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class SistemaRiegoEditarForm : XtraForm
{
    private readonly SistemaRiegoService _sistemaRiegoService = null!;
    private readonly SistemaRiegoDto? _sistemaRiegoExistente;

    public SistemaRiegoEditarForm()
    {
        InitializeComponent();
    }

    public SistemaRiegoEditarForm(SistemaRiegoService sistemaRiegoService, SistemaRiegoDto? sistemaRiegoExistente)
        : this()
    {
        _sistemaRiegoService = sistemaRiegoService;
        _sistemaRiegoExistente = sistemaRiegoExistente;

        Text = sistemaRiegoExistente is null ? "FrontOne - Nuevo sistema de riego" : "FrontOne - Editar sistema de riego";

        if (sistemaRiegoExistente is not null)
        {
            _txtNombre.Text = sistemaRiegoExistente.Nombre;
            _chkActivo.Checked = sistemaRiegoExistente.Activo;
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
            if (_sistemaRiegoExistente is null)
            {
                await _sistemaRiegoService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _sistemaRiegoService.ActualizarAsync(_sistemaRiegoExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
