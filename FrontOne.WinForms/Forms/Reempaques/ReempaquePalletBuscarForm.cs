using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Reempaques;

public enum ReempaquePalletBuscarModo
{
    // Pallets candidatos a desarmarse: ya armados (Bloqueado), no Neutros, no reempacados antes,
    // no reservados en otro folio abierto (Produccion.sp_Reempaque_ObtenerPalletsOrigenDisponibles).
    Origen,

    // Pallets candidatos a recibir cajas: Vacío o Incompleto, no Neutros, no bloqueados, excluyendo
    // los que son origen de este mismo folio (Produccion.sp_Reempaque_ObtenerPalletsDestinoDisponibles).
    Destino,
}

// Buscador de pallets del Reempaque, en modo Origen o Destino — mismo diálogo, cambia solo contra
// qué SP consulta y qué columna extra muestra (Cajas Objetivo, solo en Destino). Carga un TOP 100
// al abrir, filtra por folio al Buscar.
public partial class ReempaquePalletBuscarForm : XtraForm
{
    private readonly ReempaqueService _reempaqueService = null!;
    private readonly ReempaquePalletBuscarModo _modo;
    private readonly int _reempaqueId;

    public int? PalletIdSeleccionado { get; private set; }

    public ReempaquePalletBuscarForm()
    {
        InitializeComponent();
    }

    public ReempaquePalletBuscarForm(ReempaqueService reempaqueService, ReempaquePalletBuscarModo modo, int reempaqueId)
        : this()
    {
        _reempaqueService = reempaqueService;
        _modo = modo;
        _reempaqueId = reempaqueId;

        Text = modo == ReempaquePalletBuscarModo.Origen ? "Buscar Pallet Origen" : "Buscar Pallet Destino";

        Load += async (_, _) => await BuscarAsync(null);
        _grid.SizeChanged += (_, _) => { if (_gridView.Columns.Count > 0) _gridView.BestFitColumns(); };
    }

    private async Task BuscarAsync(string? folio)
    {
        var pallets = _modo == ReempaquePalletBuscarModo.Origen
            ? await _reempaqueService.ObtenerPalletsOrigenDisponiblesAsync(folio)
            : await _reempaqueService.ObtenerPalletsDestinoDisponiblesAsync(_reempaqueId, folio);

        _grid.DataSource = pallets.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["Id"] is { } colId)
        {
            colId.Visible = false;
        }

        if (_gridView.Columns["Folio"] is { } colFolio)
        {
            colFolio.Caption = "No. de Pallet";
        }

        if (_gridView.Columns["FechaCreacion"] is { } colFecha)
        {
            colFecha.Caption = "Fecha";
            colFecha.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["TotalCajas"] is { } colCajas)
        {
            colCajas.Caption = "Cajas";
        }

        if (_gridView.Columns["TotalKilogramos"] is { } colKilos)
        {
            colKilos.Caption = "Kilogramos";
            colKilos.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["ProductoDescripcion"] is { } colProducto)
        {
            colProducto.Caption = "Producto";
        }

        // Cajas Objetivo/EsMixto/Estatus solo aportan en modo Destino — en Origen todos los
        // candidatos ya están Bloqueados (Estatus 5), no hace falta mostrarlo.
        var esDestino = _modo == ReempaquePalletBuscarModo.Destino;

        if (_gridView.Columns["CajasObjetivo"] is { } colObjetivo)
        {
            colObjetivo.Caption = "Cajas Objetivo";
            colObjetivo.Visible = esDestino;
        }

        if (_gridView.Columns["EsMixto"] is { } colMixto)
        {
            colMixto.Visible = false;
        }

        if (_gridView.Columns["Estatus"] is { } colEstatus)
        {
            colEstatus.Visible = false;
        }

        _gridView.BestFitColumns();
    }

    private async void BtnBuscar_Click(object? sender, EventArgs e) => await BuscarAsync(_txtFolio.Text);

    private async void TxtFolio_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            await BuscarAsync(_txtFolio.Text);
        }
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => BtnSeleccionar_Click(sender, EventArgs.Empty);

    private void BtnSeleccionar_Click(object? sender, EventArgs e)
    {
        if (_gridView.GetFocusedRow() is not ReempaquePalletDisponibleDto fila)
        {
            XtraMessageBox.Show(this, "Selecciona un pallet.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        PalletIdSeleccionado = fila.Id;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
