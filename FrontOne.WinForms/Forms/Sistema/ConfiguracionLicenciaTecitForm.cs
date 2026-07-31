using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Sistema;

public partial class ConfiguracionLicenciaTecitForm : XtraForm
{
    private readonly LicenciaTecitService _licenciaTecitService = null!;

    public ConfiguracionLicenciaTecitForm()
    {
        InitializeComponent();
    }

    public ConfiguracionLicenciaTecitForm(LicenciaTecitService licenciaTecitService)
        : this()
    {
        _licenciaTecitService = licenciaTecitService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var licencia = await _licenciaTecitService.ObtenerAsync();

        _txtLicenciatario.Text = licencia.Licenciatario;
        _txtClaveLicencia.Text = licencia.ClaveLicencia;
        _txtTipoLicencia.Text = licencia.TipoLicencia;
        _txtNumeroLicencias.Text = licencia.NumeroLicencias?.ToString();
        _txtProducto.Text = licencia.Producto;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtLicenciatario.Text))
        {
            XtraMessageBox.Show(this, "Captura el nombre del licenciatario.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int? numeroLicencias = int.TryParse(_txtNumeroLicencias.Text, out var numero) ? numero : null;

        var datos = new LicenciaTecitDto(
            _txtLicenciatario.Text,
            string.IsNullOrWhiteSpace(_txtClaveLicencia.Text) ? null : _txtClaveLicencia.Text,
            string.IsNullOrWhiteSpace(_txtTipoLicencia.Text) ? null : _txtTipoLicencia.Text,
            numeroLicencias,
            string.IsNullOrWhiteSpace(_txtProducto.Text) ? null : _txtProducto.Text);

        try
        {
            await _licenciaTecitService.ActualizarAsync(datos);
            XtraMessageBox.Show(this, "Configuración de la licencia TECIT guardada correctamente.", "FrontOne",
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
