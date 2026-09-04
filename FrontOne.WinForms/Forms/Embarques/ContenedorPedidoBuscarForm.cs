using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Embarques;

// Buscador de pedidos SAP abiertos (TOP 100, ver PedidoService/ISapPedidoRepository.ObtenerAbiertosAsync)
// para elegir con cuál surtir un Contenedor nuevo. Filtro por texto es local (SAP ya limita a 100).
public partial class ContenedorPedidoBuscarForm : XtraForm
{
    private readonly ContenedorService _contenedorService = null!;
    private List<SapPedidoDto> _pedidos = new();

    public SapPedidoDto? PedidoSeleccionado { get; private set; }

    public ContenedorPedidoBuscarForm()
    {
        InitializeComponent();
    }

    public ContenedorPedidoBuscarForm(ContenedorService contenedorService) : this()
    {
        _contenedorService = contenedorService;

        Load += async (_, _) => await CargarAsync();
        _grid.SizeChanged += (_, _) => { if (_gridView.Columns.Count > 0) _gridView.BestFitColumns(); };
    }

    private async Task CargarAsync()
    {
        _pedidos = (await _contenedorService.ObtenerPedidosAbiertosAsync()).ToList();
        AplicarFiltro(null);
    }

    private void AplicarFiltro(string? texto)
    {
        var filtrados = string.IsNullOrWhiteSpace(texto)
            ? _pedidos
            : _pedidos.Where(p =>
                p.DocNum.ToString().Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.CardName.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.CardCode.Contains(texto, StringComparison.OrdinalIgnoreCase)).ToList();

        _grid.DataSource = filtrados;
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "DocEntry", "DocCurrency", "Comentarios" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["DocNum"] is { } colDocNum)
        {
            colDocNum.Caption = "No. Pedido";
        }

        if (_gridView.Columns["CardCode"] is { } colCodigo)
        {
            colCodigo.Caption = "Código Cliente";
        }

        if (_gridView.Columns["CardName"] is { } colNombre)
        {
            colNombre.Caption = "Cliente";
        }

        if (_gridView.Columns["DocDate"] is { } colFecha)
        {
            colFecha.Caption = "Fecha";
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["DocDueDate"] is { } colVence)
        {
            colVence.Caption = "Fecha de Entrega";
            colVence.DisplayFormat.FormatType = FormatType.DateTime;
            colVence.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["DocTotal"] is { } colTotal)
        {
            colTotal.DisplayFormat.FormatType = FormatType.Numeric;
            colTotal.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["FolioFronterra"] is { } colFolio)
        {
            colFolio.Caption = "Folio Fronterra";
        }

        if (_gridView.Columns["Estatus"] is { } colEstatus)
        {
            colEstatus.Caption = "Estatus";
        }

        var orden = new[] { "DocNum", "CardCode", "CardName", "DocDate", "DocDueDate", "FolioFronterra", "DocTotal", "Estatus" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridView.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridView.BestFitColumns();
    }

    private void BtnBuscar_Click(object? sender, EventArgs e) => AplicarFiltro(_txtFiltro.Text);

    private void TxtFiltro_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            AplicarFiltro(_txtFiltro.Text);
        }
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => BtnSeleccionar_Click(sender, EventArgs.Empty);

    private void BtnSeleccionar_Click(object? sender, EventArgs e)
    {
        if (_gridView.GetFocusedRow() is not SapPedidoDto fila)
        {
            XtraMessageBox.Show(this, "Selecciona un pedido.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        PedidoSeleccionado = fila;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
