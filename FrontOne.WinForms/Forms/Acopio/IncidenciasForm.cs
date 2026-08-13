using System.IO;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Constants;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Acopio;

// Grid principal del módulo Incidencias: lista TODAS las Órdenes de Corte del rango de fecha
// elegido (no una tabla Incidencia aparte) con la columna Revisada — abrir cualquier fila abre el
// mismo formulario de captura, tenga o no Incidencia ya capturada.
public partial class IncidenciasForm : XtraForm
{
    private const string CodigoReporte = "Incidencias";

    private readonly IncidenciaService _incidenciaService = null!;
    private readonly SupervisorHuertaService _supervisorHuertaService = null!;
    private readonly ReportePlantillaService _reportePlantillaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SessionContext _sessionContext = null!;

    public IncidenciasForm()
    {
        InitializeComponent();
    }

    public IncidenciasForm(
        IncidenciaService incidenciaService,
        SupervisorHuertaService supervisorHuertaService,
        ReportePlantillaService reportePlantillaService,
        EmpresaConfiguracionService empresaConfiguracionService,
        SessionContext sessionContext)
        : this()
    {
        _incidenciaService = incidenciaService;
        _supervisorHuertaService = supervisorHuertaService;
        _reportePlantillaService = reportePlantillaService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _sessionContext = sessionContext;

        _dtDesde.EditValue = DateTime.Today;
        _dtHasta.EditValue = DateTime.Today;

        Shown += async (_, _) => await CargarDatosAsync();
        _grid.SizeChanged += (_, _) => _gridView.BestFitColumns();
    }

    private async Task CargarDatosAsync()
    {
        var (fechaDesde, fechaHasta) = ObtenerRangoFecha();
        if (fechaDesde is null || fechaHasta is null)
        {
            XtraMessageBox.Show(this, "Selecciona el rango de fecha (Desde/Hasta).", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var ordenes = await _incidenciaService.ObtenerOrdenesConEstatusAsync(fechaDesde.Value, fechaHasta.Value);
            _grid.DataSource = ordenes.ToList();
            ConfigurarColumnas();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private (DateTime? Desde, DateTime? Hasta) ObtenerRangoFecha()
        => (_dtDesde.EditValue as DateTime?, _dtHasta.EditValue as DateTime?);

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "OrdenCorteId", "IncidenciaId" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Revisada"] is { } colRevisada)
        {
            colRevisada.Caption = "Revisada";
        }

        if (_gridView.Columns["HuertaNombre"] is { } colHuerta)
        {
            colHuerta.Caption = "Huerta";
        }

        if (_gridView.Columns["ProductorNombre"] is { } colProductor)
        {
            colProductor.Caption = "Productor";
        }

        if (_gridView.Columns["SupervisorNombre"] is { } colSupervisor)
        {
            colSupervisor.Caption = "Supervisor";
        }

        var ordenColumnas = new[]
        {
            "Revisada", "Folio", "Fecha", "HuertaNombre", "ProductorNombre", "Acopiador",
            "SupervisorNombre", "Cajas", "Incidencias", "Ajuste",
        };
        for (var i = 0; i < ordenColumnas.Length; i++)
        {
            if (_gridView.Columns[ordenColumnas[i]] is { } columnaOrdenada)
            {
                columnaOrdenada.VisibleIndex = i;
            }
        }

        _gridView.BestFitColumns();
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e) => await CargarDatosAsync();

    private void GridView_DoubleClick(object? sender, EventArgs e)
    {
        var punto = _grid.PointToClient(Cursor.Position);
        var info = _gridView.CalcHitInfo(punto);
        if (!info.InRowCell)
        {
            return;
        }

        BtnEditar_Click(sender, EventArgs.Empty);
    }

    private async void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionada = ObtenerSeleccionada();
        if (seleccionada is null)
        {
            XtraMessageBox.Show(this, "Selecciona una Orden de Corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new IncidenciaEditarForm(_incidenciaService, _supervisorHuertaService, seleccionada.OrdenCorteId);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnGenerarPdf_Click(object? sender, EventArgs e)
    {
        var (fechaDesde, fechaHasta) = ObtenerRangoFecha();
        if (fechaDesde is null || fechaHasta is null)
        {
            XtraMessageBox.Show(this, "Selecciona el rango de fecha (Desde/Hasta).", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!_sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.VistaPrevia))
        {
            XtraMessageBox.Show(this, "No tienes permiso para ver este reporte.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var datos = await _incidenciaService.ObtenerParaReporteAsync(fechaDesde.Value, fechaHasta.Value);
            if (datos.Count == 0)
            {
                XtraMessageBox.Show(this, "No hay Incidencias capturadas en el rango de fecha elegido.", "FrontOne",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var empresa = await _empresaConfiguracionService.ObtenerAsync();

            var reporte = await CrearReporteAsync();
            reporte.CargarDatos(fechaDesde.Value, fechaHasta.Value, datos, empresa);

            using var visor = new Sistema.VisorReporteForm(reporte, CodigoReporte, _sessionContext);
            visor.ShowDialog(this);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Si el usuario personalizó el layout en el Diseñador de Reportes, se carga esa plantilla en
    // vez del diseño default — mismo patrón que PalletEditarForm.CrearReporteAsync.
    private async Task<ReporteIncidencias> CrearReporteAsync()
    {
        var reporte = new ReporteIncidencias();

        var plantilla = await _reportePlantillaService.ObtenerPorCodigoAsync(CodigoReporte);
        if (!string.IsNullOrWhiteSpace(plantilla?.DefinicionXml))
        {
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(plantilla.DefinicionXml));
            reporte.LoadLayoutFromXml(stream);
        }

        return reporte;
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private IncidenciaListadoDto? ObtenerSeleccionada()
        => _gridView.GetFocusedRow() as IncidenciaListadoDto;
}
