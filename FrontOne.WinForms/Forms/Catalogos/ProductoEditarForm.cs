using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class ProductoEditarForm : XtraForm
{
    private readonly ProductoService _productoService = null!;
    private readonly ProductoDto? _productoExistente;

    public ProductoEditarForm()
    {
        InitializeComponent();
    }

    public ProductoEditarForm(ProductoService productoService, ProductoDto? productoExistente)
        : this()
    {
        _productoService = productoService;
        _productoExistente = productoExistente;

        Text = productoExistente is null ? "FrontOne - Nuevo producto" : "FrontOne - Editar producto";

        if (productoExistente is not null)
        {
            _txtNombre.Text = productoExistente.Nombre;
            _chkActivo.Checked = productoExistente.Activo;
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
            if (_productoExistente is null)
            {
                await _productoService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _productoService.ActualizarAsync(_productoExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
