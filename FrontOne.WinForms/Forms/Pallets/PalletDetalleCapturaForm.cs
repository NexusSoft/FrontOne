using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.WinForms.Forms.Catalogos;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Pallets;

// Captura de una línea de detalle del Pallet. No guarda nada por su cuenta: expone la selección
// (CorridaId/ProductoTerminadoId/Cajas/%MS) y quien la abre (PalletEditarForm) decide si es un
// alta o una edición. Así el mismo diálogo sirve para las dos cosas.
public partial class PalletDetalleCapturaForm : XtraForm
{
    private readonly PalletService _palletService = null!;
    private readonly ProductoTerminadoService _productoTerminadoService = null!;
    private readonly CategoriaService _categoriaService = null!;
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly MarcaService _marcaService = null!;
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly PaisService _paisService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly SessionContext _sessionContext = null!;

    private readonly string _palletFolio = string.Empty;
    private readonly int? _lineaProduccionId;
    private readonly bool _esMixto;
    private readonly int? _productoTerminadoIdEncabezado;
    private readonly int _cajasYaEnPallet;
    private readonly PalletDetalleDto? _detalleExistente;

    private List<LoteEnProcesoParaPalletDto> _lotes = new();
    private List<ProductoTerminadoDto> _productos = new();

    // Resultado de la captura, leído por PalletEditarForm cuando el diálogo cierra en OK.
    public int CorridaIdSeleccionada { get; private set; }

    public int ProductoTerminadoIdSeleccionado { get; private set; }

    public int? CajasCapturadas { get; private set; }

    // Solo trae valor cuando el producto seleccionado es Granel — en Caja se queda null (el SP
    // calcula Kilogramos a partir de Cajas × Peso Estándar).
    public decimal? KilogramosCapturados { get; private set; }

    public decimal PorcentajeMateriaSecaSeleccionado { get; private set; }

    public PalletDetalleCapturaForm()
    {
        InitializeComponent();
    }

    public PalletDetalleCapturaForm(
        PalletService palletService,
        ProductoTerminadoService productoTerminadoService,
        CategoriaService categoriaService,
        TipoProductoService tipoProductoService,
        CalibreApeamService calibreApeamService,
        MarcaService marcaService,
        PesoEstandarService pesoEstandarService,
        PaisService paisService,
        VariedadService variedadService,
        SessionContext sessionContext,
        string palletFolio,
        int? lineaProduccionId,
        bool esMixto,
        int? productoTerminadoIdEncabezado,
        int cajasYaEnPallet,
        PalletDetalleDto? detalleExistente)
        : this()
    {
        _palletService = palletService;
        _productoTerminadoService = productoTerminadoService;
        _categoriaService = categoriaService;
        _tipoProductoService = tipoProductoService;
        _calibreApeamService = calibreApeamService;
        _marcaService = marcaService;
        _pesoEstandarService = pesoEstandarService;
        _paisService = paisService;
        _variedadService = variedadService;
        _sessionContext = sessionContext;
        _palletFolio = palletFolio;
        _lineaProduccionId = lineaProduccionId;
        _esMixto = esMixto;
        _productoTerminadoIdEncabezado = productoTerminadoIdEncabezado;
        _cajasYaEnPallet = cajasYaEnPallet;
        _detalleExistente = detalleExistente;

        Load += async (_, _) => await CargarAsync();
    }

    private async Task CargarAsync()
    {
        _txtPallet.Text = _palletFolio;

        await CargarLotesAsync();
        await CargarProductosAsync();

        if (_detalleExistente is not null)
        {
            // Al editar, el Lote ya no se puede cambiar: mover una línea de un lote a otro
            // implicaría revertir kilos de una corrida y cargarlos a otra en el mismo movimiento.
            // Para eso se elimina la línea y se captura de nuevo.
            _cmbLote.EditValue = _detalleExistente.CorridaId;
            _cmbLote.Properties.ReadOnly = true;
            _cmbProducto.EditValue = _detalleExistente.ProductoTerminadoId;
            if (_detalleExistente.Cajas is { } cajasExistente)
            {
                _spnCajas.EditValue = cajasExistente;
            }
            else
            {
                _spnKilogramos.EditValue = _detalleExistente.Kilogramos;
            }
            Text = "Editar línea del pallet";
        }

        // Pallet no mixto: el producto ya viene fijado por el encabezado, el usuario solo captura
        // cajas — se autoselecciona y se bloquea, igual que el Lote al editar una línea existente.
        if (!_esMixto && _productoTerminadoIdEncabezado is { } productoId)
        {
            _cmbProducto.EditValue = productoId;
            _cmbProducto.Properties.ReadOnly = true;
        }

        ActualizarDatosLote();
        ActualizarDatosProducto();
        RecalcularKilogramos();
    }

    private async Task CargarLotesAsync()
    {
        _lotes = (await _palletService.ObtenerLotesEnProcesoAsync(_lineaProduccionId)).ToList();

        _cmbLote.Properties.DataSource = _lotes;
        _cmbLote.Properties.ValueMember = "CorridaId";
        _cmbLote.Properties.DisplayMember = "LoteFolio";
        _cmbLote.Properties.Columns.Clear();
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("LoteFolio", 90, "No. de Lote"));
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("HuertaNombre", 200, "Huerta"));
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("ProductorNombre", 200, "Productor"));
        _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("KilosDisponibles", 110, "Kg disponibles"));
        _cmbLote.Properties.PopupWidth = 630;
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

        // Limpia Cajas/Kilogramos al cambiar de producto interactivamente (los valores del
        // producto anterior ya no aplican) — no se limpia durante la carga inicial de una línea
        // existente, esa pasa por CargarAsync directo sin disparar este evento.
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

    // Huerta/Registro Sagarpa/Productor y el % Materia Seca de la línea salen del Lote elegido —
    // el mismo shape que ya trae el SP, sin otra llamada al servidor.
    private void ActualizarDatosLote()
    {
        var lote = ObtenerLoteSeleccionado();

        _txtLineaProduccion.Text = lote?.LineaProduccionNombre ?? string.Empty;
        _txtHuerta.Text = lote?.HuertaNombre ?? string.Empty;
        _txtRegistroSagarpa.Text = lote?.RegistroSagarpa ?? string.Empty;
        _txtProductor.Text = lote?.ProductorNombre ?? string.Empty;
        _spnKilosDisponibles.EditValue = lote?.KilosDisponibles ?? 0m;
        _spnPorcentajeMateriaSeca.EditValue = lote?.PorcentajeMateriaSeca ?? 0m;
    }

    // Peso Estándar (PesoNeto por caja) y Cajas por Pallet salen del Producto Terminado. Producto
    // Granel: no hay cajas que capturar, se habilita Kilogramos para captura directa y se
    // deshabilita Cajas; Producto Caja: comportamiento actual (Kilogramos solo lectura, calculado).
    private void ActualizarDatosProducto()
    {
        var producto = ObtenerProductoSeleccionado();
        var esGranel = EsGranelSeleccionado();

        _spnPesoEstandar.EditValue = producto?.PesoNeto ?? 0m;
        _spnCajasPorPallet.EditValue = producto?.CajasPorPallet ?? 0;

        _spnCajas.Enabled = !esGranel;
        _spnKilogramos.Properties.ReadOnly = !esGranel;
    }

    // Kilogramos: en Caja nunca se captura, es Peso Estándar del producto × Cajas (el mismo
    // cálculo que repite el SP del lado del servidor, acá es solo la vista previa). En Granel el
    // usuario lo captura directo — no se recalcula ni se sobreescribe.
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

    private LoteEnProcesoParaPalletDto? ObtenerLoteSeleccionado()
        => _cmbLote.EditValue is int corridaId ? _lotes.FirstOrDefault(l => l.CorridaId == corridaId) : null;

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
            XtraMessageBox.Show(this, "Selecciona un lote en proceso.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var producto = ObtenerProductoSeleccionado();
        if (producto is null)
        {
            XtraMessageBox.Show(this, "Selecciona un producto terminado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Granel: solo exige Kilogramos > 0, sin validar Peso Neto/Cajas por Pallet/tope de cajas
        // (ninguno de esos conceptos aplica a un producto que se pesa directo).
        if (producto.Presentacion == PresentacionProducto.Granel)
        {
            var kilogramos = Convert.ToDecimal(_spnKilogramos.EditValue);
            if (kilogramos <= 0)
            {
                XtraMessageBox.Show(this, "Captura los Kilogramos de esta línea.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CorridaIdSeleccionada = lote.CorridaId;
            ProductoTerminadoIdSeleccionado = producto.Id;
            CajasCapturadas = null;
            KilogramosCapturados = kilogramos;
            PorcentajeMateriaSecaSeleccionado = lote.PorcentajeMateriaSeca;

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

        // Validación en cliente (la definitiva vive en el SP): pallets no mixtos no pueden
        // exceder, entre todas sus líneas, el objetivo de cajas del producto del encabezado.
        if (!_esMixto && (_cajasYaEnPallet + cajas) > producto.CajasPorPallet)
        {
            XtraMessageBox.Show(this,
                $"No se puede exceder el objetivo de {producto.CajasPorPallet} cajas del producto — ya hay {_cajasYaEnPallet} cajas capturadas en este pallet.",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        CorridaIdSeleccionada = lote.CorridaId;
        ProductoTerminadoIdSeleccionado = producto.Id;
        CajasCapturadas = cajas;
        KilogramosCapturados = null;
        PorcentajeMateriaSecaSeleccionado = lote.PorcentajeMateriaSeca;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
