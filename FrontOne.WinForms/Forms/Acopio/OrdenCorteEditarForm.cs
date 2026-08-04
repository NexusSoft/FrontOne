using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Acopio;

// Captura una Orden de Corte que ejecuta un Acuerdo de Corte vigente. La mayoría de los campos
// de costo (Precio Acarreo, Kg Mínimo, Costo x kg/Pago por día/Cuadrilla de Apoyo) se resuelven
// y validan en el servidor (OrdenCorteService.ResolverYValidarAsync) como snapshot — lo que se
// muestra aquí mientras se captura es solo una vista previa con los mismos catálogos.
public partial class OrdenCorteEditarForm : XtraForm
{
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
    private readonly OrdenCorteDto? _ordenExistente;

    public event EventHandler? Guardado;

    private bool _cargandoInicial = true;
    private IReadOnlyList<AcuerdoVigenteDto> _acuerdosVigentes = [];
    private AcuerdoVigenteDto? _acuerdoSeleccionado;
    private IReadOnlyList<HuertaDto> _huertasDelProductor = [];
    private IReadOnlyList<ListaPrecioCorteDto> _listaPrecioCorte = [];
    private IReadOnlyList<ListaPrecioAcarreoDto> _listaPrecioAcarreo = [];
    private IReadOnlyList<ZonaDto> _zonas = [];
    private IReadOnlyList<TipoCorteDto> _tiposCorte = [];

    public OrdenCorteEditarForm()
    {
        InitializeComponent();
    }

    public OrdenCorteEditarForm(
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
        OrdenCorteDto? ordenExistente)
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
        _ordenExistente = ordenExistente;

        Text = ordenExistente is null ? "Nueva orden de corte" : "Editar orden de corte";
        _txtFolio.Text = ordenExistente?.Folio ?? "(se genera al guardar)";

        Load += async (_, _) => await CargarDatosInicialesAsync();
    }

    private async Task CargarDatosInicialesAsync()
    {
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: true, useFadeOut: true, "FrontOne", "Consultando SAP...");
        SapException? errorSap = null;
        try
        {
            await CargarProveedoresMercanciaAsync();
            await CargarProveedoresAcarreoAsync();
        }
        catch (SapException ex)
        {
            errorSap = ex;
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }

        if (errorSap is not null)
        {
            XtraMessageBox.Show(this, $"No se pudo consultar SAP Business One.\n\n{errorSap.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // La fecha y el Acuerdo se cargan primero y fuera del try/catch de abajo — si alguno de
        // los catálogos de apoyo (Cajas de Campo, Jefes de Acopio, etc.) falla al cargar, el
        // Acuerdo ya debe quedar seleccionable. Antes, una excepción en cualquier paso dejaba
        // _cargandoInicial en true para siempre y el combo de Acuerdo parecía "no dejar
        // seleccionar" (CmbAcuerdo_EditValueChanged descarta todo mientras _cargandoInicial).
        var fechaInicial = _ordenExistente?.Fecha ?? DateTime.Today;
        _dtFecha.EditValue = fechaInicial;
        await CargarAcuerdosVigentesAsync(fechaInicial);

        try
        {
            await CargarFloracionesAsync();
            await CargarVariedadesAsync();
            await CargarJefesCuadrillaAsync();
            await CargarJefesAcopioAsync();
            await CargarTiposCorteAsync();
            await CargarListaAcarreoYZonasAsync();
            await CargarCajasCampoAsync();

            if (_ordenExistente is { } orden)
            {
                _cmbAcuerdo.EditValue = orden.AcuerdoCorteId;
                _acuerdoSeleccionado = _acuerdosVigentes.FirstOrDefault(a => a.Id == orden.AcuerdoCorteId);
                _txtTipoPago.Text = ResolverTipoPagoNombre(_acuerdoSeleccionado?.TipoCorteId);
                _txtProducto.Text = _acuerdoSeleccionado?.ProductoNombre ?? string.Empty;
                _txtProductor.Text = orden.ProductorNombre;

                await CargarHuertasDelProductorAsync(orden.ProductorId);
                _cmbHuerta.EditValue = orden.HuertaId;
                _cmbFloracion.EditValue = orden.FloracionId;
                _txtRegistro.Text = orden.RegistroSagarpa;

                _cmbVariedad.EditValue = orden.VariedadId;

                _cmbPagarCorteA.EditValue = orden.PagarCorteACardCode;
                _cmbTransportista.EditValue = orden.TransportistaCardCode;

                _txtNoCandado.Text = orden.NoCandado;
                _cmbCajas.EditValue = orden.CajasEntregadas.ToString();

                _cmbJefeCuadrilla.EditValue = orden.JefeCuadrillaCardCode;

                _cmbJefeAcopio.EditValue = orden.JefeAcopioId;

                _txtPuntoReunion.Text = orden.PuntoReunion;
                _txtObservaciones.Text = orden.Observaciones;
                _chkCancelado.Checked = orden.Cancelado;
                _cmbCajaCampo.EditValue = orden.CajaCampoId;

                // Se muestran al final, después de los EditValueChanged disparados arriba, para
                // que el snapshot histórico gane sobre cualquier recálculo con el catálogo actual.
                _txtPrecioAcarreo.Text = orden.PrecioAcarreo.ToString("N2");
                _txtCostoKg.Text = orden.CostoKg.ToString("N2");
                _txtPagoDia.Text = orden.PagoDia.ToString("N2");
                _txtCuadrillaApoyo.Text = orden.CuadrillaApoyo.ToString("N2");
                _txtKgMinimo.Text = orden.KgMinimo.ToString("N2");
            }
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, $"No se pudieron cargar todos los catálogos.\n\n{ex.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _cargandoInicial = false;
        }
    }

    private async Task CargarAcuerdosVigentesAsync(DateTime fecha)
    {
        _acuerdosVigentes = await _ordenCorteService.ObtenerAcuerdosVigentesAsync(fecha);
        _cmbAcuerdo.Properties.DataSource = _acuerdosVigentes.ToList();
        _cmbAcuerdo.Properties.ValueMember = "Id";
        _cmbAcuerdo.Properties.DisplayMember = "Folio";
        _cmbAcuerdo.Properties.Columns.Clear();
        _cmbAcuerdo.Properties.Columns.Add(new LookUpColumnInfo("Folio", 80, "Folio"));
        _cmbAcuerdo.Properties.Columns.Add(new LookUpColumnInfo("ProductorNombre", 200, "Productor"));
        _cmbAcuerdo.Properties.Columns.Add(new LookUpColumnInfo("ProductoNombre", 150, "Producto"));
        _cmbAcuerdo.Properties.PopupWidth = 460;
    }

    private async Task CargarHuertasDelProductorAsync(int productorId)
    {
        _huertasDelProductor = await _huertaService.ObtenerPorProductorAsync(productorId);
        _cmbHuerta.Properties.DataSource = _huertasDelProductor.ToList();
        _cmbHuerta.Properties.ValueMember = "Id";
        _cmbHuerta.Properties.DisplayMember = "Nombre";
        _cmbHuerta.Properties.Columns.Clear();
        _cmbHuerta.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Huerta"));
        _cmbHuerta.Properties.Columns.Add(new LookUpColumnInfo("RegistroSagarpa", 150, "Registro Sagarpa"));
        _cmbHuerta.Properties.PopupWidth = 400;
    }

    private async Task CargarFloracionesAsync()
    {
        var floraciones = await _floracionService.ObtenerAsync();
        _cmbFloracion.Properties.DataSource = floraciones.ToList();
        _cmbFloracion.Properties.ValueMember = "Id";
        _cmbFloracion.Properties.DisplayMember = "Nombre";
        _cmbFloracion.Properties.Columns.Clear();
        _cmbFloracion.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Floración"));
        _cmbFloracion.Properties.PopupWidth = 250;
    }

    private async Task CargarVariedadesAsync()
    {
        var variedades = await _variedadService.ObtenerAsync();
        _cmbVariedad.Properties.DataSource = variedades.ToList();
        _cmbVariedad.Properties.ValueMember = "Id";
        _cmbVariedad.Properties.DisplayMember = "Nombre";
        _cmbVariedad.Properties.Columns.Clear();
        _cmbVariedad.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Variedad"));
        _cmbVariedad.Properties.PopupWidth = 250;
    }

    private async Task CargarProveedoresMercanciaAsync()
    {
        var proveedores = await _ordenCorteService.ObtenerProveedoresMercanciaAsync();
        _cmbPagarCorteA.Properties.DataSource = proveedores.ToList();
        _cmbPagarCorteA.Properties.ValueMember = "CardCode";
        _cmbPagarCorteA.Properties.DisplayMember = "CardName";
        _cmbPagarCorteA.Properties.Columns.Clear();
        _cmbPagarCorteA.Properties.Columns.Add(new LookUpColumnInfo("CardCode", 90, "Id"));
        _cmbPagarCorteA.Properties.Columns.Add(new LookUpColumnInfo("CardName", 260, "Proveedor"));
        _cmbPagarCorteA.Properties.PopupWidth = 380;
    }

    private async Task CargarProveedoresAcarreoAsync()
    {
        var proveedores = await _ordenCorteService.ObtenerProveedoresAcarreoAsync();
        _cmbTransportista.Properties.DataSource = proveedores.ToList();
        _cmbTransportista.Properties.ValueMember = "CardCode";
        _cmbTransportista.Properties.DisplayMember = "CardName";
        _cmbTransportista.Properties.Columns.Clear();
        _cmbTransportista.Properties.Columns.Add(new LookUpColumnInfo("CardCode", 90, "Id"));
        _cmbTransportista.Properties.Columns.Add(new LookUpColumnInfo("CardName", 260, "Proveedor"));
        _cmbTransportista.Properties.PopupWidth = 380;
    }

    private async Task CargarJefesCuadrillaAsync()
    {
        _listaPrecioCorte = await _listaPrecioCorteService.ObtenerAsync();
        _cmbJefeCuadrilla.Properties.DataSource = _listaPrecioCorte.ToList();
        _cmbJefeCuadrilla.Properties.ValueMember = "CardCode";
        _cmbJefeCuadrilla.Properties.DisplayMember = "CardName";
        _cmbJefeCuadrilla.Properties.Columns.Clear();
        _cmbJefeCuadrilla.Properties.Columns.Add(new LookUpColumnInfo("CardCode", 90, "Id"));
        _cmbJefeCuadrilla.Properties.Columns.Add(new LookUpColumnInfo("CardName", 260, "Empresa de Corte"));
        _cmbJefeCuadrilla.Properties.PopupWidth = 380;
    }

    private async Task CargarJefesAcopioAsync()
    {
        var jefes = await _jefeAcopioService.ObtenerAsync();
        _cmbJefeAcopio.Properties.DataSource = jefes.ToList();
        _cmbJefeAcopio.Properties.ValueMember = "Id";
        _cmbJefeAcopio.Properties.DisplayMember = "Nombre";
        _cmbJefeAcopio.Properties.Columns.Clear();
        _cmbJefeAcopio.Properties.Columns.Add(new LookUpColumnInfo("Clave", 70, "Clave"));
        _cmbJefeAcopio.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Nombre"));
        _cmbJefeAcopio.Properties.PopupWidth = 320;
    }

    private async Task CargarTiposCorteAsync() => _tiposCorte = await _tipoCorteService.ObtenerAsync();

    private async Task CargarCajasCampoAsync()
    {
        var cajasCampo = (await _cajaCampoService.ObtenerAsync()).Where(c => c.Activo).ToList();
        _cmbCajaCampo.Properties.DataSource = cajasCampo;
        _cmbCajaCampo.Properties.ValueMember = "Id";
        _cmbCajaCampo.Properties.DisplayMember = "Nombre";
        _cmbCajaCampo.Properties.Columns.Clear();
        _cmbCajaCampo.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Color de Caja"));
        _cmbCajaCampo.Properties.PopupWidth = 250;
    }

    private async Task CargarListaAcarreoYZonasAsync()
    {
        _listaPrecioAcarreo = await _listaPrecioAcarreoService.ObtenerAsync();
        _zonas = await _zonaService.ObtenerAsync();
    }

    private string ResolverTipoPagoNombre(int? tipoCorteId)
    {
        if (tipoCorteId is not int id)
        {
            return string.Empty;
        }

        return _tiposCorte.FirstOrDefault(t => t.Id == id)?.TipoPagoNombre ?? string.Empty;
    }

    private async void DtFecha_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cargandoInicial || _dtFecha.EditValue is not DateTime fecha)
        {
            return;
        }

        var acuerdoAnteriorId = _cmbAcuerdo.EditValue as int?;
        await CargarAcuerdosVigentesAsync(fecha);

        if (acuerdoAnteriorId is int id && !_acuerdosVigentes.Any(a => a.Id == id))
        {
            _cmbAcuerdo.EditValue = null;
            XtraMessageBox.Show(this, "El acuerdo seleccionado ya no está vigente para la nueva fecha — vuelve a elegirlo.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private async void CmbAcuerdo_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cargandoInicial)
        {
            return;
        }

        if (_cmbAcuerdo.EditValue is not int acuerdoId)
        {
            _acuerdoSeleccionado = null;
            _txtTipoPago.Text = string.Empty;
            _txtProducto.Text = string.Empty;
            _txtProductor.Text = string.Empty;
            return;
        }

        _acuerdoSeleccionado = _acuerdosVigentes.FirstOrDefault(a => a.Id == acuerdoId);
        if (_acuerdoSeleccionado is null)
        {
            return;
        }

        _txtProducto.Text = _acuerdoSeleccionado.ProductoNombre;
        _txtProductor.Text = _acuerdoSeleccionado.ProductorNombre;
        _txtTipoPago.Text = ResolverTipoPagoNombre(_acuerdoSeleccionado.TipoCorteId);
        _cmbVariedad.EditValue = _acuerdoSeleccionado.VariedadId;

        await CargarHuertasDelProductorAsync(_acuerdoSeleccionado.ProductorId);
        _cmbHuerta.EditValue = null;
        _txtRegistro.Text = string.Empty;
        LimpiarAcarreoYKgMinimo();
    }

    private void CmbHuerta_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cargandoInicial)
        {
            return;
        }

        if (_cmbHuerta.EditValue is not int huertaId)
        {
            _txtRegistro.Text = string.Empty;
            LimpiarAcarreoYKgMinimo();
            return;
        }

        var huerta = _huertasDelProductor.FirstOrDefault(h => h.Id == huertaId);
        _txtRegistro.Text = huerta?.RegistroSagarpa ?? string.Empty;

        RecalcularAcarreoYKgMinimo();
    }

    private void CmbCajas_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cargandoInicial)
        {
            return;
        }

        RecalcularAcarreoYKgMinimo();
    }

    // Vista previa: mismo cálculo que hace el servidor al guardar (Huerta.MunicipioId ->
    // ListaPrecioAcarreo -> Zona, según Cajas Entregadas), solo para mostrar en pantalla antes
    // de guardar. La validación real y el snapshot definitivo siempre los resuelve el servidor.
    private void RecalcularAcarreoYKgMinimo()
    {
        if (_cmbHuerta.EditValue is not int huertaId
            || _cmbCajas.EditValue is not string cajasTexto
            || !int.TryParse(cajasTexto, out var cajas))
        {
            LimpiarAcarreoYKgMinimo();
            return;
        }

        var huerta = _huertasDelProductor.FirstOrDefault(h => h.Id == huertaId);
        if (huerta?.MunicipioId is not int municipioId)
        {
            LimpiarAcarreoYKgMinimo();
            return;
        }

        var lista = _listaPrecioAcarreo.FirstOrDefault(l => l.MunicipioId == municipioId);
        if (lista is null)
        {
            LimpiarAcarreoYKgMinimo();
            return;
        }

        var precioAcarreo = cajas switch { 300 => lista.Precio300, 400 => lista.Precio400, _ => lista.Precio500 };
        _txtPrecioAcarreo.Text = precioAcarreo.ToString("N2");

        var zona = _zonas.FirstOrDefault(z => z.Id == lista.ZonaId);
        var kgMinimo = zona is null ? 0 : cajas switch { 300 => zona.KgMinimo300, 400 => zona.KgMinimo400, _ => zona.KgMinimo500 };
        _txtKgMinimo.Text = kgMinimo.ToString("N2");
    }

    private void LimpiarAcarreoYKgMinimo()
    {
        _txtPrecioAcarreo.Text = string.Empty;
        _txtKgMinimo.Text = string.Empty;
    }

    private void CmbJefeCuadrilla_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cargandoInicial)
        {
            return;
        }

        if (_cmbJefeCuadrilla.EditValue is not string cardCode)
        {
            _txtCostoKg.Text = string.Empty;
            _txtPagoDia.Text = string.Empty;
            _txtCuadrillaApoyo.Text = string.Empty;
            return;
        }

        var jefe = _listaPrecioCorte.FirstOrDefault(l => l.CardCode == cardCode);
        _txtCostoKg.Text = jefe?.PrecioKg.ToString("N2") ?? string.Empty;
        _txtPagoDia.Text = jefe?.PrecioDia.ToString("N2") ?? string.Empty;
        _txtCuadrillaApoyo.Text = jefe?.CuadrillaApoyo.ToString("N2") ?? string.Empty;
    }

    private async void CmbFloracion_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new FloracionesForm(_floracionService);
        form.ShowDialog(this);
        await CargarFloracionesAsync();
    }

    private async void CmbVariedad_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new VariedadesForm(_variedadService);
        form.ShowDialog(this);
        await CargarVariedadesAsync();
    }

    private async void CmbJefeCuadrilla_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new ListaPrecioCorteForm(_listaPrecioCorteService);
        form.ShowDialog(this);
        await CargarJefesCuadrillaAsync();
    }

    // Excepción documentada (igual criterio que JefeAcopioEditarForm en general): Jefes de
    // Acopio ya no tiene listado propio (JefesAcopioForm es un picker puro desde que se clonó
    // el patrón de navegación de Productor) — el + abre directo la pantalla de captura, que ya
    // arranca en blanco/modo Nuevo cuando se abre así.
    private async void CmbJefeAcopio_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new JefeAcopioEditarForm(_jefeAcopioService, _paisService, _estadoService, _municipioService, _poblacionService);
        form.ShowDialog(this);
        await CargarJefesAcopioAsync();
    }

    private async void CmbCajaCampo_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new CajasCampoForm(_cajaCampoService);
        form.ShowDialog(this);
        await CargarCajasCampoAsync();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_dtFecha.EditValue is not DateTime fecha)
        {
            XtraMessageBox.Show(this, "Captura la fecha.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbAcuerdo.EditValue is not int acuerdoId)
        {
            XtraMessageBox.Show(this, "Selecciona el acuerdo de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbHuerta.EditValue is not int huertaId)
        {
            XtraMessageBox.Show(this, "Selecciona la huerta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbFloracion.EditValue is not int floracionId)
        {
            XtraMessageBox.Show(this, "Selecciona la floración.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbVariedad.EditValue is not int variedadId)
        {
            XtraMessageBox.Show(this, "Selecciona la variedad.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbPagarCorteA.EditValue is not string pagarCorteACardCode)
        {
            XtraMessageBox.Show(this, "Selecciona a quién se le paga el corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbTransportista.EditValue is not string transportistaCardCode)
        {
            XtraMessageBox.Show(this, "Selecciona el transportista.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbCajas.EditValue is not string cajasTexto || !int.TryParse(cajasTexto, out var cajas))
        {
            XtraMessageBox.Show(this, "Selecciona las cajas a entregar.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbJefeCuadrilla.EditValue is not string jefeCuadrillaCardCode)
        {
            XtraMessageBox.Show(this, "Selecciona el jefe de cuadrilla.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbJefeAcopio.EditValue is not int jefeAcopioId)
        {
            XtraMessageBox.Show(this, "Selecciona el jefe de acopio.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbCajaCampo.EditValue is not int cajaCampoId)
        {
            XtraMessageBox.Show(this, "Selecciona el color de caja de campo.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var datos = new OrdenCorteDto(
            _ordenExistente?.Id ?? 0,
            _ordenExistente?.Folio ?? string.Empty,
            fecha,
            acuerdoId,
            _acuerdoSeleccionado?.Folio ?? string.Empty,
            _acuerdoSeleccionado?.ProductorId ?? 0,
            _txtProductor.Text,
            huertaId,
            _cmbHuerta.Text,
            floracionId,
            _cmbFloracion.Text,
            string.IsNullOrWhiteSpace(_txtRegistro.Text) ? null : _txtRegistro.Text,
            variedadId,
            _cmbVariedad.Text,
            string.Empty,
            string.Empty,
            pagarCorteACardCode,
            _cmbPagarCorteA.Text,
            transportistaCardCode,
            _cmbTransportista.Text,
            0,
            string.IsNullOrWhiteSpace(_txtNoCandado.Text) ? null : _txtNoCandado.Text,
            cajas,
            jefeCuadrillaCardCode,
            string.Empty,
            0,
            0,
            0,
            0,
            jefeAcopioId,
            string.Empty,
            string.IsNullOrWhiteSpace(_txtPuntoReunion.Text) ? null : _txtPuntoReunion.Text,
            string.IsNullOrWhiteSpace(_txtObservaciones.Text) ? null : _txtObservaciones.Text,
            _chkCancelado.Checked,
            cajaCampoId,
            _cmbCajaCampo.Text);

        try
        {
            if (_ordenExistente is null)
            {
                await _ordenCorteService.CrearAsync(datos);
            }
            else
            {
                await _ordenCorteService.ActualizarAsync(datos);
            }

            Guardado?.Invoke(this, EventArgs.Empty);
            Close();
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
