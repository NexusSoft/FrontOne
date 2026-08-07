using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Acopio;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class ProductoTerminadoEditarForm : XtraForm
{
    private const string Modulo = "Catalogos";
    private const string PantallaCategorias = "Categorias";
    private const string PantallaTiposProducto = "TiposProducto";
    private const string PantallaCalibresApeam = "CalibresApeam";
    private const string PantallaMarcas = "Marcas";
    private const string PantallaPaises = "Paises";
    private const string PantallaVariedades = "Variedades";
    private const string PantallaPesosEstandar = "PesosEstandar";
    private const string AccionCrear = "Crear";

    private readonly ProductoTerminadoService _productoTerminadoService = null!;
    private readonly CategoriaService _categoriaService = null!;
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly MarcaService _marcaService = null!;
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly PaisService _paisService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly ProductoTerminadoDto _productoExistente = null!;

    private IReadOnlyList<PesoEstandarDto> _pesosEstandar = [];

    public ProductoTerminadoEditarForm()
    {
        InitializeComponent();
    }

    public ProductoTerminadoEditarForm(
        ProductoTerminadoService productoTerminadoService,
        CategoriaService categoriaService,
        TipoProductoService tipoProductoService,
        CalibreApeamService calibreApeamService,
        MarcaService marcaService,
        PesoEstandarService pesoEstandarService,
        PaisService paisService,
        VariedadService variedadService,
        SessionContext sessionContext,
        ProductoTerminadoDto productoExistente)
        : this()
    {
        _productoTerminadoService = productoTerminadoService;
        _categoriaService = categoriaService;
        _tipoProductoService = tipoProductoService;
        _calibreApeamService = calibreApeamService;
        _marcaService = marcaService;
        _pesoEstandarService = pesoEstandarService;
        _paisService = paisService;
        _variedadService = variedadService;
        _sessionContext = sessionContext;
        _productoExistente = productoExistente;

        Text = $"FrontOne - Editar producto terminado ({productoExistente.CodigoSap})";

        _cmbPesoEstandar.EditValueChanged += CmbPesoEstandar_EditValueChanged;

        Load += async (_, _) => await CargarDatosIniciales();
    }

    private async Task CargarDatosIniciales()
    {
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: true, useFadeOut: true, "FrontOne", "Consultando SAP...");
        try
        {
            await CargarCategoriasAsync();
            await CargarTiposProductoAsync();
            await CargarCalibresApeamAsync();
            await CargarMercadoDestinoAsync();
            await CargarMarcasAsync();
            await CargarVariedadesAsync();
            await CargarPesosEstandarAsync();
            await CargarMateriasPrimaAsync();

            MostrarProductoEnFormulario();
            await CargarListaMaterialesAsync();
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }
    }

    // Materia Prima viene en vivo de SAP (grupo "MP") — no hay catálogo FrontOne que administrar,
    // por eso el LookUpEdit no lleva botón "+" (no hay nada que crear/editar desde aquí).
    private async Task CargarMateriasPrimaAsync()
    {
        try
        {
            var materiasPrima = await _productoTerminadoService.ObtenerMateriasPrimaAsync();
            _cmbMateriaPrima.Properties.DataSource = materiasPrima.ToList();
            _cmbMateriaPrima.Properties.ValueMember = "ItemCode";
            _cmbMateriaPrima.Properties.DisplayMember = "ItemName";
            _cmbMateriaPrima.Properties.Columns.Clear();
            _cmbMateriaPrima.Properties.Columns.Add(new LookUpColumnInfo("ItemCode", 90, "Código"));
            _cmbMateriaPrima.Properties.Columns.Add(new LookUpColumnInfo("ItemName", 300, "Descripción"));
            _cmbMateriaPrima.Properties.PopupWidth = 410;
        }
        catch (SapException ex)
        {
            // No bloquea el resto del form: si SAP no responde, el combo queda vacío pero el
            // producto se puede seguir editando con el resto de la información.
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void MostrarProductoEnFormulario()
    {
        var p = _productoExistente;

        _txtCodigoSap.Text = p.CodigoSap;
        _txtDescripcionSap.Text = p.DescripcionSap;
        _txtDescripcionExtranjeraSap.Text = p.DescripcionExtranjeraSap ?? string.Empty;

        _txtCodigoUpc.Text = p.CodigoUpc ?? string.Empty;
        _txtCodigoPlu.Text = p.CodigoPlu ?? string.Empty;
        _txtCodigoGtin.Text = p.CodigoGtin ?? string.Empty;
        _cmbMateriaPrima.EditValue = p.MateriaPrimaItemCode;
        _cmbCategoria.EditValue = p.CategoriaId;
        _cmbTipoProducto.EditValue = p.TipoProductoId;
        _cmbCalibreApeam.EditValue = p.CalibreApeamId;
        _txtCalibreCodigoExterno.Text = p.CalibreCodigoExterno ?? string.Empty;
        _cmbMercadoDestino.EditValue = p.MercadoDestinoPaisId;
        _cmbMarca.EditValue = p.MarcaId;
        _cmbVariedad.EditValue = p.VariedadId;
        _cmbPesoEstandar.EditValue = p.PesoEstandarId;
        _txtPesoNeto.Text = p.PesoNeto?.ToString("N3") ?? string.Empty;
        _txtPesoPromedio.Text = p.PesoPromedio?.ToString("N3") ?? string.Empty;
        _spnCajasPorPallet.Value = p.CajasPorPallet ?? 1;

        // Un producto ya desactivado en SAP queda en modo solo lectura: no tiene sentido seguir
        // capturando información de negocio de un producto que SAP ya no reconoce como vigente.
        if (!p.Activo)
        {
            _txtCodigoUpc.Properties.ReadOnly = true;
            _txtCodigoPlu.Properties.ReadOnly = true;
            _txtCodigoGtin.Properties.ReadOnly = true;
            _cmbMateriaPrima.Properties.ReadOnly = true;
            _cmbCategoria.Properties.ReadOnly = true;
            _cmbTipoProducto.Properties.ReadOnly = true;
            _cmbCalibreApeam.Properties.ReadOnly = true;
            _txtCalibreCodigoExterno.Properties.ReadOnly = true;
            _cmbMercadoDestino.Properties.ReadOnly = true;
            _cmbMarca.Properties.ReadOnly = true;
            _cmbVariedad.Properties.ReadOnly = true;
            _cmbPesoEstandar.Properties.ReadOnly = true;
            _spnCajasPorPallet.Enabled = false;
            _btnGuardar.Enabled = false;
        }
    }

    private async Task CargarListaMaterialesAsync()
    {
        try
        {
            var materiales = await _productoTerminadoService.ObtenerListaMaterialesAsync(_productoExistente.CodigoSap);
            _gridCarta.DataSource = materiales.ToList();
        }
        catch (SapException ex)
        {
            // No bloquea el resto del form: la carta técnica es informativa, el producto se
            // puede seguir editando aunque SAP no responda o el ItemCode no tenga lista de materiales.
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void CmbPesoEstandar_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cmbPesoEstandar.EditValue is not int pesoEstandarId)
        {
            _txtPesoNeto.Text = string.Empty;
            _txtPesoPromedio.Text = string.Empty;
            return;
        }

        var pesoEstandar = _pesosEstandar.FirstOrDefault(pe => pe.Id == pesoEstandarId);
        _txtPesoNeto.Text = pesoEstandar?.PesoNeto.ToString("N3") ?? string.Empty;
        _txtPesoPromedio.Text = pesoEstandar?.PesoPromedio.ToString("N3") ?? string.Empty;
    }

    private async Task CargarCategoriasAsync()
    {
        var categorias = await _categoriaService.ObtenerAsync();
        _cmbCategoria.Properties.DataSource = categorias.ToList();
        _cmbCategoria.Properties.ValueMember = "Id";
        _cmbCategoria.Properties.DisplayMember = "Nombre";
        _cmbCategoria.Properties.Columns.Clear();
        _cmbCategoria.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Categoría"));
        _cmbCategoria.Properties.PopupWidth = 250;
    }

    private async Task CargarTiposProductoAsync()
    {
        var tipos = await _tipoProductoService.ObtenerAsync();
        _cmbTipoProducto.Properties.DataSource = tipos.ToList();
        _cmbTipoProducto.Properties.ValueMember = "Id";
        _cmbTipoProducto.Properties.DisplayMember = "Nombre";
        _cmbTipoProducto.Properties.Columns.Clear();
        _cmbTipoProducto.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Tipo de Producto"));
        _cmbTipoProducto.Properties.PopupWidth = 250;
    }

    private async Task CargarCalibresApeamAsync()
    {
        var calibres = await _calibreApeamService.ObtenerAsync();
        _cmbCalibreApeam.Properties.DataSource = calibres.ToList();
        _cmbCalibreApeam.Properties.ValueMember = "Id";
        _cmbCalibreApeam.Properties.DisplayMember = "Nombre";
        _cmbCalibreApeam.Properties.Columns.Clear();
        _cmbCalibreApeam.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Calibre APEAM"));
        _cmbCalibreApeam.Properties.PopupWidth = 250;
    }

    private async Task CargarMercadoDestinoAsync()
    {
        var paises = await _paisService.ObtenerAsync();
        _cmbMercadoDestino.Properties.DataSource = paises.ToList();
        _cmbMercadoDestino.Properties.ValueMember = "Id";
        _cmbMercadoDestino.Properties.DisplayMember = "Nombre";
        _cmbMercadoDestino.Properties.Columns.Clear();
        _cmbMercadoDestino.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "País"));
        _cmbMercadoDestino.Properties.PopupWidth = 250;
    }

    private async Task CargarMarcasAsync()
    {
        var marcas = await _marcaService.ObtenerAsync();
        _cmbMarca.Properties.DataSource = marcas.ToList();
        _cmbMarca.Properties.ValueMember = "Id";
        _cmbMarca.Properties.DisplayMember = "Nombre";
        _cmbMarca.Properties.Columns.Clear();
        _cmbMarca.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Marca"));
        _cmbMarca.Properties.PopupWidth = 250;
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

    private async Task CargarPesosEstandarAsync()
    {
        _pesosEstandar = await _pesoEstandarService.ObtenerAsync();
        _cmbPesoEstandar.Properties.DataSource = _pesosEstandar.ToList();
        _cmbPesoEstandar.Properties.ValueMember = "Id";
        _cmbPesoEstandar.Properties.DisplayMember = "Descripcion";
        _cmbPesoEstandar.Properties.Columns.Clear();
        _cmbPesoEstandar.Properties.Columns.Add(new LookUpColumnInfo("Codigo", 90, "Código"));
        _cmbPesoEstandar.Properties.Columns.Add(new LookUpColumnInfo("Descripcion", 220, "Descripción"));
        _cmbPesoEstandar.Properties.PopupWidth = 330;
    }

    private async void CmbCategoria_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaCategorias))
        {
            return;
        }

        using var form = new CategoriasForm(_categoriaService);
        form.ShowDialog(this);
        await CargarCategoriasAsync();
    }

    private async void CmbTipoProducto_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaTiposProducto))
        {
            return;
        }

        using var form = new TiposProductoForm(_tipoProductoService);
        form.ShowDialog(this);
        await CargarTiposProductoAsync();
    }

    private async void CmbCalibreApeam_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaCalibresApeam))
        {
            return;
        }

        using var form = new CalibresApeamForm(_calibreApeamService);
        form.ShowDialog(this);
        await CargarCalibresApeamAsync();
    }

    private async void CmbMercadoDestino_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaPaises))
        {
            return;
        }

        using var form = new PaisesForm(_paisService);
        form.ShowDialog(this);
        await CargarMercadoDestinoAsync();
    }

    private async void CmbMarca_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaMarcas))
        {
            return;
        }

        using var form = new MarcasForm(_marcaService);
        form.ShowDialog(this);
        await CargarMarcasAsync();
    }

    private async void CmbVariedad_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaVariedades))
        {
            return;
        }

        using var form = new VariedadesForm(_variedadService);
        form.ShowDialog(this);
        await CargarVariedadesAsync();
    }

    private async void CmbPesoEstandar_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaPesosEstandar))
        {
            return;
        }

        using var form = new PesosEstandarForm(_pesoEstandarService);
        form.ShowDialog(this);
        await CargarPesosEstandarAsync();
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
        var confirmar = XtraMessageBox.Show(this, "¿Guardar los cambios de este producto terminado?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        var datos = new ProductoTerminadoDto(
            _productoExistente.Id,
            _productoExistente.CodigoSap,
            _productoExistente.DescripcionSap,
            _productoExistente.DescripcionExtranjeraSap,
            _productoExistente.Activo,
            NuloSiVacio(_txtCodigoUpc.Text),
            NuloSiVacio(_txtCodigoPlu.Text),
            NuloSiVacio(_txtCodigoGtin.Text),
            _cmbMateriaPrima.EditValue as string,
            _cmbCategoria.EditValue as int?,
            _cmbTipoProducto.EditValue as int?,
            _cmbCalibreApeam.EditValue as int?,
            NuloSiVacio(_txtCalibreCodigoExterno.Text),
            _cmbMercadoDestino.EditValue as int?,
            _cmbMarca.EditValue as int?,
            _cmbVariedad.EditValue as int?,
            _cmbPesoEstandar.EditValue as int?,
            decimal.TryParse(_txtPesoNeto.Text, out var pesoNeto) ? pesoNeto : null,
            decimal.TryParse(_txtPesoPromedio.Text, out var pesoPromedio) ? pesoPromedio : null,
            (int)_spnCajasPorPallet.Value,
            _productoExistente.FechaCreacion);

        try
        {
            await _productoTerminadoService.ActualizarAsync(datos);

            DialogResult = DialogResult.OK;
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
        catch (Exception ex)
        {
            XtraMessageBox.Show(this, $"No se pudo guardar el producto terminado.\n\n{ex.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string? NuloSiVacio(string valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
