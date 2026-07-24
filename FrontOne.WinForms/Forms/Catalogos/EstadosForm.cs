using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class EstadosForm : XtraForm
{
    private readonly EstadoService _estadoService = null!;
    private readonly PaisService _paisService = null!;

    private IReadOnlyList<EstadoDto> _estadosCache = [];

    private record EstadoGridRow(int Id, string Pais, string Clave, string Nombre, bool Activo);

    public EstadosForm()
    {
        InitializeComponent();
    }

    public EstadosForm(EstadoService estadoService, PaisService paisService)
        : this()
    {
        _estadoService = estadoService;
        _paisService = paisService;

        _cmbFiltroPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbFiltroPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbFiltroPais.ButtonClick += CmbFiltroPais_ButtonClick;

        _cmbFiltroPais.EditValueChanged += async (_, _) => await CargarDatosAsync();
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

    private async Task CargarDatosAsync()
    {
        var paisId = _cmbFiltroPais.EditValue is int id && id != 0 ? id : (int?)null;

        _estadosCache = await _estadoService.ObtenerAsync(paisId);

        var paises = await _paisService.ObtenerAsync();
        var paisesPorId = paises.ToDictionary(p => p.Id, p => p.Nombre);

        var filas = _estadosCache
            .Select(e => new EstadoGridRow(e.Id, paisesPorId.GetValueOrDefault(e.PaisId, "-"), e.Clave, e.Nombre, e.Activo))
            .ToList();

        _grid.DataSource = filas;
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new EstadoEditarForm(_estadoService, _paisService, null);
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
            XtraMessageBox.Show(this, "Selecciona un estado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new EstadoEditarForm(_estadoService, _paisService, seleccionado);
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
            XtraMessageBox.Show(this, "Selecciona un estado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar el estado '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _estadoService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private EstadoDto? ObtenerSeleccionado()
    {
        if (_gridView.GetFocusedRow() is not EstadoGridRow fila)
        {
            return null;
        }

        return _estadosCache.FirstOrDefault(e => e.Id == fila.Id);
    }
}
