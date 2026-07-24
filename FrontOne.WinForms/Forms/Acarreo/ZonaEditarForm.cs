using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acarreo;

public partial class ZonaEditarForm : XtraForm
{
    private readonly ZonaService _zonaService = null!;
    private readonly ZonaDto? _zonaExistente;

    public ZonaEditarForm()
    {
        InitializeComponent();
    }

    public ZonaEditarForm(ZonaService zonaService, ZonaDto? zonaExistente)
        : this()
    {
        _zonaService = zonaService;
        _zonaExistente = zonaExistente;

        Text = zonaExistente is null ? "FrontOne - Nueva zona" : "FrontOne - Editar zona";

        if (zonaExistente is not null)
        {
            _txtNombre.Text = zonaExistente.Nombre;
            _txtKgMinimo300.Value = zonaExistente.KgMinimo300;
            _txtKgMinimo400.Value = zonaExistente.KgMinimo400;
            _txtKgMinimo500.Value = zonaExistente.KgMinimo500;
            _chkActivo.Checked = zonaExistente.Activo;
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
            if (_zonaExistente is null)
            {
                await _zonaService.CrearAsync(_txtNombre.Text, _txtKgMinimo300.Value, _txtKgMinimo400.Value, _txtKgMinimo500.Value);
            }
            else
            {
                await _zonaService.ActualizarAsync(
                    _zonaExistente.Id, _txtNombre.Text, _txtKgMinimo300.Value, _txtKgMinimo400.Value, _txtKgMinimo500.Value, _chkActivo.Checked);
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
