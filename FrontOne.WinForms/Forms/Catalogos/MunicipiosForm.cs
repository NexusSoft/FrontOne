using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class MunicipiosForm : XtraForm
{
    private readonly MunicipioService _municipioService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;

    private IReadOnlyList<MunicipioDto> _municipiosCache = [];

    private record MunicipioGridRow(int Id, string Estado, string Nombre, bool Activo);

    public MunicipiosForm()
    {
        InitializeComponent();
    }

    public MunicipiosForm(MunicipioService municipioService, PaisService paisService, EstadoService estadoService)
        : this()
    {
        _municipioService = municipioService;
        _paisService = paisService;
        _estadoService = estadoService;

        _cmbFiltroPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbFiltroPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbFiltroPais.ButtonClick += CmbFiltroPais_ButtonClick;

        _cmbFiltroEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbFiltroEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbFiltroEstado.ButtonClick += CmbFiltroEstado_ButtonClick;

        _cmbFiltroPais.EditValueChanged += async (_, _) => await CargarEstadosFiltroAsync(seleccionarTodos: true);
        _cmbFiltroEstado.EditValueChanged += async (_, _) => await CargarDatosAsync();
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

    private async Task CargarDatosAsync()
    {
        var estadoId = _cmbFiltroEstado.EditValue is int id && id != 0 ? id : (int?)null;

        _municipiosCache = await _municipioService.ObtenerAsync(estadoId);

        var estados = await _estadoService.ObtenerAsync();
        var estadosPorId = estados.ToDictionary(e => e.Id, e => e.Nombre);

        var filas = _municipiosCache
            .Select(m => new MunicipioGridRow(m.Id, estadosPorId.GetValueOrDefault(m.EstadoId, "-"), m.Nombre, m.Activo))
            .ToList();

        _grid.DataSource = filas;
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new MunicipioEditarForm(_municipioService, _paisService, _estadoService, null);
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
            XtraMessageBox.Show(this, "Selecciona un municipio.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new MunicipioEditarForm(_municipioService, _paisService, _estadoService, seleccionado);
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
            XtraMessageBox.Show(this, "Selecciona un municipio.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el municipio '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _municipioService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private MunicipioDto? ObtenerSeleccionado()
    {
        if (_gridView.GetFocusedRow() is not MunicipioGridRow fila)
        {
            return null;
        }

        return _municipiosCache.FirstOrDefault(m => m.Id == fila.Id);
    }
}
