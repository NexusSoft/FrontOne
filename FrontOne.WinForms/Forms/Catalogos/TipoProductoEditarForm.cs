using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class TipoProductoEditarForm : XtraForm
{
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly TipoProductoDto? _tipoProductoExistente;

    public TipoProductoEditarForm()
    {
        InitializeComponent();
    }

    public TipoProductoEditarForm(TipoProductoService tipoProductoService, TipoProductoDto? tipoProductoExistente)
        : this()
    {
        _tipoProductoService = tipoProductoService;
        _tipoProductoExistente = tipoProductoExistente;

        Text = tipoProductoExistente is null ? "FrontOne - Nuevo tipo de producto" : "FrontOne - Editar tipo de producto";

        if (tipoProductoExistente is not null)
        {
            _txtNombre.Text = tipoProductoExistente.Nombre;
            _chkActivo.Checked = tipoProductoExistente.Activo;
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
            if (_tipoProductoExistente is null)
            {
                await _tipoProductoService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _tipoProductoService.ActualizarAsync(_tipoProductoExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
