using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Sistema;

public partial class LoginForm : XtraForm
{
    private readonly AuthService _authService = null!;
    private readonly PermissionService _permissionService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly ConnectionSettingsService _connectionSettingsService = null!;

    public LoginForm()
    {
        InitializeComponent();
    }

    public LoginForm(
        AuthService authService,
        PermissionService permissionService,
        SessionContext sessionContext,
        ConnectionSettingsService connectionSettingsService)
        : this()
    {
        _authService = authService;
        _permissionService = permissionService;
        _sessionContext = sessionContext;
        _connectionSettingsService = connectionSettingsService;
    }

    private void BtnConfiguracion_Click(object? sender, EventArgs e)
    {
        using var form = new ConfiguracionConexionesForm(_connectionSettingsService);
        form.ShowDialog(this);
    }

    private async void BtnEntrar_Click(object? sender, EventArgs e)
    {
        _btnEntrar.Enabled = false;
        _lblError.Text = string.Empty;

        var result = await _authService.LoginAsync(_txtUsuario.Text.Trim(), _txtPassword.Text);

        if (!result.IsSuccess || result.Value is null)
        {
            _lblError.Text = result.Error ?? "Usuario o contraseña incorrectos";
            _btnEntrar.Enabled = true;
            return;
        }

        var permisos = await _permissionService.ObtenerPermisosAsync(result.Value.Id);
        _sessionContext.IniciarSesion(result.Value, permisos);

        DialogResult = DialogResult.OK;
        Close();
    }
}
