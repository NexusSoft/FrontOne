using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class ProductoresForm : XtraForm
{
    private readonly ProductorService _productorService = null!;
    private readonly EstadoService _estadoService = null!;

    private IReadOnlyList<ProductorDto> _resultados = [];

    public ProductorDto? ProductorSeleccionado { get; private set; }

    private record ProductorGridRow(int Id, string Clave, string NombreProductor, string Estado, string Estatus);

    public ProductoresForm()
    {
        InitializeComponent();
    }

    public ProductoresForm(ProductorService productorService, PaisService paisService, EstadoService estadoService, bool modoSeleccion = true)
        : this()
    {
        _productorService = productorService;
        _estadoService = estadoService;
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

    private async void ProductoresForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _resultados = await _productorService.ObtenerTop100Async();
            await MostrarResultadosAsync("FrontOne - Buscar productor (100 más recientes — refina la búsqueda)");
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

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
            _resultados = await _productorService.BuscarAsync(filtro);

            var textoCompleto = _resultados.Count == 500
                ? "FrontOne - Buscar productor (mostrando los primeros 500 — refina la búsqueda)"
                : $"FrontOne - Buscar productor ({_resultados.Count} resultados)";
            await MostrarResultadosAsync(textoCompleto);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task MostrarResultadosAsync(string textoFormulario)
    {
        var estados = await _estadoService.ObtenerAsync();
        var estadosPorId = estados.ToDictionary(e => e.Id, e => e.Nombre);

        var filas = _resultados
            .Select(p => new ProductorGridRow(
                p.Id,
                p.Clave,
                p.NombreProductor,
                p.EstadoId is not null ? estadosPorId.GetValueOrDefault(p.EstadoId.Value, "-") : "-",
                p.Activo ? "Activo" : "Baja"))
            .ToList();

        _grid.DataSource = filas;
        Text = textoFormulario;
    }

    private void BtnSeleccionar_Click(object? sender, EventArgs e) => Seleccionar();

    private void GridView_DoubleClick(object? sender, EventArgs e) => Seleccionar();

    private void Seleccionar()
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un productor.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        ProductorSeleccionado = seleccionado;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private ProductorDto? ObtenerSeleccionado()
    {
        if (_gridView.GetFocusedRow() is not ProductorGridRow fila)
        {
            return null;
        }

        return _resultados.FirstOrDefault(p => p.Id == fila.Id);
    }
}
