using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Seguridad;

public partial class ReportePermisosForm : XtraForm
{
    private readonly RolService _rolService = null!;
    private readonly ReportePermisoService _reportePermisoService = null!;

    private List<ReportePermisoGridRow> _filas = [];

    private class ReportePermisoGridRow
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool VistaPrevia { get; set; }
        public bool Impresion { get; set; }
        public bool Exportacion { get; set; }
        public bool Diseno { get; set; }
    }

    public ReportePermisosForm()
    {
        InitializeComponent();
    }

    public ReportePermisosForm(RolService rolService, ReportePermisoService reportePermisoService)
        : this()
    {
        _rolService = rolService;
        _reportePermisoService = reportePermisoService;

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
        var matriz = await _reportePermisoService.ObtenerMatrizAsync(rolId);

        _filas = matriz
            .Select(f => new ReportePermisoGridRow
            {
                Codigo = f.Codigo,
                Nombre = f.Nombre,
                VistaPrevia = f.VistaPrevia,
                Impresion = f.Impresion,
                Exportacion = f.Exportacion,
                Diseno = f.Diseno,
            })
            .ToList();

        _grid.DataSource = _filas;

        if (_gridView.Columns["Codigo"] is { } colCodigo)
        {
            colCodigo.Visible = false;
        }

        if (_gridView.Columns["Nombre"] is { } colNombre)
        {
            colNombre.Caption = "Reporte";
        }

        if (_gridView.Columns["VistaPrevia"] is { } colVistaPrevia)
        {
            colVistaPrevia.Caption = "Vista Previa";
        }

        if (_gridView.Columns["Impresion"] is { } colImpresion)
        {
            colImpresion.Caption = "Impresión";
        }

        if (_gridView.Columns["Exportacion"] is { } colExportacion)
        {
            colExportacion.Caption = "Exportación";
        }

        if (_gridView.Columns["Diseno"] is { } colDiseno)
        {
            colDiseno.Caption = "Diseño";
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

        var filasDto = _filas
            .Select(f => new ReportePermisoFilaDto(f.Codigo, f.Nombre, f.VistaPrevia, f.Impresion, f.Exportacion, f.Diseno))
            .ToList();

        await _reportePermisoService.GuardarAsync(rolId, filasDto);

        XtraMessageBox.Show(this, "Permisos de reportes guardados.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
