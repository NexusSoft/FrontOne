using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Bascula;
using FrontOne.WinForms.Forms.Catalogos;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Pallets;

// Ventana flotante no modal (patrón CorridaEditarForm) con grid de detalle (patrón LoteEditarForm).
// A diferencia de LoteEditarForm, el detalle NO se acumula en memoria antes de guardar: cada línea
// se inserta/elimina contra la base al momento, porque cada movimiento ajusta
// Produccion.Corrida.KilosProcesados dentro de la transacción del SP. Por eso el encabezado tiene
// que existir antes de poder capturar líneas.
public partial class PalletEditarForm : XtraForm
{
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
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly EtiquetaService _etiquetaService = null!;
    private readonly LicenciaTecitService _licenciaTecitService = null!;
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
        EmpresaConfiguracionService empresaConfiguracionService,
        EtiquetaService etiquetaService,
        LicenciaTecitService licenciaTecitService,
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
        _empresaConfiguracionService = empresaConfiguracionService;
        _etiquetaService = etiquetaService;
        _licenciaTecitService = licenciaTecitService;
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
    // activos, con botón + hacia el listado completo. Excepción: el producto ya asignado al
    // encabezado de este pallet siempre se incluye aunque esté inactivo (ej. MERMA/DIFERENCIA
    // PESO A FAVOR del Pallet Neutro, que SAP marca inválidos) — si no, el LookUpEdit no puede
    // resolver el texto de un EditValue que apunta a un Id fuera de su lista.
    private async Task CargarProductosAsync()
    {
        var productos = (await _productoTerminadoService.ObtenerAsync()).ToList();
        _productos = productos.Where(p => p.Activo || p.Id == _pallet?.ProductoTerminadoId).ToList();

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
            _txtCajasObjetivo.Text = _detalle.Sum(d => d.Cajas ?? 0).ToString();
            return;
        }

        var producto = _cmbProducto.EditValue is int productoId
            ? _productos.FirstOrDefault(p => p.Id == productoId)
            : null;

        // Granel no mixto: no hay objetivo de cajas, el concepto no aplica (confirmado con el
        // usuario) — el campo queda vacío en vez de mostrar un CajasPorPallet que ya no existe.
        if (producto?.Presentacion == PresentacionProducto.Granel)
        {
            _txtCajasObjetivo.Text = string.Empty;
            return;
        }

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
        // "ImprimirCaja"/"ImprimirRegistro" no son campos de PalletDetalleDto — el grid las
        // autogenera solo si ya existen en la colección al momento de asignar DataSource (por eso
        // no se declaran en el Designer, donde rompería la autogeneración del resto de columnas);
        // se agregan aquí una sola vez, mismo patrón que CorridasForm._colDiferencia.
        if (_gridViewDetalle.Columns["ImprimirCaja"] is null)
        {
            _gridViewDetalle.Columns.Add(_colImprimirCaja);
        }

        if (_gridViewDetalle.Columns["ImprimirRegistro"] is null)
        {
            _gridViewDetalle.Columns.Add(_colImprimirRegistro);
        }

        foreach (var nombre in new[] { "Id", "PalletId", "CorridaId", "LoteId", "ProductoTerminadoId", "CodigoGs1128", "VoiceCodeLow", "VoiceCodeHigh" })
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

    // Botones de imprimir (ícono) en las columnas "ImprimirCaja"/"ImprimirRegistro": con
    // OptionsBehavior.Editable = false la celda nunca entra en modo edición, así que el
    // ButtonClick del RepositoryItemButtonEdit no llega a dispararse; el clic directo sobre la
    // celda sí (mismo patrón que CorridasForm).
    private void GridViewDetalle_RowCellClick(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
    {
        var tipo = e.Column.FieldName switch
        {
            "ImprimirCaja" => TipoEtiqueta.Caja,
            "ImprimirRegistro" => TipoEtiqueta.RegistroSagarpa,
            _ => (TipoEtiqueta?)null,
        };

        if (tipo is null || e.RowHandle < 0 || _pallet is null)
        {
            return;
        }

        if (_gridViewDetalle.GetRow(e.RowHandle) is not PalletDetalleDto fila)
        {
            return;
        }

        using var form = new PalletImprimirEtiquetaForm(
            _etiquetaService, _palletService, _empresaConfiguracionService, _licenciaTecitService, _sessionContext,
            tipo.Value, _pallet.Id, fila.Cajas ?? 0, fila.Id);
        form.ShowDialog(this);
    }

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
                form.CajasCapturadas, form.KilogramosCapturados, form.PorcentajeMateriaSecaSeleccionado);

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
                form.CajasCapturadas, form.KilogramosCapturados, form.PorcentajeMateriaSecaSeleccionado);

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

        // Una línea Granel no tiene Cajas (queda NULL) — el mensaje muestra Kilogramos en ese caso.
        var cantidadTexto = fila.Cajas is { } cajas ? $"{cajas} cajas" : $"{fila.Kilogramos:N2} kg";
        var confirmar = XtraMessageBox.Show(this,
            $"¿Quitar la línea de '{fila.ProductoDescripcion}' ({cantidadTexto}) de este pallet? Sus kilogramos se devolverán al saldo de la corrida del lote '{fila.LoteFolio}'.",
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
            .Sum(d => d.Cajas ?? 0);

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

    // "Imprimir Papeleta" abre el mismo formulario compartido (PalletImprimirEtiquetaForm) que
    // las columnas "Etiqueta Caja"/"Etiqueta Lote" del detalle (GridViewDetalle_RowCellClick) —
    // solo cambia el Tipo de etiqueta que filtra su lookupedit.
    private void BtnImprimirPapeleta_Click(object? sender, EventArgs e)
    {
        if (_pallet is null)
        {
            XtraMessageBox.Show(this, "Guarda el pallet antes de imprimir su papeleta.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new PalletImprimirEtiquetaForm(
            _etiquetaService, _palletService, _empresaConfiguracionService, _licenciaTecitService, _sessionContext,
            TipoEtiqueta.Pallet, _pallet.Id, _detalle.Sum(d => d.Cajas ?? 0));
        form.ShowDialog(this);
    }

    private void BtnCancelar_Click(object? sender, EventArgs e) => Close();
}
