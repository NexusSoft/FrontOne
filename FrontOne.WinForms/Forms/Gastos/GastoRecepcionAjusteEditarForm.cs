using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.Enums;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Gastos;

public partial class GastoRecepcionAjusteEditarForm : XtraForm
{
    private readonly GastoRecepcionAjusteService _gastoRecepcionAjusteService = null!;
    private readonly TipoAjusteService _tipoAjusteService = null!;
    private readonly int _gastoLoteId;
    private readonly int _loteRecepcionId;
    private readonly byte _tipoGasto;

    public GastoRecepcionAjusteEditarForm()
    {
        InitializeComponent();
    }

    public GastoRecepcionAjusteEditarForm(
        GastoRecepcionAjusteService gastoRecepcionAjusteService,
        TipoAjusteService tipoAjusteService,
        int gastoLoteId,
        int loteRecepcionId,
        byte tipoGasto)
        : this()
    {
        _gastoRecepcionAjusteService = gastoRecepcionAjusteService;
        _tipoAjusteService = tipoAjusteService;
        _gastoLoteId = gastoLoteId;
        _loteRecepcionId = loteRecepcionId;
        _tipoGasto = tipoGasto;

        _rdgCargoA.Properties.Items.AddRange(new RadioGroupItem[]
        {
            new((byte)CargoAGasto.Empresa, "Empresa"),
            new((byte)CargoAGasto.Productor, "Productor"),
        });
        _rdgCargoA.EditValue = (byte)CargoAGasto.Empresa;

        Load += async (_, _) => await CargarTiposAjusteAsync();
    }

    private async Task CargarTiposAjusteAsync()
    {
        var tipos = await _tipoAjusteService.ObtenerPorTipoGastoAsync(_tipoGasto);
        _cmbTipoAjuste.Properties.DataSource = tipos.ToList();
        _cmbTipoAjuste.Properties.ValueMember = "Id";
        _cmbTipoAjuste.Properties.DisplayMember = "Nombre";
        _cmbTipoAjuste.Properties.Columns.Clear();
        _cmbTipoAjuste.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", 220, "Nombre"));
        _cmbTipoAjuste.Properties.PopupWidth = 240;
    }

    private async void CmbTipoAjuste_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new TiposAjusteForm(_tipoAjusteService);
        form.ShowDialog(this);
        await CargarTiposAjusteAsync();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var tipoAjusteId = _cmbTipoAjuste.EditValue as int? ?? 0;
        var monto = _spnMonto.Value;
        var cargoA = _rdgCargoA.EditValue as byte? ?? (byte)CargoAGasto.Empresa;

        try
        {
            await _gastoRecepcionAjusteService.CrearAsync(_gastoLoteId, _loteRecepcionId, tipoAjusteId, monto, cargoA);

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
