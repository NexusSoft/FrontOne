using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.Enums;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Almacenes;

public partial class MovimientoCajaCampoEditarForm : XtraForm
{
    private readonly CajaCampoService _cajaCampoService = null!;
    private readonly MovimientoAlmacenService _movimientoAlmacenService = null!;
    private readonly TipoMovimientoAlmacen _tipo;

    public MovimientoCajaCampoEditarForm()
    {
        InitializeComponent();
    }

    public MovimientoCajaCampoEditarForm(
        CajaCampoService cajaCampoService,
        MovimientoAlmacenService movimientoAlmacenService,
        TipoMovimientoAlmacen tipo,
        int? cajaCampoPreseleccionado)
        : this()
    {
        _cajaCampoService = cajaCampoService;
        _movimientoAlmacenService = movimientoAlmacenService;
        _tipo = tipo;

        Text = tipo == TipoMovimientoAlmacen.Entrada ? "Registrar Compra de Caja de Campo" : "Ajuste de Caja de Campo";

        Load += async (_, _) =>
        {
            await CargarCajasCampoAsync();
            if (cajaCampoPreseleccionado is int id)
            {
                _cmbCajaCampo.EditValue = id;
            }
        };
    }

    private async Task CargarCajasCampoAsync()
    {
        var cajasCampo = (await _cajaCampoService.ObtenerAsync()).Where(c => c.Activo).ToList();
        _cmbCajaCampo.Properties.DataSource = cajasCampo;
        _cmbCajaCampo.Properties.ValueMember = "Id";
        _cmbCajaCampo.Properties.DisplayMember = "Nombre";
        _cmbCajaCampo.Properties.Columns.Clear();
        _cmbCajaCampo.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Color de Caja"));
        _cmbCajaCampo.Properties.PopupWidth = 250;
    }

    private async void CmbCajaCampo_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new CajasCampoForm(_cajaCampoService);
        form.ShowDialog(this);
        await CargarCajasCampoAsync();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbCajaCampo.EditValue is not int cajaCampoId)
        {
            XtraMessageBox.Show(this, "Selecciona el color de caja de campo.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var cantidad = (short)(decimal)_spnCantidad.EditValue;
            await _movimientoAlmacenService.RegistrarMovimientoManualAsync(
                cajaCampoId, _tipo, cantidad,
                string.IsNullOrWhiteSpace(_txtObservaciones.Text) ? null : _txtObservaciones.Text);

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
