using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class MonedaEditarForm : XtraForm
{
    private readonly MonedaService _monedaService = null!;
    private readonly MonedaDto? _monedaExistente;

    public MonedaEditarForm()
    {
        InitializeComponent();
    }

    public MonedaEditarForm(MonedaService monedaService, MonedaDto? monedaExistente)
        : this()
    {
        _monedaService = monedaService;
        _monedaExistente = monedaExistente;

        Text = monedaExistente is null ? "FrontOne - Nueva moneda" : "FrontOne - Editar moneda";

        if (monedaExistente is not null)
        {
            _txtNombre.Text = monedaExistente.Nombre;
            _txtNomenclatura.Text = monedaExistente.Nomenclatura;
            _chkActivo.Checked = monedaExistente.Activo;
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
            if (_monedaExistente is null)
            {
                await _monedaService.CrearAsync(_txtNombre.Text, _txtNomenclatura.Text);
            }
            else
            {
                await _monedaService.ActualizarAsync(_monedaExistente.Id, _txtNombre.Text, _txtNomenclatura.Text, _chkActivo.Checked);
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
