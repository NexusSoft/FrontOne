using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Pallets;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Reempaques;

// Pantalla principal de un Reempaque: encabezado, panel "Control de Kilogramos" y dos secciones
// (Entrada/Salida). El encabezado se guarda una sola vez (como PalletEditarForm) — Motivo ya no
// se puede tocar después: el reempaque nace y a partir de ahí solo se le agregan pallets.
//
// La Salida ya NO vive en tablas propias: cada línea que sale de aquí es una línea más de
// Produccion.PalletDetalle en un pallet del módulo de Pallets (normal o nacido de un reempaque
// anterior) — "Agregar a Pallet" busca un pallet destino Vacío/Incompleto y le agrega cajas desde
// el saldo; "Nuevo Pallet" abre el mismo PalletEditarForm del módulo de Pallets, sin duplicar
// captura. Por eso este form necesita el mismo repertorio de servicios que PalletsForm/
// PalletEditarForm — es quien los abre.
public partial class ReempaqueEditarForm : XtraForm
{
    private static readonly Color ColorCuadrado = Color.FromArgb(47, 158, 110);
    private static readonly Color ColorDescuadrado = Color.FromArgb(198, 40, 40);
    private static readonly string[] ColumnasFolioClickeable = ["PalletFolio"];

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

    private ReempaqueDto? _reempaque;
    private List<ReempaqueDetalleDto> _entrada = new();
    private List<ReempaqueSalidaFilaDto> _salida = new();

    private PalletEditarForm? _palletEditarForm;

    public event EventHandler? Guardado;

    public ReempaqueEditarForm()
    {
        InitializeComponent();
    }

    public ReempaqueEditarForm(
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
        SqlOptions sqlOptions,
        ReempaqueDto? existente)
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
        _reempaque = existente;

        ActualizarBotonesPorTab();
        Load += async (_, _) => await CargarAsync();
        _gridEntrada.SizeChanged += (_, _) => { if (_gridViewEntrada.Columns.Count > 0) _gridViewEntrada.BestFitColumns(); };
        _gridSalida.SizeChanged += (_, _) => { if (_gridViewSalida.Columns.Count > 0) _gridViewSalida.BestFitColumns(); };
    }

    // Los botones de acción viven fuera de los tabs (ver Designer, patrón fijo que no depende de
    // medir el área interna del XtraTabControl) — se alternan a mano según la pestaña activa.
    private void Tabs_SelectedPageChanged(object? sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        => ActualizarBotonesPorTab();

    private void ActualizarBotonesPorTab()
    {
        var enEntrada = _tabs.SelectedTabPage == _tabEntrada;

        _btnAgregarPallet.Visible = enEntrada;
        _btnQuitarPallet.Visible = enEntrada;
        _btnAgregarAPallet.Visible = !enEntrada;
        _btnNuevoPallet.Visible = !enEntrada;
        _btnQuitarLinea.Visible = !enEntrada;
        _btnAjusteNeutro.Visible = !enEntrada;
    }

    private async Task CargarAsync()
    {
        MostrarEncabezado();
        await CargarEntradaSalidaAsync();
    }

    private void MostrarEncabezado()
    {
        if (_reempaque is null)
        {
            _txtFolio.Text = "(se genera al guardar)";
            _txtFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
            _txtEstatus.Text = "Abierto";
            _txtMotivo.Text = string.Empty;
            _txtMotivo.Properties.ReadOnly = false;
            AplicarCierre(cerrado: false, hayPalletsOrigen: false);
            return;
        }

        _txtFolio.Text = _reempaque.Folio;
        _txtFecha.Text = _reempaque.FechaCreacion.ToString("dd/MM/yyyy");
        _txtEstatus.Text = NombreEstatus(_reempaque.Estatus);
        _txtMotivo.Text = _reempaque.Motivo;
        _txtMotivo.Properties.ReadOnly = true;
        _btnGuardar.Enabled = false;

        AplicarCierre(cerrado: _reempaque.Estatus != 1, hayPalletsOrigen: true);
    }

    private static string NombreEstatus(byte estatus) => estatus switch
    {
        1 => "Abierto",
        2 => "Cerrado",
        _ => string.Empty,
    };

    private void AplicarCierre(bool cerrado, bool hayPalletsOrigen)
    {
        _btnAgregarPallet.Enabled = !cerrado && hayPalletsOrigen;
        _btnQuitarPallet.Enabled = !cerrado && hayPalletsOrigen;
        _btnAgregarAPallet.Enabled = !cerrado && hayPalletsOrigen;
        _btnNuevoPallet.Enabled = !cerrado && hayPalletsOrigen;
        _btnQuitarLinea.Enabled = !cerrado && hayPalletsOrigen;
        _btnAjusteNeutro.Enabled = !cerrado && hayPalletsOrigen;
        _btnCerrarReempaque.Enabled = false; // se reactiva en ActualizarPanelKilos si cuadra
    }

    private async Task CargarEntradaSalidaAsync()
    {
        if (_reempaque is null)
        {
            _entrada = new List<ReempaqueDetalleDto>();
            _salida = new List<ReempaqueSalidaFilaDto>();
        }
        else
        {
            _entrada = (await _reempaqueService.ObtenerDetalleEntradaAsync(_reempaque.Id)).ToList();
            _salida = (await _reempaqueService.ObtenerDetalleSalidaAsync(_reempaque.Id)).ToList();
        }

        _gridEntrada.DataSource = _entrada;
        ConfigurarColumnasEntrada();

        _gridSalida.DataSource = _salida;
        ConfigurarColumnasSalida();

        ActualizarPanelKilos();
    }

    private void ConfigurarColumnasEntrada()
    {
        foreach (var nombre in new[] { "Id", "ReempaqueId", "PalletOrigenId", "LoteId", "ProductoTerminadoOrigenId" })
        {
            if (_gridViewEntrada.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridViewEntrada.Columns["PalletFolio"] is { } colPallet)
        {
            colPallet.Caption = "No. Pallet";
            AplicarEstiloFolioClickeable(colPallet);
        }

        if (_gridViewEntrada.Columns["LoteFolio"] is { } colLote)
        {
            colLote.Caption = "Lote";
        }

        if (_gridViewEntrada.Columns["ProductoDescripcion"] is { } colProducto)
        {
            colProducto.Caption = "Producto";
        }

        if (_gridViewEntrada.Columns["CajasEntrada"] is { } colCajas)
        {
            colCajas.Caption = "Cajas";
        }

        if (_gridViewEntrada.Columns["KilosEntrada"] is { } colKilos)
        {
            colKilos.Caption = "Kilogramos";
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        if (_gridViewEntrada.Columns["KilosDisponibles"] is { } colDisp)
        {
            colDisp.Caption = "Kg Pendientes";
            colDisp.DisplayFormat.FormatType = FormatType.Numeric;
            colDisp.DisplayFormat.FormatString = "n2";
        }

        if (_gridViewEntrada.Columns["PorcentajeMateriaSeca"] is { } colMS)
        {
            colMS.Caption = "% Materia Seca";
        }

        var orden = new[] { "PalletFolio", "LoteFolio", "ProductoDescripcion", "CajasEntrada", "KilosEntrada", "KilosDisponibles" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridViewEntrada.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridViewEntrada.BestFitColumns();
    }

    private void ConfigurarColumnasSalida()
    {
        foreach (var nombre in new[] { "PalletDetalleId", "PalletId", "EsNeutro", "LoteId", "ProductoTerminadoId", "ReempaqueDetalleId" })
        {
            if (_gridViewSalida.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridViewSalida.Columns["PalletFolio"] is { } colPallet)
        {
            colPallet.Caption = "No. Pallet";
            AplicarEstiloFolioClickeable(colPallet);
        }

        if (_gridViewSalida.Columns["PalletEstatus"] is { } colEstatus)
        {
            colEstatus.Caption = "Estatus del Pallet";
        }

        if (_gridViewSalida.Columns["LoteFolio"] is { } colLote)
        {
            colLote.Caption = "Lote";
        }

        if (_gridViewSalida.Columns["ProductoDescripcion"] is { } colProducto)
        {
            colProducto.Caption = "Producto";
        }

        if (_gridViewSalida.Columns["Cajas"] is { } colCajas)
        {
            colCajas.Caption = "Cajas";
        }

        if (_gridViewSalida.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        var orden = new[] { "PalletFolio", "PalletEstatus", "LoteFolio", "ProductoDescripcion", "Cajas", "Kilogramos" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridViewSalida.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridViewSalida.BestFitColumns();
    }

    private void GridViewSalida_CustomColumnDisplayText(object? sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "PalletEstatus" && e.Value is byte estatus)
        {
            e.DisplayText = PalletsForm.NombreEstatus(estatus);
        }
    }

    // El número puede leer 0.00 a nivel agregado; el color depende de la validación POR LOTE
    // (nunca se compensa entre lotes) — solo pasa a verde cuando TODOS los renglones de entrada
    // están en 0. Mismo criterio que exige Produccion.sp_Reempaque_Cerrar.
    private void ActualizarPanelKilos()
    {
        var kap = _reempaque?.KilosAProcesar ?? 0m;
        var kp = _reempaque?.KilosProcesados ?? 0m;
        var diferencia = kap - kp;

        _txtKilosAProcesar.Text = kap.ToString("n2");
        _txtKilosProcesados.Text = kp.ToString("n2");
        _txtDiferencia.Text = diferencia.ToString("n2");

        var cuadraPorLote = _entrada.Count > 0 && _entrada.All(d => d.KilosDisponibles == 0);

        _txtDiferencia.Properties.Appearance.BackColor = cuadraPorLote ? ColorCuadrado : ColorDescuadrado;
        _txtDiferencia.Properties.Appearance.ForeColor = Color.White;
        _txtDiferencia.Properties.Appearance.Options.UseBackColor = true;
        _txtDiferencia.Properties.Appearance.Options.UseForeColor = true;

        _btnCerrarReempaque.Enabled = cuadraPorLote && _reempaque is { Estatus: 1 };
    }

    private ReempaqueDetalleDto? ObtenerEntradaSeleccionada() => _gridViewEntrada.GetFocusedRow() as ReempaqueDetalleDto;

    private ReempaqueSalidaFilaDto? ObtenerSalidaSeleccionada() => _gridViewSalida.GetFocusedRow() as ReempaqueSalidaFilaDto;

    // Mismo patrón que PalletsForm/PalletEditarForm para la columna del folio de pallet: fuente +
    // color de hipervínculo, cursor de mano al pasar encima, clic abre PalletEditarForm.
    private static void AplicarEstiloFolioClickeable(DevExpress.XtraGrid.Columns.GridColumn columna)
    {
        columna.AppearanceCell.Font = new Font(columna.AppearanceCell.Font, FontStyle.Bold | FontStyle.Underline);
        columna.AppearanceCell.ForeColor = ColorTranslator.FromHtml("#0563C1");
        columna.AppearanceCell.Options.UseFont = true;
        columna.AppearanceCell.Options.UseForeColor = true;
    }

    private void GridViewEntrada_MouseMove(object? sender, MouseEventArgs e)
    {
        var info = _gridViewEntrada.CalcHitInfo(e.Location);
        _gridEntrada.Cursor = info.InRowCell && ColumnasFolioClickeable.Contains(info.Column?.FieldName)
            ? Cursors.Hand
            : Cursors.Default;
    }

    private void GridViewSalida_MouseMove(object? sender, MouseEventArgs e)
    {
        var info = _gridViewSalida.CalcHitInfo(e.Location);
        _gridSalida.Cursor = info.InRowCell && ColumnasFolioClickeable.Contains(info.Column?.FieldName)
            ? Cursors.Hand
            : Cursors.Default;
    }

    private async void GridViewEntrada_RowCellClick(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
    {
        if (e.Column.FieldName != "PalletFolio" || e.RowHandle < 0 || _gridViewEntrada.GetRow(e.RowHandle) is not ReempaqueDetalleDto fila)
        {
            return;
        }

        await AbrirPalletPorIdAsync(fila.PalletOrigenId);
    }

    private async void GridViewSalida_RowCellClick(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
    {
        if (e.Column.FieldName != "PalletFolio" || e.RowHandle < 0 || _gridViewSalida.GetRow(e.RowHandle) is not ReempaqueSalidaFilaDto fila)
        {
            return;
        }

        await AbrirPalletPorIdAsync(fila.PalletId);
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtMotivo.Text))
        {
            XtraMessageBox.Show(this, "Captura el motivo del reempaque.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var id = await _reempaqueService.CrearAsync(_txtMotivo.Text);
            _reempaque = await _reempaqueService.ObtenerPorIdAsync(id);
            MostrarEncabezado();
            await CargarEntradaSalidaAsync();
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

    private async void BtnAgregarPallet_Click(object? sender, EventArgs e)
    {
        if (_reempaque is null)
        {
            return;
        }

        using var form = new ReempaquePalletBuscarForm(_reempaqueService, ReempaquePalletBuscarModo.Origen, _reempaque.Id);
        if (form.ShowDialog(this) != DialogResult.OK || form.PalletIdSeleccionado is not { } palletId)
        {
            return;
        }

        try
        {
            await _reempaqueService.AgregarPalletOrigenAsync(_reempaque.Id, palletId);
            _reempaque = await _reempaqueService.ObtenerPorIdAsync(_reempaque.Id);
            await CargarEntradaSalidaAsync();
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

    private async void BtnQuitarPallet_Click(object? sender, EventArgs e)
    {
        var fila = ObtenerEntradaSeleccionada();
        if (_reempaque is null || fila is null)
        {
            XtraMessageBox.Show(this, "Selecciona un renglón de entrada.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Quitar el pallet '{fila.PalletFolio}' de este reempaque? Se revierten todos sus lotes de saldo.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _reempaqueService.QuitarPalletOrigenAsync(_reempaque.Id, fila.PalletOrigenId);
            _reempaque = await _reempaqueService.ObtenerPorIdAsync(_reempaque.Id);
            await CargarEntradaSalidaAsync();
            Guardado?.Invoke(this, EventArgs.Empty);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Busca un pallet destino (Vacío/Incompleto, no Neutro, que no sea origen de este mismo folio)
    // y luego captura de qué lote de saldo salen las cajas — mismo mecanismo, dos pasos, que
    // completa el pallet [15498] del caso de uso original.
    private async void BtnAgregarAPallet_Click(object? sender, EventArgs e)
    {
        if (_reempaque is null)
        {
            return;
        }

        using var buscador = new ReempaquePalletBuscarForm(_reempaqueService, ReempaquePalletBuscarModo.Destino, _reempaque.Id);
        if (buscador.ShowDialog(this) != DialogResult.OK || buscador.PalletIdSeleccionado is not { } palletId)
        {
            return;
        }

        var pallet = await _palletService.ObtenerPorIdAsync(palletId);
        if (pallet is null)
        {
            return;
        }

        using var captura = new ReempaqueLineaCapturaForm(
            _productoTerminadoService, _categoriaService, _tipoProductoService, _calibreApeamService,
            _marcaService, _pesoEstandarService, _paisService, _variedadService, _sessionContext,
            _entrada, pallet);
        if (captura.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            await _reempaqueService.AgregarLineaSalidaAsync(
                _reempaque.Id, palletId, captura.ReempaqueDetalleIdSeleccionado, captura.ProductoTerminadoIdSeleccionado,
                captura.CajasCapturadas, captura.KilogramosCapturados, captura.PorcentajeMateriaSecaCapturado);

            _reempaque = await _reempaqueService.ObtenerPorIdAsync(_reempaque.Id);
            await CargarEntradaSalidaAsync();
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

    // Abre el mismo PalletEditarForm del módulo de Pallets, en alta — el pallet nuevo no aparece
    // en Salida hasta que se le agregue una línea con "Agregar a Pallet" (ahí se busca por folio).
    private void BtnNuevoPallet_Click(object? sender, EventArgs e)
    {
        using var form = new PalletEditarForm(
            _palletService, _reempaqueService, _lineaProduccionService, _productoTerminadoService,
            _categoriaService, _tipoProductoService, _calibreApeamService, _marcaService,
            _pesoEstandarService, _paisService, _variedadService,
            _configuracionBasculaService, _empresaConfiguracionService,
            _etiquetaService, _licenciaTecitService, _sessionContext, _sqlOptions, null);
        form.ShowDialog(this);
    }

    private async void BtnQuitarLinea_Click(object? sender, EventArgs e)
    {
        var fila = ObtenerSalidaSeleccionada();
        if (_reempaque is null || fila is null)
        {
            XtraMessageBox.Show(this, "Selecciona una línea de salida.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (fila.EsNeutro)
        {
            XtraMessageBox.Show(this, "Un ajuste Neutro se elimina desde el módulo de Pallets.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var cantidadTexto = fila.Cajas is { } cajas ? $"{cajas} cajas" : $"{fila.Kilogramos:N2} kg";
        var confirmar = XtraMessageBox.Show(this,
            $"¿Quitar la línea de '{fila.ProductoDescripcion}' ({cantidadTexto}) del pallet '{fila.PalletFolio}'? El saldo se devuelve al lote '{fila.LoteFolio}'.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _reempaqueService.EliminarLineaSalidaAsync(fila.PalletId, fila.PalletDetalleId);
            _reempaque = await _reempaqueService.ObtenerPorIdAsync(_reempaque.Id);
            await CargarEntradaSalidaAsync();
            Guardado?.Invoke(this, EventArgs.Empty);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAjusteNeutro_Click(object? sender, EventArgs e)
    {
        if (_reempaque is null)
        {
            return;
        }

        var preseleccionado = ObtenerEntradaSeleccionada()?.Id;

        using var form = new ReempaqueNeutroCapturaForm(_reempaqueService, _productoTerminadoService, _reempaque.Id, _entrada, preseleccionado);
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _ = ActualizarTrasNeutroAsync();
    }

    private async Task ActualizarTrasNeutroAsync()
    {
        if (_reempaque is null)
        {
            return;
        }

        _reempaque = await _reempaqueService.ObtenerPorIdAsync(_reempaque.Id);
        await CargarEntradaSalidaAsync();
        Guardado?.Invoke(this, EventArgs.Empty);
    }

    private async Task AbrirPalletPorIdAsync(int palletId)
    {
        if (!_sessionContext.TienePermiso("Pallets", "Pallets", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var pallet = await _palletService.ObtenerPorIdAsync(palletId);
        if (pallet is null)
        {
            XtraMessageBox.Show(this, "El pallet ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_palletEditarForm is { IsDisposed: false })
        {
            _palletEditarForm.Activate();
            return;
        }

        _palletEditarForm = new PalletEditarForm(
            _palletService, _reempaqueService, _lineaProduccionService, _productoTerminadoService,
            _categoriaService, _tipoProductoService, _calibreApeamService, _marcaService,
            _pesoEstandarService, _paisService, _variedadService,
            _configuracionBasculaService, _empresaConfiguracionService,
            _etiquetaService, _licenciaTecitService, _sessionContext, _sqlOptions, pallet);
        _palletEditarForm.Guardado += async (_, _) =>
        {
            if (_reempaque is null)
            {
                return;
            }

            _reempaque = await _reempaqueService.ObtenerPorIdAsync(_reempaque.Id);
            await CargarEntradaSalidaAsync();
            Guardado?.Invoke(this, EventArgs.Empty);
        };
        _palletEditarForm.FormClosed += (_, _) => _palletEditarForm = null;
        _palletEditarForm.Show(this);
    }

    private async void BtnCerrarReempaque_Click(object? sender, EventArgs e)
    {
        if (_reempaque is null)
        {
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Cerrar el reempaque '{_reempaque.Folio}'? Los pallets origen quedarán marcados como Reempacados. Los pallets destino NO se bloquean: siguen su vida normal en el módulo de Pallets. Esta acción es definitiva.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _reempaqueService.CerrarAsync(_reempaque.Id);
            _reempaque = await _reempaqueService.ObtenerPorIdAsync(_reempaque.Id);
            MostrarEncabezado();
            await CargarEntradaSalidaAsync();
            Guardado?.Invoke(this, EventArgs.Empty);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
