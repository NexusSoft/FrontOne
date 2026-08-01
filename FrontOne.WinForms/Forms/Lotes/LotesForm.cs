using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Lotes;

public partial class LotesForm : XtraForm
{
    private readonly LoteService _loteService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;

    private LoteEditarForm? _loteEditarForm;

    public LotesForm()
    {
        InitializeComponent();
    }

    public LotesForm(LoteService loteService, LineaProduccionService lineaProduccionService)
        : this()
    {
        _loteService = loteService;
        _lineaProduccionService = lineaProduccionService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var lotes = await _loteService.ObtenerAsync();
        _grid.DataSource = lotes.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "Id", "Observaciones", "Personalizado", "LineaProduccionId", "Estatus" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["LineaProduccionNombre"] is { } colLinea)
        {
            colLinea.Caption = "Línea de Producción";
        }

        if (_gridView.Columns["HuertaNombre"] is { } colHuerta)
        {
            colHuerta.Caption = "Huerta";
        }

        if (_gridView.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.Caption = "Peso Neto";
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["PorcentajeMateriaSeca"] is { } colMateriaSeca)
        {
            colMateriaSeca.Caption = "Materia Seca";
            colMateriaSeca.DisplayFormat.FormatType = FormatType.Numeric;
            colMateriaSeca.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["ProductorNombre"] is { } colProductor)
        {
            colProductor.Caption = "Productor";
        }

        _gridView.BestFitColumns();

        // Mismo criterio que RecepcionesFrutaForm: el sobrante de ancho se lo damos a la última
        // columna (Productor) en vez de dejarlo en blanco.
        var anchoColumnas = _gridView.Columns.Cast<DevExpress.XtraGrid.Columns.GridColumn>()
            .Where(c => c.Visible)
            .Sum(c => c.Width);
        var anchoDisponible = _grid.Width - SystemInformation.VerticalScrollBarWidth;
        if (_gridView.Columns["ProductorNombre"] is { } colProductorFill && anchoDisponible > anchoColumnas)
        {
            colProductorFill.Width += anchoDisponible - anchoColumnas;
        }
    }

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        AbrirEditarForm(null);
    }

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un lote.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(LoteDto? loteExistente)
    {
        if (_loteEditarForm is { IsDisposed: false })
        {
            if (_loteEditarForm.WindowState == FormWindowState.Minimized)
            {
                _loteEditarForm.WindowState = FormWindowState.Normal;
            }

            _loteEditarForm.Activate();
            return;
        }

        _loteEditarForm = new LoteEditarForm(_loteService, _lineaProduccionService, loteExistente);
        _loteEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _loteEditarForm.FormClosed += (_, _) => _loteEditarForm = null;
        _loteEditarForm.Show(this);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un lote.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el lote folio '{seleccionado.Folio}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _loteService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private LoteDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as LoteDto;
}
