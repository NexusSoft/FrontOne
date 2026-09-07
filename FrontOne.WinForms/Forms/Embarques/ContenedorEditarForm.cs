using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Embarques;

// Pantalla principal de un Contenedor: Tab Pedido (encabezado + detalle del pedido SAP elegido,
// con el Status Pendiente/Surtido calculado contra lo ya embarcado) y Tab Embarque (3 secciones:
// pallets cargados, detalle del pallet seleccionado, resumen por Calibre de Exportación).
// El pedido SAP se fija al guardar el encabezado por primera vez — no se puede reasignar después.
public partial class ContenedorEditarForm : XtraForm
{
    private readonly ContenedorService _contenedorService = null!;
    private readonly PalletService _palletService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SessionContext _sessionContext = null!;

    private ContenedorDto? _contenedor;
    private SapPedidoDto? _pedidoSeleccionado;
    private List<ContenedorPedidoLineaDto> _lineasPedido = new();
    private List<ContenedorPalletDto> _pallets = new();

    // Listado fijo del combo de reportes de Carga de Contenedor (no es catálogo editable).
    private sealed record OpcionReporte(string Codigo, string Nombre);

    private static readonly IReadOnlyList<OpcionReporte> OpcionesReporte =
    [
        new("ContenedorCarga", "Carga de Contenedor"),
        new("ContenedorDetalleLote", "Detalle por Lote"),
        new("ContenedorDetalleHuerta", "Detalle por Huerta"),
        new("ContenedorResumenCalibre", "Resumen por Calibre"),
        new("ContenedorResumenLote", "Resumen por Lote"),
        new("ContenedorResumenHuerta", "Resumen por Huerta"),
        new("ContenedorResumenHuertaSinKg", "Resumen por Huerta (sin Kg)"),
    ];

    public event EventHandler? Guardado;

    public ContenedorEditarForm()
    {
        InitializeComponent();
    }

    public ContenedorEditarForm(
        ContenedorService contenedorService,
        PalletService palletService,
        EmpresaConfiguracionService empresaConfiguracionService,
        SessionContext sessionContext,
        ContenedorDto? existente) : this()
    {
        _contenedorService = contenedorService;
        _palletService = palletService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _sessionContext = sessionContext;
        _contenedor = existente;

        Load += async (_, _) => await CargarAsync();
        _gridPedido.SizeChanged += (_, _) => { if (_gridViewPedido.Columns.Count > 0) _gridViewPedido.BestFitColumns(); };
        _gridPallets.SizeChanged += (_, _) => { if (_gridViewPallets.Columns.Count > 0) _gridViewPallets.BestFitColumns(); };
        _gridPalletDetalle.SizeChanged += (_, _) => { if (_gridViewPalletDetalle.Columns.Count > 0) _gridViewPalletDetalle.BestFitColumns(); };
        _gridResumen.SizeChanged += (_, _) => { if (_gridViewResumen.Columns.Count > 0) _gridViewResumen.BestFitColumns(); };
    }

    private async Task CargarAsync()
    {
        MostrarEncabezado();

        if (_contenedor is null)
        {
            return;
        }

        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: false, useFadeOut: true, "FrontOne", "Cargando contenedor...");
        try
        {
            await CargarLineasPedidoAsync();
            await CargarPalletsAsync();
            await CargarResumenAsync();
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }
    }

    private void MostrarEncabezado()
    {
        if (_contenedor is null)
        {
            _txtFolio.Text = "(se genera al guardar)";
            _dtFecha.EditValue = DateTime.Today;
            _txtPedidoSap.Text = _pedidoSeleccionado?.DocNum.ToString() ?? string.Empty;
            _txtFolioFronterra.Text = _pedidoSeleccionado?.FolioFronterra ?? string.Empty;
            _txtCodigoCliente.Text = _pedidoSeleccionado?.CardCode ?? string.Empty;
            _txtNombreCliente.Text = _pedidoSeleccionado?.CardName ?? string.Empty;
            _txtPedidoSap.Properties.Buttons[0].Enabled = true;
            return;
        }

        _txtFolio.Text = _contenedor.Folio;
        _dtFecha.EditValue = _contenedor.Fecha;
        _txtPedidoSap.Text = _contenedor.SapDocNum.ToString();
        _txtFolioFronterra.Text = _contenedor.FolioFronterra;
        _txtCodigoCliente.Text = _contenedor.CardCode;
        _txtNombreCliente.Text = _contenedor.CardName;
        _memoObservaciones.Text = _contenedor.Observaciones;
        _txtPedidoSap.Properties.Buttons[0].Enabled = false; // el pedido queda fijo desde que se guarda el encabezado
    }

    private void TxtPedidoSap_ButtonClick(object? sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != DevExpress.XtraEditors.Controls.ButtonPredefines.Search)
        {
            return;
        }

        using var form = new ContenedorPedidoBuscarForm(_contenedorService);
        if (form.ShowDialog(this) != DialogResult.OK || form.PedidoSeleccionado is not { } pedido)
        {
            return;
        }

        _pedidoSeleccionado = pedido;
        MostrarEncabezado();
        _ = CargarLineasPedidoAsync();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var fecha = _dtFecha.EditValue is DateTime f ? f : DateTime.Today;

        try
        {
            if (_contenedor is null)
            {
                if (_pedidoSeleccionado is null)
                {
                    XtraMessageBox.Show(this, "Selecciona el pedido de SAP a surtir.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var id = await _contenedorService.CrearAsync(fecha, _pedidoSeleccionado, _memoObservaciones.Text);
                _contenedor = await _contenedorService.ObtenerPorIdAsync(id);
            }
            else
            {
                await _contenedorService.ActualizarAsync(_contenedor.Id, fecha, _memoObservaciones.Text);
                _contenedor = await _contenedorService.ObtenerPorIdAsync(_contenedor.Id);
            }

            MostrarEncabezado();
            await CargarLineasPedidoAsync();
            await CargarPalletsAsync();
            await CargarResumenAsync();
            Guardado?.Invoke(this, EventArgs.Empty);
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

    private async Task CargarLineasPedidoAsync()
    {
        var docEntry = _contenedor?.SapDocEntry ?? _pedidoSeleccionado?.DocEntry;
        if (docEntry is null)
        {
            _lineasPedido = new List<ContenedorPedidoLineaDto>();
            _gridPedido.DataSource = null;
            ActualizarEstadoReporte();
            return;
        }

        _lineasPedido = (await _contenedorService.ObtenerLineasPedidoAsync(_contenedor?.Id ?? 0, docEntry.Value)).ToList();
        _gridPedido.DataSource = _lineasPedido;
        ConfigurarColumnasPedido();
        ActualizarEstadoReporte();
    }

    // El combo/botón de reportes de Carga de Contenedor solo se habilitan cuando TODAS las
    // líneas del pedido llegan a 100% de surtido (mismo campo que ya pinta el Status "Surtido").
    private void ActualizarEstadoReporte()
    {
        var pedidoCompleto = _lineasPedido.Count > 0 && _lineasPedido.All(l => l.PorcentajeSurtido >= 100m);
        _cmbReporte.Enabled = pedidoCompleto;
        _btnImprimirReporte.Enabled = pedidoCompleto;
    }

    private void ConfigurarColumnasPedido()
    {
        if (_gridViewPedido.Columns["CodigoProducto"] is { } colCodigo)
        {
            colCodigo.Caption = "Código Producto";
        }

        if (_gridViewPedido.Columns["DescripcionProducto"] is { } colDescripcion)
        {
            colDescripcion.Caption = "Descripción Producto";
        }

        if (_gridViewPedido.Columns["CantidadCajas"] is { } colCantidad)
        {
            colCantidad.Caption = "Cantidad Cajas";
            AplicarSumaFooter(colCantidad, "{0:N0}");
        }

        if (_gridViewPedido.Columns["Presentacion"] is { } colPresentacion)
        {
            colPresentacion.Caption = "Presentación";
        }

        if (_gridViewPedido.Columns["Pallet"] is { } colPallet)
        {
            colPallet.Caption = "Pallet";
            colPallet.DisplayFormat.FormatType = FormatType.Numeric;
            colPallet.DisplayFormat.FormatString = "n2";
            AplicarSumaFooter(colPallet, "{0:N2}");
        }

        if (_gridViewPedido.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
            AplicarSumaFooter(colKilos, "{0:N2}");
        }

        if (_gridViewPedido.Columns["PorcentajeSurtido"] is { } colPorcentaje)
        {
            colPorcentaje.Caption = "% Surtido";
            colPorcentaje.DisplayFormat.FormatType = FormatType.Numeric;
            colPorcentaje.DisplayFormat.FormatString = "n1";
        }

        var orden = new[] { "CodigoProducto", "DescripcionProducto", "CantidadCajas", "Presentacion", "Pallet", "Kilogramos", "PorcentajeSurtido", "Status" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridViewPedido.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridViewPedido.BestFitColumns();
    }

    private async Task CargarPalletsAsync()
    {
        if (_contenedor is null)
        {
            _gridPallets.DataSource = null;
            return;
        }

        _pallets = (await _contenedorService.ObtenerPalletsAsync(_contenedor.Id)).ToList();
        _gridPallets.DataSource = _pallets;
        ConfigurarColumnasPallets();
    }

    private void ConfigurarColumnasPallets()
    {
        foreach (var nombre in new[] { "ContenedorPalletId", "PalletId" })
        {
            if (_gridViewPallets.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridViewPallets.Columns["NoRegistro"] is { } colNoRegistro)
        {
            colNoRegistro.Caption = "No. Registro";
            colNoRegistro.OptionsColumn.AllowEdit = false;
        }

        if (_gridViewPallets.Columns["PalletFolio"] is { } colFolio)
        {
            colFolio.Caption = "No. Pallet";
            colFolio.OptionsColumn.AllowEdit = false;
        }

        // Posición y Temperatura son las únicas columnas editables del grid — se pueden cambiar
        // directo aquí (Guardado al salir de la celda, ver GridViewPallets_CellValueChanged) sin
        // tener que quitar y volver a agregar el pallet.
        if (_gridViewPallets.Columns["Posicion"] is { } colPosicion)
        {
            colPosicion.Caption = "Posición";
            colPosicion.ColumnEdit = _repoSpinPosicionPallet;
            colPosicion.OptionsColumn.AllowEdit = true;
        }

        if (_gridViewPallets.Columns["Cajas"] is { } colCajas)
        {
            colCajas.OptionsColumn.AllowEdit = false;
            AplicarSumaFooter(colCajas, "{0:N0}");
        }

        if (_gridViewPallets.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.OptionsColumn.AllowEdit = false;
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
            AplicarSumaFooter(colKilos, "{0:N2}");
        }

        if (_gridViewPallets.Columns["Temperatura"] is { } colTemperatura)
        {
            colTemperatura.Caption = "Temperatura (°F)";
            colTemperatura.ColumnEdit = _repoSpinTemperaturaPallet;
            colTemperatura.OptionsColumn.AllowEdit = true;
            colTemperatura.DisplayFormat.FormatType = FormatType.Numeric;
            colTemperatura.DisplayFormat.FormatString = "n2";
        }

        var orden = new[] { "NoRegistro", "PalletFolio", "Posicion", "Cajas", "Kilogramos", "Temperatura" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridViewPallets.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridViewPallets.BestFitColumns();
    }

    private async void GridViewPallets_FocusedRowChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
    {
        if (_gridViewPallets.GetFocusedRow() is not ContenedorPalletDto fila)
        {
            _gridPalletDetalle.DataSource = null;
            return;
        }

        await CargarDetallePalletAsync(fila);
    }

    // Guarda Posición/Temperatura apenas se editan en el grid — para la columna que SÍ cambió se
    // usa e.Value (valor ya confirmado); para la otra se usa el valor actual de fila (record con
    // propiedades init, no garantizado mutable vía reflexión de DevExpress).
    private async void GridViewPallets_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (_contenedor is null || _gridViewPallets.GetRow(e.RowHandle) is not ContenedorPalletDto fila)
        {
            return;
        }

        int posicion;
        decimal? temperatura;
        if (e.Column.FieldName == "Posicion")
        {
            posicion = Convert.ToInt32(e.Value);
            temperatura = fila.Temperatura;
        }
        else if (e.Column.FieldName == "Temperatura")
        {
            posicion = fila.Posicion;
            temperatura = e.Value as decimal?;
        }
        else
        {
            return;
        }

        try
        {
            await _contenedorService.ActualizarPalletAsync(_contenedor.Id, fila.ContenedorPalletId, posicion, temperatura);
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            await CargarPalletsAsync();
        }
    }

    private async Task CargarDetallePalletAsync(ContenedorPalletDto fila)
    {
        var detalle = await _palletService.ObtenerDetalleAsync(fila.PalletId);
        var filas = detalle.Select(d => new PalletDetalleFilaDto(
            d.ProductoCodigoSap, d.ProductoDescripcion, d.Cajas, d.Kilogramos, fila.PalletFolio, d.LoteFolio)).ToList();

        _gridPalletDetalle.DataSource = filas;
        ConfigurarColumnasPalletDetalle();
    }

    private void ConfigurarColumnasPalletDetalle()
    {
        if (_gridViewPalletDetalle.Columns["ProductoCodigoSap"] is { } colCodigo)
        {
            colCodigo.Caption = "Código Producto";
        }

        if (_gridViewPalletDetalle.Columns["ProductoDescripcion"] is { } colDescripcion)
        {
            colDescripcion.Caption = "Producto";
        }

        if (_gridViewPalletDetalle.Columns["Cajas"] is { } colCajas)
        {
            AplicarSumaFooter(colCajas, "{0:N0}");
        }

        if (_gridViewPalletDetalle.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
            AplicarSumaFooter(colKilos, "{0:N2}");
        }

        if (_gridViewPalletDetalle.Columns["PalletFolio"] is { } colPallet)
        {
            colPallet.Caption = "No. Pallet";
        }

        if (_gridViewPalletDetalle.Columns["LoteFolio"] is { } colLote)
        {
            colLote.Caption = "Lote";
        }

        var orden = new[] { "ProductoCodigoSap", "ProductoDescripcion", "Cajas", "Kilogramos", "PalletFolio", "LoteFolio" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridViewPalletDetalle.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridViewPalletDetalle.BestFitColumns();
    }

    private async Task CargarResumenAsync()
    {
        if (_contenedor is null)
        {
            _gridResumen.DataSource = null;
            return;
        }

        var resumen = await _contenedorService.ObtenerResumenAsync(_contenedor.Id);
        _gridResumen.DataSource = resumen.ToList();
        ConfigurarColumnasResumen();
    }

    private void ConfigurarColumnasResumen()
    {
        if (_gridViewResumen.Columns["CalibreExportacion"] is { } colCalibre)
        {
            colCalibre.Caption = "Calibre de Exportación";
        }

        if (_gridViewResumen.Columns["TotalPallets"] is { } colPallets)
        {
            colPallets.Caption = "Total de Pallets";
            AplicarSumaFooter(colPallets, "{0:N0}");
        }

        if (_gridViewResumen.Columns["TotalCajas"] is { } colCajas)
        {
            colCajas.Caption = "Total de Cajas";
            AplicarSumaFooter(colCajas, "{0:N0}");
        }

        if (_gridViewResumen.Columns["TotalKilogramos"] is { } colKilos)
        {
            colKilos.Caption = "Total de Kilogramos";
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
            AplicarSumaFooter(colKilos, "{0:N2}");
        }

        var orden = new[] { "CalibreExportacion", "TotalPallets", "TotalCajas", "TotalKilogramos" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridViewResumen.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridViewResumen.BestFitColumns();
    }

    private static void AplicarSumaFooter(DevExpress.XtraGrid.Columns.GridColumn columna, string formato)
    {
        columna.SummaryItem.SummaryType = SummaryItemType.Sum;
        columna.SummaryItem.DisplayFormat = formato;
    }

    private void Tabs_SelectedPageChanged(object? sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
    {
        if (_tabs.SelectedTabPage == _tabEmbarque && _contenedor is null)
        {
            XtraMessageBox.Show(this, "Guarda el contenedor (elige un pedido) antes de agregar pallets.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            _tabs.SelectedTabPage = _tabPedido;
        }
    }

    private async void BtnAgregarPallet_Click(object? sender, EventArgs e)
    {
        if (_contenedor is null)
        {
            return;
        }

        var codigosPendientes = _lineasPedido.Where(l => l.PorcentajeSurtido < 100m).Select(l => l.CodigoProducto).ToList();
        if (codigosPendientes.Count == 0)
        {
            XtraMessageBox.Show(this, "Todos los productos del pedido ya están surtidos al 100%.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var posicionesOcupadas = _pallets.Select(p => p.Posicion).ToList();
        using var form = new ContenedorPalletAgregarForm(_contenedorService, codigosPendientes, posicionesOcupadas);
        if (form.ShowDialog(this) != DialogResult.OK || form.PalletsSeleccionados.Count == 0)
        {
            return;
        }

        try
        {
            foreach (var (palletId, posicion) in form.PalletsSeleccionados)
            {
                await _contenedorService.AgregarPalletAsync(_contenedor.Id, palletId, posicion, form.Temperatura);
            }

            await CargarPalletsAsync();
            await CargarResumenAsync();
            await CargarLineasPedidoAsync();
            Guardado?.Invoke(this, EventArgs.Empty);
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            await CargarPalletsAsync();
            await CargarResumenAsync();
            await CargarLineasPedidoAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await CargarPalletsAsync();
            await CargarResumenAsync();
            await CargarLineasPedidoAsync();
        }
    }

    private async void BtnEliminarPallet_Click(object? sender, EventArgs e)
    {
        if (_contenedor is null || _gridViewPallets.GetFocusedRow() is not ContenedorPalletDto fila)
        {
            XtraMessageBox.Show(this, "Selecciona un pallet.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Quitar el pallet '{fila.PalletFolio}' de este contenedor? Regresa a estatus Empacado.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _contenedorService.QuitarPalletAsync(_contenedor.Id, fila.ContenedorPalletId);
            await CargarPalletsAsync();
            await CargarResumenAsync();
            await CargarLineasPedidoAsync();
            Guardado?.Invoke(this, EventArgs.Empty);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnImprimirReporte_Click(object? sender, EventArgs e)
    {
        if (_contenedor is null || _cmbReporte.EditValue is not string codigo)
        {
            return;
        }

        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: false, useFadeOut: true, "FrontOne", "Generando reporte...");
        try
        {
            var lineas = await _contenedorService.ObtenerCargaParaReporteAsync(_contenedor.Id);
            var empresa = await _empresaConfiguracionService.ObtenerAsync();
            // Solo CargarDatos: ya deja el reporte con datos reales listos para imprimir.
            // ConectarOrigenDatos (SqlDataSource para el Field List) solo hace falta al abrir el
            // Diseñador de Reportes (ver ReportesForm), no para esta vista previa/impresión.
            var reporte = CrearYCargarReporte(codigo, _contenedor, lineas, empresa);

            using var visor = new VisorReporteForm(reporte, codigo, _sessionContext);
            SplashScreenManager.CloseDefaultWaitForm();
            visor.ShowDialog(this);
        }
        catch
        {
            SplashScreenManager.CloseDefaultWaitForm();
            throw;
        }
    }

    // Mismo switch tipado que ya usa ReportesForm.ConectarOrigenDatos/DesconectarOrigenDatos —
    // cada reporte trae su propio CargarDatos con la misma firma (encabezado + líneas + empresa).
    private static XtraReport CrearYCargarReporte(
        string codigo, ContenedorDto encabezado, IReadOnlyList<ContenedorCargaReporteLineaDto> lineas, EmpresaConfiguracionDto empresa)
    {
        switch (codigo)
        {
            case "ContenedorCarga":
                var carga = new ReporteContenedorCarga();
                carga.CargarDatos(encabezado, lineas, empresa);
                return carga;
            case "ContenedorDetalleLote":
                var detalleLote = new ReporteContenedorDetallePorLote();
                detalleLote.CargarDatos(encabezado, lineas, empresa);
                return detalleLote;
            case "ContenedorDetalleHuerta":
                var detalleHuerta = new ReporteContenedorDetallePorHuerta();
                detalleHuerta.CargarDatos(encabezado, lineas, empresa);
                return detalleHuerta;
            case "ContenedorResumenCalibre":
                var resumenCalibre = new ReporteContenedorResumenCalibre();
                resumenCalibre.CargarDatos(encabezado, lineas, empresa);
                return resumenCalibre;
            case "ContenedorResumenLote":
                var resumenLote = new ReporteContenedorResumenLote();
                resumenLote.CargarDatos(encabezado, lineas, empresa);
                return resumenLote;
            case "ContenedorResumenHuerta":
                var resumenHuerta = new ReporteContenedorResumenHuerta();
                resumenHuerta.CargarDatos(encabezado, lineas, empresa);
                return resumenHuerta;
            case "ContenedorResumenHuertaSinKg":
                var resumenHuertaSinKg = new ReporteContenedorResumenHuertaSinKg();
                resumenHuertaSinKg.CargarDatos(encabezado, lineas, empresa);
                return resumenHuertaSinKg;
            default:
                throw new InvalidOperationException($"No hay reporte de Contenedor registrado para el código '{codigo}'.");
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    // Fila del grid derecha-arriba: detalle del pallet seleccionado, con el folio del pallet
    // (conocido del renglón izquierdo, PalletDetalleDto no lo trae) para cumplir la columna
    // "No Pallet" pedida en la especificación.
    private sealed record PalletDetalleFilaDto(
        string ProductoCodigoSap, string ProductoDescripcion, int? Cajas, decimal Kilogramos, string PalletFolio, string LoteFolio);
}
