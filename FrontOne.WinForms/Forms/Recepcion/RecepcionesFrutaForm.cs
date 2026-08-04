using System.Drawing;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Constants;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Acopio;
using FrontOne.WinForms.Forms.Lotes;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Recepcion;

public partial class RecepcionesFrutaForm : XtraForm
{
    private const string CodigoReporte = "RecepcionFruta";

    private static readonly Image ImagenCandadoCerrado = CargarIconoCandado("candado_cerrado.png");
    private static readonly Image ImagenCandadoAbierto = CargarIconoCandado("candado_abierto.png");

    private readonly RecepcionFrutaService _recepcionFrutaService = null!;
    private readonly ReportePlantillaService _reportePlantillaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly SqlOptions _sqlOptions = null!;

    // Servicios para abrir Acuerdo/Orden/Lote al hacer clic en su folio (columnas F. Acuerdo,
    // F. OrdenC, F. Lote) — los mismos que ya piden AcuerdoCorteEditarForm/OrdenCorteEditarForm/
    // LoteEditarForm, inyectados aquí también para poder instanciarlos desde este form.
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
    private readonly LoteService _loteService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly CajaCampoService _cajaCampoService = null!;

    private RecepcionFrutaEditarForm? _recepcionFrutaEditarForm;
    private AcuerdoCorteEditarForm? _acuerdoCorteEditarFormDesdeGrid;
    private OrdenCorteEditarForm? _ordenCorteEditarFormDesdeGrid;
    private LoteEditarForm? _loteEditarFormDesdeGrid;

    public RecepcionesFrutaForm()
    {
        InitializeComponent();
    }

    public RecepcionesFrutaForm(
        RecepcionFrutaService recepcionFrutaService,
        ReportePlantillaService reportePlantillaService,
        EmpresaConfiguracionService empresaConfiguracionService,
        SessionContext sessionContext,
        SqlOptions sqlOptions,
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
        LoteService loteService,
        LineaProduccionService lineaProduccionService,
        CajaCampoService cajaCampoService)
        : this()
    {
        _recepcionFrutaService = recepcionFrutaService;
        _reportePlantillaService = reportePlantillaService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _sessionContext = sessionContext;
        _sqlOptions = sqlOptions;
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
        _loteService = loteService;
        _lineaProduccionService = lineaProduccionService;
        _cajaCampoService = cajaCampoService;

        _btnVistaPrevia.Enabled = _sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.VistaPrevia);
        _btnDisenarReporte.Enabled = _sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.Diseno);

        _gridView.CustomDrawCell += GridView_CustomDrawCell;
        _gridView.MouseMove += GridView_MouseMove;
        _gridView.RowCellClick += GridView_RowCellClick;
        _gridView.DoubleClick += GridView_DoubleClick;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private static Image CargarIconoCandado(string nombreArchivo)
    {
        using var stream = typeof(RecepcionesFrutaForm).Assembly
            .GetManifestResourceStream($"FrontOne.WinForms.Resources.Icons.{nombreArchivo}")!;
        return Image.FromStream(stream);
    }

    // La columna "Bloqueo" (EstaEnLote) no se muestra como texto/checkbox — se dibuja un
    // candado cerrado (rojo) o abierto (verde) para que se distinga de un vistazo en el listado.
    private void GridView_CustomDrawCell(object? sender, RowCellCustomDrawEventArgs e)
    {
        if (e.Column.FieldName != "EstaEnLote")
        {
            return;
        }

        e.Appearance.FillRectangle(e.Cache, e.Bounds);

        var estaEnLote = e.CellValue is true;
        var imagen = estaEnLote ? ImagenCandadoCerrado : ImagenCandadoAbierto;
        const int tamano = 16;
        var rect = new Rectangle(
            e.Bounds.Left + (e.Bounds.Width - tamano) / 2,
            e.Bounds.Top + (e.Bounds.Height - tamano) / 2,
            tamano, tamano);
        e.Cache.DrawImage(imagen, rect);
        e.Handled = true;
    }

    // F. Acuerdo, F. OrdenC y F. Lote se ven como folio clickeable (azul + negritas) — al hacer
    // clic abren el módulo correspondiente cargado con ese registro (ver GridView_RowCellClick).
    private static void AplicarEstiloFolioClickeable(DevExpress.XtraGrid.Columns.GridColumn columna)
    {
        columna.AppearanceCell.Font = new Font(columna.AppearanceCell.Font, FontStyle.Bold | FontStyle.Underline);
        columna.AppearanceCell.ForeColor = ColorTranslator.FromHtml("#0563C1");
        columna.AppearanceCell.Options.UseFont = true;
        columna.AppearanceCell.Options.UseForeColor = true;
    }

    private static readonly string[] ColumnasFolioClickeable = ["AcuerdoCorteFolio", "OrdenCorteFolio", "NoLote"];

    private void GridView_MouseMove(object? sender, MouseEventArgs e)
    {
        var info = _gridView.CalcHitInfo(e.Location);
        _grid.Cursor = info.InRowCell && ColumnasFolioClickeable.Contains(info.Column?.FieldName)
            ? Cursors.Hand
            : Cursors.Default;
    }

    // Doble clic en cualquier celda que NO sea un folio-hipervínculo dispara Editar directo,
    // sin tener que seleccionar la fila y luego ir al botón.
    private void GridView_DoubleClick(object? sender, EventArgs e)
    {
        var punto = _grid.PointToClient(Cursor.Position);
        var info = _gridView.CalcHitInfo(punto);
        if (!info.InRowCell || ColumnasFolioClickeable.Contains(info.Column?.FieldName))
        {
            return;
        }

        BtnEditar_Click(sender, EventArgs.Empty);
    }

    private async void GridView_RowCellClick(object? sender, RowCellClickEventArgs e)
    {
        switch (e.Column.FieldName)
        {
            case "AcuerdoCorteFolio":
                await AbrirAcuerdoPorFolioAsync(e.CellValue as string);
                break;
            case "OrdenCorteFolio":
                await AbrirOrdenPorFolioAsync(e.CellValue as string);
                break;
            case "NoLote":
                await AbrirLotePorRecepcionAsync(_gridView.GetRow(e.RowHandle) as RecepcionFrutaDto);
                break;
        }
    }

    private async Task AbrirAcuerdoPorFolioAsync(string? folio)
    {
        if (string.IsNullOrWhiteSpace(folio))
        {
            return;
        }

        if (!_sessionContext.TienePermiso("Acopio", "AcuerdosCorte", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var acuerdo = await _acuerdoCorteService.ObtenerPorFolioAsync(folio);
        if (acuerdo is null)
        {
            XtraMessageBox.Show(this, "El acuerdo de corte ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_acuerdoCorteEditarFormDesdeGrid is { IsDisposed: false })
        {
            if (_acuerdoCorteEditarFormDesdeGrid.WindowState == FormWindowState.Minimized)
            {
                _acuerdoCorteEditarFormDesdeGrid.WindowState = FormWindowState.Normal;
            }

            _acuerdoCorteEditarFormDesdeGrid.Activate();
            return;
        }

        _acuerdoCorteEditarFormDesdeGrid = new AcuerdoCorteEditarForm(
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService, acuerdo);
        _acuerdoCorteEditarFormDesdeGrid.FormClosed += (_, _) => _acuerdoCorteEditarFormDesdeGrid = null;
        _acuerdoCorteEditarFormDesdeGrid.Show(this);
    }

    private async Task AbrirOrdenPorFolioAsync(string? folio)
    {
        if (string.IsNullOrWhiteSpace(folio))
        {
            return;
        }

        if (!_sessionContext.TienePermiso("Acopio", "OrdenesCorte", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var orden = await _ordenCorteService.ObtenerPorFolioAsync(folio);
        if (orden is null)
        {
            XtraMessageBox.Show(this, "La orden de corte ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_ordenCorteEditarFormDesdeGrid is { IsDisposed: false })
        {
            if (_ordenCorteEditarFormDesdeGrid.WindowState == FormWindowState.Minimized)
            {
                _ordenCorteEditarFormDesdeGrid.WindowState = FormWindowState.Normal;
            }

            _ordenCorteEditarFormDesdeGrid.Activate();
            return;
        }

        _ordenCorteEditarFormDesdeGrid = new OrdenCorteEditarForm(
            _ordenCorteService, _huertaService, _floracionService, _variedadService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _tipoCorteService,
            _paisService, _estadoService, _municipioService, _poblacionService, _cajaCampoService, orden);
        _ordenCorteEditarFormDesdeGrid.FormClosed += (_, _) => _ordenCorteEditarFormDesdeGrid = null;
        _ordenCorteEditarFormDesdeGrid.Show(this);
    }

    private async Task AbrirLotePorRecepcionAsync(RecepcionFrutaDto? recepcion)
    {
        if (recepcion is null)
        {
            return;
        }

        if (!_sessionContext.TienePermiso("Lotes", "Lotes", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var lote = await _loteService.ObtenerPorRecepcionFrutaIdAsync(recepcion.Id);
        if (lote is null)
        {
            XtraMessageBox.Show(this, "Esta recepción no forma parte de ningún Lote.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_loteEditarFormDesdeGrid is { IsDisposed: false })
        {
            if (_loteEditarFormDesdeGrid.WindowState == FormWindowState.Minimized)
            {
                _loteEditarFormDesdeGrid.WindowState = FormWindowState.Normal;
            }

            _loteEditarFormDesdeGrid.Activate();
            return;
        }

        _loteEditarFormDesdeGrid = new LoteEditarForm(
            _loteService, _lineaProduccionService, lote, _sessionContext,
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService,
            _ordenCorteService, _huertaService, _floracionService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _municipioService, _poblacionService,
            _recepcionFrutaService, _cajaCampoService);
        _loteEditarFormDesdeGrid.FormClosed += (_, _) => _loteEditarFormDesdeGrid = null;
        _loteEditarFormDesdeGrid.Show(this);
    }

    private async Task CargarDatosAsync()
    {
        var recepciones = await _recepcionFrutaService.ObtenerAsync();
        _grid.DataSource = recepciones.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[]
        {
            "Id", "Placas", "Observaciones", "PesoBruto", "PesoTara", "TaraCajas", "PesoMuestra",
            "PesoProductor", "CajasPorEntregar", "CajasEntregadas", "CajasCortadas", "CajasRecibidasVacias",
            "CajasDiferencia", "CamionDestarado", "TicketPesadaArchivo", "TicketPesadaNombreArchivo",
        })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["AcuerdoCorteFolio"] is { } colAcuerdo)
        {
            colAcuerdo.Caption = "F. Acuerdo";
            colAcuerdo.VisibleIndex = 0;
            AplicarEstiloFolioClickeable(colAcuerdo);
        }

        if (_gridView.Columns["OrdenCorteFolio"] is { } colOrden)
        {
            colOrden.Caption = "F. OrdenC";
            colOrden.VisibleIndex = 1;
            AplicarEstiloFolioClickeable(colOrden);
        }

        if (_gridView.Columns["Folio"] is { } colFolio)
        {
            colFolio.Caption = "F. Recepcion";
            colFolio.VisibleIndex = 2;
        }

        if (_gridView.Columns["NoLote"] is { } colLote)
        {
            colLote.Caption = "F. Lote";
            colLote.VisibleIndex = 3;
            AplicarEstiloFolioClickeable(colLote);
        }

        if (_gridView.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["NumeroTicket"] is { } colTicket)
        {
            colTicket.Caption = "Ticket";
        }

        if (_gridView.Columns["CoprefBico"] is { } colCopref)
        {
            colCopref.Caption = "COPREF/BICO";
        }

        if (_gridView.Columns["Huertas"] is { } colHuertas)
        {
            colHuertas.Caption = "Huerta";
        }

        if (_gridView.Columns["PesoNeto"] is { } colPesoNeto)
        {
            colPesoNeto.Caption = "Peso Neto";
            colPesoNeto.DisplayFormat.FormatType = FormatType.Numeric;
            colPesoNeto.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["PorcentajeMateriaSeca"] is { } colMateriaSeca)
        {
            colMateriaSeca.Caption = "Materia Seca";
            colMateriaSeca.DisplayFormat.FormatType = FormatType.Numeric;
            colMateriaSeca.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["EstaEnLote"] is { } colBloqueo)
        {
            colBloqueo.Caption = "Bloqueo";
            colBloqueo.OptionsColumn.AllowEdit = false;
            colBloqueo.Width = 70;
        }

        _gridView.BestFitColumns();

        // BestFitColumns ajusta cada columna a su contenido real (regla dura del proyecto — no
        // se comprime para forzar que quepan, se usa scroll horizontal si hace falta), pero eso
        // deja espacio vacío a la derecha cuando el grid es más ancho que la suma de columnas.
        // Ese sobrante se lo damos a la última columna (Huerta) en vez de dejarlo en blanco.
        var anchoColumnas = _gridView.Columns.Cast<DevExpress.XtraGrid.Columns.GridColumn>()
            .Where(c => c.Visible)
            .Sum(c => c.Width);
        var anchoDisponible = _grid.Width - SystemInformation.VerticalScrollBarWidth;
        if (_gridView.Columns["Huertas"] is { } colHuertaFill && anchoDisponible > anchoColumnas)
        {
            colHuertaFill.Width += anchoDisponible - anchoColumnas;
        }
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
            XtraMessageBox.Show(this, "Selecciona una recepción de fruta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(RecepcionFrutaDto? recepcionExistente)
    {
        if (_recepcionFrutaEditarForm is { IsDisposed: false })
        {
            if (_recepcionFrutaEditarForm.WindowState == FormWindowState.Minimized)
            {
                _recepcionFrutaEditarForm.WindowState = FormWindowState.Normal;
            }

            _recepcionFrutaEditarForm.Activate();
            return;
        }

        _recepcionFrutaEditarForm = new RecepcionFrutaEditarForm(_recepcionFrutaService, recepcionExistente);
        _recepcionFrutaEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _recepcionFrutaEditarForm.FormClosed += (_, _) => _recepcionFrutaEditarForm = null;
        _recepcionFrutaEditarForm.Show(this);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una recepción de fruta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar la recepción de fruta folio '{seleccionado.Folio}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _recepcionFrutaService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnVistaPrevia_Click(object? sender, EventArgs e)
    {
        if (!_sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.VistaPrevia))
        {
            XtraMessageBox.Show(this, "No tienes permiso para ver este reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una recepción de fruta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var datosReporte = await _recepcionFrutaService.ObtenerParaReporteAsync(seleccionado.Id);
            if (datosReporte is null)
            {
                XtraMessageBox.Show(this, "No se encontró la Orden de Corte asociada a esta Recepción — agrega una línea antes de imprimir.",
                    "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var empresa = await _empresaConfiguracionService.ObtenerAsync();
            var reporte = await CrearReporteAsync();
            reporte.CargarDatos(datosReporte, empresa);

            // Además de las etiquetas ya llenadas por CargarDatos, se conecta el origen de datos
            // por si el usuario arrastró un campo nuevo en el Diseñador — así también sale con
            // dato real en Vista Previa, no solo en blanco dentro del Diseñador.
            reporte.ConectarOrigenDatos(_sqlOptions, seleccionado.Id);

            using var visor = new VisorReporteForm(reporte, CodigoReporte, _sessionContext);
            visor.ShowDialog(this);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnDisenarReporte_Click(object? sender, EventArgs e)
    {
        if (!_sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.Diseno))
        {
            XtraMessageBox.Show(this, "No tienes permiso para diseñar este reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var reporte = await CrearReporteAsync();
        await DisenadorReporteForm.MostrarAsync(
            this, _reportePlantillaService, CodigoReporte, "Recepción de Fruta", reporte,
            r => ((ReporteRecepcionFruta)r).ConectarOrigenDatos(_sqlOptions, 0),
            r => ((ReporteRecepcionFruta)r).DesconectarOrigenDatos());
    }

    private async Task<ReporteRecepcionFruta> CrearReporteAsync()
    {
        var reporte = new ReporteRecepcionFruta();

        var plantilla = await _reportePlantillaService.ObtenerPorCodigoAsync(CodigoReporte);
        if (!string.IsNullOrWhiteSpace(plantilla?.DefinicionXml))
        {
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(plantilla.DefinicionXml));
            reporte.LoadLayoutFromXml(stream);
        }

        return reporte;
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private RecepcionFrutaDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as RecepcionFrutaDto;
}
