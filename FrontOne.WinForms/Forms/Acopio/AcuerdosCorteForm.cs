using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class AcuerdosCorteForm : XtraForm
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

    public AcuerdosCorteForm()
    {
        InitializeComponent();
    }

    public AcuerdosCorteForm(
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
        ListaPrecioFrutaService listaPrecioFrutaService)
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

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var acuerdos = await _acuerdoCorteService.ObtenerAsync();
        _grid.DataSource = acuerdos.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[]
        {
            "Id", "ProductorId", "ProductoId", "VariedadId", "TipoComercializacionId", "TipoCorteId",
            "MonedaId", "ListaPrecioProductorId", "Activo",
        })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["ProductorNombre"] is { } colProductor)
        {
            colProductor.Caption = "Productor";
        }

        if (_gridView.Columns["FechaInicio"] is { } colFechaInicio)
        {
            colFechaInicio.Caption = "Fecha";
            colFechaInicio.DisplayFormat.FormatType = FormatType.DateTime;
            colFechaInicio.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["FechaFin"] is { } colFechaFin)
        {
            colFechaFin.Caption = "Caducidad";
            colFechaFin.DisplayFormat.FormatType = FormatType.DateTime;
            colFechaFin.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["ProductoNombre"] is { } colProducto)
        {
            colProducto.Caption = "Producto";
        }

        if (_gridView.Columns["VariedadNombre"] is { } colVariedad)
        {
            colVariedad.Caption = "Variedad";
        }

        if (_gridView.Columns["TipoComercializacionNombre"] is { } colTipoComercializacion)
        {
            colTipoComercializacion.Caption = "Tipo de comercialización";
        }

        if (_gridView.Columns["TipoCorteNombre"] is { } colTipoCorte)
        {
            colTipoCorte.Caption = "Tipo de corte";
        }

        if (_gridView.Columns["Precio"] is { } colPrecio)
        {
            colPrecio.DisplayFormat.FormatType = FormatType.Numeric;
            colPrecio.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["ListaPrecioFecha"] is { } colListaPrecioFecha)
        {
            colListaPrecioFecha.Caption = "Lista de precios";
            colListaPrecioFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colListaPrecioFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["MonedaNombre"] is { } colMoneda)
        {
            colMoneda.Caption = "Moneda";
        }

        _gridView.BestFitColumns();
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new AcuerdoCorteEditarForm(
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService, null);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un acuerdo de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new AcuerdoCorteEditarForm(
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService, seleccionado);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un acuerdo de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el acuerdo de corte folio '{seleccionado.Folio}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _acuerdoCorteService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private AcuerdoCorteDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as AcuerdoCorteDto;
}
