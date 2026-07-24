using System.ComponentModel;
using System.Drawing.Drawing2D;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class ProductorEditarForm : XtraForm
{
    private readonly ProductorService _productorService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly HuertaService _huertaService = null!;
    private readonly PoblacionService _poblacionService = null!;

    private ProductorDto? _productorActual;

    private record HuertaDelProductorRow(string Nombre, string RegistroSagarpa, string Pais, string Estado, string Poblacion, string Municipio, string EstadoHuerta);

    public ProductorEditarForm()
    {
        InitializeComponent();
        AsignarIconosNavegacion();
    }

    public ProductorEditarForm(
        ProductorService productorService,
        PaisService paisService,
        EstadoService estadoService,
        MunicipioService municipioService,
        HuertaService huertaService,
        PoblacionService poblacionService)
        : this()
    {
        _productorService = productorService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;
        _huertaService = huertaService;
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
        => await CargarProductorEnFormularioAsync(await _productorService.ObtenerPrimeroAsync());

    private async void BtnFin_Click(object? sender, EventArgs e)
        => await CargarProductorEnFormularioAsync(await _productorService.ObtenerUltimoAsync());

    private async void BtnAnterior_Click(object? sender, EventArgs e)
    {
        if (_productorActual is null)
        {
            return;
        }

        var anterior = await _productorService.ObtenerAnteriorAsync(_productorActual.Id);
        if (anterior is not null)
        {
            await CargarProductorEnFormularioAsync(anterior);
        }
    }

    private async void BtnSiguiente_Click(object? sender, EventArgs e)
    {
        var siguiente = _productorActual is null
            ? await _productorService.ObtenerPrimeroAsync()
            : await _productorService.ObtenerSiguienteAsync(_productorActual.Id);

        if (siguiente is not null)
        {
            await CargarProductorEnFormularioAsync(siguiente);
        }
    }

    // Íconos de navegación dibujados por código (sin depender de archivos externos
    // ni de licencias de terceros) — mismo criterio que HuertaEditarForm.
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

    private async Task CargarProductorEnFormularioAsync(ProductorDto? productor)
    {
        _productorActual = productor;

        if (productor is null)
        {
            LimpiarFormulario();
            return;
        }

        _txtClave.Text = productor.Clave;
        _txtFechaRegistro.Text = productor.FechaRegistro.ToString("dd/MM/yyyy");
        _txtNombreProductor.Text = productor.NombreProductor;
        _txtDomicilio.Text = productor.Domicilio ?? string.Empty;
        _txtColonia.Text = productor.Colonia ?? string.Empty;
        _txtCodigoPostal.Text = productor.CodigoPostal ?? string.Empty;
        _txtRfc.Text = productor.Rfc ?? string.Empty;
        _txtTelefono.Text = productor.Telefono ?? string.Empty;
        _txtCelular.Text = productor.Celular ?? string.Empty;
        _txtEmail.Text = productor.Email ?? string.Empty;
        _txtOrganizacion.Text = productor.Organizacion ?? string.Empty;
        _txtObservaciones.Text = productor.Observaciones ?? string.Empty;
        _txtUsuario.Text = productor.Usuario ?? string.Empty;
        _txtPassword.Text = productor.Password ?? string.Empty;
        _spnDiasCredito.Value = productor.DiasCredito;
        _chkObsoleto.Checked = !productor.Activo;

        await CargarHuertasDelProductorAsync(productor.Id);

        if (productor.EstadoId is not null)
        {
            var estados = await _estadoService.ObtenerAsync();
            var estado = estados.FirstOrDefault(e => e.Id == productor.EstadoId);
            if (estado is not null)
            {
                _cmbPais.EditValue = estado.PaisId;
                await CargarEstadosDelPaisAsync(seleccionarPrimero: false);
                _cmbEstado.EditValue = estado.Id;
                await CargarMunicipiosDelEstadoAsync(seleccionarPrimero: false);
                _cmbMunicipio.EditValue = productor.MunicipioId;
                await CargarPoblacionesDelMunicipioAsync(seleccionarPrimero: false);
                _cmbPoblacion.EditValue = productor.PoblacionId;
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

    private async Task CargarHuertasDelProductorAsync(int productorId)
    {
        // Filtro server-side: con 54k huertas en catálogo, bajar todo por cada navegación saturaba el pool de conexiones.
        var huertas = await _huertaService.ObtenerPorProductorAsync(productorId);

        var paises = await _paisService.ObtenerAsync();
        var paisesPorId = paises.ToDictionary(p => p.Id, p => p.Nombre);

        var estados = await _estadoService.ObtenerAsync();
        var estadosPorId = estados.ToDictionary(e => e.Id, e => e);

        var poblaciones = await _poblacionService.ObtenerAsync();
        var poblacionesPorId = poblaciones.ToDictionary(p => p.Id, p => p.Nombre);

        var municipios = await _municipioService.ObtenerAsync();
        var municipiosPorId = municipios.ToDictionary(m => m.Id, m => m.Nombre);

        var filas = huertas.Select(h =>
        {
            var estado = h.EstadoId is not null && estadosPorId.TryGetValue(h.EstadoId.Value, out var e) ? e : null;
            var nombrePais = estado is not null && paisesPorId.TryGetValue(estado.PaisId, out var pais) ? pais : "-";
            var nombreEstado = estado?.Nombre ?? "-";
            var nombrePoblacion = h.PoblacionId is not null && poblacionesPorId.TryGetValue(h.PoblacionId.Value, out var poblacion) ? poblacion : "-";
            var nombreMunicipio = h.MunicipioId is not null && municipiosPorId.TryGetValue(h.MunicipioId.Value, out var municipio) ? municipio : "-";

            return new HuertaDelProductorRow(
                h.Nombre,
                h.RegistroSagarpa ?? "-",
                nombrePais,
                nombreEstado,
                nombrePoblacion,
                nombreMunicipio,
                h.Activo ? "Activa" : "Baja");
        }).ToList();

        _gridHuertas.DataSource = filas;
    }

    private void LimpiarFormulario()
    {
        _productorActual = null;

        _txtClave.Text = "(Se genera al guardar)";
        _txtFechaRegistro.Text = DateTime.Today.ToString("dd/MM/yyyy");
        _txtNombreProductor.Text = string.Empty;
        _txtDomicilio.Text = string.Empty;
        _txtColonia.Text = string.Empty;
        _txtCodigoPostal.Text = string.Empty;
        _cmbPais.EditValue = null;
        _cmbEstado.EditValue = null;
        _cmbMunicipio.Properties.DataSource = null;
        _cmbMunicipio.EditValue = null;
        _cmbPoblacion.Properties.DataSource = null;
        _cmbPoblacion.EditValue = null;
        _txtRfc.Text = string.Empty;
        _txtTelefono.Text = string.Empty;
        _txtCelular.Text = string.Empty;
        _txtEmail.Text = string.Empty;
        _txtOrganizacion.Text = string.Empty;
        _txtObservaciones.Text = string.Empty;
        _txtUsuario.Text = string.Empty;
        _txtPassword.Text = string.Empty;
        _spnDiasCredito.Value = 0;
        _chkObsoleto.Checked = false;
        _gridHuertas.DataSource = null;
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
        if (_productorActual is null)
        {
            XtraMessageBox.Show(this, "No hay ningún productor cargado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el productor '{_productorActual.NombreProductor}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _productorService.EliminarAsync(_productorActual.Id);
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

    private async void TxtNombreProductor_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Search)
        {
            return;
        }

        using var form = new ProductoresForm(_productorService, _paisService, _estadoService, modoSeleccion: true);
        if (form.ShowDialog(this) != DialogResult.OK || form.ProductorSeleccionado is null)
        {
            return;
        }

        var productor = await _productorService.ObtenerPorIdAsync(form.ProductorSeleccionado.Id);
        await CargarProductorEnFormularioAsync(productor);
    }

    private void BtnCancelar_Click(object? sender, EventArgs e) => Close();

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var estadoId = _cmbEstado.EditValue is int id ? id : (int?)null;

        var datos = new ProductorDto(
            _productorActual?.Id ?? 0,
            _productorActual?.Clave ?? string.Empty,
            _productorActual?.FechaRegistro ?? DateTime.Today,
            _txtNombreProductor.Text.Trim(),
            NuloSiVacio(_txtDomicilio.Text),
            NuloSiVacio(_txtColonia.Text),
            NuloSiVacio(_txtCodigoPostal.Text),
            _cmbPoblacion.EditValue as int?,
            _cmbMunicipio.EditValue as int?,
            estadoId,
            NuloSiVacio(_txtRfc.Text),
            NuloSiVacio(_txtTelefono.Text),
            NuloSiVacio(_txtCelular.Text),
            NuloSiVacio(_txtEmail.Text),
            NuloSiVacio(_txtOrganizacion.Text),
            NuloSiVacio(_txtObservaciones.Text),
            NuloSiVacio(_txtUsuario.Text),
            NuloSiVacio(_txtPassword.Text),
            (int)_spnDiasCredito.Value,
            !_chkObsoleto.Checked);

        try
        {
            int idGuardado;

            if (_productorActual is null)
            {
                var (nuevoId, _) = await _productorService.CrearAsync(datos);
                idGuardado = nuevoId;
            }
            else
            {
                await _productorService.ActualizarAsync(datos);
                idGuardado = _productorActual.Id;
            }

            var productorGuardado = await _productorService.ObtenerPorIdAsync(idGuardado);
            await CargarProductorEnFormularioAsync(productorGuardado);

            XtraMessageBox.Show(this, "Productor guardado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
