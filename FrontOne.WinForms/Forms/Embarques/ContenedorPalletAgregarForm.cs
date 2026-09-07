using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Embarques;

// Un solo diálogo: buscador de pallets disponibles (TOP 100 al abrir) + captura de Posición y
// Temperatura del pallet dentro del contenedor.
public partial class ContenedorPalletAgregarForm : XtraForm
{
    private readonly ContenedorService _contenedorService = null!;
    private readonly IReadOnlyList<string> _codigosSapPendientes = Array.Empty<string>();
    private readonly IReadOnlyList<int> _posicionesOcupadas = Array.Empty<int>();

    // Un pallet + su Posición asignada, ya resuelto el corrimiento contra posiciones ocupadas
    // (ver BtnGuardar_Click) — la Temperatura capturada aplica igual a todos los seleccionados.
    public IReadOnlyList<(int PalletId, int Posicion)> PalletsSeleccionados { get; private set; } = Array.Empty<(int, int)>();
    public decimal? Temperatura { get; private set; }

    public ContenedorPalletAgregarForm()
    {
        InitializeComponent();
    }

    // codigosSapPendientes: productos del pedido todavía no surtidos al 100% — el buscador solo
    // muestra pallets de esos productos (los Mixtos siempre se muestran, ver el SP).
    public ContenedorPalletAgregarForm(ContenedorService contenedorService, IReadOnlyList<string> codigosSapPendientes, IReadOnlyList<int> posicionesOcupadas) : this()
    {
        _contenedorService = contenedorService;
        _codigosSapPendientes = codigosSapPendientes;
        _posicionesOcupadas = posicionesOcupadas;

        Load += async (_, _) => await BuscarAsync(null);
        _grid.SizeChanged += (_, _) => { if (_gridView.Columns.Count > 0) _gridView.BestFitColumns(); };
    }

    private async Task BuscarAsync(string? folio)
    {
        var pallets = await _contenedorService.ObtenerPalletsDisponiblesAsync(folio, _codigosSapPendientes);
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
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["Estatus"] is { } colEstatus)
        {
            colEstatus.Caption = "Estatus";
        }

        if (_gridView.Columns["TotalCajas"] is { } colCajas)
        {
            colCajas.Caption = "Cajas";
        }

        if (_gridView.Columns["TotalKilogramos"] is { } colKilos)
        {
            colKilos.Caption = "Kilogramos";
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["ProductoDescripcion"] is { } colProducto)
        {
            colProducto.Caption = "Producto";
        }

        if (_gridView.Columns["EsMixto"] is { } colMixto)
        {
            colMixto.Visible = false;
        }

        _gridView.CustomColumnDisplayText -= GridView_CustomColumnDisplayText;
        _gridView.CustomColumnDisplayText += GridView_CustomColumnDisplayText;

        var orden = new[] { "Folio", "FechaCreacion", "Estatus", "ProductoDescripcion", "TotalCajas", "TotalKilogramos" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridView.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridView.BestFitColumns();
    }

    private void GridView_CustomColumnDisplayText(object? sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "Estatus" && e.Value is byte estatus)
        {
            e.DisplayText = Pallets.PalletsForm.NombreEstatus(estatus);
        }
    }

    private async void BtnBuscar_Click(object? sender, EventArgs e) => await BuscarAsync(_txtFiltro.Text);

    private async void TxtFiltro_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            await BuscarAsync(_txtFiltro.Text);
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var filas = _gridView.GetSelectedRows()
            .Select(handle => _gridView.GetRow(handle))
            .OfType<PalletDisponibleEmbarqueDto>()
            .ToList();
        if (filas.Count == 0)
        {
            XtraMessageBox.Show(this, "Selecciona al menos un pallet.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_spnPosicion.EditValue is null)
        {
            XtraMessageBox.Show(this, "Captura la posición inicial.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Con varios pallets seleccionados, cada uno toma la siguiente posición libre a partir de
        // la capturada — se salta las ya ocupadas (incluidas las que se van asignando en este
        // mismo lote), en vez de exigir capturar una posición por pallet.
        var posicion = Convert.ToInt32(_spnPosicion.EditValue);
        var ocupadas = new HashSet<int>(_posicionesOcupadas);
        var seleccion = new List<(int PalletId, int Posicion)>();
        foreach (var fila in filas)
        {
            while (ocupadas.Contains(posicion))
            {
                posicion++;
            }

            seleccion.Add((fila.Id, posicion));
            ocupadas.Add(posicion);
            posicion++;
        }

        PalletsSeleccionados = seleccion;
        Temperatura = _spnTemperatura.EditValue is null ? null : Convert.ToDecimal(_spnTemperatura.EditValue);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
