using System.IO;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class OrdenesCorteForm : XtraForm
{
    // Mismos íconos que RecepcionesFrutaForm — candado cerrado (rojo)/abierto (verde) para la
    // columna "Bloqueo" (EstaEnRecepcion).
    private static readonly Image ImagenCandadoCerrado = CargarIconoCandado("candado_cerrado.png");
    private static readonly Image ImagenCandadoAbierto = CargarIconoCandado("candado_abierto.png");

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

    // Servicios adicionales solo para abrir AcuerdoCorteEditarForm al hacer clic en F. Acuerdo.
    private readonly AcuerdoCorteService _acuerdoCorteService = null!;
    private readonly ProductorService _productorService = null!;
    private readonly ProductoService _productoService = null!;
    private readonly TipoComercializacionService _tipoComercializacionService = null!;
    private readonly TipoPagoService _tipoPagoService = null!;
    private readonly MonedaService _monedaService = null!;
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly SessionContext _sessionContext = null!;

    private OrdenCorteEditarForm? _ordenCorteEditarForm;
    private AcuerdoCorteEditarForm? _acuerdoCorteEditarFormDesdeGrid;

    private static readonly string[] ColumnasFolioClickeable = ["AcuerdoCorteFolio"];

    public OrdenesCorteForm()
    {
        InitializeComponent();
    }

    public OrdenesCorteForm(
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
        CajaCampoService cajaCampoService,
        AcuerdoCorteService acuerdoCorteService,
        ProductorService productorService,
        ProductoService productoService,
        TipoComercializacionService tipoComercializacionService,
        TipoPagoService tipoPagoService,
        MonedaService monedaService,
        ListaPrecioFrutaService listaPrecioFrutaService,
        SessionContext sessionContext)
        : this()
    {
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
        _acuerdoCorteService = acuerdoCorteService;
        _productorService = productorService;
        _productoService = productoService;
        _tipoComercializacionService = tipoComercializacionService;
        _tipoPagoService = tipoPagoService;
        _monedaService = monedaService;
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _sessionContext = sessionContext;

        _gridView.CustomDrawCell += GridView_CustomDrawCell;
        _gridView.MouseMove += GridView_MouseMove;
        _gridView.RowCellClick += GridView_RowCellClick;
        _gridView.DoubleClick += GridView_DoubleClick;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var ordenes = await _ordenCorteService.ObtenerAsync();
        _grid.DataSource = ordenes.ToList();
        ConfigurarColumnas();
    }

    private static Image CargarIconoCandado(string nombreArchivo)
    {
        using var stream = typeof(OrdenesCorteForm).Assembly
            .GetManifestResourceStream($"FrontOne.WinForms.Resources.Icons.{nombreArchivo}")!;
        return Image.FromStream(stream);
    }

    // La columna "Bloqueo" (EstaEnRecepcion) no se muestra como texto/checkbox — se dibuja un
    // candado cerrado (rojo) o abierto (verde), mismo criterio que RecepcionesFrutaForm.
    private void GridView_CustomDrawCell(object? sender, RowCellCustomDrawEventArgs e)
    {
        if (e.Column.FieldName != "EstaEnRecepcion")
        {
            return;
        }

        e.Appearance.FillRectangle(e.Cache, e.Bounds);

        var estaEnRecepcion = e.CellValue is true;
        var imagen = estaEnRecepcion ? ImagenCandadoCerrado : ImagenCandadoAbierto;
        const int tamano = 16;
        var rect = new Rectangle(
            e.Bounds.Left + (e.Bounds.Width - tamano) / 2,
            e.Bounds.Top + (e.Bounds.Height - tamano) / 2,
            tamano, tamano);
        e.Cache.DrawImage(imagen, rect);
        e.Handled = true;
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[]
        {
            "Id", "AcuerdoCorteId", "ProductorId", "HuertaId", "FloracionId", "VariedadId",
            "PagarCorteACardCode", "TransportistaCardCode", "JefeCuadrillaCardCode", "JefeAcopioId",
            "PrecioAcarreo", "NoCandado", "CostoKg", "PagoDia", "CuadrillaApoyo",
            "PuntoReunion", "Observaciones", "CajaCampoId", "CajaCampoNombre", "KgMinimo",
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

        if (_gridView.Columns["Folio"] is { } colFolioOrden)
        {
            colFolioOrden.Caption = "F. OrdenC";
            colFolioOrden.VisibleIndex = 1;
        }

        if (_gridView.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["ProductorNombre"] is { } colProductor)
        {
            colProductor.Caption = "Productor";
        }

        if (_gridView.Columns["HuertaNombre"] is { } colHuerta)
        {
            colHuerta.Caption = "Huerta";
        }

        if (_gridView.Columns["FloracionNombre"] is { } colFloracion)
        {
            colFloracion.Caption = "Floración";
        }

        if (_gridView.Columns["RegistroSagarpa"] is { } colRegistro)
        {
            colRegistro.Caption = "Registro";
        }

        if (_gridView.Columns["VariedadNombre"] is { } colVariedad)
        {
            colVariedad.Caption = "Variedad";
        }

        if (_gridView.Columns["TipoPagoNombre"] is { } colTipoPago)
        {
            colTipoPago.Caption = "Tipo de Pago";
        }

        if (_gridView.Columns["TipoCorteNombre"] is { } colTipoCorte)
        {
            colTipoCorte.Caption = "Tipo de Corte";
        }

        if (_gridView.Columns["PagarCorteANombre"] is { } colPagarCorteA)
        {
            colPagarCorteA.Caption = "Pagar el Corte a";
        }

        if (_gridView.Columns["TransportistaNombre"] is { } colTransportista)
        {
            colTransportista.Caption = "Transportista";
        }

        if (_gridView.Columns["CajasEntregadas"] is { } colCajas)
        {
            colCajas.Caption = "Cajas";
        }

        if (_gridView.Columns["JefeCuadrillaNombre"] is { } colJefeCuadrilla)
        {
            colJefeCuadrilla.Caption = "Jefe de Cuadrilla";
        }

        if (_gridView.Columns["JefeAcopioNombre"] is { } colJefeAcopio)
        {
            colJefeAcopio.Caption = "Jefe de Acopio";
        }

        if (_gridView.Columns["Cancelado"] is { } colCancelado)
        {
            colCancelado.Caption = "Cancelado";
        }

        if (_gridView.Columns["EstaEnRecepcion"] is { } colBloqueo)
        {
            colBloqueo.Caption = "Bloqueo";
            colBloqueo.OptionsColumn.AllowEdit = false;
            colBloqueo.Width = 70;
        }

        _gridView.BestFitColumns();

        // BestFitColumns ajusta cada columna a su contenido real (regla dura del proyecto — no
        // se comprime para forzar que quepan, se usa scroll horizontal si hace falta), pero eso
        // deja espacio vacío a la derecha cuando el grid es más ancho que la suma de columnas.
        // Ese sobrante se lo damos a la última columna (Huerta) en vez de dejarlo en blanco —
        // mismo criterio que RecepcionesFrutaForm.
        var anchoColumnas = _gridView.Columns.Cast<DevExpress.XtraGrid.Columns.GridColumn>()
            .Where(c => c.Visible)
            .Sum(c => c.Width);
        var anchoDisponible = _grid.Width - SystemInformation.VerticalScrollBarWidth;
        if (_gridView.Columns["HuertaNombre"] is { } colHuertaFill && anchoDisponible > anchoColumnas)
        {
            colHuertaFill.Width += anchoDisponible - anchoColumnas;
        }
    }

    // F. Acuerdo se ve como folio clickeable (azul + negritas + subrayado) — al hacer clic abre
    // el Acuerdo de Corte correspondiente (mismo criterio que RecepcionesFrutaForm).
    private static void AplicarEstiloFolioClickeable(DevExpress.XtraGrid.Columns.GridColumn columna)
    {
        columna.AppearanceCell.Font = new Font(columna.AppearanceCell.Font, FontStyle.Bold | FontStyle.Underline);
        columna.AppearanceCell.ForeColor = ColorTranslator.FromHtml("#0563C1");
        columna.AppearanceCell.Options.UseFont = true;
        columna.AppearanceCell.Options.UseForeColor = true;
    }

    private void GridView_MouseMove(object? sender, MouseEventArgs e)
    {
        var info = _gridView.CalcHitInfo(e.Location);
        _grid.Cursor = info.InRowCell && ColumnasFolioClickeable.Contains(info.Column?.FieldName)
            ? Cursors.Hand
            : Cursors.Default;
    }

    private async void GridView_RowCellClick(object? sender, RowCellClickEventArgs e)
    {
        if (e.Column.FieldName == "AcuerdoCorteFolio")
        {
            await AbrirAcuerdoPorFolioAsync(e.CellValue as string);
        }
    }

    // Doble clic en cualquier celda que NO sea un folio-hipervínculo dispara Editar directo,
    // sin tener que seleccionar la fila y luego ir al botón — igual que pedido para Recepciones.
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

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        AbrirEditarForm(null);
    }

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una orden de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(OrdenCorteDto? ordenExistente)
    {
        if (_ordenCorteEditarForm is { IsDisposed: false })
        {
            if (_ordenCorteEditarForm.WindowState == FormWindowState.Minimized)
            {
                _ordenCorteEditarForm.WindowState = FormWindowState.Normal;
            }

            _ordenCorteEditarForm.Activate();
            return;
        }

        _ordenCorteEditarForm = new OrdenCorteEditarForm(
            _ordenCorteService, _huertaService, _floracionService, _variedadService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _tipoCorteService,
            _paisService, _estadoService, _municipioService, _poblacionService, _cajaCampoService, ordenExistente);
        _ordenCorteEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _ordenCorteEditarForm.FormClosed += (_, _) => _ordenCorteEditarForm = null;
        _ordenCorteEditarForm.Show(this);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una orden de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar la orden de corte folio '{seleccionado.Folio}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _ordenCorteService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private OrdenCorteDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as OrdenCorteDto;
}
