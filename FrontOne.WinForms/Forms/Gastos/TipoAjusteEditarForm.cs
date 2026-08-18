using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Gastos;

public partial class TipoAjusteEditarForm : XtraForm
{
    private readonly TipoAjusteService _tipoAjusteService = null!;
    private readonly TipoAjusteDto? _tipoAjusteExistente;

    public TipoAjusteEditarForm()
    {
        InitializeComponent();
    }

    public TipoAjusteEditarForm(TipoAjusteService tipoAjusteService, TipoAjusteDto? tipoAjusteExistente)
        : this()
    {
        _tipoAjusteService = tipoAjusteService;
        _tipoAjusteExistente = tipoAjusteExistente;

        _cmbTipoGasto.Properties.Items.AddRange(new object[] { "Cosecha", "Acarreo" });
        _cmbSigno.Properties.Items.AddRange(new object[] { "A Favor", "En Contra" });

        Text = tipoAjusteExistente is null ? "FrontOne - Nuevo tipo de ajuste" : "FrontOne - Editar tipo de ajuste";

        if (tipoAjusteExistente is not null)
        {
            _txtNombre.Text = tipoAjusteExistente.Nombre;
            _cmbTipoGasto.SelectedIndex = tipoAjusteExistente.TipoGasto - 1;
            _cmbSigno.SelectedIndex = tipoAjusteExistente.Signo - 1;
            _chkActivo.Checked = tipoAjusteExistente.Activo;
        }
        else
        {
            _cmbTipoGasto.SelectedIndex = 0;
            _cmbSigno.SelectedIndex = 0;
            _chkActivo.Checked = true;
        }
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var tipoGasto = (byte)(_cmbTipoGasto.SelectedIndex + 1);
        var signo = (byte)(_cmbSigno.SelectedIndex + 1);

        try
        {
            if (_tipoAjusteExistente is null)
            {
                await _tipoAjusteService.CrearAsync(_txtNombre.Text, tipoGasto, signo);
            }
            else
            {
                await _tipoAjusteService.ActualizarAsync(_tipoAjusteExistente.Id, _txtNombre.Text, tipoGasto, signo, _chkActivo.Checked);
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
