using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Acopio;

// Picker embebido de Estimación (folio de precio sugerido) — mismo patrón que HuertasForm: TOP
// 100 al abrir, búsqueda por texto (Folio o Huerta) con mínimo 2 caracteres.
public partial class BuscarEstimacionForm : XtraForm
{
    private readonly EstimacionService _estimacionService = null!;
    private readonly int? _huertaId;
    private readonly bool _soloAutorizadas;

    private IReadOnlyList<EstimacionBusquedaDto> _resultados = [];

    public EstimacionBusquedaDto? EstimacionSeleccionada { get; private set; }

    private record EstimacionGridRow(int Id, string Folio, DateTime Fecha, string Huerta, decimal PrecioSugerido, string Estatus);

    public BuscarEstimacionForm()
    {
        InitializeComponent();
    }

    // huertaId/soloAutorizadas restringen el picker cuando se abre desde OrdenCorteEditarForm
    // (solo estimaciones autorizadas de la huerta ya elegida) — EstimacionForm sigue llamando el
    // constructor sin estos parámetros para poder reabrir cualquier estimación.
    public BuscarEstimacionForm(EstimacionService estimacionService, int? huertaId = null, bool soloAutorizadas = false)
        : this()
    {
        _estimacionService = estimacionService;
        _huertaId = huertaId;
        _soloAutorizadas = soloAutorizadas;
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

    private async void BuscarEstimacionForm_Load(object? sender, EventArgs e)
    {
        _resultados = await _estimacionService.ObtenerTop100Async(_huertaId, _soloAutorizadas);
        MostrarResultados("FrontOne - Buscar estimación (100 más recientes — refina la búsqueda)");
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

        _resultados = await _estimacionService.BuscarAsync(filtro, _huertaId, _soloAutorizadas);

        var texto = _resultados.Count == 500
            ? "FrontOne - Buscar estimación (mostrando las primeras 500 — refina la búsqueda)"
            : $"FrontOne - Buscar estimación ({_resultados.Count} resultados)";
        MostrarResultados(texto);
    }

    private void MostrarResultados(string textoFormulario)
    {
        var filas = _resultados
            .Select(e => new EstimacionGridRow(e.Id, e.Folio, e.Fecha, e.HuertaNombre, e.PrecioSugerido,
                e.Cerrada ? "Cerrada" : (e.Autorizada ? "Autorizada" : "Pendiente de autorizar")))
            .ToList();

        _grid.DataSource = filas;
        Text = textoFormulario;
    }

    private void BtnSeleccionar_Click(object? sender, EventArgs e) => Seleccionar();

    private void GridView_DoubleClick(object? sender, EventArgs e) => Seleccionar();

    private void Seleccionar()
    {
        if (_gridView.GetFocusedRow() is not EstimacionGridRow fila)
        {
            XtraMessageBox.Show(this, "Selecciona una estimación.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        EstimacionSeleccionada = _resultados.FirstOrDefault(e => e.Id == fila.Id);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
