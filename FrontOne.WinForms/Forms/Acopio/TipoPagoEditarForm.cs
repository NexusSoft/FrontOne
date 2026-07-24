using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class TipoPagoEditarForm : XtraForm
{
    private readonly TipoPagoService _tipoPagoService = null!;
    private readonly TipoPagoDto? _tipoPagoExistente;

    public TipoPagoEditarForm()
    {
        InitializeComponent();
    }

    public TipoPagoEditarForm(TipoPagoService tipoPagoService, TipoPagoDto? tipoPagoExistente)
        : this()
    {
        _tipoPagoService = tipoPagoService;
        _tipoPagoExistente = tipoPagoExistente;

        Text = tipoPagoExistente is null ? "FrontOne - Nuevo tipo de pago" : "FrontOne - Editar tipo de pago";

        if (tipoPagoExistente is not null)
        {
            _txtNombre.Text = tipoPagoExistente.Nombre;
            _chkNecesitaListaPrecios.Checked = tipoPagoExistente.NecesitaListaPrecios;
            _chkActivo.Checked = tipoPagoExistente.Activo;
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
            if (_tipoPagoExistente is null)
            {
                await _tipoPagoService.CrearAsync(_txtNombre.Text, _chkNecesitaListaPrecios.Checked);
            }
            else
            {
                await _tipoPagoService.ActualizarAsync(_tipoPagoExistente.Id, _txtNombre.Text, _chkNecesitaListaPrecios.Checked, _chkActivo.Checked);
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
