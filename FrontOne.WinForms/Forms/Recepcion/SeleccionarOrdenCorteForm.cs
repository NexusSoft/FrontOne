using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Recepcion;

public partial class SeleccionarOrdenCorteForm : XtraForm
{
    private readonly RecepcionFrutaService _recepcionFrutaService = null!;

    private IReadOnlyList<OrdenCorteDisponibleDto> _resultados = [];

    public OrdenCorteDisponibleDto? OrdenSeleccionada { get; private set; }

    public SeleccionarOrdenCorteForm()
    {
        InitializeComponent();
    }

    public SeleccionarOrdenCorteForm(RecepcionFrutaService recepcionFrutaService)
        : this()
    {
        _recepcionFrutaService = recepcionFrutaService;
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

    private async void SeleccionarOrdenCorteForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _resultados = await _recepcionFrutaService.ObtenerOrdenesDisponiblesTop100Async();
            MostrarResultados("FrontOne - Seleccionar Orden de Corte (100 más recientes — refina la búsqueda)");
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
            _resultados = await _recepcionFrutaService.BuscarOrdenesDisponiblesAsync(filtro);

            var textoCompleto = _resultados.Count == 500
                ? "FrontOne - Seleccionar Orden de Corte (mostrando las primeras 500 — refina la búsqueda)"
                : $"FrontOne - Seleccionar Orden de Corte ({_resultados.Count} resultados)";
            MostrarResultados(textoCompleto);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void MostrarResultados(string textoFormulario)
    {
        _grid.DataSource = _resultados.ToList();
        Text = textoFormulario;
    }

    private void BtnSeleccionar_Click(object? sender, EventArgs e) => Seleccionar();

    private void GridView_DoubleClick(object? sender, EventArgs e) => Seleccionar();

    private void Seleccionar()
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una orden de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        OrdenSeleccionada = seleccionado;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private OrdenCorteDisponibleDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as OrdenCorteDisponibleDto;
}
