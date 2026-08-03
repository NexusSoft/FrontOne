using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class CategoriaEditarForm : XtraForm
{
    private readonly CategoriaService _categoriaService = null!;
    private readonly CategoriaDto? _categoriaExistente;

    public CategoriaEditarForm()
    {
        InitializeComponent();
    }

    public CategoriaEditarForm(CategoriaService categoriaService, CategoriaDto? categoriaExistente)
        : this()
    {
        _categoriaService = categoriaService;
        _categoriaExistente = categoriaExistente;

        Text = categoriaExistente is null ? "FrontOne - Nueva categoría" : "FrontOne - Editar categoría";

        if (categoriaExistente is not null)
        {
            _txtNombre.Text = categoriaExistente.Nombre;
            _chkActivo.Checked = categoriaExistente.Activo;
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
            if (_categoriaExistente is null)
            {
                await _categoriaService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _categoriaService.ActualizarAsync(_categoriaExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
