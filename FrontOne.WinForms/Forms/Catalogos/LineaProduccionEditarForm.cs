using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class LineaProduccionEditarForm : XtraForm
{
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly LineaProduccionDto? _lineaProduccionExistente;

    public LineaProduccionEditarForm()
    {
        InitializeComponent();
    }

    public LineaProduccionEditarForm(LineaProduccionService lineaProduccionService, LineaProduccionDto? lineaProduccionExistente)
        : this()
    {
        _lineaProduccionService = lineaProduccionService;
        _lineaProduccionExistente = lineaProduccionExistente;

        Text = lineaProduccionExistente is null ? "FrontOne - Nueva línea de producción" : "FrontOne - Editar línea de producción";

        if (lineaProduccionExistente is not null)
        {
            _txtNombre.Text = lineaProduccionExistente.Nombre;
            _chkActivo.Checked = lineaProduccionExistente.Activo;
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
            if (_lineaProduccionExistente is null)
            {
                await _lineaProduccionService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _lineaProduccionService.ActualizarAsync(_lineaProduccionExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
