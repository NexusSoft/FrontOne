using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Reflection;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Session;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class HuertaEditarForm : XtraForm
{
    private const string Modulo = "Catalogos";
    private const string PantallaProductores = "Productores";
    private const string PantallaPaises = "Paises";
    private const string PantallaEstados = "Estados";
    private const string PantallaHuertas = "Huertas";
    private const string AccionCrear = "Crear";
    private const string RecursoIconoPin = "FrontOne.WinForms.Resources.Mapas.PinHuerta.png";

    private readonly HuertaService _huertaService = null!;
    private readonly ProductorService _productorService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PoblacionService _poblacionService = null!;
    private readonly ProductoService _productoService = null!;
    private readonly SistemaRiegoService _sistemaRiegoService = null!;
    private readonly StatusHuertaService _statusHuertaService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly GMapOverlay _mapOverlayHuerta = new("Marcador");

    private HuertaDto? _huertaActual;

    // Con 54k huertas / 33k productores el form ya no carga catálogos completos:
    // el navegador solo contiene las huertas del productor seleccionado,
    // y el productor se elige vía picker de búsqueda (no LookUpEdit con todo).
    private int? _productorSeleccionadoId;
    private readonly Dictionary<int, string> _nombresProductorCache = [];

    public HuertaEditarForm()
    {
        InitializeComponent();
        AsignarIconosNavegacion();
    }

    public HuertaEditarForm(
        HuertaService huertaService,
        ProductorService productorService,
        PaisService paisService,
        EstadoService estadoService,
        MunicipioService municipioService,
        PoblacionService poblacionService,
        ProductoService productoService,
        SistemaRiegoService sistemaRiegoService,
        StatusHuertaService statusHuertaService,
        SessionContext sessionContext)
        : this()
    {
        _huertaService = huertaService;
        _productorService = productorService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;
        _poblacionService = poblacionService;
        _productoService = productoService;
        _sistemaRiegoService = sistemaRiegoService;
        _statusHuertaService = statusHuertaService;
        _sessionContext = sessionContext;

        InicializarMapa();

        _cmbPais.EditValueChanged += async (_, _) => await CargarEstadosDelPaisAsync(seleccionarPrimero: true);
        _cmbEstado.EditValueChanged += async (_, _) => await CargarMunicipiosDelEstadoAsync(seleccionarPrimero: false);
        _cmbMunicipio.EditValueChanged += async (_, _) => await CargarPoblacionesDelMunicipioAsync(seleccionarPrimero: false);
        _spnLatitud.EditValueChanged += (_, _) => ActualizarMapaUbicacion();
        _spnLongitud.EditValueChanged += (_, _) => ActualizarMapaUbicacion();

        Load += async (_, _) => await CargarDatosIniciales();
    }

    private async Task CargarDatosIniciales()
    {
        await CargarPaisesAsync();
        await CargarProductosAsync();
        await CargarSistemasRiegoAsync();
        await CargarStatusHuertaAsync();
        LimpiarFormulario();
    }

    // Navegación del catálogo completo por Id de creación, sin cargar nada a
    // memoria: cada botón pide un solo registro al servidor (seek por índice).
    private async void BtnInicio_Click(object? sender, EventArgs e)
        => await CargarHuertaEnFormularioAsync(await _huertaService.ObtenerPrimeroAsync());

    private async void BtnFin_Click(object? sender, EventArgs e)
        => await CargarHuertaEnFormularioAsync(await _huertaService.ObtenerUltimoAsync());

    private async void BtnAnterior_Click(object? sender, EventArgs e)
    {
        if (_huertaActual is null)
        {
            return;
        }

        var anterior = await _huertaService.ObtenerAnteriorAsync(_huertaActual.Id);
        if (anterior is not null)
        {
            await CargarHuertaEnFormularioAsync(anterior);
        }
    }

    private async void BtnSiguiente_Click(object? sender, EventArgs e)
    {
        var siguiente = _huertaActual is null
            ? await _huertaService.ObtenerPrimeroAsync()
            : await _huertaService.ObtenerSiguienteAsync(_huertaActual.Id);

        if (siguiente is not null)
        {
            await CargarHuertaEnFormularioAsync(siguiente);
        }
    }

    private async Task CargarHuertaEnFormularioAsync(HuertaDto? huerta)
    {
        _huertaActual = huerta;

        if (huerta is null)
        {
            LimpiarFormulario();
            return;
        }

        _lblIdHuerta.Text = $"Id: {huerta.Id}";
        _txtNombreHuerta.Text = huerta.Nombre;
        await MostrarProductorAsync(huerta.ProductorId);
        _txtUbicacion.Text = huerta.Ubicacion ?? string.Empty;
        _cmbProducto.EditValue = huerta.ProductoId;
        _txtEncargadoNombre.Text = huerta.EncargadoNombre ?? string.Empty;
        _txtEncargadoTelefono.Text = huerta.EncargadoTelefono ?? string.Empty;
        _txtObservaciones.Text = huerta.Observaciones ?? string.Empty;
        _txtRegistroSagarpa.Text = huerta.RegistroSagarpa ?? string.Empty;
        _chkGlobalGap.Checked = huerta.CertificadoGlobalGap;
        _txtRegistroFda.Text = huerta.RegistroFda ?? string.Empty;
        _txtNumeroGlobalGap.Text = huerta.NumeroGlobalGap ?? string.Empty;
        ActualizarEstadoCamposGlobalGap();

        _spnSuperficie.Value = huerta.Superficie ?? 0;
        _spnAltura.Value = huerta.Altura ?? 0;
        _spnNumeroArboles.Value = huerta.NumeroArboles ?? 0;
        _spnEdadArboles.Value = huerta.EdadArboles ?? 0;
        _cmbSistemaRiego.EditValue = huerta.SistemaRiegoId;
        _spnPorcentajeMecanizacion.Value = huerta.PorcentajeMecanizacion ?? 0;
        _spnLatitud.Value = huerta.Latitud ?? 0;
        _spnLongitud.Value = huerta.Longitud ?? 0;
        ActualizarMapaUbicacion();

        _cmbStatusHuerta.EditValue = huerta.StatusHuertaId;
        _dtFechaCambioStatus.EditValue = huerta.FechaCambioStatus;
        _txtAniosEnStatus.Text = huerta.AniosEnStatusActual?.ToString("N1") ?? string.Empty;
        _cmbEstatusActivo.SelectedIndex = huerta.Activo ? 0 : 1;
        _dtFechaVencimiento.EditValue = huerta.FechaVencimiento;
        _txtDiasVencimiento.Text = huerta.DiasParaVencimiento?.ToString() ?? string.Empty;

        if (huerta.EstadoId is not null)
        {
            var estados = await _estadoService.ObtenerAsync();
            var estado = estados.FirstOrDefault(e => e.Id == huerta.EstadoId);
            if (estado is not null)
            {
                _cmbPais.EditValue = estado.PaisId;
                await CargarEstadosDelPaisAsync(seleccionarPrimero: false);
                _cmbEstado.EditValue = estado.Id;
                await CargarMunicipiosDelEstadoAsync(seleccionarPrimero: false);
                _cmbMunicipio.EditValue = huerta.MunicipioId;
                await CargarPoblacionesDelMunicipioAsync(seleccionarPrimero: false);
                _cmbPoblacion.EditValue = huerta.PoblacionId;
                return;
            }
        }

        _cmbPais.EditValue = null;
        _cmbEstado.EditValue = null;
        _cmbMunicipio.Properties.DataSource = null;
        _cmbMunicipio.EditValue = null;
        _cmbPoblacion.Properties.DataSource = null;
        _cmbPoblacion.EditValue = null;
    }

    private void LimpiarFormulario()
    {
        _huertaActual = null;

        _lblIdHuerta.Text = "Id: (nueva)";
        _txtNombreHuerta.Text = string.Empty;
        _productorSeleccionadoId = null;
        _cmbProductor.Text = string.Empty;
        _txtUbicacion.Text = string.Empty;
        _cmbPoblacion.EditValue = null;
        _cmbMunicipio.EditValue = null;
        _cmbPais.EditValue = null;
        _cmbEstado.EditValue = null;
        _cmbProducto.EditValue = null;
        _txtEncargadoNombre.Text = string.Empty;
        _txtEncargadoTelefono.Text = string.Empty;
        _txtObservaciones.Text = string.Empty;
        _txtRegistroSagarpa.Text = string.Empty;
        _chkGlobalGap.Checked = false;
        _txtRegistroFda.Text = string.Empty;
        _txtNumeroGlobalGap.Text = string.Empty;
        ActualizarEstadoCamposGlobalGap();

        _spnSuperficie.Value = 0;
        _spnAltura.Value = 0;
        _spnNumeroArboles.Value = 0;
        _spnEdadArboles.Value = 0;
        _cmbSistemaRiego.EditValue = null;
        _spnPorcentajeMecanizacion.Value = 0;
        _spnLatitud.Value = 0;
        _spnLongitud.Value = 0;
        ActualizarMapaUbicacion();

        _cmbStatusHuerta.EditValue = null;
        _dtFechaCambioStatus.EditValue = null;
        _txtAniosEnStatus.Text = string.Empty;
        _cmbEstatusActivo.SelectedIndex = 0;
        _dtFechaVencimiento.EditValue = null;
        _txtDiasVencimiento.Text = string.Empty;
    }

    private void ChkGlobalGap_CheckedChanged(object? sender, EventArgs e) => ActualizarEstadoCamposGlobalGap();

    private void ActualizarEstadoCamposGlobalGap()
    {
        _txtRegistroFda.Enabled = _chkGlobalGap.Checked;
        _txtNumeroGlobalGap.Enabled = _chkGlobalGap.Checked;
    }

    private void InicializarMapa()
    {
        GMaps.Instance.Mode = AccessMode.ServerOnly;

        _mapHuerta.DragButton = MouseButtons.Left;
        _mapHuerta.CanDragMap = true;
        _mapHuerta.MapProvider = GMapProviders.GoogleHybridMap;
        _mapHuerta.Position = new PointLatLng(23.6345, -102.5528);
        _mapHuerta.MinZoom = 0;
        _mapHuerta.MaxZoom = 24;
        _mapHuerta.Zoom = 5;
        _mapHuerta.AutoScroll = true;
        _mapHuerta.Overlays.Add(_mapOverlayHuerta);
    }

    private void ActualizarMapaUbicacion()
    {
        var latitud = _spnLatitud.Value;
        var longitud = _spnLongitud.Value;

        _mapOverlayHuerta.Markers.Clear();

        if (latitud == 0 || longitud == 0)
        {
            return;
        }

        var punto = new PointLatLng((double)latitud, (double)longitud);
        var iconoPin = CrearBitmapPin();
        var marcador = new GMarkerGoogle(punto, iconoPin)
        {
            Offset = new Point(-iconoPin.Width / 2, -iconoPin.Height),
            ToolTipMode = MarkerTooltipMode.Always,
            ToolTipText = chk_mostrar.Checked
                ? string.Format("\n{0}\nLatitud: {1}\nLongitud: {2}", _txtNombreHuerta.Text, latitud, longitud)
                : string.Format("\n{0}", _txtNombreHuerta.Text),
        };
        marcador.ToolTip.Fill = new SolidBrush(Color.CornflowerBlue);
        marcador.ToolTip.Font = new Font("Tahoma", 8f, FontStyle.Bold);
        marcador.ToolTip.TextPadding = new Size(8, 8);
        marcador.ToolTip.Foreground = new SolidBrush(Color.White);
        marcador.ToolTip.Stroke = Pens.Black;

        _mapOverlayHuerta.Markers.Add(marcador);
        _mapHuerta.Position = punto;
        _mapHuerta.Zoom = 14;
    }

    // Íconos de navegación dibujados por código (sin depender de archivos externos
    // ni de licencias de terceros) — mismo criterio que CrearBitmapPin().
    private void AsignarIconosNavegacion()
    {
        _btnInicio.ImageOptions.Image = CrearIconoFlecha(conBarra: true, apuntaDerecha: false);
        _btnAnterior.ImageOptions.Image = CrearIconoFlecha(conBarra: false, apuntaDerecha: false);
        _btnSiguiente.ImageOptions.Image = CrearIconoFlecha(conBarra: false, apuntaDerecha: true);
        _btnFin.ImageOptions.Image = CrearIconoFlecha(conBarra: true, apuntaDerecha: true);
    }

    private static Bitmap CrearIconoFlecha(bool conBarra, bool apuntaDerecha)
    {
        const int tam = 16;
        var bitmap = new Bitmap(tam, tam);
        using var g = Graphics.FromImage(bitmap);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var pincel = new SolidBrush(Color.CornflowerBlue);

        if (apuntaDerecha)
        {
            g.FillPolygon(pincel, [new Point(4, 2), new Point(4, 14), new Point(12, 8)]);
            if (conBarra)
            {
                g.FillRectangle(pincel, 12, 2, 2, 12);
            }
        }
        else
        {
            g.FillPolygon(pincel, [new Point(12, 2), new Point(12, 14), new Point(4, 8)]);
            if (conBarra)
            {
                g.FillRectangle(pincel, 2, 2, 2, 12);
            }
        }

        return bitmap;
    }

    private static Bitmap CrearBitmapPin()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(RecursoIconoPin)
            ?? throw new InvalidOperationException($"No se encontró el recurso embebido '{RecursoIconoPin}' del ícono del mapa.");

        return new Bitmap(stream);
    }

    private void ChkMostrar_CheckedChanged(object? sender, EventArgs e) => ActualizarMapaUbicacion();

    private void BtRecargarMapa_Click(object? sender, EventArgs e)
    {
        _mapHuerta.MapProvider = comboBoxEdit1.Text switch
        {
            "Google Maps Satélite" => GMapProviders.GoogleSatelliteMap,
            "Google Maps Callejero" => GMapProviders.GoogleMap,
            "Google Maps Híbrido" => GMapProviders.GoogleHybridMap,
            "OpenClycleMap" => GMapProviders.OpenCycleMap,
            _ => _mapHuerta.MapProvider,
        };

        _mapHuerta.Refresh();
    }

    private async Task CargarPaisesAsync()
    {
        var paises = await _paisService.ObtenerAsync();
        _cmbPais.Properties.DataSource = paises.ToList();
        _cmbPais.Properties.ValueMember = "Id";
        _cmbPais.Properties.DisplayMember = "Nombre";
        _cmbPais.Properties.Columns.Clear();
        _cmbPais.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "País"));
        _cmbPais.Properties.PopupWidth = 250;
    }

    private async Task CargarEstadosDelPaisAsync(bool seleccionarPrimero)
    {
        if (_cmbPais.EditValue is not int paisId)
        {
            _cmbEstado.Properties.DataSource = null;
            return;
        }

        var estados = await _estadoService.ObtenerAsync(paisId);
        _cmbEstado.Properties.DataSource = estados.ToList();
        _cmbEstado.Properties.ValueMember = "Id";
        _cmbEstado.Properties.DisplayMember = "Nombre";
        _cmbEstado.Properties.Columns.Clear();
        _cmbEstado.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Estado"));
        _cmbEstado.Properties.PopupWidth = 250;

        if (seleccionarPrimero)
        {
            _cmbEstado.EditValue = estados.Count > 0 ? estados[0].Id : null;
        }
    }

    private async Task CargarMunicipiosDelEstadoAsync(bool seleccionarPrimero)
    {
        if (_cmbEstado.EditValue is not int estadoId)
        {
            _cmbMunicipio.Properties.DataSource = null;
            _cmbMunicipio.EditValue = null;
            return;
        }

        var municipios = await _municipioService.ObtenerAsync(estadoId);
        _cmbMunicipio.Properties.DataSource = municipios.ToList();
        _cmbMunicipio.Properties.ValueMember = "Id";
        _cmbMunicipio.Properties.DisplayMember = "Nombre";
        _cmbMunicipio.Properties.Columns.Clear();
        _cmbMunicipio.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Municipio"));
        _cmbMunicipio.Properties.PopupWidth = 250;

        if (seleccionarPrimero)
        {
            _cmbMunicipio.EditValue = municipios.Count > 0 ? municipios[0].Id : null;
        }
    }

    private async Task CargarPoblacionesDelMunicipioAsync(bool seleccionarPrimero)
    {
        if (_cmbMunicipio.EditValue is not int municipioId)
        {
            _cmbPoblacion.Properties.DataSource = null;
            _cmbPoblacion.EditValue = null;
            return;
        }

        var poblaciones = await _poblacionService.ObtenerAsync(municipioId);
        _cmbPoblacion.Properties.DataSource = poblaciones.ToList();
        _cmbPoblacion.Properties.ValueMember = "Id";
        _cmbPoblacion.Properties.DisplayMember = "Nombre";
        _cmbPoblacion.Properties.Columns.Clear();
        _cmbPoblacion.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Población"));
        _cmbPoblacion.Properties.PopupWidth = 250;

        if (seleccionarPrimero)
        {
            _cmbPoblacion.EditValue = poblaciones.Count > 0 ? poblaciones[0].Id : null;
        }
    }

    private async Task CargarProductosAsync()
    {
        var productos = await _productoService.ObtenerAsync();
        _cmbProducto.Properties.DataSource = productos.ToList();
        _cmbProducto.Properties.ValueMember = "Id";
        _cmbProducto.Properties.DisplayMember = "Nombre";
        _cmbProducto.Properties.Columns.Clear();
        _cmbProducto.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Producto"));
        _cmbProducto.Properties.PopupWidth = 250;
    }

    private async Task CargarSistemasRiegoAsync()
    {
        var sistemas = await _sistemaRiegoService.ObtenerAsync();
        _cmbSistemaRiego.Properties.DataSource = sistemas.ToList();
        _cmbSistemaRiego.Properties.ValueMember = "Id";
        _cmbSistemaRiego.Properties.DisplayMember = "Nombre";
        _cmbSistemaRiego.Properties.Columns.Clear();
        _cmbSistemaRiego.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Sistema de Riego"));
        _cmbSistemaRiego.Properties.PopupWidth = 250;
    }

    private async Task CargarStatusHuertaAsync()
    {
        var estatus = await _statusHuertaService.ObtenerAsync();
        _cmbStatusHuerta.Properties.DataSource = estatus.ToList();
        _cmbStatusHuerta.Properties.ValueMember = "Id";
        _cmbStatusHuerta.Properties.DisplayMember = "Nombre";
        _cmbStatusHuerta.Properties.Columns.Clear();
        _cmbStatusHuerta.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Estatus"));
        _cmbStatusHuerta.Properties.PopupWidth = 250;
    }

    // Muestra el nombre del productor en el ButtonEdit sin bajar el catálogo completo;
    // cachea los nombres ya consultados para no repetir viajes al navegar.
    private async Task MostrarProductorAsync(int productorId)
    {
        _productorSeleccionadoId = productorId;

        if (!_nombresProductorCache.TryGetValue(productorId, out var nombre))
        {
            var productor = await _productorService.ObtenerPorIdAsync(productorId);
            nombre = productor?.NombreProductor ?? "-";
            _nombresProductorCache[productorId] = nombre;
        }

        _cmbProductor.Text = nombre;
    }

    private void BtnNuevo_Click(object? sender, EventArgs e) => LimpiarFormulario();

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        if (_huertaActual is null)
        {
            XtraMessageBox.Show(this, "No hay ninguna huerta cargada.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar la huerta '{_huertaActual.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _huertaService.EliminarAsync(_huertaActual.Id);
            LimpiarFormulario();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void TxtNombreHuerta_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Search)
        {
            return;
        }

        using var form = new HuertasForm(_huertaService, _productorService);
        if (form.ShowDialog(this) != DialogResult.OK || form.HuertaSeleccionada is null)
        {
            return;
        }

        var huerta = await _huertaService.ObtenerPorIdAsync(form.HuertaSeleccionada.Id);
        await CargarHuertaEnFormularioAsync(huerta);
    }

    private async void CmbProductor_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind == ButtonPredefines.Search)
        {
            using var buscador = new ProductoresForm(_productorService, _paisService, _estadoService);
            if (buscador.ShowDialog(this) != DialogResult.OK || buscador.ProductorSeleccionado is null)
            {
                return;
            }

            _productorSeleccionadoId = buscador.ProductorSeleccionado.Id;
            _nombresProductorCache[buscador.ProductorSeleccionado.Id] = buscador.ProductorSeleccionado.NombreProductor;
            _cmbProductor.Text = buscador.ProductorSeleccionado.NombreProductor;
            return;
        }

        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        if (!TienePermisoCrear(PantallaProductores))
        {
            return;
        }

        using var form = new ProductorEditarForm(_productorService, _paisService, _estadoService, _municipioService, _huertaService, _poblacionService);
        form.ShowDialog(this);
        // El productor recién creado se selecciona con el botón de búsqueda; no hay catálogo que recargar.
        await Task.CompletedTask;
    }

    private async void CmbPais_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        if (!TienePermisoCrear(PantallaPaises))
        {
            return;
        }

        using var form = new PaisesForm(_paisService);
        form.ShowDialog(this);
        await CargarPaisesAsync();
    }

    private async void CmbEstado_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        if (!TienePermisoCrear(PantallaEstados))
        {
            return;
        }

        using var form = new EstadosForm(_estadoService, _paisService);
        form.ShowDialog(this);
        await CargarEstadosDelPaisAsync(seleccionarPrimero: false);
    }

    private async void CmbMunicipio_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        if (!TienePermisoCrear(PantallaHuertas))
        {
            return;
        }

        using var form = new MunicipiosForm(_municipioService, _paisService, _estadoService);
        form.ShowDialog(this);
        await CargarMunicipiosDelEstadoAsync(seleccionarPrimero: false);
    }

    private async void CmbPoblacion_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        if (!TienePermisoCrear(PantallaHuertas))
        {
            return;
        }

        using var form = new PoblacionesForm(_poblacionService, _paisService, _estadoService, _municipioService);
        form.ShowDialog(this);
        await CargarPoblacionesDelMunicipioAsync(seleccionarPrimero: false);
    }

    private async void CmbProducto_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        if (!TienePermisoCrear(PantallaHuertas))
        {
            return;
        }

        using var form = new ProductosForm(_productoService);
        form.ShowDialog(this);
        await CargarProductosAsync();
    }

    private async void CmbSistemaRiego_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        if (!TienePermisoCrear(PantallaHuertas))
        {
            return;
        }

        using var form = new SistemasRiegoForm(_sistemaRiegoService);
        form.ShowDialog(this);
        await CargarSistemasRiegoAsync();
    }

    private async void CmbStatusHuerta_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        if (!TienePermisoCrear(PantallaHuertas))
        {
            return;
        }

        using var form = new StatusHuertaForm(_statusHuertaService);
        form.ShowDialog(this);
        await CargarStatusHuertaAsync();
    }

    private bool TienePermisoCrear(string pantalla)
    {
        if (_sessionContext.TienePermiso(Modulo, pantalla, AccionCrear))
        {
            return true;
        }

        XtraMessageBox.Show(this, "No tienes permiso para acceder a este formulario.", "FrontOne",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void BtnCancelar_Click(object? sender, EventArgs e) => Close();

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_productorSeleccionadoId is not int productorId)
        {
            XtraMessageBox.Show(this, "Selecciona un productor.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var datos = new HuertaDto(
            _huertaActual?.Id ?? 0,
            _txtNombreHuerta.Text.Trim(),
            productorId,
            NuloSiVacio(_txtUbicacion.Text),
            _cmbPoblacion.EditValue as int?,
            _cmbMunicipio.EditValue as int?,
            _cmbEstado.EditValue as int?,
            _cmbProducto.EditValue as int?,
            NuloSiVacio(_txtEncargadoNombre.Text),
            NuloSiVacio(_txtEncargadoTelefono.Text),
            NuloSiVacio(_txtObservaciones.Text),
            NuloSiVacio(_txtRegistroSagarpa.Text),
            _chkGlobalGap.Checked,
            NuloSiVacio(_txtRegistroFda.Text),
            NuloSiVacio(_txtNumeroGlobalGap.Text),
            _spnSuperficie.Value,
            _spnAltura.Value,
            (int)_spnNumeroArboles.Value,
            (int)_spnEdadArboles.Value,
            _cmbSistemaRiego.EditValue as int?,
            _spnPorcentajeMecanizacion.Value,
            _cmbStatusHuerta.EditValue as int?,
            _dtFechaCambioStatus.EditValue as DateTime?,
            _dtFechaVencimiento.EditValue as DateTime?,
            _spnLatitud.Value == 0 ? null : _spnLatitud.Value,
            _spnLongitud.Value == 0 ? null : _spnLongitud.Value,
            _cmbEstatusActivo.SelectedIndex == 0,
            null,
            null);

        try
        {
            int idGuardado;

            if (_huertaActual is null)
            {
                idGuardado = await _huertaService.CrearAsync(datos);
            }
            else
            {
                await _huertaService.ActualizarAsync(datos);
                idGuardado = _huertaActual.Id;
            }

            var huertaGuardada = await _huertaService.ObtenerPorIdAsync(idGuardado);
            await CargarHuertaEnFormularioAsync(huertaGuardada);

            XtraMessageBox.Show(this, "Huerta guardada.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    private static string? NuloSiVacio(string valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
