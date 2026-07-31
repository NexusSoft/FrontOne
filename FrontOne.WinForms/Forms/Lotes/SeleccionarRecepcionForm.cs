using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Lotes;

public partial class SeleccionarRecepcionForm : XtraForm
{
    private readonly LoteService _loteService = null!;
    private readonly int? _huertaId;
    private readonly int? _acuerdoCorteId;
    private readonly string? _pagarCorteACardCode;

    private IReadOnlyList<RecepcionDisponibleParaLoteDto> _resultados = [];

    public RecepcionDisponibleParaLoteDto? RecepcionSeleccionada { get; private set; }

    public SeleccionarRecepcionForm()
    {
        InitializeComponent();
    }

    // huertaId/acuerdoCorteId/pagarCorteACardCode vienen NULL si el Lote todavía no tiene
    // líneas (se muestran todas las Recepciones disponibles); si ya tiene al menos una línea,
    // se filtra en SQL a solo las compatibles (misma Huerta/Acuerdo/Proveedor).
    public SeleccionarRecepcionForm(LoteService loteService, int? huertaId, int? acuerdoCorteId, string? pagarCorteACardCode)
        : this()
    {
        _loteService = loteService;
        _huertaId = huertaId;
        _acuerdoCorteId = acuerdoCorteId;
        _pagarCorteACardCode = pagarCorteACardCode;
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

    private async void SeleccionarRecepcionForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _resultados = await _loteService.ObtenerRecepcionesDisponiblesTop100Async(_huertaId, _acuerdoCorteId, _pagarCorteACardCode);
            MostrarResultados("FrontOne - Seleccionar Recepción (100 más recientes — refina la búsqueda)");
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
            _resultados = await _loteService.BuscarRecepcionesDisponiblesAsync(filtro, _huertaId, _acuerdoCorteId, _pagarCorteACardCode);

            var textoCompleto = _resultados.Count == 500
                ? "FrontOne - Seleccionar Recepción (mostrando las primeras 500 — refina la búsqueda)"
                : $"FrontOne - Seleccionar Recepción ({_resultados.Count} resultados)";
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
            XtraMessageBox.Show(this, "Selecciona una recepción.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        RecepcionSeleccionada = seleccionado;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private RecepcionDisponibleParaLoteDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as RecepcionDisponibleParaLoteDto;
}
