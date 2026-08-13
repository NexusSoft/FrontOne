using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

// Formulario único de captura de Incidencia: los campos derivados de la Orden de Corte se
// muestran de solo lectura (resueltos por sp_Incidencia_ObtenerPorOrdenCorteId, que siempre
// regresa una fila aunque la Incidencia todavía no exista), y los propios de Incidencia quedan
// libres para capturarse. Sirve tanto para la primera captura como para editar una ya capturada
// — la diferencia la marca IncidenciaDto.Id (null = todavía no existe).
public partial class IncidenciaEditarForm : XtraForm
{
    private readonly IncidenciaService _incidenciaService = null!;
    private readonly SupervisorHuertaService _supervisorHuertaService = null!;
    private readonly int _ordenCorteId;

    private IncidenciaDto? _incidencia;

    public IncidenciaEditarForm()
    {
        InitializeComponent();
    }

    public IncidenciaEditarForm(IncidenciaService incidenciaService, SupervisorHuertaService supervisorHuertaService, int ordenCorteId)
        : this()
    {
        _incidenciaService = incidenciaService;
        _supervisorHuertaService = supervisorHuertaService;
        _ordenCorteId = ordenCorteId;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        await CargarSupervisoresAsync();

        _incidencia = await _incidenciaService.ObtenerPorOrdenCorteIdAsync(_ordenCorteId);
        if (_incidencia is null)
        {
            XtraMessageBox.Show(this, "La Orden de Corte seleccionada ya no existe.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }

        Text = _incidencia.Id is null
            ? $"FrontOne - Capturar Incidencia (Orden {_incidencia.Folio})"
            : $"FrontOne - Editar Incidencia (Orden {_incidencia.Folio})";

        // Solo lectura, derivados de la Orden de Corte.
        _txtFolio.Text = _incidencia.Folio;
        _txtFecha.Text = _incidencia.Fecha.ToString("dd/MM/yyyy");
        _txtAcopiador.Text = _incidencia.Acopiador;
        _txtRegistroSagarpa.Text = _incidencia.RegistroSagarpa;
        _txtHuerta.Text = _incidencia.HuertaNombre;
        _txtCajasEntregadas.Text = _incidencia.CajasEntregadas.ToString();
        _txtProductor.Text = _incidencia.ProductorNombre;
        _txtBeneficiario.Text = _incidencia.Beneficiario;
        _txtTipoCorte.Text = _incidencia.TipoCorteNombre;
        _txtTipoPago.Text = _incidencia.TipoPagoNombre;
        _txtCostoKg.Text = _incidencia.CostoKg.ToString("N2");
        _txtJefeCuadrilla.Text = _incidencia.JefeCuadrillaNombre;
        _txtTransportista.Text = _incidencia.TransportistaNombre;
        _txtFloracion.Text = _incidencia.FloracionNombre;
        _txtMunicipio.Text = _incidencia.MunicipioNombre;
        _txtPoblacion.Text = _incidencia.PoblacionNombre;

        // Captura propia de Incidencia — vacío si es la primera vez que se abre para esta orden.
        _cmbSupervisorHuerta.EditValue = _incidencia.SupervisorHuertaId;
        _txtOdcSapCosecha.Text = _incidencia.OdcSapCosecha;
        _txtOdcSapFlete.Text = _incidencia.OdcSapFlete;
        _txtNumeroTelefono.Text = _incidencia.NumeroTelefono;
        _txtPlacas.Text = _incidencia.Placas;
        _txtBascula.Text = _incidencia.Bascula;
        _spnCajasCosechadas.EditValue = _incidencia.CajasCosechadas;
        _txtPuntoReunion.Text = _incidencia.PuntoReunion;
        _txtCajaPorCuadrilla.Text = _incidencia.CajaPorCuadrilla;
        _timeHoraLlegada.EditValue = _incidencia.HoraLlegadaHuerta is { } hora ? DateTime.Today.Add(hora) : null;
        _memoObservaciones.Text = _incidencia.Observaciones;
        _memoIncidencias.Text = _incidencia.Incidencias;
        _memoAjuste.Text = _incidencia.Ajuste;
    }

    private async Task CargarSupervisoresAsync()
    {
        var supervisores = await _supervisorHuertaService.ObtenerAsync();
        _cmbSupervisorHuerta.Properties.DataSource = supervisores.Where(s => s.Activo).ToList();
        _cmbSupervisorHuerta.Properties.ValueMember = "Id";
        _cmbSupervisorHuerta.Properties.DisplayMember = "Nombre";
    }

    private async void CmbSupervisorHuerta_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new SupervisoresHuertaForm(_supervisorHuertaService);
        form.ShowDialog(this);
        await CargarSupervisoresAsync();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_incidencia is null)
        {
            return;
        }

        var datos = _incidencia with
        {
            SupervisorHuertaId = _cmbSupervisorHuerta.EditValue as int?,
            OdcSapCosecha = TextoOVacio(_txtOdcSapCosecha.Text),
            NumeroTelefono = TextoOVacio(_txtNumeroTelefono.Text),
            Placas = TextoOVacio(_txtPlacas.Text),
            OdcSapFlete = TextoOVacio(_txtOdcSapFlete.Text),
            Bascula = TextoOVacio(_txtBascula.Text),
            PuntoReunion = TextoOVacio(_txtPuntoReunion.Text),
            HoraLlegadaHuerta = _timeHoraLlegada.EditValue is DateTime hora ? hora.TimeOfDay : null,
            CajasCosechadas = _spnCajasCosechadas.EditValue is null ? null : Convert.ToInt32(_spnCajasCosechadas.EditValue),
            CajaPorCuadrilla = TextoOVacio(_txtCajaPorCuadrilla.Text),
            Observaciones = TextoOVacio(_memoObservaciones.Text),
            Incidencias = TextoOVacio(_memoIncidencias.Text),
            Ajuste = TextoOVacio(_memoAjuste.Text),
        };

        try
        {
            if (datos.Id is null)
            {
                await _incidenciaService.CrearAsync(datos);
            }
            else
            {
                await _incidenciaService.ActualizarAsync(datos);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string? TextoOVacio(string? texto) => string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
