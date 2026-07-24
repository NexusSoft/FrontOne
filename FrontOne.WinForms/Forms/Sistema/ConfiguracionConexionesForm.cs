using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Forms.Sistema;

public partial class ConfiguracionConexionesForm : XtraForm
{
    private readonly ConnectionSettingsService _connectionSettingsService = null!;

    public ConfiguracionConexionesForm()
    {
        InitializeComponent();
    }

    public ConfiguracionConexionesForm(ConnectionSettingsService connectionSettingsService)
        : this()
    {
        _connectionSettingsService = connectionSettingsService;

        CargarValoresGuardados();
    }

    private void CargarValoresGuardados()
    {
        var sql = _connectionSettingsService.GetSqlCredentials();
        if (sql is not null)
        {
            _txtSqlServer.Text = sql.Server;
            _txtSqlDatabase.Text = sql.Database;
            _chkSqlIntegratedSecurity.Checked = sql.IntegratedSecurity;
            _txtSqlUserId.Text = sql.UserId ?? string.Empty;
            _txtSqlPassword.Text = sql.Password ?? string.Empty;
        }

        var sap = _connectionSettingsService.GetSapCredentials();
        if (sap is not null)
        {
            _txtSapServiceLayerUrl.Text = sap.ServiceLayerUrl;
            _txtSapCompanyDb.Text = sap.CompanyDb;
            _txtSapUserName.Text = sap.UserName;
            _txtSapPassword.Text = sap.Password ?? string.Empty;
        }
    }

    private SqlConnectionCredentials LeerSqlCredentials()
        => new(
            _txtSqlServer.Text.Trim(),
            _txtSqlDatabase.Text.Trim(),
            _chkSqlIntegratedSecurity.Checked,
            string.IsNullOrWhiteSpace(_txtSqlUserId.Text) ? null : _txtSqlUserId.Text.Trim(),
            string.IsNullOrWhiteSpace(_txtSqlPassword.Text) ? null : _txtSqlPassword.Text);

    private SapConnectionCredentials LeerSapCredentials()
        => new(
            _txtSapServiceLayerUrl.Text.Trim(),
            _txtSapCompanyDb.Text.Trim(),
            _txtSapUserName.Text.Trim(),
            string.IsNullOrWhiteSpace(_txtSapPassword.Text) ? null : _txtSapPassword.Text);

    private async void BtnSqlProbar_Click(object? sender, EventArgs e)
    {
        if (sender is SimpleButton btn)
        {
            btn.Enabled = false;
        }

        UseWaitCursor = true;
        try
        {
            var resultado = await _connectionSettingsService.TestSqlConnectionAsync(LeerSqlCredentials());
            MostrarResultado("SQL Server", resultado);
        }
        finally
        {
            UseWaitCursor = false;
            if (sender is SimpleButton btn2)
            {
                btn2.Enabled = true;
            }
        }
    }

    private void BtnSqlGuardar_Click(object? sender, EventArgs e)
    {
        _connectionSettingsService.SaveSqlCredentials(LeerSqlCredentials());
        XtraMessageBox.Show(this, "Credenciales de SQL Server guardadas.", "FrontOne",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private async void BtnSapProbar_Click(object? sender, EventArgs e)
    {
        if (sender is SimpleButton btn)
        {
            btn.Enabled = false;
        }

        UseWaitCursor = true;
        try
        {
            var resultado = await _connectionSettingsService.TestSapConnectionAsync(LeerSapCredentials());
            MostrarResultado("SAP Business One", resultado);
        }
        finally
        {
            UseWaitCursor = false;
            if (sender is SimpleButton btn2)
            {
                btn2.Enabled = true;
            }
        }
    }

    private void BtnSapGuardar_Click(object? sender, EventArgs e)
    {
        _connectionSettingsService.SaveSapCredentials(LeerSapCredentials());
        XtraMessageBox.Show(this, "Credenciales de SAP Business One guardadas.", "FrontOne",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private void MostrarResultado(string origen, ConnectionTestResult resultado)
    {
        if (resultado.Success)
        {
            XtraMessageBox.Show(this, $"Conexión a {origen} exitosa.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            XtraMessageBox.Show(this, $"No se pudo conectar a {origen}.\n\n{resultado.Error}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
