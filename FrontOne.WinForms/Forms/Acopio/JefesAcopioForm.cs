using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

// Buscador embebido: se abre desde el botón de búsqueda del campo Nombre en
// JefeAcopioEditarForm, mismo patrón que ProductoresForm/HuertasForm.
public partial class JefesAcopioForm : XtraForm
{
    private readonly JefeAcopioService _jefeAcopioService = null!;
    private readonly EstadoService _estadoService = null!;

    private IReadOnlyList<JefeAcopioDto> _resultados = [];

    public JefeAcopioDto? JefeAcopioSeleccionado { get; private set; }

    private record JefeAcopioGridRow(int Id, string Clave, string Nombre, string Estado, string Estatus);

    public JefesAcopioForm()
    {
        InitializeComponent();
    }

    public JefesAcopioForm(JefeAcopioService jefeAcopioService, EstadoService estadoService)
        : this()
    {
        _jefeAcopioService = jefeAcopioService;
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

    private async void JefesAcopioForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _resultados = await _jefeAcopioService.ObtenerTop100Async();
            await MostrarResultadosAsync("FrontOne - Buscar jefe de acopio (100 más recientes — refina la búsqueda)");
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
            _resultados = await _jefeAcopioService.BuscarAsync(filtro);

            var texto = _resultados.Count == 500
                ? "FrontOne - Buscar jefe de acopio (mostrando los primeros 500 — refina la búsqueda)"
                : $"FrontOne - Buscar jefe de acopio ({_resultados.Count} resultados)";
            await MostrarResultadosAsync(texto);
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
            .Select(j => new JefeAcopioGridRow(
                j.Id,
                j.Clave,
                j.Nombre,
                j.EstadoId is not null ? estadosPorId.GetValueOrDefault(j.EstadoId.Value, "-") : "-",
                j.Activo ? "Activo" : "Baja"))
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
            XtraMessageBox.Show(this, "Selecciona un jefe de acopio.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        JefeAcopioSeleccionado = seleccionado;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private JefeAcopioDto? ObtenerSeleccionado()
    {
        if (_gridView.GetFocusedRow() is not JefeAcopioGridRow fila)
        {
            return null;
        }

        return _resultados.FirstOrDefault(j => j.Id == fila.Id);
    }
}
