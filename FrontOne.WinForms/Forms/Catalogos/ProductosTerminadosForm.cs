using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class ProductosTerminadosForm : XtraForm
{
    private readonly ProductoTerminadoService _productoTerminadoService = null!;
    private readonly CategoriaService _categoriaService = null!;
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly MarcaService _marcaService = null!;
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly PaisService _paisService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly SessionContext _sessionContext = null!;

    public ProductosTerminadosForm()
    {
        InitializeComponent();
        ConfigurarFormatoCondicional();
    }

    public ProductosTerminadosForm(
        ProductoTerminadoService productoTerminadoService,
        CategoriaService categoriaService,
        TipoProductoService tipoProductoService,
        CalibreApeamService calibreApeamService,
        MarcaService marcaService,
        PesoEstandarService pesoEstandarService,
        PaisService paisService,
        VariedadService variedadService,
        SessionContext sessionContext)
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
    }

    // Pinta de rojo suave la fila completa cuando el producto ya no está vigente en SAP (Activo = False).
    // Primer uso de GridFormatRule/FormatConditionRuleExpression en el proyecto.
    private void ConfigurarFormatoCondicional()
    {
        var regla = new DevExpress.XtraGrid.StyleFormatCondition
        {
            Expression = "[Activo] = False",
            ApplyToRow = true,
        };
        regla.Appearance.BackColor = System.Drawing.Color.MistyRose;
        regla.Appearance.Options.UseBackColor = true;
        _gridView.FormatConditions.Add(regla);
    }

    private async void ProductosTerminadosForm_Load(object? sender, EventArgs e)
    {
        try
        {
            // Sincronización silenciosa al abrir: mantiene el catálogo al día sin molestar
            // al usuario, salvo que haya errores (esos sí se avisan).
            var resultado = await _productoTerminadoService.SincronizarConSapAsync();
            if (resultado.Errores > 0)
            {
                XtraMessageBox.Show(this,
                    $"La sincronización con SAP terminó con {resultado.Errores} error(es). Revisa el catálogo.",
                    "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (SapException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        await CargarTop100Async();
    }

    private async Task CargarTop100Async()
    {
        try
        {
            var productos = await _productoTerminadoService.ObtenerTop100Async();
            _grid.DataSource = productos.ToList();
            Text = "FrontOne - Productos Terminados (100 más recientes — refina la búsqueda)";
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void TxtBuscar_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            _ = BuscarAsync();
        }
    }

    private async void BtnBuscar_Click(object? sender, EventArgs e) => await BuscarAsync();

    private async Task BuscarAsync()
    {
        var filtro = _txtBuscar.Text.Trim();
        if (filtro.Length < 2)
        {
            XtraMessageBox.Show(this, "Escribe al menos 2 caracteres para buscar.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var productos = await _productoTerminadoService.BuscarAsync(filtro);
            _grid.DataSource = productos.ToList();
            Text = productos.Count == 500
                ? "FrontOne - Productos Terminados (mostrando los primeros 500 — refina la búsqueda)"
                : $"FrontOne - Productos Terminados ({productos.Count} resultados)";
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnSincronizar_Click(object? sender, EventArgs e)
    {
        try
        {
            var resultado = await _productoTerminadoService.SincronizarConSapAsync();
            XtraMessageBox.Show(this,
                $"Sincronización con SAP terminada.\n\nNuevos: {resultado.Nuevos}\nActualizados: {resultado.Actualizados}\n" +
                $"Reactivados: {resultado.Reactivados}\nDesactivados: {resultado.Desactivados}\nErrores: {resultado.Errores}",
                "FrontOne", MessageBoxButtons.OK, resultado.Errores > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            await CargarTop100Async();
        }
        catch (SapException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un producto terminado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new ProductoTerminadoEditarForm(
            _productoTerminadoService, _categoriaService, _tipoProductoService, _calibreApeamService,
            _marcaService, _pesoEstandarService, _paisService, _variedadService, _sessionContext, seleccionado);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarTop100Async();
        }
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => BtnEditar_Click(sender, e);

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private ProductoTerminadoDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as ProductoTerminadoDto;
}
