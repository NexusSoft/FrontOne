using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class MateriaPrimaEditarForm : XtraForm
{
    private const string Modulo = "Catalogos";
    private const string PantallaCategorias = "Categorias";
    private const string PantallaCalibresApeam = "CalibresApeam";
    private const string AccionCrear = "Crear";

    private readonly MateriaPrimaService _materiaPrimaService = null!;
    private readonly CategoriaService _categoriaService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly MateriaPrimaDto _materiaPrimaExistente = null!;

    public MateriaPrimaEditarForm()
    {
        InitializeComponent();
    }

    public MateriaPrimaEditarForm(
        MateriaPrimaService materiaPrimaService,
        CategoriaService categoriaService,
        CalibreApeamService calibreApeamService,
        SessionContext sessionContext,
        MateriaPrimaDto materiaPrimaExistente)
        : this()
    {
        _materiaPrimaService = materiaPrimaService;
        _categoriaService = categoriaService;
        _calibreApeamService = calibreApeamService;
        _sessionContext = sessionContext;
        _materiaPrimaExistente = materiaPrimaExistente;

        Text = $"FrontOne - Editar materia prima ({materiaPrimaExistente.CodigoSap})";

        Load += async (_, _) => await CargarDatosIniciales();
    }

    private async Task CargarDatosIniciales()
    {
        // useFadeIn: false — con fade-in activo el splash se muestra de forma asíncrona; si la
        // carga termina antes de que la animación registre el splash como visible,
        // CloseDefaultWaitForm truena con "Splash Form is not displayed".
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: false, useFadeOut: true, "FrontOne", "Consultando datos...");
        try
        {
            await CargarCategoriasAsync();
            await CargarCalibresApeamAsync();

            MostrarMateriaPrimaEnFormulario();
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }
    }

    private void MostrarMateriaPrimaEnFormulario()
    {
        var m = _materiaPrimaExistente;

        _txtCodigoSap.Text = m.CodigoSap;
        _txtDescripcionSap.Text = m.DescripcionSap;
        _cmbCategoria.EditValue = m.CategoriaId;
        _cmbCalibreApeam.EditValue = m.CalibreApeamId;

        // Una materia prima ya desactivada en SAP queda en modo solo lectura: no tiene sentido
        // seguir capturando información de negocio de algo que SAP ya no reconoce como vigente.
        if (!m.Activo)
        {
            _cmbCategoria.Properties.ReadOnly = true;
            _cmbCalibreApeam.Properties.ReadOnly = true;
            _btnGuardar.Enabled = false;
        }
    }

    private async Task CargarCategoriasAsync()
    {
        var categorias = await _categoriaService.ObtenerAsync();
        _cmbCategoria.Properties.DataSource = categorias.ToList();
        _cmbCategoria.Properties.ValueMember = "Id";
        _cmbCategoria.Properties.DisplayMember = "Nombre";
        _cmbCategoria.Properties.Columns.Clear();
        _cmbCategoria.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Categoría"));
        _cmbCategoria.Properties.PopupWidth = 250;
    }

    private async Task CargarCalibresApeamAsync()
    {
        var calibres = await _calibreApeamService.ObtenerAsync();
        _cmbCalibreApeam.Properties.DataSource = calibres.ToList();
        _cmbCalibreApeam.Properties.ValueMember = "Id";
        _cmbCalibreApeam.Properties.DisplayMember = "Nombre";
        _cmbCalibreApeam.Properties.Columns.Clear();
        _cmbCalibreApeam.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Calibre APEAM"));
        _cmbCalibreApeam.Properties.PopupWidth = 250;
    }

    private async void CmbCategoria_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaCategorias))
        {
            return;
        }

        using var form = new CategoriasForm(_categoriaService);
        form.ShowDialog(this);
        await CargarCategoriasAsync();
    }

    private async void CmbCalibreApeam_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus || !TienePermisoCrear(PantallaCalibresApeam))
        {
            return;
        }

        using var form = new CalibresApeamForm(_calibreApeamService);
        form.ShowDialog(this);
        await CargarCalibresApeamAsync();
    }

    private bool TienePermisoCrear(string pantalla)
    {
        if (_sessionContext.TienePermiso(Modulo, pantalla, AccionCrear))
        {
            return true;
        }

        XtraMessageBox.Show(this, "No tienes permiso para acceder a este formulario.", "FrontOne",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void BtnCancelar_Click(object? sender, EventArgs e) => Close();

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var confirmar = XtraMessageBox.Show(this, "¿Guardar los cambios de esta materia prima?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        var datos = new MateriaPrimaDto(
            _materiaPrimaExistente.Id,
            _materiaPrimaExistente.CodigoSap,
            _materiaPrimaExistente.DescripcionSap,
            _materiaPrimaExistente.Activo,
            _cmbCategoria.EditValue as int?,
            _cmbCalibreApeam.EditValue as int?,
            _materiaPrimaExistente.FechaCreacion);

        try
        {
            await _materiaPrimaService.ActualizarAsync(datos);

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
        catch (Exception ex)
        {
            XtraMessageBox.Show(this, $"No se pudo guardar la materia prima.\n\n{ex.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
