using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Seguridad;

// Permisos del sitio web (FrontOne.Web): tabla propia Seguridad.WebPermiso, mismo patrón que
// Seguridad.MovilPermiso — independiente de Seguridad.Pantalla/Permiso genérico, así que guardar
// aquí no toca ni requiere reconstruir los permisos de escritorio del rol.
public partial class PermisosAplicacionWebForm : XtraForm
{
    private readonly RolService _rolService = null!;
    private readonly WebPermisoService _webPermisoService = null!;

    private List<PermisoGridRow> _filas = [];

    private class PermisoGridRow
    {
        public string PantallaCodigo { get; set; } = string.Empty;
        public string Modulo { get; set; } = string.Empty;
        public bool Consultar { get; set; }
        public bool Crear { get; set; }
        public bool Modificar { get; set; }
        public bool Eliminar { get; set; }
    }

    public PermisosAplicacionWebForm()
    {
        InitializeComponent();
    }

    public PermisosAplicacionWebForm(RolService rolService, WebPermisoService webPermisoService)
        : this()
    {
        _rolService = rolService;
        _webPermisoService = webPermisoService;

        _cmbRol.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbRol.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbRol.ButtonClick += CmbRol_ButtonClick;

        Load += async (_, _) => await CargarRolesAsync(seleccionarPrimero: true);
    }

    private async Task CargarRolesAsync(bool seleccionarPrimero)
    {
        var rolSeleccionado = _cmbRol.EditValue;

        var roles = await _rolService.ObtenerAsync();

        _cmbRol.Properties.DataSource = roles.ToList();
        _cmbRol.Properties.ValueMember = "Id";
        _cmbRol.Properties.DisplayMember = "Nombre";
        _cmbRol.Properties.Columns.Clear();
        _cmbRol.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Rol"));
        _cmbRol.Properties.PopupWidth = 250;

        if (!seleccionarPrimero && roles.Any(r => r.Id == (rolSeleccionado as int?)))
        {
            _cmbRol.EditValue = rolSeleccionado;
        }
        else if (roles.Count > 0)
        {
            _cmbRol.EditValue = roles[0].Id;
        }
    }

    private async void CmbRol_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new RolesForm(_rolService);
        form.ShowDialog(this);
        await CargarRolesAsync(seleccionarPrimero: false);
    }

    private async void CmbRol_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cmbRol.EditValue is int rolId)
        {
            await CargarMatrizAsync(rolId);
        }
    }

    private async Task CargarMatrizAsync(int rolId)
    {
        var matriz = await _webPermisoService.ObtenerMatrizAsync(rolId);

        _filas = matriz
            .Select(f => new PermisoGridRow
            {
                PantallaCodigo = f.PantallaCodigo,
                Modulo = f.Modulo,
                Consultar = f.Consultar,
                Crear = f.Crear,
                Modificar = f.Modificar,
                Eliminar = f.Eliminar,
            })
            .ToList();

        _grid.DataSource = _filas;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbRol.EditValue is not int rolId)
        {
            XtraMessageBox.Show(this, "Selecciona un rol.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _gridView.CloseEditor();
        _gridView.UpdateCurrentRow();

        var filasDto = _filas
            .Select(f => new WebPermisoPantallaDto(f.PantallaCodigo, f.Modulo, f.Consultar, f.Crear, f.Modificar, f.Eliminar))
            .ToList();

        await _webPermisoService.GuardarAsync(rolId, filasDto);

        XtraMessageBox.Show(this, "Permisos de la aplicación web guardados.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
