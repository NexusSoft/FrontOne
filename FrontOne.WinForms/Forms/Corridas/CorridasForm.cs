using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Corridas;

public partial class CorridasForm : XtraForm
{
    private const byte EstatusEnProceso = 1;
    private const byte EstatusProcesado = 2;

    private readonly CorridaService _corridaService = null!;
    private readonly SessionContext _sessionContext = null!;

    private CorridaEditarForm? _corridaEditarForm;
    private List<CorridaDto> _corridas = new();

    public CorridasForm()
    {
        InitializeComponent();
    }

    public CorridasForm(CorridaService corridaService, SessionContext sessionContext)
        : this()
    {
        _corridaService = corridaService;
        _sessionContext = sessionContext;

        _cmbFiltroStatus.Properties.Items.AddRange(new object[] { "Todos", "En Proceso", "Procesado" });
        _cmbFiltroStatus.SelectedIndex = 0;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        _corridas = (await _corridaService.ObtenerAsync()).ToList();
        AplicarFiltro();
    }

    private void AplicarFiltro()
    {
        IEnumerable<CorridaDto> filtradas = _cmbFiltroStatus.SelectedIndex switch
        {
            1 => _corridas.Where(c => c.Estatus == EstatusEnProceso),
            2 => _corridas.Where(c => c.Estatus == EstatusProcesado),
            _ => _corridas,
        };

        _grid.DataSource = filtradas.ToList();
        ConfigurarColumnas();
    }

    private void CmbFiltroStatus_SelectedIndexChanged(object? sender, EventArgs e) => AplicarFiltro();

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "Id", "LoteId", "FechaCreacion" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["LoteFolio"] is { } colFolio)
        {
            colFolio.Caption = "No. de Lote";
        }

        if (_gridView.Columns["CodigoTrazabilidad"] is { } colCodigo)
        {
            colCodigo.Caption = "Código de Trazabilidad";
        }

        if (_gridView.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["HuertaNombre"] is { } colHuerta)
        {
            colHuerta.Caption = "Huerta";
        }

        if (_gridView.Columns["RegistroSagarpa"] is { } colSagarpa)
        {
            colSagarpa.Caption = "Registro Sagarpa";
        }

        if (_gridView.Columns["ProductorNombre"] is { } colProductor)
        {
            colProductor.Caption = "Productor";
        }

        if (_gridView.Columns["KilosAProcesar"] is { } colKilosAProcesar)
        {
            colKilosAProcesar.Caption = "Kilos a Procesar";
            colKilosAProcesar.DisplayFormat.FormatType = FormatType.Numeric;
            colKilosAProcesar.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["KilosProcesados"] is { } colKilosProcesados)
        {
            colKilosProcesados.Caption = "Kilos Procesados";
            colKilosProcesados.DisplayFormat.FormatType = FormatType.Numeric;
            colKilosProcesados.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["FechaHoraInicio"] is { } colInicio)
        {
            colInicio.Caption = "Fecha/Hora Inicio";
            colInicio.DisplayFormat.FormatType = FormatType.DateTime;
            colInicio.DisplayFormat.FormatString = "dd/MM/yyyy hh:mm tt";
        }

        if (_gridView.Columns["FechaHoraFin"] is { } colFin)
        {
            colFin.Caption = "Fecha/Hora Fin";
            colFin.DisplayFormat.FormatType = FormatType.DateTime;
            colFin.DisplayFormat.FormatString = "dd/MM/yyyy hh:mm tt";
        }

        if (_gridView.Columns["Estatus"] is { } colEstatus)
        {
            colEstatus.Caption = "Status";
        }

        // Orden explícito del grid, con Status siempre al final — asignar VisibleIndex uno por uno
        // es la única forma confiable de fijar el orden (el índice relativo del enum de origen no
        // basta: DevExpress no compacta ni reordena solo con eso).
        var ordenColumnas = new[]
        {
            "LoteFolio", "CodigoTrazabilidad", "Kilogramos", "HuertaNombre", "RegistroSagarpa",
            "ProductorNombre", "Beneficiario", "FechaHoraInicio", "FechaHoraFin",
            "KilosAProcesar", "KilosProcesados", "Estatus",
        };
        for (var i = 0; i < ordenColumnas.Length; i++)
        {
            if (_gridView.Columns[ordenColumnas[i]] is { } columnaOrdenada)
            {
                columnaOrdenada.VisibleIndex = i;
            }
        }

        // BestFitColumns se recalcula cada vez que se recarga el grid (filtro/CRUD), así que el
        // ancho por contenido queda correcto en cuanto haya datos reales, no solo con el grid vacío.
        _gridView.BestFitColumns();

        var anchoColumnas = _gridView.Columns.Cast<DevExpress.XtraGrid.Columns.GridColumn>()
            .Where(c => c.Visible)
            .Sum(c => c.Width);
        var anchoDisponible = _grid.Width - SystemInformation.VerticalScrollBarWidth;
        if (_gridView.Columns["Beneficiario"] is { } colBeneficiarioFill && anchoDisponible > anchoColumnas)
        {
            colBeneficiarioFill.Width += anchoDisponible - anchoColumnas;
        }
    }

    private void GridView_CustomColumnDisplayText(object? sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName != "Estatus" || e.Value is not byte estatus)
        {
            return;
        }

        e.DisplayText = estatus switch
        {
            EstatusEnProceso => "En Proceso",
            EstatusProcesado => "Procesado",
            _ => e.DisplayText,
        };
    }

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

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        AbrirEditarForm(null);
    }

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionada = ObtenerSeleccionada();
        if (seleccionada is null)
        {
            XtraMessageBox.Show(this, "Selecciona una corrida.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionada);
    }

    private void AbrirEditarForm(CorridaDto? corridaExistente)
    {
        if (_corridaEditarForm is { IsDisposed: false })
        {
            if (_corridaEditarForm.WindowState == FormWindowState.Minimized)
            {
                _corridaEditarForm.WindowState = FormWindowState.Normal;
            }

            _corridaEditarForm.Activate();
            return;
        }

        _corridaEditarForm = new CorridaEditarForm(_corridaService, corridaExistente);
        _corridaEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _corridaEditarForm.FormClosed += (_, _) => _corridaEditarForm = null;
        _corridaEditarForm.Show(this);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionada = ObtenerSeleccionada();
        if (seleccionada is null)
        {
            XtraMessageBox.Show(this, "Selecciona una corrida.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Eliminar la corrida del lote '{seleccionada.LoteFolio}'? El lote volverá a estar disponible para capturarse de nuevo.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _corridaService.EliminarAsync(seleccionada.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private CorridaDto? ObtenerSeleccionada()
        => _gridView.GetFocusedRow() as CorridaDto;
}
