using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Seguridad;

/// <summary>
/// Permisos de la aplicación móvil (FrontOne.Android), separados de la matriz general de
/// Permisos para que no se pierdan entre las decenas de pantallas de escritorio — mismo modelo
/// de datos que <see cref="PermisosForm"/> (Seguridad.Modulo/Pantalla/Accion/Permiso, mismo
/// <see cref="PermisoService"/>), solo que esta pantalla muestra únicamente las 9 Pantallas
/// móviles: "AccesoMovil" (módulo Seguridad, el gate de "puede entrar a la app o no") y las 8
/// del módulo "AplicacionMovil" (una tarjeta de Inicio cada una). Con las 4 acciones estándar
/// (Consultar/Crear/Modificar/Eliminar) editables por rol, igual que cualquier otra pantalla.
///
/// OJO al guardar: <see cref="PermisoService.GuardarAsync"/> reemplaza TODOS los permisos del
/// rol (borra todo y vuelve a insertar lo que se le mande) — nunca se le puede mandar solo el
/// subconjunto móvil o se borrarían los permisos de escritorio de ese rol. Por eso esta pantalla
/// guarda en <see cref="_matrizCompleta"/> la matriz completa tal como la trae
/// <see cref="PermisoService.ObtenerMatrizAsync"/>, solo edita en el grid el subconjunto móvil, y
/// al guardar reconstruye la lista completa (pantallas no-móviles intactas + móviles editadas).
/// </summary>
public partial class PermisosAplicacionMovilForm : XtraForm
{
    private readonly RolService _rolService = null!;
    private readonly PermisoService _permisoService = null!;

    private List<PermisoPantallaDto> _matrizCompleta = [];
    private List<PermisoGridRow> _filas = [];

    private class PermisoGridRow
    {
        public int PantallaId { get; set; }
        public string Pantalla { get; set; } = string.Empty;
        public bool Consultar { get; set; }
        public bool Crear { get; set; }
        public bool Modificar { get; set; }
        public bool Eliminar { get; set; }
    }

    public PermisosAplicacionMovilForm()
    {
        InitializeComponent();
    }

    public PermisosAplicacionMovilForm(RolService rolService, PermisoService permisoService)
        : this()
    {
        _rolService = rolService;
        _permisoService = permisoService;

        _cmbRol.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbRol.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbRol.ButtonClick += CmbRol_ButtonClick;

        Load += async (_, _) => await CargarRolesAsync(seleccionarPrimero: true);
    }

    private static bool EsPantallaMovil(string moduloNombre, string pantallaNombre) =>
        moduloNombre == "AplicacionMovil" || (moduloNombre == "Seguridad" && pantallaNombre == "AccesoMovil");

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
        _matrizCompleta = (await _permisoService.ObtenerMatrizAsync(rolId)).ToList();

        _filas = _matrizCompleta
            .Where(f => EsPantallaMovil(f.ModuloNombre, f.PantallaNombre))
            .Select(f => new PermisoGridRow
            {
                PantallaId = f.PantallaId,
                Pantalla = f.PantallaNombre,
                Consultar = f.Consultar,
                Crear = f.Crear,
                Modificar = f.Modificar,
                Eliminar = f.Eliminar,
            })
            .ToList();

        _grid.DataSource = _filas;

        if (_gridView.Columns["PantallaId"] is { } colPantallaId)
        {
            colPantallaId.Visible = false;
        }

        if (_gridView.Columns["Pantalla"] is { } colPantalla)
        {
            colPantalla.Caption = "Módulo de la App";
        }

        _gridView.BestFitColumns();
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

        // No mandar solo el subconjunto móvil: GuardarAsync reemplaza TODOS los permisos del rol,
        // así que hay que reconstruir la matriz completa (pantallas de escritorio intactas, tal
        // como llegaron; pantallas móviles con los valores que se acaban de editar en el grid).
        var pantallasMovilIds = _filas.Select(f => f.PantallaId).ToHashSet();

        var filasFinal = _matrizCompleta
            .Where(f => !pantallasMovilIds.Contains(f.PantallaId))
            .Concat(_filas.Select(f => new PermisoPantallaDto(
                f.PantallaId,
                "AplicacionMovil",
                f.Pantalla,
                f.Consultar,
                f.Crear,
                f.Modificar,
                f.Eliminar)))
            .ToList();

        // El Modulo real de "AccesoMovil" es "Seguridad", no "AplicacionMovil" — GuardarAsync solo
        // usa PantallaId/AccionId para grabar (ver PermisoService.GuardarAsync), así que el nombre
        // de Modulo en el DTO reconstruido no afecta el guardado, pero se corrige aquí para no
        // dejar datos inconsistentes en memoria por si algo más adelante llega a depender de él.
        filasFinal = filasFinal
            .Select(f => f.PantallaNombre == "AccesoMovil" ? f with { ModuloNombre = "Seguridad" } : f)
            .ToList();

        await _permisoService.GuardarAsync(rolId, filasFinal);

        XtraMessageBox.Show(this, "Permisos de la aplicación móvil guardados.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
