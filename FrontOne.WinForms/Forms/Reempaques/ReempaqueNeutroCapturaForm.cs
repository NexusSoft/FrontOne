using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Reempaques;

// Ajuste (Merma / Diferencia a Favor) de UN lote específico dentro del reempaque — misma lógica y
// mismos productos SAP que PalletNeutroCapturaForm, solo que aquí el "saldo a cerrar" es
// ReempaqueDetalle.KilosDisponibles de un lote, no Corrida.KilosRestantes.
public partial class ReempaqueNeutroCapturaForm : XtraForm
{
    private const string DescripcionMerma = "MERMA";
    private const string DescripcionDiferenciaAFavor = "DIFERENCIA PESO A FAVOR";

    private readonly ReempaqueService _reempaqueService = null!;
    private readonly ProductoTerminadoService _productoTerminadoService = null!;
    private readonly int _reempaqueId;
    private readonly List<ReempaqueDetalleDto> _entrada;

    private ProductoTerminadoDto? _productoMerma;
    private ProductoTerminadoDto? _productoDiferenciaAFavor;

    public ReempaqueNeutroCapturaForm()
    {
        InitializeComponent();
    }

    public ReempaqueNeutroCapturaForm(
        ReempaqueService reempaqueService,
        ProductoTerminadoService productoTerminadoService,
        int reempaqueId,
        IReadOnlyList<ReempaqueDetalleDto> entrada,
        int? reempaqueDetalleIdPreseleccionado)
        : this()
    {
        _reempaqueService = reempaqueService;
        _productoTerminadoService = productoTerminadoService;
        _reempaqueId = reempaqueId;
        _entrada = entrada.Where(d => d.KilosDisponibles != 0).ToList();

        _cmbProducto.Properties.Items.AddRange(new object[] { "Merma", "Diferencia a Favor" });

        Load += async (_, _) => await CargarAsync(reempaqueDetalleIdPreseleccionado);
    }

    private async Task CargarAsync(int? preseleccionado)
    {
        _cmbLote.Properties.DataSource = _entrada;
        _cmbLote.Properties.ValueMember = "Id";
        _cmbLote.Properties.DisplayMember = "LoteFolio";
        _cmbLote.Properties.Columns.Clear();
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("LoteFolio", 90, "No. de Lote"));
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("PalletFolio", 100, "Pallet Origen"));
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("KilosDisponibles", 110, "Kg Pendientes"));
        _cmbLote.Properties.PopupWidth = 320;

        var productos = await _productoTerminadoService.ObtenerAsync();
        _productoMerma = productos.FirstOrDefault(p => p.DescripcionSap == DescripcionMerma);
        _productoDiferenciaAFavor = productos.FirstOrDefault(p => p.DescripcionSap == DescripcionDiferenciaAFavor);

        if (_productoMerma is null || _productoDiferenciaAFavor is null)
        {
            XtraMessageBox.Show(this,
                "No se encontraron los productos 'MERMA' y/o 'DIFERENCIA PESO A FAVOR' en el catálogo de Productos Terminados.",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _btnGuardar.Enabled = false;
            return;
        }

        _cmbLote.EditValue = preseleccionado ?? _entrada.FirstOrDefault()?.Id;
        CmbLote_EditValueChanged(this, EventArgs.Empty);
    }

    private void CmbLote_EditValueChanged(object? sender, EventArgs e)
    {
        var lote = ObtenerLoteSeleccionado();
        var pendiente = lote?.KilosDisponibles ?? 0m;
        _txtKilosDisponibles.Text = pendiente.ToString("n2");
        _spnKilogramos.EditValue = Math.Abs(pendiente);

        // Se preselecciona el producto según el signo: sobra saldo por consumir (Merma) o se
        // consumió de más (Diferencia a Favor) — mismo criterio que PalletNeutroCapturaForm.
        _cmbProducto.SelectedIndex = pendiente < 0 ? 1 : 0;
    }

    private ReempaqueDetalleDto? ObtenerLoteSeleccionado()
        => _cmbLote.EditValue is int id ? _entrada.FirstOrDefault(d => d.Id == id) : null;

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var lote = ObtenerLoteSeleccionado();
        if (lote is null)
        {
            XtraMessageBox.Show(this, "Selecciona el lote a ajustar.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var kilogramos = Convert.ToDecimal(_spnKilogramos.EditValue);
        if (kilogramos <= 0)
        {
            XtraMessageBox.Show(this, "Captura un monto de Kilogramos mayor a cero.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var esMerma = _cmbProducto.SelectedIndex == 0;
        var producto = esMerma ? _productoMerma : _productoDiferenciaAFavor;
        var kilogramosConSigno = esMerma ? kilogramos : -kilogramos;

        if (producto is null)
        {
            return;
        }

        try
        {
            await _reempaqueService.CrearNeutroAsync(_reempaqueId, lote.Id, producto.Id, kilogramosConSigno);
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
