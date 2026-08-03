using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class MarcaEditarForm : XtraForm
{
    private readonly MarcaService _marcaService = null!;
    private readonly MarcaDto? _marcaExistente;

    public MarcaEditarForm()
    {
        InitializeComponent();
    }

    public MarcaEditarForm(MarcaService marcaService, MarcaDto? marcaExistente)
        : this()
    {
        _marcaService = marcaService;
        _marcaExistente = marcaExistente;

        Text = marcaExistente is null ? "FrontOne - Nueva marca" : "FrontOne - Editar marca";

        if (marcaExistente is not null)
        {
            _txtNombre.Text = marcaExistente.Nombre;
            _chkActivo.Checked = marcaExistente.Activo;
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
            if (_marcaExistente is null)
            {
                await _marcaService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _marcaService.ActualizarAsync(_marcaExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
