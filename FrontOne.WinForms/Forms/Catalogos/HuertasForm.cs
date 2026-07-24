using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class HuertasForm : XtraForm
{
    private readonly HuertaService _huertaService = null!;

    private IReadOnlyList<HuertaBusquedaDto> _resultados = [];

    public HuertaBusquedaDto? HuertaSeleccionada { get; private set; }

    private record HuertaGridRow(int Id, string Nombre, string Productor, string RegistroSagarpa, string Estatus);

    public HuertasForm()
    {
        InitializeComponent();
    }

    public HuertasForm(HuertaService huertaService, ProductorService productorService)
        : this()
    {
        _huertaService = huertaService;
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

    private async void HuertasForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _resultados = await _huertaService.ObtenerTop100Async();
            MostrarResultados("FrontOne - Buscar huerta (100 más recientes — refina la búsqueda)");
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
            _resultados = await _huertaService.BuscarAsync(filtro);

            var texto = _resultados.Count == 500
                ? "FrontOne - Buscar huerta (mostrando los primeros 500 — refina la búsqueda)"
                : $"FrontOne - Buscar huerta ({_resultados.Count} resultados)";
            MostrarResultados(texto);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void MostrarResultados(string textoFormulario)
    {
        var filas = _resultados
            .Select(h => new HuertaGridRow(h.Id, h.Nombre, h.NombreProductor, h.RegistroSagarpa ?? "-", h.Activo ? "Activa" : "Baja"))
            .ToList();

        _grid.DataSource = filas;
        Text = textoFormulario;
    }

    private void BtnSeleccionar_Click(object? sender, EventArgs e) => Seleccionar();

    private void GridView_DoubleClick(object? sender, EventArgs e) => Seleccionar();

    private void Seleccionar()
    {
        if (_gridView.GetFocusedRow() is not HuertaGridRow fila)
        {
            XtraMessageBox.Show(this, "Selecciona una huerta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        HuertaSeleccionada = _resultados.FirstOrDefault(h => h.Id == fila.Id);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
