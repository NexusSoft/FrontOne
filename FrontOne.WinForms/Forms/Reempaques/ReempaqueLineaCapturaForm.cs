using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.WinForms.Forms.Catalogos;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Reempaques;

// Captura de una línea de salida: de qué lote de saldo (ReempaqueDetalleDto) del reempaque salen
// las cajas/kilogramos que van al pallet destino ya elegido en el buscador. Mismo molde que
// PalletDetalleCapturaForm — no guarda nada, solo expone la selección; quien abre el diálogo
// (ReempaqueEditarForm) decide qué hacer con ella.
public partial class ReempaqueLineaCapturaForm : XtraForm
{
    private readonly ProductoTerminadoService _productoTerminadoService = null!;
    private readonly CategoriaService _categoriaService = null!;
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly MarcaService _marcaService = null!;
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly PaisService _paisService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly SessionContext _sessionContext = null!;

    private readonly List<ReempaqueDetalleDto> _entrada = new();
    private readonly PalletDto _palletDestino = null!;

    private List<ProductoTerminadoDto> _productos = new();

    public int ReempaqueDetalleIdSeleccionado { get; private set; }

    public int ProductoTerminadoIdSeleccionado { get; private set; }

    public int? CajasCapturadas { get; private set; }

    // Solo trae valor cuando el producto seleccionado es Granel.
    public decimal? KilogramosCapturados { get; private set; }

    public decimal PorcentajeMateriaSecaCapturado { get; private set; }

    public ReempaqueLineaCapturaForm()
    {
        InitializeComponent();
    }

    public ReempaqueLineaCapturaForm(
        ProductoTerminadoService productoTerminadoService,
        CategoriaService categoriaService,
        TipoProductoService tipoProductoService,
        CalibreApeamService calibreApeamService,
        MarcaService marcaService,
        PesoEstandarService pesoEstandarService,
        PaisService paisService,
        VariedadService variedadService,
        SessionContext sessionContext,
        IReadOnlyList<ReempaqueDetalleDto> entrada,
        PalletDto palletDestino)
        : this()
    {
        _productoTerminadoService = productoTerminadoService;
        _categoriaService = categoriaService;
        _tipoProductoService = tipoProductoService;
        _calibreApeamService = calibreApeamService;
        _marcaService = marcaService;
        _pesoEstandarService = pesoEstandarService;
        _paisService = paisService;
        _variedadService = variedadService;
        _sessionContext = sessionContext;
        _entrada = entrada.Where(d => d.KilosDisponibles != 0).ToList();
        _palletDestino = palletDestino;

        Load += async (_, _) => await CargarAsync();
    }

    private async Task CargarAsync()
    {
        _txtPalletDestino.Text = _palletDestino.Folio;

        _cmbLote.Properties.DataSource = _entrada;
        _cmbLote.Properties.ValueMember = "Id";
        _cmbLote.Properties.DisplayMember = "LoteFolio";
        _cmbLote.Properties.Columns.Clear();
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("LoteFolio", 90, "No. de Lote"));
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("PalletFolio", 100, "Pallet Origen"));
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("KilosDisponibles", 110, "Kg Pendientes"));
        _cmbLote.Properties.PopupWidth = 320;

        await CargarProductosAsync();

        // Pallet destino no mixto: el producto ya viene fijado por su encabezado, igual que en
        // PalletDetalleCapturaForm — se autoselecciona y se bloquea.
        if (!_palletDestino.EsMixto && _palletDestino.ProductoTerminadoId is { } productoId)
        {
            _cmbProducto.EditValue = productoId;
            _cmbProducto.Properties.ReadOnly = true;
        }

        _cmbLote.EditValue = _entrada.FirstOrDefault()?.Id;
        ActualizarDatosLote();
        ActualizarDatosProducto();
        RecalcularKilogramos();
    }

    private async Task CargarProductosAsync()
    {
        _productos = (await _productoTerminadoService.ObtenerAsync()).Where(p => p.Activo).ToList();

        _cmbProducto.Properties.DataSource = _productos;
        _cmbProducto.Properties.ValueMember = "Id";
        _cmbProducto.Properties.DisplayMember = "DescripcionSap";
        _cmbProducto.Properties.Columns.Clear();
        _cmbProducto.Properties.Columns.Add(new LookUpColumnInfo("CodigoSap", 100, "Código SAP"));
        _cmbProducto.Properties.Columns.Add(new LookUpColumnInfo("DescripcionSap", 300, "Descripción"));
        _cmbProducto.Properties.PopupWidth = 430;
    }

    private void CmbLote_EditValueChanged(object? sender, EventArgs e)
    {
        ActualizarDatosLote();
        RecalcularKilogramos();
    }

    private void CmbProducto_EditValueChanged(object? sender, EventArgs e)
    {
        ActualizarDatosProducto();

        if (EsGranelSeleccionado())
        {
            _spnCajas.EditValue = 0;
        }
        else
        {
            _spnKilogramos.EditValue = 0m;
        }

        RecalcularKilogramos();
    }

    private void SpnCajas_EditValueChanged(object? sender, EventArgs e) => RecalcularKilogramos();

    private void ActualizarDatosLote()
    {
        var lote = ObtenerLoteSeleccionado();
        _txtKilosDisponibles.Text = (lote?.KilosDisponibles ?? 0m).ToString("n2");
        _spnPorcentajeMateriaSeca.EditValue = lote?.PorcentajeMateriaSeca ?? 0m;
    }

    private void ActualizarDatosProducto()
    {
        var producto = ObtenerProductoSeleccionado();
        var esGranel = EsGranelSeleccionado();

        _spnPesoEstandar.EditValue = producto?.PesoNeto ?? 0m;
        _spnCajasPorPallet.EditValue = producto?.CajasPorPallet ?? 0;

        _spnCajas.Enabled = !esGranel;
        _spnKilogramos.Properties.ReadOnly = !esGranel;
    }

    private void RecalcularKilogramos()
    {
        if (EsGranelSeleccionado())
        {
            return;
        }

        var pesoEstandar = Convert.ToDecimal(_spnPesoEstandar.EditValue);
        var cajas = Convert.ToInt32(_spnCajas.EditValue);
        _spnKilogramos.EditValue = pesoEstandar * cajas;
    }

    private bool EsGranelSeleccionado() => ObtenerProductoSeleccionado()?.Presentacion == PresentacionProducto.Granel;

    private ReempaqueDetalleDto? ObtenerLoteSeleccionado()
        => _cmbLote.EditValue is int id ? _entrada.FirstOrDefault(d => d.Id == id) : null;

    private ProductoTerminadoDto? ObtenerProductoSeleccionado()
        => _cmbProducto.EditValue is int productoId ? _productos.FirstOrDefault(p => p.Id == productoId) : null;

    private async void CmbProducto_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || _cmbProducto.Properties.ReadOnly)
        {
            return;
        }

        var seleccionado = _cmbProducto.EditValue;

        using var form = new ProductosTerminadosForm(
            _productoTerminadoService, _categoriaService, _tipoProductoService, _calibreApeamService,
            _marcaService, _pesoEstandarService, _paisService, _variedadService, _sessionContext);
        form.ShowDialog(this);

        await CargarProductosAsync();
        _cmbProducto.EditValue = seleccionado;
        ActualizarDatosProducto();
        RecalcularKilogramos();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var lote = ObtenerLoteSeleccionado();
        if (lote is null)
        {
            XtraMessageBox.Show(this, "Selecciona el lote de origen.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var producto = ObtenerProductoSeleccionado();
        if (producto is null)
        {
            XtraMessageBox.Show(this, "Selecciona un producto terminado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (producto.Presentacion == PresentacionProducto.Granel)
        {
            var kilogramos = Convert.ToDecimal(_spnKilogramos.EditValue);
            if (kilogramos <= 0)
            {
                XtraMessageBox.Show(this, "Captura los Kilogramos de esta línea.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ReempaqueDetalleIdSeleccionado = lote.Id;
            ProductoTerminadoIdSeleccionado = producto.Id;
            CajasCapturadas = null;
            KilogramosCapturados = kilogramos;
            PorcentajeMateriaSecaCapturado = lote.PorcentajeMateriaSeca;

            DialogResult = DialogResult.OK;
            Close();
            return;
        }

        if (producto.PesoNeto is null or <= 0)
        {
            XtraMessageBox.Show(this,
                "El producto terminado seleccionado no tiene Peso Neto configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (producto.CajasPorPallet is null or <= 0)
        {
            XtraMessageBox.Show(this,
                "El producto terminado seleccionado no tiene Cajas por Pallet configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cajas = Convert.ToInt32(_spnCajas.EditValue);
        if (cajas <= 0)
        {
            XtraMessageBox.Show(this, "Captura las cajas de esta línea.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_palletDestino.EsMixto && (_palletDestino.TotalCajas + cajas) > producto.CajasPorPallet)
        {
            XtraMessageBox.Show(this,
                $"No se puede exceder el objetivo de {producto.CajasPorPallet} cajas del producto — el pallet '{_palletDestino.Folio}' ya tiene {_palletDestino.TotalCajas} cajas capturadas.",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ReempaqueDetalleIdSeleccionado = lote.Id;
        ProductoTerminadoIdSeleccionado = producto.Id;
        CajasCapturadas = cajas;
        KilogramosCapturados = null;
        PorcentajeMateriaSecaCapturado = lote.PorcentajeMateriaSeca;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
