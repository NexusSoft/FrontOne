using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using FrontOne.Application.Services;
using FrontOne.Domain.Constants;
using FrontOne.Domain.Enums;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Constants;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Acopio;
using FrontOne.WinForms.Forms.Recepcion;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Gastos;

public partial class GastoLoteForm : XtraForm
{
    private readonly int _loteId;
    private readonly GastoLoteService _gastoLoteService = null!;
    private readonly GastoFrutaCategoriaService _gastoFrutaCategoriaService = null!;
    private readonly GastoRecepcionService _gastoRecepcionService = null!;
    private readonly GastoRecepcionAjusteService _gastoRecepcionAjusteService = null!;
    private readonly TipoAjusteService _tipoAjusteService = null!;
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SqlOptions _sqlOptions = null!;
    private readonly SessionContext _sessionContext = null!;

    // Servicios solo para abrir Recepción de Fruta / Orden de Corte al hacer clic en su folio
    // (mismo criterio que RecepcionesFrutaForm.AbrirOrdenPorFolioAsync) — OrdenCorteEditarForm
    // necesita sus ~14 servicios aunque aquí solo se use para consultar, no para capturar.
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

    private RecepcionFrutaEditarForm? _recepcionFrutaEditarFormDesdeGrid;
    private OrdenCorteEditarForm? _ordenCorteEditarFormDesdeGrid;

    private int _gastoLoteId;

    public GastoLoteForm()
    {
        InitializeComponent();
    }

    public GastoLoteForm(
        int loteId,
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
        _loteId = loteId;
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

        _cmbCostoEstimadoListaPrecioNumero.Properties.Items.AddRange(ListasPrecioFruta.Nombres.Cast<object>().ToArray());
        _cmbTipoReporte.Properties.Items.AddRange(new object[] { "Reporte de Proceso", "Reporte de Proceso y Liquidación para Productor" });
        _cmbTipoReporte.SelectedIndex = 0;

        Load += async (_, _) => await CargarTodoAsync();
    }

    private async Task CargarTodoAsync()
    {
        _gastoLoteId = await _gastoLoteService.ObtenerOCrearAsync(_loteId);

        await CargarEncabezadoAsync();
        await CargarCosechaAsync();
        await CargarAcarreoAsync();
        await CargarFrutaAsync();
    }

    private async Task CargarEncabezadoAsync()
    {
        var encabezado = await _gastoLoteService.ObtenerEncabezadoAsync(_loteId);
        if (encabezado is null)
        {
            return;
        }

        _lblFolio.Text = encabezado.LoteFolio;
        _lblHuerta.Text = encabezado.HuertaNombre ?? string.Empty;
        _lblRegistroSagarpa.Text = encabezado.RegistroSagarpa ?? string.Empty;
        _lblFecha.Text = encabezado.FechaCorrida?.ToString("dd/MM/yyyy") ?? string.Empty;
        _lblPeso.Text = encabezado.Kilogramos.ToString("N2");
        _lblTipoPago.Text = encabezado.TipoPagoNombre ?? string.Empty;
        _lblTipoCorte.Text = encabezado.TipoCorteNombre ?? string.Empty;
        _lblVariedad.Text = encabezado.VariedadNombre ?? string.Empty;
        _lblPrecioAcordado.Text = encabezado.PrecioAcordadoTexto;

        _lblVigenciaEstimado.Text = encabezado.CostoEstimadoListaPrecioFecha is { } fecha
            ? $"{fecha:dd/MM/yyyy} - {encabezado.CostoEstimadoListaPrecioProductorNombre ?? "General"}"
            : "(sin vigencia elegida)";
        _cmbCostoEstimadoListaPrecioNumero.SelectedIndex = (encabezado.CostoEstimadoListaPrecioNumero ?? 1) - 1;
    }

    private async Task CargarFrutaAsync()
    {
        var lineas = await _gastoFrutaCategoriaService.ObtenerAsync(_gastoLoteId);
        var filas = lineas.Select(l => new GastoFrutaCategoriaFila
        {
            MateriaPrimaItemCode = l.MateriaPrimaItemCode,
            MateriaPrimaNombre = l.MateriaPrimaNombre,
            KilogramosSeleccionados = l.KilogramosSeleccionados,
            Porcentaje = l.Porcentaje,
            KilogramosComprados = l.KilogramosComprados,
            CostoRealUnitario = l.CostoRealUnitario,
            ImporteReal = l.ImporteReal,
            CostoEstimadoUnitario = l.CostoEstimadoUnitario,
            ImporteEstimado = l.ImporteEstimado,
        }).ToList();
        _gridFruta.DataSource = filas;
        _gridViewFruta.BestFitColumns();
    }

    private async void GridViewFruta_CellValueChanged(object? sender, CellValueChangedEventArgs e)
    {
        if (_gridViewFruta.GetRow(e.RowHandle) is not GastoFrutaCategoriaFila fila)
        {
            return;
        }

        if (e.Column == _colCostoRealUnitario)
        {
            await _gastoFrutaCategoriaService.GuardarCostoRealAsync(_gastoLoteId, fila.MateriaPrimaItemCode, e.Value as decimal?);
            await CargarFrutaAsync();
        }
        else if (e.Column == _colCostoEstimadoUnitario)
        {
            await _gastoFrutaCategoriaService.GuardarCostoEstimadoAsync(_gastoLoteId, fila.MateriaPrimaItemCode, e.Value as decimal?);
            await CargarFrutaAsync();
        }
    }

    private async void BtnVigenciaEstimado_Click(object? sender, EventArgs e)
    {
        using var buscador = new BuscarListaPrecioFrutaForm(_listaPrecioFrutaService);
        if (buscador.ShowDialog(this) != DialogResult.OK || buscador.VigenciaSeleccionada is not { } vigencia)
        {
            return;
        }

        var numero = (byte)(_cmbCostoEstimadoListaPrecioNumero.SelectedIndex + 1);
        await _gastoLoteService.ActualizarVigenciaEstimadoAsync(_gastoLoteId, vigencia.Fecha, vigencia.ProductorId, numero);
        await CargarEncabezadoAsync();
        await CargarFrutaAsync();
    }

    private async Task CargarCosechaAsync() => await CargarRecepcionAsync(
        (byte)TipoGasto.Cosecha, _gridCosecha, _lblTotalEmpresaCosecha, _lblTotalProductorCosecha);

    private async Task CargarAcarreoAsync() => await CargarRecepcionAsync(
        (byte)TipoGasto.Acarreo, _gridAcarreo, _lblTotalEmpresaAcarreo, _lblTotalProductorAcarreo);

    private async Task CargarRecepcionAsync(byte tipoGasto, DevExpress.XtraGrid.GridControl grid, LabelControl lblTotalEmpresa, LabelControl lblTotalProductor)
    {
        var resumen = await _gastoRecepcionService.ObtenerResumenAsync(_gastoLoteId, tipoGasto);

        var filas = resumen.Base.Select(b => new GastoRecepcionFila
        {
            EsBase = true,
            Id = b.GastoRecepcionId,
            LoteRecepcionId = b.LoteRecepcionId,
            RecepcionFrutaId = b.RecepcionFrutaId,
            RecepcionFolio = b.RecepcionFolio,
            PesoNeto = b.PesoNeto,
            PesoProductor = b.PesoProductor,
            OrdenCorteId = b.OrdenCorteId,
            OrdenCorteFolio = b.OrdenCorteFolio,
            Concepto = b.Proveedor ?? string.Empty,
            Cantidad = b.Cantidad,
            PrecioUnitario = b.PrecioUnitario,
            Importe = b.Importe,
            CargoEmpresa = b.CargoA == (byte)CargoAGasto.Empresa,
            CargoProductor = b.CargoA == (byte)CargoAGasto.Productor,
        }).Concat(resumen.Ajustes.Select(a => new GastoRecepcionFila
        {
            EsBase = false,
            Id = a.Id,
            LoteRecepcionId = a.LoteRecepcionId,
            RecepcionFrutaId = a.RecepcionFrutaId,
            RecepcionFolio = a.RecepcionFolio,
            PesoNeto = a.PesoNeto,
            PesoProductor = a.PesoProductor,
            OrdenCorteId = a.OrdenCorteId,
            OrdenCorteFolio = a.OrdenCorteFolio,
            Concepto = a.TipoAjusteNombre,
            Cantidad = 1,
            PrecioUnitario = a.Importe,
            Importe = a.Importe,
            CargoEmpresa = a.CargoA == (byte)CargoAGasto.Empresa,
            CargoProductor = a.CargoA == (byte)CargoAGasto.Productor,
            AjusteOrigen = a,
        })).ToList();

        grid.DataSource = filas;
        ((GridView)grid.MainView).BestFitColumns();

        lblTotalEmpresa.Text = resumen.TotalEmpresa.ToString("C2");
        lblTotalProductor.Text = resumen.TotalProductor.ToString("C2");
    }

    // Recepción/Orden de Corte se ven como folio clickeable (mismo criterio que
    // RecepcionesFrutaForm) — cursor de mano al pasar el mouse, clic abre el módulo respectivo.
    private static readonly string[] ColumnasFolioClickeable = ["RecepcionFolio", "OrdenCorteFolio"];

    private void GridViewRecepcion_MouseMove(object? sender, MouseEventArgs e)
    {
        if (sender is not GridView view)
        {
            return;
        }

        var info = view.CalcHitInfo(e.Location);
        view.GridControl.Cursor = info.InRowCell && ColumnasFolioClickeable.Contains(info.Column?.FieldName)
            ? Cursors.Hand
            : Cursors.Default;
    }

    private async void GridViewRecepcion_RowCellClick(object? sender, RowCellClickEventArgs e)
    {
        if (sender is not GridView view || view.GetRow(e.RowHandle) is not GastoRecepcionFila fila)
        {
            return;
        }

        switch (e.Column.FieldName)
        {
            case "RecepcionFolio":
                await AbrirRecepcionAsync(fila.RecepcionFrutaId);
                break;
            case "OrdenCorteFolio":
                await AbrirOrdenCortePorFolioAsync(fila.OrdenCorteFolio);
                break;
            case "CargoEmpresa":
                await CambiarCargoAAsync(view, e.RowHandle, fila, marcarEmpresa: true);
                break;
            case "CargoProductor":
                await CambiarCargoAAsync(view, e.RowHandle, fila, marcarEmpresa: false);
                break;
        }
    }

    // Con cargo a Empresa/Productor: columnas de solo lectura (Designer.cs, AllowEdit=false) —
    // el toggle se maneja aquí, a mano, en vez de con el checkbox nativo del grid. El checkbox
    // nativo entra en modo edición al primer clic y solo postea/repinta la celda hermana hasta
    // que la fila pierde el foco, lo que dejaba ambas columnas marcadas visualmente aunque el
    // dato guardado ya fuera correcto. Sin editor de por medio no hay ese desfase.
    private async Task CambiarCargoAAsync(GridView view, int rowHandle, GastoRecepcionFila fila, bool marcarEmpresa)
    {
        fila.CargoEmpresa = marcarEmpresa;
        fila.CargoProductor = !marcarEmpresa;
        view.SetRowCellValue(rowHandle, "CargoEmpresa", fila.CargoEmpresa);
        view.SetRowCellValue(rowHandle, "CargoProductor", fila.CargoProductor);
        view.RefreshRow(rowHandle);

        var cargoA = marcarEmpresa ? (byte)CargoAGasto.Empresa : (byte)CargoAGasto.Productor;
        if (fila.EsBase)
        {
            await _gastoRecepcionService.ActualizarCargoAAsync(fila.Id, cargoA);
        }
        else if (fila.AjusteOrigen is { } ajuste)
        {
            await _gastoRecepcionAjusteService.ActualizarAsync(fila.Id, ajuste.TipoAjusteId, ajuste.Monto, cargoA);
        }

        var (lblTotalEmpresa, lblTotalProductor) = view == _gridViewCosecha
            ? (_lblTotalEmpresaCosecha, _lblTotalProductorCosecha)
            : (_lblTotalEmpresaAcarreo, _lblTotalProductorAcarreo);

        var filas = (view.GridControl.DataSource as IEnumerable<GastoRecepcionFila>)?.ToList() ?? [];
        lblTotalEmpresa.Text = filas.Where(f => f.CargoEmpresa).Sum(f => f.Importe).ToString("C2");
        lblTotalProductor.Text = filas.Where(f => f.CargoProductor).Sum(f => f.Importe).ToString("C2");
    }

    private async Task AbrirRecepcionAsync(int recepcionFrutaId)
    {
        if (!_sessionContext.TienePermiso("Recepcion", "RecepcionesFruta", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var recepcion = await _recepcionFrutaService.ObtenerPorIdAsync(recepcionFrutaId);
        if (recepcion is null)
        {
            XtraMessageBox.Show(this, "La recepción ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_recepcionFrutaEditarFormDesdeGrid is { IsDisposed: false })
        {
            if (_recepcionFrutaEditarFormDesdeGrid.WindowState == FormWindowState.Minimized)
            {
                _recepcionFrutaEditarFormDesdeGrid.WindowState = FormWindowState.Normal;
            }

            _recepcionFrutaEditarFormDesdeGrid.Activate();
            return;
        }

        _recepcionFrutaEditarFormDesdeGrid = new RecepcionFrutaEditarForm(_recepcionFrutaService, recepcion);
        _recepcionFrutaEditarFormDesdeGrid.FormClosed += (_, _) => _recepcionFrutaEditarFormDesdeGrid = null;
        _recepcionFrutaEditarFormDesdeGrid.Show(this);
    }

    private async Task AbrirOrdenCortePorFolioAsync(string? folio)
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

    private async void BtnAgregarAjusteCosecha_Click(object? sender, EventArgs e) => await AgregarAjusteAsync((byte)TipoGasto.Cosecha, _gridViewCosecha);

    private async void BtnAgregarAjusteAcarreo_Click(object? sender, EventArgs e) => await AgregarAjusteAsync((byte)TipoGasto.Acarreo, _gridViewAcarreo);

    private async Task AgregarAjusteAsync(byte tipoGasto, GridView view)
    {
        if (view.GetFocusedRow() is not GastoRecepcionFila filaSeleccionada)
        {
            XtraMessageBox.Show(this, "Selecciona la recepción a la que aplica el ajuste.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new GastoRecepcionAjusteEditarForm(_gastoRecepcionAjusteService, _tipoAjusteService, _gastoLoteId, filaSeleccionada.LoteRecepcionId, tipoGasto);
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        if (tipoGasto == (byte)TipoGasto.Cosecha)
        {
            await CargarCosechaAsync();
        }
        else
        {
            await CargarAcarreoAsync();
        }

        await CargarFrutaAsync();
    }

    private async void BtnEliminarAjusteCosecha_Click(object? sender, EventArgs e) => await EliminarAjusteAsync(_gridViewCosecha, CargarCosechaAsync);

    private async void BtnEliminarAjusteAcarreo_Click(object? sender, EventArgs e) => await EliminarAjusteAsync(_gridViewAcarreo, CargarAcarreoAsync);

    private async Task EliminarAjusteAsync(GridView view, Func<Task> recargar)
    {
        if (view.GetFocusedRow() is not GastoRecepcionFila fila)
        {
            XtraMessageBox.Show(this, "Selecciona un ajuste.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (fila.EsBase)
        {
            XtraMessageBox.Show(this, "La fila base no se puede eliminar.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, "¿Eliminar este ajuste?", "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        await _gastoRecepcionAjusteService.EliminarAsync(fila.Id);
        await recargar();
        await CargarFrutaAsync();
    }

    private async void BtnImprimir_Click(object? sender, EventArgs e)
    {
        var codigo = _cmbTipoReporte.SelectedIndex == 1 ? "LiquidacionProductor" : "ProcesoLote";

        if (!_sessionContext.TienePermisoReporte(codigo, AccionReporte.VistaPrevia))
        {
            XtraMessageBox.Show(this, "No tienes permiso para ver este reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var datos = await _gastoLoteService.ObtenerParaReporteAsync(_gastoLoteId);
            if (datos is null)
            {
                XtraMessageBox.Show(this, "No se encontró el encabezado de Gastos de este Lote.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var categorias = await _gastoFrutaCategoriaService.ObtenerAsync(_gastoLoteId);
            var relacionGastos = await _gastoRecepcionService.ObtenerParaReporteAsync(_gastoLoteId);
            var empresa = await _empresaConfiguracionService.ObtenerAsync();

            DevExpress.XtraReports.UI.XtraReport reporte;
            if (codigo == "LiquidacionProductor")
            {
                var categoriasSinMerma = categorias.Where(c => c.Mercado != "Merma").ToList();
                var reporteLiquidacion = new Reports.ReporteLiquidacionProductor();
                reporteLiquidacion.CargarDatos(datos, categoriasSinMerma, relacionGastos, empresa);
                reporteLiquidacion.ConectarOrigenDatos(_sqlOptions, _gastoLoteId);
                reporte = reporteLiquidacion;
            }
            else
            {
                var resumenMercado = await _gastoFrutaCategoriaService.ObtenerResumenMercadoAsync(_gastoLoteId);
                var reporteProceso = new Reports.ReporteProcesoLote();
                reporteProceso.CargarDatos(datos, categorias, resumenMercado, relacionGastos, empresa);
                reporteProceso.ConectarOrigenDatos(_sqlOptions, _gastoLoteId);
                reporte = reporteProceso;
            }

            using var visor = new VisorReporteForm(reporte, codigo, _sessionContext);
            visor.ShowDialog(this);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
