using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Seguridad;

public partial class UsuarioEditarForm : XtraForm
{
    private readonly UsuarioService _usuarioService = null!;
    private readonly RolService _rolService = null!;
    private readonly UsuarioAdminDto? _usuarioExistente;

    public UsuarioEditarForm()
    {
        InitializeComponent();
    }

    public UsuarioEditarForm(UsuarioService usuarioService, RolService rolService, UsuarioAdminDto? usuarioExistente)
        : this()
    {
        _usuarioService = usuarioService;
        _rolService = rolService;
        _usuarioExistente = usuarioExistente;

        Text = usuarioExistente is null ? "FrontOne - Nuevo usuario" : "FrontOne - Editar usuario";

        if (usuarioExistente is not null)
        {
            _txtNombreUsuario.Text = usuarioExistente.NombreUsuario;
            _txtNombreCompleto.Text = usuarioExistente.NombreCompleto;
            _txtEmail.Text = usuarioExistente.Email ?? string.Empty;
            _chkActivo.Checked = usuarioExistente.Activo;
        }
        else
        {
            _chkActivo.Checked = true;
        }

        Load += async (_, _) => await CargarRolesAsync();
    }

    private async Task CargarRolesAsync()
    {
        var roles = await _rolService.ObtenerAsync();
        var rolIdsAsignados = _usuarioExistente?.RolIds ?? [];

        _clbRoles.Items.Clear();
        foreach (var rol in roles)
        {
            var checkState = rolIdsAsignados.Contains(rol.Id) ? CheckState.Checked : CheckState.Unchecked;
            _clbRoles.Items.Add(new CheckedListBoxItem(rol.Id, rol.Nombre, checkState));
        }
    }

    private List<int> ObtenerRolesSeleccionados()
    {
        var seleccionados = new List<int>();

        foreach (CheckedListBoxItem item in _clbRoles.Items)
        {
            if (item.CheckState == CheckState.Checked && item.Value is int rolId)
            {
                seleccionados.Add(rolId);
            }
        }

        return seleccionados;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var datos = new UsuarioAdminDto(
            _usuarioExistente?.Id ?? 0,
            _txtNombreUsuario.Text.Trim(),
            _txtNombreCompleto.Text.Trim(),
            string.IsNullOrWhiteSpace(_txtEmail.Text) ? null : _txtEmail.Text.Trim(),
            string.IsNullOrWhiteSpace(_txtPassword.Text) ? null : _txtPassword.Text,
            _chkActivo.Checked,
            ObtenerRolesSeleccionados());

        try
        {
            if (_usuarioExistente is null)
            {
                await _usuarioService.CrearAsync(datos);
            }
            else
            {
                await _usuarioService.ActualizarAsync(datos);
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
