using System.Drawing.Drawing2D;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Acopio;

// Clon del patrón de ProductorEditarForm: pantalla única con navegación por todo el
// catálogo (Inicio/Anterior/Siguiente/Fin) y búsqueda embebida por Nombre, en vez del
// patrón de listado + diálogo que usa el resto de los catálogos del proyecto.
public partial class JefeAcopioEditarForm : XtraForm
{
    private readonly JefeAcopioService _jefeAcopioService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PoblacionService _poblacionService = null!;

    private JefeAcopioDto? _jefeAcopioActual;

    public JefeAcopioEditarForm()
    {
        InitializeComponent();
        AsignarIconosNavegacion();
    }

    public JefeAcopioEditarForm(
        JefeAcopioService jefeAcopioService,
        PaisService paisService,
        EstadoService estadoService,
        MunicipioService municipioService,
        PoblacionService poblacionService)
        : this()
    {
        _jefeAcopioService = jefeAcopioService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;
        _poblacionService = poblacionService;

        _cmbPais.EditValueChanged += async (_, _) => await CargarEstadosDelPaisAsync(seleccionarPrimero: true);
        _cmbEstado.EditValueChanged += async (_, _) => await CargarMunicipiosDelEstadoAsync(seleccionarPrimero: false);
        _cmbMunicipio.EditValueChanged += async (_, _) => await CargarPoblacionesDelMunicipioAsync(seleccionarPrimero: false);

        Load += async (_, _) => await CargarDatosIniciales();
    }

    private async Task CargarDatosIniciales()
    {
        await CargarPaisesAsync();
        LimpiarFormulario();
    }

    // Navegación del catálogo completo por Id de creación, sin cargar nada a
    // memoria: cada botón pide un solo registro al servidor (seek por índice).
    private async void BtnInicio_Click(object? sender, EventArgs e)
        => await CargarJefeAcopioEnFormularioAsync(await _jefeAcopioService.ObtenerPrimeroAsync());

    private async void BtnFin_Click(object? sender, EventArgs e)
        => await CargarJefeAcopioEnFormularioAsync(await _jefeAcopioService.ObtenerUltimoAsync());

    private async void BtnAnterior_Click(object? sender, EventArgs e)
    {
        if (_jefeAcopioActual is null)
        {
            return;
        }

        var anterior = await _jefeAcopioService.ObtenerAnteriorAsync(_jefeAcopioActual.Id);
        if (anterior is not null)
        {
            await CargarJefeAcopioEnFormularioAsync(anterior);
        }
    }

    private async void BtnSiguiente_Click(object? sender, EventArgs e)
    {
        var siguiente = _jefeAcopioActual is null
            ? await _jefeAcopioService.ObtenerPrimeroAsync()
            : await _jefeAcopioService.ObtenerSiguienteAsync(_jefeAcopioActual.Id);

        if (siguiente is not null)
        {
            await CargarJefeAcopioEnFormularioAsync(siguiente);
        }
    }

    // Íconos de navegación dibujados por código (sin depender de archivos externos
    // ni de licencias de terceros) — mismo criterio que ProductorEditarForm/HuertaEditarForm.
    private void AsignarIconosNavegacion()
    {
        _btnInicio.ImageOptions.Image = CrearIconoFlecha(conBarra: true, apuntaDerecha: false);
        _btnAnterior.ImageOptions.Image = CrearIconoFlecha(conBarra: false, apuntaDerecha: false);
        _btnSiguiente.ImageOptions.Image = CrearIconoFlecha(conBarra: false, apuntaDerecha: true);
        _btnFin.ImageOptions.Image = CrearIconoFlecha(conBarra: true, apuntaDerecha: true);
    }

    private static Bitmap CrearIconoFlecha(bool conBarra, bool apuntaDerecha)
    {
        const int tam = 16;
        var bitmap = new Bitmap(tam, tam);
        using var g = Graphics.FromImage(bitmap);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var pincel = new SolidBrush(Color.CornflowerBlue);

        if (apuntaDerecha)
        {
            g.FillPolygon(pincel, [new Point(4, 2), new Point(4, 14), new Point(12, 8)]);
            if (conBarra)
            {
                g.FillRectangle(pincel, 12, 2, 2, 12);
            }
        }
        else
        {
            g.FillPolygon(pincel, [new Point(12, 2), new Point(12, 14), new Point(4, 8)]);
            if (conBarra)
            {
                g.FillRectangle(pincel, 2, 2, 2, 12);
            }
        }

        return bitmap;
    }

    private async Task CargarJefeAcopioEnFormularioAsync(JefeAcopioDto? jefeAcopio)
    {
        _jefeAcopioActual = jefeAcopio;

        if (jefeAcopio is null)
        {
            LimpiarFormulario();
            return;
        }

        _txtClave.Text = jefeAcopio.Clave;
        _txtNombre.Text = jefeAcopio.Nombre;
        _txtDomicilio.Text = jefeAcopio.Domicilio ?? string.Empty;
        _txtColonia.Text = jefeAcopio.Colonia ?? string.Empty;
        _txtCodigoPostal.Text = jefeAcopio.CodigoPostal ?? string.Empty;
        _txtTelefono.Text = jefeAcopio.Telefono ?? string.Empty;
        _txtCelular.Text = jefeAcopio.Celular ?? string.Empty;
        _txtEmail.Text = jefeAcopio.Email ?? string.Empty;
        _txtObservaciones.Text = jefeAcopio.Observaciones ?? string.Empty;
        _chkActivo.Checked = jefeAcopio.Activo;

        if (jefeAcopio.EstadoId is not null)
        {
            var estados = await _estadoService.ObtenerAsync();
            var estado = estados.FirstOrDefault(e => e.Id == jefeAcopio.EstadoId);
            if (estado is not null)
            {
                _cmbPais.EditValue = estado.PaisId;
                await CargarEstadosDelPaisAsync(seleccionarPrimero: false);
                _cmbEstado.EditValue = estado.Id;
                await CargarMunicipiosDelEstadoAsync(seleccionarPrimero: false);
                _cmbMunicipio.EditValue = jefeAcopio.MunicipioId;
                await CargarPoblacionesDelMunicipioAsync(seleccionarPrimero: false);
                _cmbPoblacion.EditValue = jefeAcopio.PoblacionId;
                return;
            }
        }

        _cmbPais.EditValue = null;
        _cmbEstado.EditValue = null;
        _cmbMunicipio.Properties.DataSource = null;
        _cmbMunicipio.EditValue = null;
        _cmbPoblacion.Properties.DataSource = null;
        _cmbPoblacion.EditValue = null;
    }

    private void LimpiarFormulario()
    {
        _jefeAcopioActual = null;

        _txtClave.Text = "(Se genera al guardar)";
        _txtNombre.Text = string.Empty;
        _txtDomicilio.Text = string.Empty;
        _txtColonia.Text = string.Empty;
        _txtCodigoPostal.Text = string.Empty;
        _cmbPais.EditValue = null;
        _cmbEstado.EditValue = null;
        _cmbMunicipio.Properties.DataSource = null;
        _cmbMunicipio.EditValue = null;
        _cmbPoblacion.Properties.DataSource = null;
        _cmbPoblacion.EditValue = null;
        _txtTelefono.Text = string.Empty;
        _txtCelular.Text = string.Empty;
        _txtEmail.Text = string.Empty;
        _txtObservaciones.Text = string.Empty;
        _chkActivo.Checked = true;
    }

    private async Task CargarPaisesAsync()
    {
        var paises = await _paisService.ObtenerAsync();

        _cmbPais.Properties.DataSource = paises.ToList();
        _cmbPais.Properties.ValueMember = "Id";
        _cmbPais.Properties.DisplayMember = "Nombre";
        _cmbPais.Properties.Columns.Clear();
        _cmbPais.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "País"));
        _cmbPais.Properties.PopupWidth = 250;
    }

    private async Task CargarEstadosDelPaisAsync(bool seleccionarPrimero)
    {
        if (_cmbPais.EditValue is not int paisId)
        {
            _cmbEstado.Properties.DataSource = null;
            return;
        }

        var estadosDelPais = await _estadoService.ObtenerAsync(paisId);

        _cmbEstado.Properties.DataSource = estadosDelPais.ToList();
        _cmbEstado.Properties.ValueMember = "Id";
        _cmbEstado.Properties.DisplayMember = "Nombre";
        _cmbEstado.Properties.Columns.Clear();
        _cmbEstado.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Estado"));
        _cmbEstado.Properties.PopupWidth = 250;

        if (seleccionarPrimero)
        {
            _cmbEstado.EditValue = estadosDelPais.Count > 0 ? estadosDelPais[0].Id : null;
        }
    }

    private async Task CargarMunicipiosDelEstadoAsync(bool seleccionarPrimero)
    {
        if (_cmbEstado.EditValue is not int estadoId)
        {
            _cmbMunicipio.Properties.DataSource = null;
            _cmbMunicipio.EditValue = null;
            return;
        }

        var municipios = await _municipioService.ObtenerAsync(estadoId);
        _cmbMunicipio.Properties.DataSource = municipios.ToList();
        _cmbMunicipio.Properties.ValueMember = "Id";
        _cmbMunicipio.Properties.DisplayMember = "Nombre";
        _cmbMunicipio.Properties.Columns.Clear();
        _cmbMunicipio.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Municipio"));
        _cmbMunicipio.Properties.PopupWidth = 250;

        if (seleccionarPrimero)
        {
            _cmbMunicipio.EditValue = municipios.Count > 0 ? municipios[0].Id : null;
        }
    }

    private async Task CargarPoblacionesDelMunicipioAsync(bool seleccionarPrimero)
    {
        if (_cmbMunicipio.EditValue is not int municipioId)
        {
            _cmbPoblacion.Properties.DataSource = null;
            _cmbPoblacion.EditValue = null;
            return;
        }

        var poblaciones = await _poblacionService.ObtenerAsync(municipioId);
        _cmbPoblacion.Properties.DataSource = poblaciones.ToList();
        _cmbPoblacion.Properties.ValueMember = "Id";
        _cmbPoblacion.Properties.DisplayMember = "Nombre";
        _cmbPoblacion.Properties.Columns.Clear();
        _cmbPoblacion.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Población"));
        _cmbPoblacion.Properties.PopupWidth = 250;

        if (seleccionarPrimero)
        {
            _cmbPoblacion.EditValue = poblaciones.Count > 0 ? poblaciones[0].Id : null;
        }
    }

    private void BtnNuevo_Click(object? sender, EventArgs e) => LimpiarFormulario();

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        if (_jefeAcopioActual is null)
        {
            XtraMessageBox.Show(this, "No hay ningún jefe de acopio cargado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el jefe de acopio '{_jefeAcopioActual.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _jefeAcopioService.EliminarAsync(_jefeAcopioActual.Id);
            LimpiarFormulario();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void CmbPais_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new PaisesForm(_paisService);
        form.ShowDialog(this);
        await CargarPaisesAsync();
    }

    private async void CmbEstado_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new EstadosForm(_estadoService, _paisService);
        form.ShowDialog(this);
        await CargarEstadosDelPaisAsync(seleccionarPrimero: false);
    }

    private async void CmbMunicipio_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new MunicipiosForm(_municipioService, _paisService, _estadoService);
        form.ShowDialog(this);
        await CargarMunicipiosDelEstadoAsync(seleccionarPrimero: false);
    }

    private async void CmbPoblacion_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new PoblacionesForm(_poblacionService, _paisService, _estadoService, _municipioService);
        form.ShowDialog(this);
        await CargarPoblacionesDelMunicipioAsync(seleccionarPrimero: false);
    }

    private async void TxtNombre_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Search)
        {
            return;
        }

        using var form = new JefesAcopioForm(_jefeAcopioService, _estadoService);
        if (form.ShowDialog(this) != DialogResult.OK || form.JefeAcopioSeleccionado is null)
        {
            return;
        }

        var jefeAcopio = await _jefeAcopioService.ObtenerPorIdAsync(form.JefeAcopioSeleccionado.Id);
        await CargarJefeAcopioEnFormularioAsync(jefeAcopio);
    }

    private void BtnCancelar_Click(object? sender, EventArgs e) => Close();

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNombre.Text))
        {
            XtraMessageBox.Show(this, "Captura el nombre del jefe de acopio.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var datos = new JefeAcopioDto(
            _jefeAcopioActual?.Id ?? 0,
            _jefeAcopioActual?.Clave ?? string.Empty,
            _txtNombre.Text.Trim(),
            NuloSiVacio(_txtDomicilio.Text),
            NuloSiVacio(_txtColonia.Text),
            NuloSiVacio(_txtCodigoPostal.Text),
            _cmbPoblacion.EditValue as int?,
            _cmbMunicipio.EditValue as int?,
            _cmbEstado.EditValue as int?,
            NuloSiVacio(_txtTelefono.Text),
            NuloSiVacio(_txtCelular.Text),
            NuloSiVacio(_txtEmail.Text),
            NuloSiVacio(_txtObservaciones.Text),
            _chkActivo.Checked);

        try
        {
            int idGuardado;

            if (_jefeAcopioActual is null)
            {
                var (nuevoId, _) = await _jefeAcopioService.CrearAsync(datos);
                idGuardado = nuevoId;
            }
            else
            {
                await _jefeAcopioService.ActualizarAsync(datos);
                idGuardado = _jefeAcopioActual.Id;
            }

            var jefeAcopioGuardado = await _jefeAcopioService.ObtenerPorIdAsync(idGuardado);
            await CargarJefeAcopioEnFormularioAsync(jefeAcopioGuardado);

            XtraMessageBox.Show(this, "Jefe de acopio guardado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    private static string? NuloSiVacio(string valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
