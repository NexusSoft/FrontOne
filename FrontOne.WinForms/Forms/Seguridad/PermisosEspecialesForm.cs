using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Seguridad;

// Permisos especiales por rol: catálogo de acciones independiente de las pantallas web.
public partial class PermisosEspecialesForm : XtraForm
{
    private readonly RolService _rolService = null!;
    private readonly PermisoEspecialService _permisoEspecialService = null!;

    private List<PermisoGridRow> _filas = [];

    private class PermisoGridRow
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Habilitado { get; set; }
    }

    public PermisosEspecialesForm()
    {
        InitializeComponent();
    }

    public PermisosEspecialesForm(RolService rolService, PermisoEspecialService permisoEspecialService)
        : this()
    {
        _rolService = rolService;
        _permisoEspecialService = permisoEspecialService;

        _cmbRol.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbRol.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbRol.ButtonClick += CmbRol_ButtonClick;

        Load += PermisosEspecialesForm_Load;
    }

    private async void PermisosEspecialesForm_Load(object? sender, EventArgs e)
        => await CargarRolesAsync(seleccionarPrimero: true);

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
        var matriz = await _permisoEspecialService.ObtenerMatrizAsync(rolId);

        _filas = matriz
            .Select(f => new PermisoGridRow
            {
                Codigo = f.Codigo,
                Descripcion = f.Descripcion,
                Habilitado = f.Habilitado,
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
            .Select(f => new PermisoEspecialFilaDto(f.Codigo, f.Descripcion, f.Habilitado))
            .ToList();

        await _permisoEspecialService.GuardarAsync(rolId, filasDto);

        XtraMessageBox.Show(this, "Permisos especiales guardados.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
