using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Constants;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Bascula;
using FrontOne.WinForms.Forms.Catalogos;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Pallets;

// Ventana flotante no modal (patrón CorridaEditarForm) con grid de detalle (patrón LoteEditarForm).
// A diferencia de LoteEditarForm, el detalle NO se acumula en memoria antes de guardar: cada línea
// se inserta/elimina contra la base al momento, porque cada movimiento ajusta
// Produccion.Corrida.KilosProcesados dentro de la transacción del SP. Por eso el encabezado tiene
// que existir antes de poder capturar líneas.
public partial class PalletEditarForm : XtraForm
{
    private const string CodigoReporte = "Pallet";
    private const byte EstatusCompleto = 3;

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
    private readonly ReportePlantillaService _reportePlantillaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly SqlOptions _sqlOptions = null!;

    private PalletDto? _pallet;
    private List<PalletDetalleDto> _detalle = new();
    private List<ProductoTerminadoDto> _productos = new();

    public event EventHandler? Guardado;

    public PalletEditarForm()
    {
        InitializeComponent();
    }

    public PalletEditarForm(
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
        SqlOptions sqlOptions,
        PalletDto? palletExistente)
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
        _pallet = palletExistente;

        Load += async (_, _) => await CargarAsync();
    }

    private async Task CargarAsync()
    {
        await CargarLineasProduccionAsync();
        await CargarProductosAsync();
        MostrarEncabezado();
        await CargarDetalleAsync();
    }

    // Mismo patrón exacto que PalletDetalleCapturaForm.CargarProductosAsync — solo productos
    // activos, con botón + hacia el listado completo.
    private async Task CargarProductosAsync()
    {
        _productos = (await _productoTerminadoService.ObtenerAsync()).Where(p => p.Activo).ToList();

        _cmbProducto.Properties.DataSource = _productos;
        _cmbProducto.Properties.ValueMember = "Id";
        _cmbProducto.Properties.DisplayMember = "DescripcionSap";
        _cmbProducto.Properties.Columns.Clear();
        _cmbProducto.Properties.Columns.Add(new LookUpColumnInfo("CodigoSap", 100, "Código SAP"));
        _cmbProducto.Properties.Columns.Add(new LookUpColumnInfo("DescripcionSap", 300, "Descripción"));
        _cmbProducto.Properties.PopupWidth = 430;
    }

    private async Task CargarLineasProduccionAsync()
    {
        var lineas = (await _lineaProduccionService.ObtenerAsync()).ToList();
        _cmbLineaProduccion.Properties.DataSource = lineas;
        _cmbLineaProduccion.Properties.ValueMember = "Id";
        _cmbLineaProduccion.Properties.DisplayMember = "Nombre";
        _cmbLineaProduccion.Properties.Columns.Clear();
        _cmbLineaProduccion.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 200, "Línea de Producción"));
        _cmbLineaProduccion.Properties.PopupWidth = 230;
    }

    private void MostrarEncabezado()
    {
        if (_pallet is null)
        {
            _txtFolio.Text = "(se genera al guardar)";
            _txtFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
            _txtHora.Text = string.Empty;
            _txtEstatus.Text = PalletsForm.NombreEstatus(1);
            _cmbProducto.EditValue = null;
            _cmbProducto.Properties.NullText = "Seleccionar";
            _txtCajasObjetivo.Text = string.Empty;
            _spnPorcentajeMateriaSeca.EditValue = 0m;
            _spnPesoReal.EditValue = 0m;
            _txtNoReempaque.Text = string.Empty;
            _chkEsMixto.Checked = false;
            AplicarBloqueo(false);
            return;
        }

        _txtFolio.Text = _pallet.Folio;
        _txtFecha.Text = _pallet.FechaCreacion.ToString("dd/MM/yyyy");
        _txtHora.Text = _pallet.HoraCreacion.ToString(@"hh\:mm");
        _txtEstatus.Text = PalletsForm.NombreEstatus(_pallet.Estatus);
        _cmbLineaProduccion.EditValue = _pallet.LineaProduccionId;
        _chkEsMixto.Checked = _pallet.EsMixto;
        _cmbProducto.Properties.NullText = _pallet.EsMixto ? "PALLET MIXTO" : "Seleccionar";
        _cmbProducto.EditValue = _pallet.ProductoTerminadoId;
        ActualizarCajasObjetivo();
        _spnPorcentajeMateriaSeca.EditValue = _pallet.PorcentajeMateriaSeca;
        _spnPesoReal.EditValue = _pallet.PesoReal ?? 0m;
        _txtNoReempaque.Text = _pallet.NoReempaque;

        AplicarBloqueo(_pallet.Bloqueado);
    }

    // Un Pallet bloqueado es definitivo: ni el encabezado ni el detalle se pueden volver a tocar.
    // El Producto del encabezado, además, se bloquea en cuanto ya hay al menos una línea de
    // detalle capturada (mismo criterio que el Lote de una línea) o cuando el pallet es mixto
    // (ahí no aplica: cada línea trae su propio producto libremente, como siempre).
    private void AplicarBloqueo(bool bloqueado)
    {
        _cmbLineaProduccion.Properties.ReadOnly = bloqueado;
        _chkEsMixto.Properties.ReadOnly = bloqueado;
        _cmbProducto.Properties.ReadOnly = bloqueado || _chkEsMixto.Checked || _detalle.Count > 0;
        _spnPesoReal.Properties.ReadOnly = bloqueado;
        _btnTomarLectura.Enabled = !bloqueado;
        _btnGuardar.Enabled = !bloqueado;
        _btnBloquear.Enabled = !bloqueado && _pallet is not null;
        _btnDetalleNuevo.Enabled = !bloqueado && _pallet is not null;

        ActualizarBotonesDetalle();
    }

    private void ChkEsMixto_CheckedChanged(object? sender, EventArgs e)
    {
        if (_chkEsMixto.Checked)
        {
            _cmbProducto.EditValue = null;
        }

        _cmbProducto.Properties.NullText = _chkEsMixto.Checked ? "PALLET MIXTO" : "Seleccionar";
        ActualizarCajasObjetivo();
        AplicarBloqueo(_pallet?.Bloqueado ?? false);
    }

    private void CmbProducto_EditValueChanged(object? sender, EventArgs e) => ActualizarCajasObjetivo();

    // En un pallet mixto no hay un producto único que fije el objetivo, así que el campo Cajas
    // Objetivo se convierte en la suma de las cajas ya capturadas en el grid de detalle.
    private void ActualizarCajasObjetivo()
    {
        if (_chkEsMixto.Checked)
        {
            _txtCajasObjetivo.Text = _detalle.Sum(d => d.Cajas).ToString();
            return;
        }

        var producto = _cmbProducto.EditValue is int productoId
            ? _productos.FirstOrDefault(p => p.Id == productoId)
            : null;

        _txtCajasObjetivo.Text = producto?.CajasPorPallet?.ToString() ?? string.Empty;
    }

    private async void CmbProducto_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        var seleccionado = _cmbProducto.EditValue;

        using var form = new ProductosTerminadosForm(
            _productoTerminadoService, _categoriaService, _tipoProductoService, _calibreApeamService,
            _marcaService, _pesoEstandarService, _paisService, _variedadService, _sessionContext);
        form.ShowDialog(this);

        await CargarProductosAsync();
        _cmbProducto.EditValue = seleccionado;
        ActualizarCajasObjetivo();
    }

    private async Task CargarDetalleAsync()
    {
        if (_pallet is null)
        {
            _detalle = new List<PalletDetalleDto>();
            _gridDetalle.DataSource = _detalle;
            ConfigurarColumnasDetalle();
            AplicarBloqueo(false);
            return;
        }

        try
        {
            _detalle = (await _palletService.ObtenerDetalleAsync(_pallet.Id)).ToList();
            _gridDetalle.DataSource = _detalle;
            ConfigurarColumnasDetalle();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // El bloqueo del Producto de encabezado y, en pallet mixto, la suma de Cajas Objetivo
        // dependen del detalle recién cargado — se reaplican aquí porque MostrarEncabezado corre
        // antes que este método.
        ActualizarCajasObjetivo();
        AplicarBloqueo(_pallet.Bloqueado);
    }

    private void ConfigurarColumnasDetalle()
    {
        foreach (var nombre in new[] { "Id", "PalletId", "CorridaId", "LoteId", "ProductoTerminadoId" })
        {
            if (_gridViewDetalle.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridViewDetalle.Columns["ProductoCodigoSap"] is { } colCodigo)
        {
            colCodigo.Caption = "Código SAP";
        }

        if (_gridViewDetalle.Columns["ProductoDescripcion"] is { } colProducto)
        {
            colProducto.Caption = "Producto";
        }

        if (_gridViewDetalle.Columns["LoteFolio"] is { } colLote)
        {
            colLote.Caption = "Lote";
        }

        if (_gridViewDetalle.Columns["Cajas"] is { } colCajas)
        {
            colCajas.Summary.Clear();
            colCajas.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Cajas", "Suma: {0:n0}");
        }

        if (_gridViewDetalle.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
            colKilos.Summary.Clear();
            colKilos.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Kilogramos", "Suma: {0:n2}");
        }

        if (_gridViewDetalle.Columns["PorcentajeMateriaSeca"] is { } colMateriaSeca)
        {
            colMateriaSeca.Caption = "% Materia Seca";
            colMateriaSeca.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMateriaSeca.DisplayFormat.FormatString = "n2";
        }

        if (_gridViewDetalle.Columns["CajasPorPallet"] is { } colCajasPorPallet)
        {
            colCajasPorPallet.Caption = "Cajas por Pallet";
        }

        if (_gridViewDetalle.Columns["LoteEnProceso"] is { } colEnProceso)
        {
            colEnProceso.Caption = "Lote en Proceso";
        }

        _gridViewDetalle.OptionsView.ShowFooter = true;
        _gridViewDetalle.BestFitColumns();
        ActualizarBotonesDetalle();
    }

    // Una línea cuya Corrida ya se finalizó queda de solo lectura aunque el Pallet siga sin
    // bloquear: sus kilos ya se cerraron del lado de la Corrida y revertirlos descuadraría el lote.
    private void ActualizarBotonesDetalle()
    {
        var bloqueado = _pallet?.Bloqueado ?? false;
        var fila = ObtenerFilaSeleccionada();
        var editable = !bloqueado && fila is { LoteEnProceso: true };

        _btnDetalleEditar.Enabled = editable;
        _btnDetalleBorrar.Enabled = editable;
    }

    private void GridViewDetalle_FocusedRowChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        => ActualizarBotonesDetalle();

    private PalletDetalleDto? ObtenerFilaSeleccionada() => _gridViewDetalle.GetFocusedRow() as PalletDetalleDto;

    private async void CmbLineaProduccion_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        var seleccionada = _cmbLineaProduccion.EditValue;

        using var form = new LineasProduccionForm(_lineaProduccionService);
        form.ShowDialog(this);

        await CargarLineasProduccionAsync();
        _cmbLineaProduccion.EditValue = seleccionada;
    }

    private async void BtnTomarLectura_Click(object? sender, EventArgs e)
    {
        try
        {
            var configuracion = await _configuracionBasculaService.ObtenerAsync();
            var peso = BasculaLecturaService.TomarLectura(configuracion);
            _spnPesoReal.EditValue = peso;
        }
        catch (InvalidOperationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // El encabezado se guarda sin cerrar la ventana: un Pallet nuevo tiene que existir en la base
    // antes de poder capturarle líneas (cada línea consume kilos de una Corrida en el mismo SP).
    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbLineaProduccion.EditValue is not int lineaProduccionId)
        {
            XtraMessageBox.Show(this, "Selecciona una línea de producción.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var productoTerminadoId = _cmbProducto.EditValue as int?;
        if (!_chkEsMixto.Checked && productoTerminadoId is null)
        {
            XtraMessageBox.Show(this, "Selecciona un producto para este pallet.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var pesoReal = Convert.ToDecimal(_spnPesoReal.EditValue);
        decimal? pesoRealGuardar = pesoReal > 0 ? pesoReal : null;

        try
        {
            if (_pallet is null)
            {
                var id = await _palletService.CrearAsync(lineaProduccionId, _chkEsMixto.Checked, productoTerminadoId, pesoRealGuardar);
                _pallet = await _palletService.ObtenerPorIdAsync(id);
            }
            else
            {
                await _palletService.ActualizarEncabezadoAsync(_pallet.Id, lineaProduccionId, _chkEsMixto.Checked, productoTerminadoId, pesoRealGuardar);
                _pallet = await _palletService.ObtenerPorIdAsync(_pallet.Id);
            }

            MostrarEncabezado();
            await CargarDetalleAsync();
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

    private async void BtnBloquear_Click(object? sender, EventArgs e)
    {
        if (_pallet is null)
        {
            return;
        }

        // Un pallet no mixto solo se puede bloquear ya Completo (cajas = objetivo exacto); uno
        // mixto no tiene esa restricción, no hay un objetivo único de cajas que exigirle.
        if (!_pallet.EsMixto && _pallet.Estatus != EstatusCompleto)
        {
            XtraMessageBox.Show(this,
                "No se puede bloquear: el pallet todavía no está Completo (las cajas capturadas no igualan el objetivo del producto).",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Bloquear el pallet '{_pallet.Folio}'? Esta acción es definitiva: el encabezado y todas sus líneas quedarán de solo lectura y no hay forma de desbloquearlo.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _palletService.BloquearAsync(_pallet.Id);
            _pallet = await _palletService.ObtenerPorIdAsync(_pallet.Id);
            MostrarEncabezado();
            await CargarDetalleAsync();
            Guardado?.Invoke(this, EventArgs.Empty);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnDetalleNuevo_Click(object? sender, EventArgs e)
    {
        if (_pallet is null)
        {
            XtraMessageBox.Show(this, "Guarda el pallet antes de agregarle líneas de detalle.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = CrearFormularioCaptura(null);
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            await _palletService.AgregarLineaAsync(
                _pallet.Id, form.CorridaIdSeleccionada, form.ProductoTerminadoIdSeleccionado,
                form.CajasCapturadas, form.PorcentajeMateriaSecaSeleccionado);

            await RefrescarTrasCambioDetalleAsync();
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

    private async void BtnDetalleEditar_Click(object? sender, EventArgs e)
    {
        var fila = ObtenerFilaSeleccionada();
        if (_pallet is null || fila is null)
        {
            XtraMessageBox.Show(this, "Selecciona un renglón del detalle.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = CrearFormularioCaptura(fila);
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            await _palletService.ActualizarLineaAsync(
                _pallet.Id, fila.Id, form.ProductoTerminadoIdSeleccionado,
                form.CajasCapturadas, form.PorcentajeMateriaSecaSeleccionado);

            await RefrescarTrasCambioDetalleAsync();
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

    private async void BtnDetalleBorrar_Click(object? sender, EventArgs e)
    {
        var fila = ObtenerFilaSeleccionada();
        if (_pallet is null || fila is null)
        {
            XtraMessageBox.Show(this, "Selecciona un renglón del detalle.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Quitar la línea de '{fila.ProductoDescripcion}' ({fila.Cajas} cajas) de este pallet? Sus kilogramos se devolverán al saldo de la corrida del lote '{fila.LoteFolio}'.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _palletService.EliminarLineaAsync(_pallet.Id, fila.Id);
            await RefrescarTrasCambioDetalleAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Cualquier movimiento del detalle recalcula Estatus y % Materia Seca del encabezado dentro
    // del SP, así que hay que releer las dos partes, no solo el grid.
    private async Task RefrescarTrasCambioDetalleAsync()
    {
        if (_pallet is null)
        {
            return;
        }

        _pallet = await _palletService.ObtenerPorIdAsync(_pallet.Id);
        MostrarEncabezado();
        await CargarDetalleAsync();
        Guardado?.Invoke(this, EventArgs.Empty);
    }

    private PalletDetalleCapturaForm CrearFormularioCaptura(PalletDetalleDto? detalleExistente)
    {
        // Cajas ya capturadas en las DEMÁS líneas del pallet — la que se está editando (si aplica)
        // no cuenta contra sí misma en el tope de cajas del producto de encabezado.
        var cajasYaEnPallet = _detalle
            .Where(d => detalleExistente is null || d.Id != detalleExistente.Id)
            .Sum(d => d.Cajas);

        return new PalletDetalleCapturaForm(
            _palletService, _productoTerminadoService, _categoriaService, _tipoProductoService,
            _calibreApeamService, _marcaService, _pesoEstandarService, _paisService, _variedadService,
            _sessionContext,
            _pallet?.Folio ?? string.Empty,
            _cmbLineaProduccion.EditValue as int?,
            _chkEsMixto.Checked,
            _cmbProducto.EditValue as int?,
            cajasYaEnPallet,
            detalleExistente);
    }

    // Placeholder a propósito: el catálogo de plantillas de etiquetas es un módulo aparte
    // (Etiquetado) que todavía no existe. Cuando exista, aquí va el selector de plantillas en vez
    // de este mensaje — el resto del formulario no depende de eso.
    private void BtnImprimirEtiquetas_Click(object? sender, EventArgs e)
        => XtraMessageBox.Show(this, "El módulo de Etiquetado aún no está disponible.", "FrontOne",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

    private async void BtnImprimirPapeleta_Click(object? sender, EventArgs e)
    {
        if (_pallet is null)
        {
            XtraMessageBox.Show(this, "Guarda el pallet antes de imprimir su papeleta.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!_sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.VistaPrevia))
        {
            XtraMessageBox.Show(this, "No tienes permiso para ver este reporte.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var datosReporte = await _palletService.ObtenerParaReporteAsync(_pallet.Id);
            if (datosReporte is null)
            {
                XtraMessageBox.Show(this, "El pallet ya no existe.", "FrontOne",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detalle = await _palletService.ObtenerDetalleAsync(_pallet.Id);
            var empresa = await _empresaConfiguracionService.ObtenerAsync();

            var reporte = await CrearReporteAsync();
            reporte.CargarDatos(datosReporte, detalle, empresa);

            using var visor = new VisorReporteForm(reporte, CodigoReporte, _sessionContext);
            visor.ShowDialog(this);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Si el usuario personalizó el layout en el Diseñador de Reportes, se carga esa plantilla en
    // vez del diseño default — mismo patrón que RecepcionesFrutaForm.CrearReporteAsync.
    private async Task<ReportePallet> CrearReporteAsync()
    {
        var reporte = new ReportePallet();

        var plantilla = await _reportePlantillaService.ObtenerPorCodigoAsync(CodigoReporte);
        if (!string.IsNullOrWhiteSpace(plantilla?.DefinicionXml))
        {
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(plantilla.DefinicionXml));
            reporte.LoadLayoutFromXml(stream);
        }

        return reporte;
    }

    private void BtnCancelar_Click(object? sender, EventArgs e) => Close();
}
