using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Corridas;

// Captura del Pallet Neutro (Merma / Diferencia a Favor) que cierra Kilos Restantes en 0 para
// poder Finalizar una Corrida. Formulario mínimo: no hay edición posterior — si el usuario se
// equivoca, elimina el Pallet Neutro desde PalletsForm (flujo normal, ya revierte KilosProcesados)
// y vuelve a capturar aquí.
public partial class PalletNeutroCapturaForm : XtraForm
{
    private const string DescripcionMerma = "MERMA";
    private const string DescripcionDiferenciaAFavor = "DIFERENCIA PESO A FAVOR";

    private readonly PalletService _palletService = null!;
    private readonly ProductoTerminadoService _productoTerminadoService = null!;
    private readonly CorridaDto _corrida = null!;

    private ProductoTerminadoDto? _productoMerma;
    private ProductoTerminadoDto? _productoDiferenciaAFavor;

    public PalletNeutroCapturaForm()
    {
        InitializeComponent();
    }

    public PalletNeutroCapturaForm(PalletService palletService, ProductoTerminadoService productoTerminadoService, CorridaDto corrida)
        : this()
    {
        _palletService = palletService;
        _productoTerminadoService = productoTerminadoService;
        _corrida = corrida;

        _cmbProducto.Properties.Items.AddRange(new object[] { "Merma", "Diferencia a Favor" });

        Load += async (_, _) => await CargarAsync();
    }

    private async Task CargarAsync()
    {
        _txtLote.Text = _corrida.LoteFolio;
        _txtKilosRestantes.Text = _corrida.KilosRestantes.ToString("n2");
        _spnKilogramos.EditValue = Math.Abs(_corrida.KilosRestantes);

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

        // Se preselecciona el producto según el signo de Kilos Restantes: si falta producto
        // (Restantes > 0) lo natural es Merma; si sobró (Restantes < 0), Diferencia a Favor.
        _cmbProducto.SelectedIndex = _corrida.KilosRestantes < 0 ? 1 : 0;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var kilogramos = Convert.ToDecimal(_spnKilogramos.EditValue);
        if (kilogramos <= 0)
        {
            XtraMessageBox.Show(this, "Captura un monto de Kilogramos mayor a cero.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // El signo lo decide el producto elegido: Merma suma a Kilos Procesados, Diferencia a
        // Favor resta — el usuario solo teclea la magnitud.
        var esMerma = _cmbProducto.SelectedIndex == 0;
        var producto = esMerma ? _productoMerma : _productoDiferenciaAFavor;
        var kilogramosConSigno = esMerma ? kilogramos : -kilogramos;

        if (producto is null)
        {
            return;
        }

        try
        {
            await _palletService.CrearNeutroAsync(_corrida.Id, producto.Id, kilogramosConSigno);
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
