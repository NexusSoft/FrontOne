using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Sistema;

public partial class ConfiguracionEmpresaForm : XtraForm
{
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private byte[]? _logo;

    public ConfiguracionEmpresaForm()
    {
        InitializeComponent();
    }

    public ConfiguracionEmpresaForm(EmpresaConfiguracionService empresaConfiguracionService)
        : this()
    {
        _empresaConfiguracionService = empresaConfiguracionService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var empresa = await _empresaConfiguracionService.ObtenerAsync();

        _txtRazonSocial.Text = empresa.RazonSocial;
        _txtDomicilio.Text = empresa.Domicilio;
        _txtRfc.Text = empresa.Rfc;
        _txtTelefono.Text = empresa.Telefono;
        _txtCorreo.Text = empresa.Correo;

        _logo = empresa.Logo;
        MostrarLogo();
    }

    private void MostrarLogo()
    {
        _picLogo.Image = _logo is { Length: > 0 } ? ImagenDesdeBytes(_logo) : null;
    }

    private static Image ImagenDesdeBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return Image.FromStream(stream);
    }

    private void BtnCargarLogo_Click(object? sender, EventArgs e)
    {
        using var dialogo = new OpenFileDialog
        {
            Title = "Seleccionar logo de la empresa",
            Filter = "Imágenes (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp",
        };

        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _logo = File.ReadAllBytes(dialogo.FileName);
        MostrarLogo();
    }

    private void BtnQuitarLogo_Click(object? sender, EventArgs e)
    {
        _logo = null;
        MostrarLogo();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtRazonSocial.Text))
        {
            XtraMessageBox.Show(this, "Captura la razón social.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var datos = new EmpresaConfiguracionDto(
            _txtRazonSocial.Text,
            string.IsNullOrWhiteSpace(_txtDomicilio.Text) ? null : _txtDomicilio.Text,
            string.IsNullOrWhiteSpace(_txtRfc.Text) ? null : _txtRfc.Text,
            string.IsNullOrWhiteSpace(_txtTelefono.Text) ? null : _txtTelefono.Text,
            string.IsNullOrWhiteSpace(_txtCorreo.Text) ? null : _txtCorreo.Text,
            _logo);

        try
        {
            await _empresaConfiguracionService.ActualizarAsync(datos);
            XtraMessageBox.Show(this, "Configuración de la empresa guardada correctamente.", "FrontOne",
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
