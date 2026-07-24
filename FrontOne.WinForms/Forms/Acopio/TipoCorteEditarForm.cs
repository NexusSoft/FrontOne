using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class TipoCorteEditarForm : XtraForm
{
    private readonly TipoCorteService _tipoCorteService = null!;
    private readonly TipoPagoService _tipoPagoService = null!;
    private readonly TipoCorteDto? _tipoCorteExistente;

    public TipoCorteEditarForm()
    {
        InitializeComponent();
    }

    public TipoCorteEditarForm(TipoCorteService tipoCorteService, TipoPagoService tipoPagoService, TipoCorteDto? tipoCorteExistente)
        : this()
    {
        _tipoCorteService = tipoCorteService;
        _tipoPagoService = tipoPagoService;
        _tipoCorteExistente = tipoCorteExistente;

        Text = tipoCorteExistente is null ? "FrontOne - Nuevo tipo de corte" : "FrontOne - Editar tipo de corte";

        if (tipoCorteExistente is null)
        {
            _chkActivo.Checked = true;
        }

        Load += async (_, _) => await CargarTiposPagoAsync();
    }

    private async Task CargarTiposPagoAsync()
    {
        var tiposPago = await _tipoPagoService.ObtenerAsync();

        _cmbTipoPago.Properties.DataSource = tiposPago.ToList();
        _cmbTipoPago.Properties.ValueMember = "Id";
        _cmbTipoPago.Properties.DisplayMember = "Nombre";
        _cmbTipoPago.Properties.Columns.Clear();
        _cmbTipoPago.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 200, "Tipo de pago"));
        _cmbTipoPago.Properties.Columns.Add(new LookUpColumnInfo("NecesitaListaPrecios", 100, "Necesita lista"));
        _cmbTipoPago.Properties.PopupWidth = 320;

        if (_tipoCorteExistente is not null)
        {
            _txtNombre.Text = _tipoCorteExistente.Nombre;
            _spnFueraDeNorma.Value = _tipoCorteExistente.FueraDeNormaGr;
            _chkDanioMinimo.Checked = _tipoCorteExistente.DanioMinimo;
            _cmbTipoPago.EditValue = _tipoCorteExistente.TipoPagoId;
            _chkActivo.Checked = _tipoCorteExistente.Activo;
        }
    }

    // El "+" abre el listado completo de Tipos de Pago (crear/editar/eliminar), no el alta
    // directa — regla dura del proyecto para todo LookUpEdit con botón Plus.
    private async void CmbTipoPago_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new TiposPagoForm(_tipoPagoService);
        form.ShowDialog(this);
        await CargarTiposPagoAsync();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var tipoPagoId = _cmbTipoPago.EditValue is int id ? id : 0;

        var datos = new TipoCorteDto(
            _tipoCorteExistente?.Id ?? 0,
            _txtNombre.Text,
            _spnFueraDeNorma.Value,
            _chkDanioMinimo.Checked,
            tipoPagoId,
            _chkActivo.Checked,
            string.Empty);

        try
        {
            if (_tipoCorteExistente is null)
            {
                await _tipoCorteService.CrearAsync(datos);
            }
            else
            {
                await _tipoCorteService.ActualizarAsync(datos);
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
