using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.Constants;

namespace FrontOne.WinForms.Forms.Acopio;

// Solo lectura: muestra los productos y precios de una vigencia de Lista de Precio Fruta,
// para consultarla desde Acuerdo de Corte sin salir del flujo ni poder editarla ahí.
public partial class VerListaPrecioFrutaForm : XtraForm
{
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly DateTime _fecha;
    private readonly int? _productorId;
    private readonly int? _listaSeleccionada;

    public VerListaPrecioFrutaForm()
    {
        InitializeComponent();
    }

    public VerListaPrecioFrutaForm(
        ListaPrecioFrutaService listaPrecioFrutaService,
        DateTime fecha,
        int? productorId,
        string? productorNombre,
        int? listaSeleccionada)
        : this()
    {
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _fecha = fecha;
        _productorId = productorId;
        _listaSeleccionada = listaSeleccionada;

        _lblTitulo.Text = $"Lista del {fecha:dd/MM/yyyy} - {(string.IsNullOrEmpty(productorNombre) ? "General" : productorNombre)}";

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var filas = await _listaPrecioFrutaService.ObtenerPorFechaAsync(_fecha, _productorId, null);
        _grid.DataSource = filas.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "Id", "FechaInicio", "FechaFin", "ProductorId", "VariedadId", "Activo" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["ItemName"] is { } colItemName)
        {
            colItemName.Caption = "Producto";
        }

        var nombresLista = new[] { "Lista1", "Lista2", "Lista3" };
        for (var i = 0; i < nombresLista.Length; i++)
        {
            if (_gridView.Columns[nombresLista[i]] is not { } columna)
            {
                continue;
            }

            var numero = i + 1;
            var nombre = ListasPrecioFruta.Nombres[i];
            var esLaSeleccionada = _listaSeleccionada == numero;
            columna.Caption = esLaSeleccionada ? $"{nombre} (elegida)" : nombre;
            columna.DisplayFormat.FormatType = FormatType.Numeric;
            columna.DisplayFormat.FormatString = "n2";
        }

        _gridView.BestFitColumns();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
