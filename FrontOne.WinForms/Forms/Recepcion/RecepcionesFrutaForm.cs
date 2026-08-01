using System.Drawing;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Constants;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Recepcion;

public partial class RecepcionesFrutaForm : XtraForm
{
    private const string CodigoReporte = "RecepcionFruta";

    private static readonly Image ImagenCandadoCerrado = CargarIconoCandado("candado_cerrado.png");
    private static readonly Image ImagenCandadoAbierto = CargarIconoCandado("candado_abierto.png");

    private readonly RecepcionFrutaService _recepcionFrutaService = null!;
    private readonly ReportePlantillaService _reportePlantillaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly SqlOptions _sqlOptions = null!;

    private RecepcionFrutaEditarForm? _recepcionFrutaEditarForm;

    public RecepcionesFrutaForm()
    {
        InitializeComponent();
    }

    public RecepcionesFrutaForm(
        RecepcionFrutaService recepcionFrutaService,
        ReportePlantillaService reportePlantillaService,
        EmpresaConfiguracionService empresaConfiguracionService,
        SessionContext sessionContext,
        SqlOptions sqlOptions)
        : this()
    {
        _recepcionFrutaService = recepcionFrutaService;
        _reportePlantillaService = reportePlantillaService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _sessionContext = sessionContext;
        _sqlOptions = sqlOptions;

        _btnVistaPrevia.Enabled = _sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.VistaPrevia);
        _btnDisenarReporte.Enabled = _sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.Diseno);

        _gridView.CustomDrawCell += GridView_CustomDrawCell;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private static Image CargarIconoCandado(string nombreArchivo)
    {
        using var stream = typeof(RecepcionesFrutaForm).Assembly
            .GetManifestResourceStream($"FrontOne.WinForms.Resources.Icons.{nombreArchivo}")!;
        return Image.FromStream(stream);
    }

    // La columna "Bloqueo" (EstaEnLote) no se muestra como texto/checkbox — se dibuja un
    // candado cerrado (rojo) o abierto (verde) para que se distinga de un vistazo en el listado.
    private void GridView_CustomDrawCell(object? sender, RowCellCustomDrawEventArgs e)
    {
        if (e.Column.FieldName != "EstaEnLote")
        {
            return;
        }

        e.Appearance.FillRectangle(e.Cache, e.Bounds);

        var estaEnLote = e.CellValue is true;
        var imagen = estaEnLote ? ImagenCandadoCerrado : ImagenCandadoAbierto;
        const int tamano = 16;
        var rect = new Rectangle(
            e.Bounds.Left + (e.Bounds.Width - tamano) / 2,
            e.Bounds.Top + (e.Bounds.Height - tamano) / 2,
            tamano, tamano);
        e.Cache.DrawImage(imagen, rect);
        e.Handled = true;
    }

    private async Task CargarDatosAsync()
    {
        var recepciones = await _recepcionFrutaService.ObtenerAsync();
        _grid.DataSource = recepciones.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[]
        {
            "Id", "Placas", "Observaciones", "PesoBruto", "PesoTara", "TaraCajas", "PesoMuestra",
            "PesoProductor", "CajasPorEntregar", "CajasEntregadas", "CajasCortadas", "CajasRecibidasVacias",
            "CajasDiferencia", "CamionDestarado", "TicketPesadaArchivo", "TicketPesadaNombreArchivo",
        })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["NoLote"] is { } colLote)
        {
            colLote.Caption = "No. de Lote";
        }

        if (_gridView.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["NumeroTicket"] is { } colTicket)
        {
            colTicket.Caption = "Ticket";
        }

        if (_gridView.Columns["CoprefBico"] is { } colCopref)
        {
            colCopref.Caption = "COPREF/BICO";
        }

        if (_gridView.Columns["Huertas"] is { } colHuertas)
        {
            colHuertas.Caption = "Huerta";
        }

        if (_gridView.Columns["PesoNeto"] is { } colPesoNeto)
        {
            colPesoNeto.Caption = "Peso Neto";
            colPesoNeto.DisplayFormat.FormatType = FormatType.Numeric;
            colPesoNeto.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["PorcentajeMateriaSeca"] is { } colMateriaSeca)
        {
            colMateriaSeca.Caption = "Materia Seca";
            colMateriaSeca.DisplayFormat.FormatType = FormatType.Numeric;
            colMateriaSeca.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["EstaEnLote"] is { } colBloqueo)
        {
            colBloqueo.Caption = "Bloqueo";
            colBloqueo.OptionsColumn.AllowEdit = false;
            colBloqueo.Width = 70;
        }

        _gridView.BestFitColumns();

        // BestFitColumns ajusta cada columna a su contenido real (regla dura del proyecto — no
        // se comprime para forzar que quepan, se usa scroll horizontal si hace falta), pero eso
        // deja espacio vacío a la derecha cuando el grid es más ancho que la suma de columnas.
        // Ese sobrante se lo damos a la última columna (Huerta) en vez de dejarlo en blanco.
        var anchoColumnas = _gridView.Columns.Cast<DevExpress.XtraGrid.Columns.GridColumn>()
            .Where(c => c.Visible)
            .Sum(c => c.Width);
        var anchoDisponible = _grid.Width - SystemInformation.VerticalScrollBarWidth;
        if (_gridView.Columns["Huertas"] is { } colHuertaFill && anchoDisponible > anchoColumnas)
        {
            colHuertaFill.Width += anchoDisponible - anchoColumnas;
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
            XtraMessageBox.Show(this, "Selecciona una recepción de fruta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(RecepcionFrutaDto? recepcionExistente)
    {
        if (_recepcionFrutaEditarForm is { IsDisposed: false })
        {
            if (_recepcionFrutaEditarForm.WindowState == FormWindowState.Minimized)
            {
                _recepcionFrutaEditarForm.WindowState = FormWindowState.Normal;
            }

            _recepcionFrutaEditarForm.Activate();
            return;
        }

        _recepcionFrutaEditarForm = new RecepcionFrutaEditarForm(_recepcionFrutaService, recepcionExistente);
        _recepcionFrutaEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _recepcionFrutaEditarForm.FormClosed += (_, _) => _recepcionFrutaEditarForm = null;
        _recepcionFrutaEditarForm.Show(this);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una recepción de fruta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar la recepción de fruta folio '{seleccionado.Folio}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _recepcionFrutaService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnVistaPrevia_Click(object? sender, EventArgs e)
    {
        if (!_sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.VistaPrevia))
        {
            XtraMessageBox.Show(this, "No tienes permiso para ver este reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una recepción de fruta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var datosReporte = await _recepcionFrutaService.ObtenerParaReporteAsync(seleccionado.Id);
            if (datosReporte is null)
            {
                XtraMessageBox.Show(this, "No se encontró la Orden de Corte asociada a esta Recepción — agrega una línea antes de imprimir.",
                    "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var empresa = await _empresaConfiguracionService.ObtenerAsync();
            var reporte = await CrearReporteAsync();
            reporte.CargarDatos(datosReporte, empresa);

            // Además de las etiquetas ya llenadas por CargarDatos, se conecta el origen de datos
            // por si el usuario arrastró un campo nuevo en el Diseñador — así también sale con
            // dato real en Vista Previa, no solo en blanco dentro del Diseñador.
            reporte.ConectarOrigenDatos(_sqlOptions, seleccionado.Id);

            using var visor = new VisorReporteForm(reporte, CodigoReporte, _sessionContext);
            visor.ShowDialog(this);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnDisenarReporte_Click(object? sender, EventArgs e)
    {
        if (!_sessionContext.TienePermisoReporte(CodigoReporte, AccionReporte.Diseno))
        {
            XtraMessageBox.Show(this, "No tienes permiso para diseñar este reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var reporte = await CrearReporteAsync();
        await DisenadorReporteForm.MostrarAsync(
            this, _reportePlantillaService, CodigoReporte, "Recepción de Fruta", reporte,
            r => ((ReporteRecepcionFruta)r).ConectarOrigenDatos(_sqlOptions, 0),
            r => ((ReporteRecepcionFruta)r).DesconectarOrigenDatos());
    }

    private async Task<ReporteRecepcionFruta> CrearReporteAsync()
    {
        var reporte = new ReporteRecepcionFruta();

        var plantilla = await _reportePlantillaService.ObtenerPorCodigoAsync(CodigoReporte);
        if (!string.IsNullOrWhiteSpace(plantilla?.DefinicionXml))
        {
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(plantilla.DefinicionXml));
            reporte.LoadLayoutFromXml(stream);
        }

        return reporte;
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private RecepcionFrutaDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as RecepcionFrutaDto;
}
