using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Seguridad;

public partial class UsuariosForm : XtraForm
{
    private readonly UsuarioService _usuarioService = null!;
    private readonly RolService _rolService = null!;

    private IReadOnlyList<UsuarioAdminDto> _usuariosCache = [];

    private record UsuarioGridRow(int Id, string NombreUsuario, string NombreCompleto, string Email, bool Activo);

    public UsuariosForm()
    {
        InitializeComponent();
    }

    public UsuariosForm(UsuarioService usuarioService, RolService rolService)
        : this()
    {
        _usuarioService = usuarioService;
        _rolService = rolService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        _usuariosCache = await _usuarioService.ObtenerAsync();

        var filas = _usuariosCache
            .Select(u => new UsuarioGridRow(u.Id, u.NombreUsuario, u.NombreCompleto, u.Email ?? "-", u.Activo))
            .ToList();

        _grid.DataSource = filas;
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new UsuarioEditarForm(_usuarioService, _rolService, null);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un usuario.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new UsuarioEditarForm(_usuarioService, _rolService, seleccionado);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un usuario.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el usuario '{seleccionado.NombreUsuario}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _usuarioService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private UsuarioAdminDto? ObtenerSeleccionado()
    {
        if (_gridView.GetFocusedRow() is not UsuarioGridRow fila)
        {
            return null;
        }

        return _usuariosCache.FirstOrDefault(u => u.Id == fila.Id);
    }
}
