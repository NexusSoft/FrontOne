using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class AcuerdoCorteEditarForm : XtraForm
{
    private readonly AcuerdoCorteService _acuerdoCorteService = null!;
    private readonly ProductorService _productorService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly ProductoService _productoService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly TipoComercializacionService _tipoComercializacionService = null!;
    private readonly TipoCorteService _tipoCorteService = null!;
    private readonly TipoPagoService _tipoPagoService = null!;
    private readonly MonedaService _monedaService = null!;
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly AcuerdoCorteDto? _acuerdoExistente;

    public event EventHandler? Guardado;

    private IReadOnlyList<TipoCorteDto> _tiposCorte = [];

    private int? _productorId;
    private int? _listaPrecioProductorId;
    private DateTime? _listaPrecioFecha;
    private string? _listaPrecioProductorNombre;

    public AcuerdoCorteEditarForm()
    {
        InitializeComponent();
    }

    public AcuerdoCorteEditarForm(
        AcuerdoCorteService acuerdoCorteService,
        ProductorService productorService,
        PaisService paisService,
        EstadoService estadoService,
        ProductoService productoService,
        VariedadService variedadService,
        TipoComercializacionService tipoComercializacionService,
        TipoCorteService tipoCorteService,
        TipoPagoService tipoPagoService,
        MonedaService monedaService,
        ListaPrecioFrutaService listaPrecioFrutaService,
        AcuerdoCorteDto? acuerdoExistente)
        : this()
    {
        _acuerdoCorteService = acuerdoCorteService;
        _productorService = productorService;
        _paisService = paisService;
        _estadoService = estadoService;
        _productoService = productoService;
        _variedadService = variedadService;
        _tipoComercializacionService = tipoComercializacionService;
        _tipoCorteService = tipoCorteService;
        _tipoPagoService = tipoPagoService;
        _monedaService = monedaService;
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _acuerdoExistente = acuerdoExistente;

        Text = acuerdoExistente is null ? "Nuevo acuerdo de corte" : "Editar acuerdo de corte";

        _txtFolio.Text = acuerdoExistente?.Folio ?? "(se genera al guardar)";
        MostrarModoPrecio(necesitaLista: false);

        Load += async (_, _) => await CargarCatalogosAsync();
    }

    private async Task CargarCatalogosAsync()
    {
        await CargarProductosAsync();
        await CargarVariedadesAsync();
        await CargarTiposComercializacionAsync();
        await CargarTiposCorteAsync();
        await CargarMonedasAsync();

        if (_acuerdoExistente is { } acuerdo)
        {
            _productorId = acuerdo.ProductorId;
            _cmbProductor.Text = acuerdo.ProductorNombre;
            _dtFechaInicio.EditValue = acuerdo.FechaInicio;
            _dtFechaFin.EditValue = acuerdo.FechaFin;
            _cmbProducto.EditValue = acuerdo.ProductoId;
            _cmbVariedad.EditValue = acuerdo.VariedadId;
            _cmbTipoComercializacion.EditValue = acuerdo.TipoComercializacionId;
            _cmbTipoCorte.EditValue = acuerdo.TipoCorteId;
            _cmbMoneda.EditValue = acuerdo.MonedaId;
            _txtObservaciones.Text = acuerdo.Observaciones;

            if (acuerdo.ListaPrecioFecha is { } fecha)
            {
                _listaPrecioFecha = fecha;
                _listaPrecioProductorId = acuerdo.ListaPrecioProductorId;
                _listaPrecioProductorNombre = acuerdo.ListaPrecioProductorNombre;
                _cmbListaPrecios.Text = DescribirListaPrecios(fecha, acuerdo.ListaPrecioProductorNombre);
                _cmbListaPrecioNumero.SelectedIndex = (acuerdo.ListaPrecioNumero ?? 1) - 1;
                MostrarModoPrecio(necesitaLista: true);
            }
            else
            {
                _spnPrecio.Value = acuerdo.Precio ?? 0;
                MostrarModoPrecio(necesitaLista: false);
            }
        }
        else
        {
            _dtFechaInicio.EditValue = DateTime.Today;
        }
    }

    private async Task CargarProductosAsync()
    {
        var productos = await _productoService.ObtenerAsync();
        _cmbProducto.Properties.DataSource = productos.ToList();
        _cmbProducto.Properties.ValueMember = "Id";
        _cmbProducto.Properties.DisplayMember = "Nombre";
        _cmbProducto.Properties.Columns.Clear();
        _cmbProducto.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Producto"));
        _cmbProducto.Properties.PopupWidth = 250;
    }

    private async Task CargarVariedadesAsync()
    {
        var variedades = await _variedadService.ObtenerAsync();
        _cmbVariedad.Properties.DataSource = variedades.ToList();
        _cmbVariedad.Properties.ValueMember = "Id";
        _cmbVariedad.Properties.DisplayMember = "Nombre";
        _cmbVariedad.Properties.Columns.Clear();
        _cmbVariedad.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Variedad"));
        _cmbVariedad.Properties.PopupWidth = 250;
    }

    private async Task CargarTiposComercializacionAsync()
    {
        var tipos = await _tipoComercializacionService.ObtenerAsync();
        _cmbTipoComercializacion.Properties.DataSource = tipos.ToList();
        _cmbTipoComercializacion.Properties.ValueMember = "Id";
        _cmbTipoComercializacion.Properties.DisplayMember = "Nombre";
        _cmbTipoComercializacion.Properties.Columns.Clear();
        _cmbTipoComercializacion.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 240, "Tipo de comercialización"));
        _cmbTipoComercializacion.Properties.PopupWidth = 270;
    }

    private async Task CargarTiposCorteAsync()
    {
        var tipos = await _tipoCorteService.ObtenerAsync();
        _tiposCorte = tipos;
        _cmbTipoCorte.Properties.DataSource = tipos.ToList();
        _cmbTipoCorte.Properties.ValueMember = "Id";
        _cmbTipoCorte.Properties.DisplayMember = "Nombre";
        _cmbTipoCorte.Properties.Columns.Clear();
        _cmbTipoCorte.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Tipo de corte"));
        _cmbTipoCorte.Properties.PopupWidth = 250;
    }

    private async Task CargarMonedasAsync()
    {
        var monedas = await _monedaService.ObtenerAsync();
        _cmbMoneda.Properties.DataSource = monedas.ToList();
        _cmbMoneda.Properties.ValueMember = "Id";
        _cmbMoneda.Properties.DisplayMember = "Nombre";
        _cmbMoneda.Properties.Columns.Clear();
        _cmbMoneda.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 200, "Moneda"));
        _cmbMoneda.Properties.Columns.Add(new LookUpColumnInfo("Nomenclatura", 100, "Nomenclatura"));
        _cmbMoneda.Properties.PopupWidth = 320;
    }

    private static string DescribirListaPrecios(DateTime fecha, string? productorNombre)
        => $"{fecha:dd/MM/yyyy} - {(string.IsNullOrEmpty(productorNombre) ? "General" : productorNombre)}";

    // El precio se captura fijo o se referencia una lista de precios, nunca ambos — lo decide
    // el TipoPago del TipoCorte elegido (Acopio.TipoPago.NecesitaListaPrecios).
    private void MostrarModoPrecio(bool necesitaLista)
    {
        _lblPrecio.Visible = !necesitaLista;
        _spnPrecio.Visible = !necesitaLista;
        _lblListaPrecios.Visible = necesitaLista;
        _cmbListaPrecios.Visible = necesitaLista;
        _lblListaNro.Visible = necesitaLista;
        _cmbListaPrecioNumero.Visible = necesitaLista;
        _btnVerListaPrecios.Visible = necesitaLista;
    }

    private async void CmbTipoCorte_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cmbTipoCorte.EditValue is not int tipoCorteId)
        {
            _txtTipoPago.Text = string.Empty;
            return;
        }

        _txtTipoPago.Text = _tiposCorte.FirstOrDefault(t => t.Id == tipoCorteId)?.TipoPagoNombre ?? string.Empty;

        var necesitaLista = await _acuerdoCorteService.NecesitaListaDePreciosAsync(tipoCorteId);
        MostrarModoPrecio(necesitaLista);
    }

    private void CmbProductor_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Search)
        {
            return;
        }

        using var buscador = new ProductoresForm(_productorService, _paisService, _estadoService);
        if (buscador.ShowDialog(this) != DialogResult.OK || buscador.ProductorSeleccionado is null)
        {
            return;
        }

        _productorId = buscador.ProductorSeleccionado.Id;
        _cmbProductor.Text = buscador.ProductorSeleccionado.NombreProductor;
    }

    private void CmbListaPrecios_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Search)
        {
            return;
        }

        if (_productorId is not int productorId)
        {
            XtraMessageBox.Show(this, "Selecciona primero el productor.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_dtFechaInicio.EditValue is not DateTime fechaInicio || _dtFechaFin.EditValue is not DateTime fechaFin)
        {
            XtraMessageBox.Show(this, "Captura primero la vigencia del acuerdo (fecha y caducidad).", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var buscador = new BuscarListaPrecioFrutaForm(_listaPrecioFrutaService, productorId, fechaInicio, fechaFin);
        if (buscador.ShowDialog(this) != DialogResult.OK || buscador.VigenciaSeleccionada is not { } vigencia)
        {
            return;
        }

        _listaPrecioFecha = vigencia.Fecha;
        _listaPrecioProductorId = vigencia.ProductorId;
        _listaPrecioProductorNombre = vigencia.ProductorNombre;
        _cmbListaPrecios.Text = DescribirListaPrecios(vigencia.Fecha, vigencia.ProductorNombre);
    }

    private void BtnVerListaPrecios_Click(object? sender, EventArgs e)
    {
        if (_listaPrecioFecha is not DateTime fecha)
        {
            XtraMessageBox.Show(this, "Selecciona primero la lista de precios.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        int? listaSeleccionada = _cmbListaPrecioNumero.SelectedIndex >= 0 ? _cmbListaPrecioNumero.SelectedIndex + 1 : null;

        using var visor = new VerListaPrecioFrutaForm(_listaPrecioFrutaService, fecha, _listaPrecioProductorId, _listaPrecioProductorNombre, listaSeleccionada);
        visor.ShowDialog(this);
    }

    private async void CmbProducto_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new ProductosForm(_productoService);
        form.ShowDialog(this);
        await CargarProductosAsync();
    }

    private async void CmbVariedad_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new VariedadesForm(_variedadService);
        form.ShowDialog(this);
        await CargarVariedadesAsync();
    }

    private async void CmbTipoComercializacion_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new TiposComercializacionForm(_tipoComercializacionService);
        form.ShowDialog(this);
        await CargarTiposComercializacionAsync();
    }

    private async void CmbTipoCorte_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new TiposCorteForm(_tipoCorteService, _tipoPagoService);
        form.ShowDialog(this);
        await CargarTiposCorteAsync();
    }

    private async void CmbMoneda_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new MonedasForm(_monedaService);
        form.ShowDialog(this);
        await CargarMonedasAsync();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_productorId is not int productorId)
        {
            XtraMessageBox.Show(this, "Selecciona el productor.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_dtFechaInicio.EditValue is not DateTime fechaInicio || _dtFechaFin.EditValue is not DateTime fechaFin)
        {
            XtraMessageBox.Show(this, "Captura la fecha y la caducidad del acuerdo.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbProducto.EditValue is not int productoId || _cmbVariedad.EditValue is not int variedadId
            || _cmbTipoComercializacion.EditValue is not int tipoComercializacionId || _cmbTipoCorte.EditValue is not int tipoCorteId
            || _cmbMoneda.EditValue is not int monedaId)
        {
            XtraMessageBox.Show(this, "Completa Producto, Variedad, Tipo de comercialización, Tipo de corte y Moneda.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var necesitaLista = _lblListaPrecios.Visible;

        int? listaPrecioNumero = _cmbListaPrecioNumero.SelectedIndex >= 0 ? _cmbListaPrecioNumero.SelectedIndex + 1 : null;

        if (necesitaLista && listaPrecioNumero is null)
        {
            XtraMessageBox.Show(this, $"Selecciona cuál de las 3 listas de precios ({string.Join(", ", ListasPrecioFruta.Nombres)}) aplica a este acuerdo.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var datos = new AcuerdoCorteDto(
            _acuerdoExistente?.Id ?? 0,
            _acuerdoExistente?.Folio ?? string.Empty,
            productorId,
            _cmbProductor.Text,
            fechaInicio,
            fechaFin,
            productoId,
            _cmbProducto.Text,
            variedadId,
            _cmbVariedad.Text,
            tipoComercializacionId,
            _cmbTipoComercializacion.Text,
            tipoCorteId,
            _cmbTipoCorte.Text,
            necesitaLista ? null : _spnPrecio.Value,
            necesitaLista ? _listaPrecioFecha : null,
            necesitaLista ? _listaPrecioProductorId : null,
            null,
            necesitaLista ? listaPrecioNumero : null,
            monedaId,
            _cmbMoneda.Text,
            string.IsNullOrWhiteSpace(_txtObservaciones.Text) ? null : _txtObservaciones.Text,
            true);

        try
        {
            if (_acuerdoExistente is null)
            {
                await _acuerdoCorteService.CrearAsync(datos);
            }
            else
            {
                await _acuerdoCorteService.ActualizarAsync(datos);
            }

            Guardado?.Invoke(this, EventArgs.Empty);
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
        Close();
    }
}
