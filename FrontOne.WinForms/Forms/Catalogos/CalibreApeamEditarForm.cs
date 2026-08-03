using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class CalibreApeamEditarForm : XtraForm
{
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly CalibreApeamDto? _calibreApeamExistente;

    public CalibreApeamEditarForm()
    {
        InitializeComponent();
    }

    public CalibreApeamEditarForm(CalibreApeamService calibreApeamService, CalibreApeamDto? calibreApeamExistente)
        : this()
    {
        _calibreApeamService = calibreApeamService;
        _calibreApeamExistente = calibreApeamExistente;

        Text = calibreApeamExistente is null ? "FrontOne - Nuevo calibre APEAM" : "FrontOne - Editar calibre APEAM";

        if (calibreApeamExistente is not null)
        {
            _txtNombre.Text = calibreApeamExistente.Nombre;
            _chkActivo.Checked = calibreApeamExistente.Activo;
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
            if (_calibreApeamExistente is null)
            {
                await _calibreApeamService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _calibreApeamService.ActualizarAsync(_calibreApeamExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
