using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Reempaques;

// Listado de Reempaques. Mismo molde que PalletsForm/CorridasForm.
public partial class ReempaquesForm : XtraForm
{
    private readonly ReempaqueService _reempaqueService = null!;
    private readonly PalletService _palletService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly ProductoTerminadoService _productoTerminadoService = null!;
    private readonly CategoriaService _categoriaService = null!;
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly MarcaService _marcaService = null!;
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly PaisService _paisService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly ConfiguracionBasculaService _configuracionBasculaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly EtiquetaService _etiquetaService = null!;
    private readonly LicenciaTecitService _licenciaTecitService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly SqlOptions _sqlOptions = null!;

    private ReempaqueEditarForm? _reempaqueEditarForm;
    private List<ReempaqueDto> _reempaques = new();

    public ReempaquesForm()
    {
        InitializeComponent();
    }

    public ReempaquesForm(
        ReempaqueService reempaqueService,
        PalletService palletService,
        LineaProduccionService lineaProduccionService,
        ProductoTerminadoService productoTerminadoService,
        CategoriaService categoriaService,
        TipoProductoService tipoProductoService,
        CalibreApeamService calibreApeamService,
        MarcaService marcaService,
        PesoEstandarService pesoEstandarService,
        PaisService paisService,
        VariedadService variedadService,
        ConfiguracionBasculaService configuracionBasculaService,
        EmpresaConfiguracionService empresaConfiguracionService,
        EtiquetaService etiquetaService,
        LicenciaTecitService licenciaTecitService,
        SessionContext sessionContext,
        SqlOptions sqlOptions)
        : this()
    {
        _reempaqueService = reempaqueService;
        _palletService = palletService;
        _lineaProduccionService = lineaProduccionService;
        _productoTerminadoService = productoTerminadoService;
        _categoriaService = categoriaService;
        _tipoProductoService = tipoProductoService;
        _calibreApeamService = calibreApeamService;
        _marcaService = marcaService;
        _pesoEstandarService = pesoEstandarService;
        _paisService = paisService;
        _variedadService = variedadService;
        _configuracionBasculaService = configuracionBasculaService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _etiquetaService = etiquetaService;
        _licenciaTecitService = licenciaTecitService;
        _sessionContext = sessionContext;
        _sqlOptions = sqlOptions;

        Shown += async (_, _) => await CargarDatosAsync();
        _grid.SizeChanged += (_, _) => { if (_gridView.Columns.Count > 0) _gridView.BestFitColumns(); };
    }

    private async Task CargarDatosAsync()
    {
        _reempaques = (await _reempaqueService.ObtenerAsync()).ToList();
        _grid.DataSource = _reempaques;
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "Id", "HoraCreacion", "FechaCreacionRegistro" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Folio"] is { } colFolio)
        {
            colFolio.Caption = "Folio";
        }

        if (_gridView.Columns["FechaCreacion"] is { } colFecha)
        {
            colFecha.Caption = "Fecha";
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["Estatus"] is { } colEstatus)
        {
            colEstatus.Caption = "Status";
        }

        if (_gridView.Columns["KilosAProcesar"] is { } colKap)
        {
            colKap.Caption = "Kg a Procesar";
            colKap.DisplayFormat.FormatType = FormatType.Numeric;
            colKap.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["KilosProcesados"] is { } colKp)
        {
            colKp.Caption = "Kg Procesados";
            colKp.DisplayFormat.FormatType = FormatType.Numeric;
            colKp.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["Diferencia"] is { } colDif)
        {
            colDif.DisplayFormat.FormatType = FormatType.Numeric;
            colDif.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["FechaCierre"] is { } colCierre)
        {
            colCierre.Caption = "Fecha de Cierre";
            colCierre.DisplayFormat.FormatType = FormatType.DateTime;
            colCierre.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
        }

        var orden = new[] { "Folio", "FechaCreacion", "Motivo", "Estatus", "KilosAProcesar", "KilosProcesados", "Diferencia", "FechaCierre" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridView.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridView.BestFitColumns();
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => BtnEditar_Click(sender, EventArgs.Empty);

    private void BtnNuevo_Click(object? sender, EventArgs e) => AbrirEditarForm(null);

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un reempaque.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(ReempaqueDto? existente)
    {
        if (_reempaqueEditarForm is { IsDisposed: false })
        {
            if (_reempaqueEditarForm.WindowState == FormWindowState.Minimized)
            {
                _reempaqueEditarForm.WindowState = FormWindowState.Normal;
            }

            _reempaqueEditarForm.Activate();
            return;
        }

        _reempaqueEditarForm = new ReempaqueEditarForm(
            _reempaqueService, _palletService, _lineaProduccionService, _productoTerminadoService,
            _categoriaService, _tipoProductoService, _calibreApeamService, _marcaService,
            _pesoEstandarService, _paisService, _variedadService,
            _configuracionBasculaService, _empresaConfiguracionService,
            _etiquetaService, _licenciaTecitService, _sessionContext, _sqlOptions, existente);
        _reempaqueEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _reempaqueEditarForm.FormClosed += (_, _) => _reempaqueEditarForm = null;
        _reempaqueEditarForm.Show(this);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un reempaque.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (seleccionado.Estatus != 1)
        {
            XtraMessageBox.Show(this, "Este reempaque ya está cerrado: no se puede eliminar.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Eliminar el reempaque '{seleccionado.Folio}'? Los pallets origen regresan a Empacado (pierden su marca de Reempacado). Si ya tiene cajas capturadas en pallets destino, primero hay que quitarlas desde ahí.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _reempaqueService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private ReempaqueDto? ObtenerSeleccionado() => _gridView.GetFocusedRow() as ReempaqueDto;
}
