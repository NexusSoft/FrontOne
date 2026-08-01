using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Lotes;

public partial class LotesForm : XtraForm
{
    private readonly LoteService _loteService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly SessionContext _sessionContext = null!;
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
    private readonly OrdenCorteService _ordenCorteService = null!;
    private readonly HuertaService _huertaService = null!;
    private readonly FloracionService _floracionService = null!;
    private readonly ListaPrecioAcarreoService _listaPrecioAcarreoService = null!;
    private readonly ZonaService _zonaService = null!;
    private readonly ListaPrecioCorteService _listaPrecioCorteService = null!;
    private readonly JefeAcopioService _jefeAcopioService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PoblacionService _poblacionService = null!;
    private readonly RecepcionFrutaService _recepcionFrutaService = null!;

    private LoteEditarForm? _loteEditarForm;

    public LotesForm()
    {
        InitializeComponent();
    }

    public LotesForm(
        LoteService loteService,
        LineaProduccionService lineaProduccionService,
        SessionContext sessionContext,
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
        OrdenCorteService ordenCorteService,
        HuertaService huertaService,
        FloracionService floracionService,
        ListaPrecioAcarreoService listaPrecioAcarreoService,
        ZonaService zonaService,
        ListaPrecioCorteService listaPrecioCorteService,
        JefeAcopioService jefeAcopioService,
        MunicipioService municipioService,
        PoblacionService poblacionService,
        RecepcionFrutaService recepcionFrutaService)
        : this()
    {
        _loteService = loteService;
        _lineaProduccionService = lineaProduccionService;
        _sessionContext = sessionContext;
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
        _ordenCorteService = ordenCorteService;
        _huertaService = huertaService;
        _floracionService = floracionService;
        _listaPrecioAcarreoService = listaPrecioAcarreoService;
        _zonaService = zonaService;
        _listaPrecioCorteService = listaPrecioCorteService;
        _jefeAcopioService = jefeAcopioService;
        _municipioService = municipioService;
        _poblacionService = poblacionService;
        _recepcionFrutaService = recepcionFrutaService;

        _gridView.DoubleClick += GridView_DoubleClick;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var lotes = await _loteService.ObtenerAsync();
        _grid.DataSource = lotes.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "Id", "Observaciones", "Personalizado", "LineaProduccionId", "Estatus" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["LineaProduccionNombre"] is { } colLinea)
        {
            colLinea.Caption = "Línea de Producción";
        }

        if (_gridView.Columns["HuertaNombre"] is { } colHuerta)
        {
            colHuerta.Caption = "Huerta";
        }

        if (_gridView.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.Caption = "Peso Neto";
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["PorcentajeMateriaSeca"] is { } colMateriaSeca)
        {
            colMateriaSeca.Caption = "Materia Seca";
            colMateriaSeca.DisplayFormat.FormatType = FormatType.Numeric;
            colMateriaSeca.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["ProductorNombre"] is { } colProductor)
        {
            colProductor.Caption = "Productor";
        }

        _gridView.BestFitColumns();

        // Mismo criterio que RecepcionesFrutaForm: el sobrante de ancho se lo damos a la última
        // columna (Productor) en vez de dejarlo en blanco.
        var anchoColumnas = _gridView.Columns.Cast<DevExpress.XtraGrid.Columns.GridColumn>()
            .Where(c => c.Visible)
            .Sum(c => c.Width);
        var anchoDisponible = _grid.Width - SystemInformation.VerticalScrollBarWidth;
        if (_gridView.Columns["ProductorNombre"] is { } colProductorFill && anchoDisponible > anchoColumnas)
        {
            colProductorFill.Width += anchoDisponible - anchoColumnas;
        }
    }

    // Doble clic en cualquier celda dispara Editar directo, sin tener que seleccionar la fila
    // y luego ir al botón (este grid no tiene columnas de folio-hipervínculo).
    private void GridView_DoubleClick(object? sender, EventArgs e)
    {
        var punto = _grid.PointToClient(Cursor.Position);
        var info = _gridView.CalcHitInfo(punto);
        if (!info.InRowCell)
        {
            return;
        }

        BtnEditar_Click(sender, EventArgs.Empty);
    }

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        AbrirEditarForm(null);
    }

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un lote.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(LoteDto? loteExistente)
    {
        if (_loteEditarForm is { IsDisposed: false })
        {
            if (_loteEditarForm.WindowState == FormWindowState.Minimized)
            {
                _loteEditarForm.WindowState = FormWindowState.Normal;
            }

            _loteEditarForm.Activate();
            return;
        }

        _loteEditarForm = new LoteEditarForm(
            _loteService, _lineaProduccionService, loteExistente, _sessionContext,
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService,
            _ordenCorteService, _huertaService, _floracionService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _municipioService, _poblacionService,
            _recepcionFrutaService);
        _loteEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _loteEditarForm.FormClosed += (_, _) => _loteEditarForm = null;
        _loteEditarForm.Show(this);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un lote.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el lote folio '{seleccionado.Folio}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _loteService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private LoteDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as LoteDto;
}
