using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Bascula;

namespace FrontOne.WinForms.Forms.Sistema;

// Configuración del puerto serie de la báscula. Singleton (una sola fila, Id = 1), mismo criterio
// que ConfiguracionEmpresaForm: no tiene Nuevo ni Eliminar, solo Guardar/Cancelar.
public partial class ConfiguracionBasculaForm : XtraForm
{
    private readonly ConfiguracionBasculaService _configuracionBasculaService = null!;

    public ConfiguracionBasculaForm()
    {
        InitializeComponent();
    }

    public ConfiguracionBasculaForm(ConfiguracionBasculaService configuracionBasculaService)
        : this()
    {
        _configuracionBasculaService = configuracionBasculaService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        CargarPuertosDisponibles();
        CargarCatalogosFijos();

        var bascula = await _configuracionBasculaService.ObtenerAsync();

        // El puerto guardado puede no estar presente ahora mismo (báscula desconectada): se agrega
        // a la lista para no perder la configuración solo porque el equipo está apagado.
        if (!string.IsNullOrWhiteSpace(bascula.Puerto) && !_cmbPuerto.Properties.Items.Contains(bascula.Puerto))
        {
            _cmbPuerto.Properties.Items.Add(bascula.Puerto);
        }

        _cmbPuerto.EditValue = bascula.Puerto;
        _spnBaudRate.EditValue = bascula.BaudRate;
        _cmbParity.SelectedIndex = bascula.Parity <= 4 ? bascula.Parity : 0;
        _spnDataBits.EditValue = (int)bascula.DataBits;
        _cmbStopBits.SelectedIndex = bascula.StopBits <= 3 ? bascula.StopBits : 1;
        _txtPatronLectura.Text = bascula.PatronLectura;
    }

    private void CargarPuertosDisponibles()
    {
        _cmbPuerto.Properties.Items.Clear();
        _cmbPuerto.Properties.Items.AddRange(BasculaLecturaService.ObtenerPuertosDisponibles().ToArray());
    }

    // Parity y StopBits se muestran con el nombre del enum de System.IO.Ports y se guardan por su
    // índice, que es exactamente el valor numérico del enum — por eso el orden de estos arreglos
    // no se puede alterar.
    private void CargarCatalogosFijos()
    {
        if (_cmbParity.Properties.Items.Count == 0)
        {
            _cmbParity.Properties.Items.AddRange(new object[] { "Ninguna", "Impar", "Par", "Marca", "Espacio" });
        }

        if (_cmbStopBits.Properties.Items.Count == 0)
        {
            _cmbStopBits.Properties.Items.AddRange(new object[] { "Ninguno", "Uno", "Dos", "Uno y medio" });
        }
    }

    private void BtnActualizarPuertos_Click(object? sender, EventArgs e)
    {
        var seleccionado = _cmbPuerto.EditValue as string;
        CargarPuertosDisponibles();
        _cmbPuerto.EditValue = seleccionado;
    }

    private void BtnProbarLectura_Click(object? sender, EventArgs e)
    {
        var datos = ArmarDto();
        if (string.IsNullOrWhiteSpace(datos.Puerto))
        {
            XtraMessageBox.Show(this, "Selecciona el puerto de la báscula antes de probar la lectura.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;
            var trama = BasculaLecturaService.LeerTrama(datos);
            var peso = BasculaLecturaService.ExtraerPeso(trama, datos.PatronLectura);

            XtraMessageBox.Show(this,
                $"Trama recibida:\n{trama.Trim()}\n\nPeso interpretado: {peso:N2}",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (InvalidOperationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            await _configuracionBasculaService.ActualizarAsync(ArmarDto());
            XtraMessageBox.Show(this, "Configuración de la báscula guardada correctamente.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, $"No se pudo guardar la configuración.\n\n{ex.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private ConfiguracionBasculaDto ArmarDto() => new(
        _cmbPuerto.EditValue as string ?? string.Empty,
        Convert.ToInt32(_spnBaudRate.EditValue),
        (byte)Math.Max(_cmbParity.SelectedIndex, 0),
        (byte)Convert.ToInt32(_spnDataBits.EditValue),
        (byte)Math.Max(_cmbStopBits.SelectedIndex, 0),
        string.IsNullOrWhiteSpace(_txtPatronLectura.Text) ? null : _txtPatronLectura.Text);

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
