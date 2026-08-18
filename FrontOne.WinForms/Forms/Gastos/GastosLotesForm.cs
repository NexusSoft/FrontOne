using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Gastos;

public partial class GastosLotesForm : XtraForm
{
    private readonly GastoLoteService _gastoLoteService = null!;
    private readonly GastoFrutaCategoriaService _gastoFrutaCategoriaService = null!;
    private readonly GastoRecepcionService _gastoRecepcionService = null!;
    private readonly GastoRecepcionAjusteService _gastoRecepcionAjusteService = null!;
    private readonly TipoAjusteService _tipoAjusteService = null!;
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SqlOptions _sqlOptions = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly RecepcionFrutaService _recepcionFrutaService = null!;
    private readonly OrdenCorteService _ordenCorteService = null!;
    private readonly HuertaService _huertaService = null!;
    private readonly FloracionService _floracionService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly ListaPrecioAcarreoService _listaPrecioAcarreoService = null!;
    private readonly ZonaService _zonaService = null!;
    private readonly ListaPrecioCorteService _listaPrecioCorteService = null!;
    private readonly JefeAcopioService _jefeAcopioService = null!;
    private readonly TipoCorteService _tipoCorteService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PoblacionService _poblacionService = null!;
    private readonly CajaCampoService _cajaCampoService = null!;

    public GastosLotesForm()
    {
        InitializeComponent();
    }

    public GastosLotesForm(
        GastoLoteService gastoLoteService,
        GastoFrutaCategoriaService gastoFrutaCategoriaService,
        GastoRecepcionService gastoRecepcionService,
        GastoRecepcionAjusteService gastoRecepcionAjusteService,
        TipoAjusteService tipoAjusteService,
        ListaPrecioFrutaService listaPrecioFrutaService,
        EmpresaConfiguracionService empresaConfiguracionService,
        SqlOptions sqlOptions,
        SessionContext sessionContext,
        RecepcionFrutaService recepcionFrutaService,
        OrdenCorteService ordenCorteService,
        HuertaService huertaService,
        FloracionService floracionService,
        VariedadService variedadService,
        ListaPrecioAcarreoService listaPrecioAcarreoService,
        ZonaService zonaService,
        ListaPrecioCorteService listaPrecioCorteService,
        JefeAcopioService jefeAcopioService,
        TipoCorteService tipoCorteService,
        PaisService paisService,
        EstadoService estadoService,
        MunicipioService municipioService,
        PoblacionService poblacionService,
        CajaCampoService cajaCampoService)
        : this()
    {
        _gastoLoteService = gastoLoteService;
        _gastoFrutaCategoriaService = gastoFrutaCategoriaService;
        _gastoRecepcionService = gastoRecepcionService;
        _gastoRecepcionAjusteService = gastoRecepcionAjusteService;
        _tipoAjusteService = tipoAjusteService;
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _sqlOptions = sqlOptions;
        _sessionContext = sessionContext;
        _recepcionFrutaService = recepcionFrutaService;
        _ordenCorteService = ordenCorteService;
        _huertaService = huertaService;
        _floracionService = floracionService;
        _variedadService = variedadService;
        _listaPrecioAcarreoService = listaPrecioAcarreoService;
        _zonaService = zonaService;
        _listaPrecioCorteService = listaPrecioCorteService;
        _jefeAcopioService = jefeAcopioService;
        _tipoCorteService = tipoCorteService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;
        _poblacionService = poblacionService;
        _cajaCampoService = cajaCampoService;

        _gridView.DoubleClick += GridView_DoubleClick;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var lotes = await _gastoLoteService.ObtenerLotesCosteablesAsync();
        _grid.DataSource = lotes.ToList();
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => AbrirSeleccionado();

    private void BtnAbrir_Click(object? sender, EventArgs e) => AbrirSeleccionado();

    private void AbrirSeleccionado()
    {
        if (_gridView.GetFocusedRow() is not GastoLoteListadoDto seleccionado)
        {
            XtraMessageBox.Show(this, "Selecciona un lote.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new GastoLoteForm(
            seleccionado.LoteId,
            _gastoLoteService,
            _gastoFrutaCategoriaService,
            _gastoRecepcionService,
            _gastoRecepcionAjusteService,
            _tipoAjusteService,
            _listaPrecioFrutaService,
            _empresaConfiguracionService,
            _sqlOptions,
            _sessionContext,
            _recepcionFrutaService,
            _ordenCorteService,
            _huertaService,
            _floracionService,
            _variedadService,
            _listaPrecioAcarreoService,
            _zonaService,
            _listaPrecioCorteService,
            _jefeAcopioService,
            _tipoCorteService,
            _paisService,
            _estadoService,
            _municipioService,
            _poblacionService,
            _cajaCampoService);
        form.ShowDialog(this);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
