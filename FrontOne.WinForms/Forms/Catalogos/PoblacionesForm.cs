using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class PoblacionesForm : XtraForm
{
    private readonly PoblacionService _poblacionService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;

    private IReadOnlyList<PoblacionDto> _poblacionesCache = [];

    private record PoblacionGridRow(int Id, string Nombre, string Municipio, string Estado, bool Activo);

    public PoblacionesForm()
    {
        InitializeComponent();
    }

    public PoblacionesForm(PoblacionService poblacionService, PaisService paisService, EstadoService estadoService, MunicipioService municipioService)
        : this()
    {
        _poblacionService = poblacionService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;

        _cmbFiltroPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbFiltroPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbFiltroPais.ButtonClick += CmbFiltroPais_ButtonClick;

        _cmbFiltroEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbFiltroEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbFiltroEstado.ButtonClick += CmbFiltroEstado_ButtonClick;

        _cmbFiltroMunicipio.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbFiltroMunicipio.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbFiltroMunicipio.ButtonClick += CmbFiltroMunicipio_ButtonClick;

        _cmbFiltroPais.EditValueChanged += async (_, _) => await CargarEstadosFiltroAsync(seleccionarTodos: true);
        _cmbFiltroEstado.EditValueChanged += async (_, _) => await CargarMunicipiosFiltroAsync(seleccionarTodos: true);
        _cmbFiltroMunicipio.EditValueChanged += async (_, _) => await CargarDatosAsync();
        Load += async (_, _) => await CargarPaisesFiltroAsync(seleccionarTodos: true);
    }

    private async Task CargarPaisesFiltroAsync(bool seleccionarTodos)
    {
        var paisSeleccionado = _cmbFiltroPais.EditValue;

        var paises = await _paisService.ObtenerAsync();

        var items = new List<PaisDto> { new(0, "***", "Todos los países", true) };
        items.AddRange(paises);

        _cmbFiltroPais.Properties.DataSource = items;
        _cmbFiltroPais.Properties.ValueMember = "Id";
        _cmbFiltroPais.Properties.DisplayMember = "Nombre";
        _cmbFiltroPais.Properties.Columns.Clear();
        _cmbFiltroPais.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "País"));
        _cmbFiltroPais.Properties.PopupWidth = 250;
        _cmbFiltroPais.EditValue = seleccionarTodos ? 0 : paisSeleccionado;
    }

    private async Task CargarEstadosFiltroAsync(bool seleccionarTodos)
    {
        var estadoSeleccionado = _cmbFiltroEstado.EditValue;

        var paisId = _cmbFiltroPais.EditValue is int id && id != 0 ? id : (int?)null;
        var estados = (await _estadoService.ObtenerAsync(paisId)).ToList();

        var items = new List<EstadoDto> { new(0, paisId ?? 0, "***", "Todos los estados", true) };
        items.AddRange(estados);

        _cmbFiltroEstado.Properties.DataSource = items;
        _cmbFiltroEstado.Properties.ValueMember = "Id";
        _cmbFiltroEstado.Properties.DisplayMember = "Nombre";
        _cmbFiltroEstado.Properties.Columns.Clear();
        _cmbFiltroEstado.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Estado"));
        _cmbFiltroEstado.Properties.PopupWidth = 250;
        _cmbFiltroEstado.EditValue = seleccionarTodos ? 0 : estadoSeleccionado;
    }

    private async Task CargarMunicipiosFiltroAsync(bool seleccionarTodos)
    {
        var municipioSeleccionado = _cmbFiltroMunicipio.EditValue;

        var estadoId = _cmbFiltroEstado.EditValue is int id && id != 0 ? id : (int?)null;
        var municipios = (await _municipioService.ObtenerAsync(estadoId)).ToList();

        var items = new List<MunicipioDto> { new(0, "*** Todos los municipios", estadoId ?? 0, true) };
        items.AddRange(municipios);

        _cmbFiltroMunicipio.Properties.DataSource = items;
        _cmbFiltroMunicipio.Properties.ValueMember = "Id";
        _cmbFiltroMunicipio.Properties.DisplayMember = "Nombre";
        _cmbFiltroMunicipio.Properties.Columns.Clear();
        _cmbFiltroMunicipio.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Municipio"));
        _cmbFiltroMunicipio.Properties.PopupWidth = 250;
        _cmbFiltroMunicipio.EditValue = seleccionarTodos ? 0 : municipioSeleccionado;
    }

    private async void CmbFiltroPais_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new PaisesForm(_paisService);
        form.ShowDialog(this);
        await CargarPaisesFiltroAsync(seleccionarTodos: false);
    }

    private async void CmbFiltroEstado_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new EstadosForm(_estadoService, _paisService);
        form.ShowDialog(this);
        await CargarEstadosFiltroAsync(seleccionarTodos: false);
    }

    private async void CmbFiltroMunicipio_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new MunicipiosForm(_municipioService, _paisService, _estadoService);
        form.ShowDialog(this);
        await CargarMunicipiosFiltroAsync(seleccionarTodos: false);
    }

    private async Task CargarDatosAsync()
    {
        var municipioId = _cmbFiltroMunicipio.EditValue is int id && id != 0 ? id : (int?)null;

        _poblacionesCache = await _poblacionService.ObtenerAsync(municipioId);

        var municipios = await _municipioService.ObtenerAsync();
        var municipiosPorId = municipios.ToDictionary(m => m.Id, m => m);

        var estados = await _estadoService.ObtenerAsync();
        var estadosPorId = estados.ToDictionary(e => e.Id, e => e.Nombre);

        var filas = _poblacionesCache
            .Select(p =>
            {
                var municipio = municipiosPorId.GetValueOrDefault(p.MunicipioId);
                var nombreEstado = municipio is not null ? estadosPorId.GetValueOrDefault(municipio.EstadoId, "-") : "-";
                return new PoblacionGridRow(p.Id, p.Nombre, municipio?.Nombre ?? "-", nombreEstado, p.Activo);
            })
            .ToList();

        _grid.DataSource = filas;
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        var municipioFiltro = _cmbFiltroMunicipio.EditValue is int id && id != 0 ? id : (int?)null;

        using var form = new PoblacionEditarForm(_poblacionService, _paisService, _estadoService, _municipioService, null, municipioFiltro);
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
            XtraMessageBox.Show(this, "Selecciona una población.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new PoblacionEditarForm(_poblacionService, _paisService, _estadoService, _municipioService, seleccionado, null);
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
            XtraMessageBox.Show(this, "Selecciona una población.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar la población '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _poblacionService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private PoblacionDto? ObtenerSeleccionado()
    {
        if (_gridView.GetFocusedRow() is not PoblacionGridRow fila)
        {
            return null;
        }

        return _poblacionesCache.FirstOrDefault(p => p.Id == fila.Id);
    }
}
