using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using FrontOne.Application.Services;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Acarreo;

// Captura tipo Excel: el grid es editable directo (celda por celda), con "Guardar cambios"
// persistiendo todas las filas pendientes de una sola vez — patrón distinto al resto del
// proyecto (listado + diálogo Nuevo/Editar), pedido explícitamente por el usuario.
public partial class ListaPrecioAcarreoForm : XtraForm
{
    private readonly ListaPrecioAcarreoService _listaPrecioAcarreoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly ZonaService _zonaService = null!;
    private BindingList<ListaPrecioAcarreoFila> _filas = new();
    private Dictionary<int, string> _estadoPorMunicipioId = new();

    public ListaPrecioAcarreoForm()
    {
        InitializeComponent();
    }

    public ListaPrecioAcarreoForm(
        ListaPrecioAcarreoService listaPrecioAcarreoService,
        MunicipioService municipioService,
        PaisService paisService,
        EstadoService estadoService,
        ZonaService zonaService)
        : this()
    {
        _listaPrecioAcarreoService = listaPrecioAcarreoService;
        _municipioService = municipioService;
        _paisService = paisService;
        _estadoService = estadoService;
        _zonaService = zonaService;

        _repoMunicipio.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _repoMunicipio.Buttons.Add(new EditorButton(ButtonPredefines.Plus) { ToolTip = "Agregar municipio" });
        _repoMunicipio.ButtonClick += RepoMunicipio_ButtonClick;

        _repoZona.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _repoZona.Buttons.Add(new EditorButton(ButtonPredefines.Plus) { ToolTip = "Agregar zona" });
        _repoZona.ButtonClick += RepoZona_ButtonClick;

        _gridView.CellValueChanged += GridView_CellValueChanged;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        await CargarMunicipiosAsync();
        await CargarZonasAsync();

        var lista = await _listaPrecioAcarreoService.ObtenerAsync();
        _filas = new BindingList<ListaPrecioAcarreoFila>(lista.Select(l => new ListaPrecioAcarreoFila
        {
            Id = l.Id,
            MunicipioId = l.MunicipioId,
            EstadoNombre = l.EstadoNombre,
            ZonaId = l.ZonaId,
            Precio300 = l.Precio300,
            Precio400 = l.Precio400,
            Precio500 = l.Precio500,
            ToleranciaKgFaltante = l.ToleranciaKgFaltante,
            Activo = l.Activo,
        }).ToList());
        _grid.DataSource = _filas;
    }

    // Recargan solo la fuente del lookup correspondiente (no tocan _filas/_grid.DataSource)
    // para no perder capturas sin guardar cuando el usuario da de alta un municipio/zona
    // nuevo a media captura.
    private async Task CargarMunicipiosAsync()
    {
        var municipios = await _municipioService.ObtenerAsync();
        var estados = (await _estadoService.ObtenerAsync()).ToDictionary(e => e.Id, e => e.Nombre);
        var municipiosConEstado = municipios
            .Select(m => new MunicipioListItem(m.Id, estados.GetValueOrDefault(m.EstadoId, string.Empty), m.Nombre))
            .ToList();
        _estadoPorMunicipioId = municipiosConEstado.ToDictionary(m => m.Id, m => m.EstadoNombre);
        _repoMunicipio.DataSource = municipiosConEstado;
        _repoMunicipio.ValueMember = "Id";
        _repoMunicipio.DisplayMember = "MunicipioNombre";
        _repoMunicipio.Columns.Clear();
        _repoMunicipio.Columns.Add(new LookUpColumnInfo("EstadoNombre", 160, "Estado"));
        _repoMunicipio.Columns.Add(new LookUpColumnInfo("MunicipioNombre", 220, "Municipio"));
        _repoMunicipio.PopupWidth = 400;
    }

    private async Task CargarZonasAsync()
    {
        var zonas = await _zonaService.ObtenerAsync();
        _repoZona.DataSource = zonas.ToList();
        _repoZona.ValueMember = "Id";
        _repoZona.DisplayMember = "Nombre";
        _repoZona.Columns.Clear();
        _repoZona.Columns.Add(new LookUpColumnInfo("Nombre", 200, "Zona"));
        _repoZona.PopupWidth = 220;
    }

    // Estado es de solo lectura: se autocompleta según el Municipio elegido, nunca se captura a mano.
    private void GridView_CellValueChanged(object? sender, CellValueChangedEventArgs e)
    {
        if (e.Column != _colMunicipio)
        {
            return;
        }

        var estadoNombre = e.Value is int municipioId && _estadoPorMunicipioId.TryGetValue(municipioId, out var nombre)
            ? nombre
            : null;
        _gridView.SetRowCellValue(e.RowHandle, _colEstado, estadoNombre);
    }

    private async void RepoMunicipio_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new MunicipiosForm(_municipioService, _paisService, _estadoService);
        form.ShowDialog(this);
        await CargarMunicipiosAsync();
    }

    private async void RepoZona_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new ZonasForm(_zonaService);
        form.ShowDialog(this);
        await CargarZonasAsync();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        _gridView.CloseEditor();
        _gridView.UpdateCurrentRow();

        foreach (var fila in _filas.ToList())
        {
            if (FilaVacia(fila))
            {
                continue;
            }

            if (!FilaCompleta(fila))
            {
                XtraMessageBox.Show(this, "Hay una fila incompleta: completa Municipio, Zona, los 3 precios y la tolerancia antes de guardar.",
                    "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (fila.Id == 0)
                {
                    fila.Id = await _listaPrecioAcarreoService.CrearAsync(
                        fila.MunicipioId!.Value, fila.ZonaId!.Value, fila.Precio300!.Value, fila.Precio400!.Value, fila.Precio500!.Value, fila.ToleranciaKgFaltante!.Value);
                }
                else
                {
                    await _listaPrecioAcarreoService.ActualizarAsync(
                        fila.Id, fila.MunicipioId!.Value, fila.ZonaId!.Value, fila.Precio300!.Value, fila.Precio400!.Value, fila.Precio500!.Value, fila.ToleranciaKgFaltante!.Value, fila.Activo);
                }
            }
            catch (ValidationException ex)
            {
                XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (SqlRepositoryException ex)
            {
                XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        await CargarDatosAsync();
        XtraMessageBox.Show(this, "Cambios guardados.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        if (_gridView.GetFocusedRow() is not ListaPrecioAcarreoFila fila)
        {
            XtraMessageBox.Show(this, "Selecciona una fila.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (fila.Id == 0)
        {
            _filas.Remove(fila);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, "¿Eliminar esta fila de la lista de precios?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _listaPrecioAcarreoService.EliminarAsync(fila.Id);
            _filas.Remove(fila);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private static bool FilaVacia(ListaPrecioAcarreoFila fila)
        => fila.MunicipioId is null && fila.ZonaId is null && fila.Precio300 is null
           && fila.Precio400 is null && fila.Precio500 is null && fila.ToleranciaKgFaltante is null;

    private static bool FilaCompleta(ListaPrecioAcarreoFila fila)
        => fila.MunicipioId is not null && fila.ZonaId is not null && fila.Precio300 is not null
           && fila.Precio400 is not null && fila.Precio500 is not null && fila.ToleranciaKgFaltante is not null;
}
