using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class BuscarListaPrecioFrutaForm : XtraForm
{
    private const string EtiquetaGeneral = "General";

    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;

    // Si viene con valor, el buscador solo muestra vigencias generales o del productor
    // indicado, dentro del rango de fechas (usado desde Acuerdo de Corte). Sin filtro,
    // muestra todas las vigencias guardadas (uso normal desde Lista de Precio Fruta).
    private (int ProductorId, DateTime FechaInicio, DateTime FechaFin)? _filtro;

    public VigenciaListaPrecioFrutaDto? VigenciaSeleccionada { get; private set; }

    public BuscarListaPrecioFrutaForm()
    {
        InitializeComponent();
    }

    public BuscarListaPrecioFrutaForm(ListaPrecioFrutaService listaPrecioFrutaService)
        : this()
    {
        _listaPrecioFrutaService = listaPrecioFrutaService;

        Load += async (_, _) => await CargarDatosAsync();
        _gridView.DoubleClick += (_, _) => Seleccionar();
    }

    public BuscarListaPrecioFrutaForm(ListaPrecioFrutaService listaPrecioFrutaService, int productorId, DateTime fechaInicio, DateTime fechaFin)
        : this(listaPrecioFrutaService)
    {
        _filtro = (productorId, fechaInicio, fechaFin);
    }

    private async Task CargarDatosAsync()
    {
        var vigencias = _filtro is { } filtro
            ? await _listaPrecioFrutaService.ObtenerFechasPorProductorYRangoAsync(filtro.ProductorId, filtro.FechaInicio, filtro.FechaFin)
            : await _listaPrecioFrutaService.ObtenerFechasAsync();

        // Cada fecha puede tener una lista general y varias especiales por productor —
        // se muestran como filas separadas para que el usuario elija cuál abrir.
        _grid.DataSource = vigencias
            .Select(v => v with { ProductorNombre = v.ProductorNombre ?? EtiquetaGeneral })
            .ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["ProductorId"] is { } colProductorId)
        {
            colProductorId.Visible = false;
        }

        if (_gridView.Columns["ProductorNombre"] is { } colProductor)
        {
            colProductor.Caption = "Productor";
        }
    }

    private void BtnSeleccionar_Click(object? sender, EventArgs e) => Seleccionar();

    private void Seleccionar()
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una fecha de la lista.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        VigenciaSeleccionada = seleccionado;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private VigenciaListaPrecioFrutaDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as VigenciaListaPrecioFrutaDto;
}
