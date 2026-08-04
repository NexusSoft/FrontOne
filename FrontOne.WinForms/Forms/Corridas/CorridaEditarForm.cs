using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Corridas;

// Una Corrida es una sola fila que se actualiza dos veces: Iniciar Proceso la crea
// (FechaHoraInicio), Finalizar Corrida la cierra (FechaHoraFin). No hay botón "Guardar"
// tradicional — cada acción persiste de inmediato.
public partial class CorridaEditarForm : XtraForm
{
    private const byte EstatusEnProceso = 1;

    private readonly CorridaService _corridaService = null!;

    public event EventHandler? Guardado;

    private CorridaDto? _corridaActual;
    private IReadOnlyList<LoteDisponibleParaCorridaDto> _lotesDisponibles = [];

    public CorridaEditarForm()
    {
        InitializeComponent();
    }

    public CorridaEditarForm(CorridaService corridaService, CorridaDto? corridaExistente)
        : this()
    {
        _corridaService = corridaService;
        _corridaActual = corridaExistente;

        Text = corridaExistente is null ? "Nueva Corrida" : "Editar Corrida";

        Load += async (_, _) => await CargarDatosInicialesAsync();
    }

    private async Task CargarDatosInicialesAsync()
    {
        if (_corridaActual is null)
        {
            _cmbLote.Visible = true;
            _txtLoteFolio.Visible = false;

            _lotesDisponibles = await _corridaService.ObtenerLotesDisponiblesAsync();
            _cmbLote.Properties.DataSource = _lotesDisponibles.ToList();
            _cmbLote.Properties.ValueMember = "LoteId";
            _cmbLote.Properties.DisplayMember = "LoteFolio";
            _cmbLote.Properties.Columns.Clear();
            _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("LoteFolio", 90, "No. de Lote"));
            _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("CodigoTrazabilidad", 130, "Código de Trazabilidad"));
            _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("Fecha", 90, "Fecha"));
            _cmbLote.Properties.Columns.Add(new LookUpColumnInfo("Kilogramos", 90, "Kilogramos"));
            _cmbLote.Properties.PopupWidth = 420;

            _btnIniciarProceso.Enabled = true;
            _btnFinalizarCorrida.Enabled = false;
        }
        else
        {
            _cmbLote.Visible = false;
            _txtLoteFolio.Visible = true;

            MostrarCorridaEnFormulario(_corridaActual);

            _btnIniciarProceso.Visible = false;
            _btnFinalizarCorrida.Enabled = _corridaActual.Estatus == EstatusEnProceso;
        }
    }

    private void CmbLote_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cmbLote.EditValue is not int loteId)
        {
            return;
        }

        var lote = _lotesDisponibles.FirstOrDefault(l => l.LoteId == loteId);
        if (lote is null)
        {
            return;
        }

        _txtCodigoTrazabilidad.Text = lote.CodigoTrazabilidad;
        _txtHuerta.Text = lote.HuertaNombre;
        _txtRegistroSagarpa.Text = lote.RegistroSagarpa;
        _txtProductor.Text = lote.ProductorNombre;
        _txtBeneficiario.Text = lote.Beneficiario;
        _txtKilogramos.Text = lote.Kilogramos.ToString("n2");
        _txtKilosAProcesar.Text = lote.Kilogramos.ToString("n2");
        _txtKilosProcesados.Text = 0m.ToString("n2");
    }

    private void MostrarCorridaEnFormulario(CorridaDto c)
    {
        _txtLoteFolio.Text = c.LoteFolio;
        _txtCodigoTrazabilidad.Text = c.CodigoTrazabilidad;
        _txtHuerta.Text = c.HuertaNombre;
        _txtRegistroSagarpa.Text = c.RegistroSagarpa;
        _txtProductor.Text = c.ProductorNombre;
        _txtBeneficiario.Text = c.Beneficiario;
        _txtKilogramos.Text = c.Kilogramos.ToString("n2");
        _txtKilosAProcesar.Text = c.KilosAProcesar.ToString("n2");
        _txtKilosProcesados.Text = c.KilosProcesados.ToString("n2");
        _txtFechaHoraInicio.Text = c.FechaHoraInicio.ToString("dd/MM/yyyy hh:mm tt");
        _txtFechaHoraFin.Text = c.FechaHoraFin?.ToString("dd/MM/yyyy hh:mm tt") ?? string.Empty;
    }

    private async void BtnIniciarProceso_Click(object? sender, EventArgs e)
    {
        if (_cmbLote.EditValue is not int loteId)
        {
            XtraMessageBox.Show(this, "Selecciona un lote.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var id = await _corridaService.IniciarAsync(loteId);
            _corridaActual = await _corridaService.ObtenerPorIdAsync(id);
            if (_corridaActual is not null)
            {
                _cmbLote.Visible = false;
                _txtLoteFolio.Visible = true;
                MostrarCorridaEnFormulario(_corridaActual);
            }

            _btnIniciarProceso.Enabled = false;
            _btnFinalizarCorrida.Enabled = true;
            Guardado?.Invoke(this, EventArgs.Empty);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnFinalizarCorrida_Click(object? sender, EventArgs e)
    {
        if (_corridaActual is null)
        {
            return;
        }

        var confirmar = XtraMessageBox.Show(this, "¿Finalizar esta corrida?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _corridaService.FinalizarAsync(_corridaActual.Id);
            _corridaActual = await _corridaService.ObtenerPorIdAsync(_corridaActual.Id);
            if (_corridaActual is not null)
            {
                MostrarCorridaEnFormulario(_corridaActual);
            }

            _btnFinalizarCorrida.Enabled = false;
            Guardado?.Invoke(this, EventArgs.Empty);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancelar_Click(object? sender, EventArgs e) => Close();
}
