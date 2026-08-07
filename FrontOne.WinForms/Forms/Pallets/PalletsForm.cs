using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Pallets;

// Listado de Pallets ya capturados, para editarlos/completarlos. Mismo molde que CorridasForm:
// tab MDI singleton con filtro por Status y carga en Shown (no en Load: en un tab MDI el grid
// todavía no tiene su tamaño real durante Load).
public partial class PalletsForm : XtraForm
{
    private readonly PalletService _palletService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly ProductoTerminadoService _productoTerminadoService = null!;

    // Servicios que solo hacen falta para el botón `+` del combo de Producto Terminado dentro de
    // PalletDetalleCapturaForm (abre ProductosTerminadosForm, que los pide todos) — mismo criterio
    // de plomería que ya usa LotesForm con AcuerdoCorte/OrdenCorte.
    private readonly CategoriaService _categoriaService = null!;
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly MarcaService _marcaService = null!;
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly PaisService _paisService = null!;
    private readonly VariedadService _variedadService = null!;

    private readonly ConfiguracionBasculaService _configuracionBasculaService = null!;
    private readonly ReportePlantillaService _reportePlantillaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly SqlOptions _sqlOptions = null!;

    private PalletEditarForm? _palletEditarForm;
    private List<PalletDto> _pallets = new();

    public PalletsForm()
    {
        InitializeComponent();
    }

    public PalletsForm(
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
        ReportePlantillaService reportePlantillaService,
        EmpresaConfiguracionService empresaConfiguracionService,
        SessionContext sessionContext,
        SqlOptions sqlOptions)
        : this()
    {
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
        _reportePlantillaService = reportePlantillaService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _sessionContext = sessionContext;
        _sqlOptions = sqlOptions;

        // El orden de estas entradas es el valor del Estatus (índice 0 = "Todos"), así el filtro
        // se resuelve comparando contra el índice sin tabla de traducción aparte.
        _cmbFiltroStatus.Properties.Items.AddRange(new object[]
        {
            "Todos", "Vacío", "Incompleto", "Completo", "Excedido", "Empacado", "Reempacado",
        });
        _cmbFiltroStatus.SelectedIndex = 0;

        Shown += async (_, _) => await CargarDatosAsync();
        _grid.SizeChanged += (_, _) => AjustarAnchoColumnas();
    }

    private async Task CargarDatosAsync()
    {
        _pallets = (await _palletService.ObtenerAsync()).ToList();
        AplicarFiltro();
    }

    private void AplicarFiltro()
    {
        var indice = _cmbFiltroStatus.SelectedIndex;
        IEnumerable<PalletDto> filtrados = indice <= 0
            ? _pallets
            : _pallets.Where(p => p.Estatus == indice);

        _grid.DataSource = filtrados.ToList();
        ConfigurarColumnas();
    }

    private void CmbFiltroStatus_SelectedIndexChanged(object? sender, EventArgs e) => AplicarFiltro();

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[]
        {
            "Id", "LineaProduccionId", "FechaCreacionRegistro", "FechaBloqueo", "PrimeraCorrida",
            "PorcentajeMateriaSeca", "PesoReal", "EsMixto", "NoReempaque", "HoraCreacion",
        })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Folio"] is { } colFolio)
        {
            colFolio.Caption = "No. de Pallet";
        }

        if (_gridView.Columns["Estatus"] is { } colEstatus)
        {
            colEstatus.Caption = "Status";
        }

        if (_gridView.Columns["LineaProduccionNombre"] is { } colLinea)
        {
            colLinea.Caption = "Línea de Producción";
        }

        if (_gridView.Columns["FechaCreacion"] is { } colFecha)
        {
            colFecha.Caption = "Fecha";
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["TotalCajas"] is { } colCajas)
        {
            colCajas.Caption = "Cajas";
        }

        if (_gridView.Columns["TotalKilogramos"] is { } colKilos)
        {
            colKilos.Caption = "Kilogramos";
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["ProductoDescripcion"] is { } colProducto)
        {
            colProducto.Caption = "Producto";
        }

        if (_gridView.Columns["Bloqueado"] is { } colBloqueado)
        {
            colBloqueado.Caption = "Bloqueado";
        }

        // Orden explícito: asignar VisibleIndex uno por uno es la única forma confiable de fijar
        // el orden de columnas en DevExpress (mismo criterio ya usado en CorridasForm).
        var ordenColumnas = new[]
        {
            "Folio", "LineaProduccionNombre", "FechaCreacion", "TotalCajas", "TotalKilogramos",
            "ProductoDescripcion", "Bloqueado", "Estatus",
        };
        for (var i = 0; i < ordenColumnas.Length; i++)
        {
            if (_gridView.Columns[ordenColumnas[i]] is { } columnaOrdenada)
            {
                columnaOrdenada.VisibleIndex = i;
            }
        }

        AjustarAnchoColumnas();
    }

    private void AjustarAnchoColumnas()
    {
        if (_gridView.Columns.Count == 0)
        {
            return;
        }

        _gridView.BestFitColumns();
    }

    private void GridView_CustomColumnDisplayText(object? sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName != "Estatus" || e.Value is not byte estatus)
        {
            return;
        }

        e.DisplayText = NombreEstatus(estatus);
    }

    internal static string NombreEstatus(byte estatus) => estatus switch
    {
        1 => "Vacío",
        2 => "Incompleto",
        3 => "Completo",
        4 => "Excedido",
        5 => "Empacado",
        6 => "Reempacado",
        _ => string.Empty,
    };

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

    private void BtnNuevo_Click(object? sender, EventArgs e) => AbrirEditarForm(null);

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un pallet.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(PalletDto? palletExistente)
    {
        if (_palletEditarForm is { IsDisposed: false })
        {
            if (_palletEditarForm.WindowState == FormWindowState.Minimized)
            {
                _palletEditarForm.WindowState = FormWindowState.Normal;
            }

            _palletEditarForm.Activate();
            return;
        }

        _palletEditarForm = new PalletEditarForm(
            _palletService, _lineaProduccionService, _productoTerminadoService,
            _categoriaService, _tipoProductoService, _calibreApeamService, _marcaService,
            _pesoEstandarService, _paisService, _variedadService,
            _configuracionBasculaService, _reportePlantillaService, _empresaConfiguracionService,
            _sessionContext, _sqlOptions, palletExistente);
        _palletEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _palletEditarForm.FormClosed += (_, _) => _palletEditarForm = null;
        _palletEditarForm.Show(this);
    }

    // Eliminar un Pallet completo revierte, del lado del SP, los Kilogramos de todas sus líneas
    // contra la Corrida correspondiente. Un Pallet bloqueado no se puede eliminar (lo rechaza el
    // propio SP con THROW 50000, y aquí además se avisa antes de intentarlo).
    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un pallet.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (seleccionado.Bloqueado)
        {
            XtraMessageBox.Show(this, "Este pallet ya está bloqueado: no se puede eliminar.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Eliminar el pallet '{seleccionado.Folio}'? Los kilogramos de sus líneas se devolverán al saldo de la corrida correspondiente.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _palletService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private PalletDto? ObtenerSeleccionado() => _gridView.GetFocusedRow() as PalletDto;
}
