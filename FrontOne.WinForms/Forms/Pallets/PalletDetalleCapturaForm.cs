using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
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
    private readonly PalletDetalleDto? _detalleExistente;

    private List<LoteEnProcesoParaPalletDto> _lotes = new();
    private List<ProductoTerminadoDto> _productos = new();

    // Resultado de la captura, leído por PalletEditarForm cuando el diálogo cierra en OK.
    public int CorridaIdSeleccionada { get; private set; }

    public int ProductoTerminadoIdSeleccionado { get; private set; }

    public int CajasCapturadas { get; private set; }

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
            _spnCajas.EditValue = _detalleExistente.Cajas;
            Text = "Editar línea del pallet";
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
        RecalcularKilogramos();
    }

    private void SpnCajas_EditValueChanged(object? sender, EventArgs e) => RecalcularKilogramos();

    // Huerta/Registro Sagarpa/Productor y el % Materia Seca de la línea salen del Lote elegido —
    // el mismo shape que ya trae el SP, sin otra llamada al servidor.
    private void ActualizarDatosLote()
    {
        var lote = ObtenerLoteSeleccionado();

        _txtHuerta.Text = lote?.HuertaNombre ?? string.Empty;
        _txtRegistroSagarpa.Text = lote?.RegistroSagarpa ?? string.Empty;
        _txtProductor.Text = lote?.ProductorNombre ?? string.Empty;
        _spnKilosDisponibles.EditValue = lote?.KilosDisponibles ?? 0m;
        _spnPorcentajeMateriaSeca.EditValue = lote?.PorcentajeMateriaSeca ?? 0m;
    }

    // Peso Estándar (PesoNeto por caja) y Cajas por Pallet salen del Producto Terminado.
    private void ActualizarDatosProducto()
    {
        var producto = ObtenerProductoSeleccionado();

        _spnPesoEstandar.EditValue = producto?.PesoNeto ?? 0m;
        _spnCajasPorPallet.EditValue = producto?.CajasPorPallet ?? 0;
    }

    // Kilogramos nunca se captura: es Peso Estándar del producto × Cajas, el mismo cálculo que
    // repite el SP del lado del servidor (acá es solo la vista previa para el usuario).
    private void RecalcularKilogramos()
    {
        var pesoEstandar = Convert.ToDecimal(_spnPesoEstandar.EditValue);
        var cajas = Convert.ToInt32(_spnCajas.EditValue);
        _spnKilogramos.EditValue = pesoEstandar * cajas;
    }

    private LoteEnProcesoParaPalletDto? ObtenerLoteSeleccionado()
        => _cmbLote.EditValue is int corridaId ? _lotes.FirstOrDefault(l => l.CorridaId == corridaId) : null;

    private ProductoTerminadoDto? ObtenerProductoSeleccionado()
        => _cmbProducto.EditValue is int productoId ? _productos.FirstOrDefault(p => p.Id == productoId) : null;

    private async void CmbProducto_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
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

        CorridaIdSeleccionada = lote.CorridaId;
        ProductoTerminadoIdSeleccionado = producto.Id;
        CajasCapturadas = cajas;
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
