using System.ComponentModel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class ListaPrecioFrutaForm : XtraForm
{
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly ProductorService _productorService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private BindingList<FilaListaPrecioFruta> _filas = [];

    // Productor opcional: null = lista general, con valor = lista especial solo para ese
    // productor. Convive con la lista general del mismo ItemCode/día (no chocan entre sí).
    private int? _productorId;

    // null = modo captura (Consultar SAP, Guardar inserta). No-null = modo edición (se cargó
    // una fecha ya guardada vía Buscar Lista de Precios, Guardar actualiza esos mismos Id).
    private Dictionary<string, int>? _idsPorItemCode;

    public ListaPrecioFrutaForm()
    {
        InitializeComponent();
    }

    public ListaPrecioFrutaForm(
        ListaPrecioFrutaService listaPrecioFrutaService,
        ProductorService productorService,
        VariedadService variedadService,
        PaisService paisService,
        EstadoService estadoService)
        : this()
    {
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _productorService = productorService;
        _variedadService = variedadService;
        _paisService = paisService;
        _estadoService = estadoService;
        _dtFechaInicio.EditValue = DateTime.Today;

        Load += async (_, _) => await CargarVariedadesAsync();
        Shown += async (_, _) => await ConsultarSapAsync();
    }

    private async Task CargarVariedadesAsync()
    {
        var variedades = await _variedadService.ObtenerAsync();
        _cmbVariedad.Properties.DataSource = variedades.ToList();
        _cmbVariedad.Properties.ValueMember = "Id";
        _cmbVariedad.Properties.DisplayMember = "Nombre";
        _cmbVariedad.Properties.Columns.Clear();
        _cmbVariedad.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Variedad"));
        _cmbVariedad.Properties.PopupWidth = 250;
    }

    private async void CmbVariedad_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new VariedadesForm(_variedadService);
        form.ShowDialog(this);
        await CargarVariedadesAsync();
    }

    private void ChkPreciosPorRango_CheckedChanged(object? sender, EventArgs e)
    {
        _dtFechaFin.Enabled = _chkPreciosPorRango.Checked;
        if (!_chkPreciosPorRango.Checked)
        {
            _dtFechaFin.EditValue = null;
        }
        else
        {
            _dtFechaFin.EditValue ??= DateTime.Today;
        }
    }

    // Search abre el mismo buscador de productores del módulo Productores; Delete limpia la
    // selección (el productor es opcional).
    private void CmbProductor_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind == ButtonPredefines.Delete)
        {
            _productorId = null;
            _cmbProductor.Text = string.Empty;
            return;
        }

        if (e.Button.Kind != ButtonPredefines.Search)
        {
            return;
        }

        using var buscador = new ProductoresForm(_productorService, _paisService, _estadoService);
        if (buscador.ShowDialog(this) != DialogResult.OK || buscador.ProductorSeleccionado is null)
        {
            return;
        }

        _productorId = buscador.ProductorSeleccionado.Id;
        _cmbProductor.Text = buscador.ProductorSeleccionado.NombreProductor;
    }

    private async void BtnConsultarSap_Click(object? sender, EventArgs e) => await ConsultarSapAsync();

    // Se dispara solo al abrir el form (Shown) y también desde el botón, para poder
    // refrescar manualmente. Siempre vuelve a modo captura (si venías de editar una fecha
    // guardada, se abandona esa edición). Muestra el wait form estándar de DevExpress mientras
    // dura la consulta a SAP, para que no parezca que la app se congeló.
    private async Task ConsultarSapAsync()
    {
        SalirModoEdicion();

        _btnConsultarSap.Enabled = false;
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: true, useFadeOut: true, "FrontOne", "Consultando SAP...");

        IReadOnlyList<SapItemDto> items = [];
        SapException? error = null;
        try
        {
            items = await _listaPrecioFrutaService.ObtenerItemsFrutaDeSapAsync();
        }
        catch (SapException ex)
        {
            error = ex;
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
            _btnConsultarSap.Enabled = true;
        }

        if (error is not null)
        {
            XtraMessageBox.Show(this, $"No se pudo consultar SAP Business One.\n\n{error.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (items.Count == 0)
        {
            XtraMessageBox.Show(this, "No se encontraron productos con ItemCode que inicie con 'MP' en SAP.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        _filas = new BindingList<FilaListaPrecioFruta>(items
            .Select(i => new FilaListaPrecioFruta { ItemCode = i.ItemCode, ItemName = i.ItemName })
            .ToList());

        _grid.DataSource = _filas;
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["ItemCode"] is { } colItemCode)
        {
            colItemCode.OptionsColumn.AllowEdit = false;
        }

        if (_gridView.Columns["ItemName"] is { } colItemName)
        {
            colItemName.Caption = "Producto";
            colItemName.OptionsColumn.AllowEdit = false;
        }

        foreach (var nombre in new[] { "Lista1", "Lista2", "Lista3" })
        {
            if (_gridView.Columns[nombre] is not { } columna)
            {
                continue;
            }

            columna.Caption = nombre == "Lista1" ? "Lista 1" : nombre == "Lista2" ? "Lista 2" : "Lista 3";
            columna.DisplayFormat.FormatType = FormatType.Numeric;
            columna.DisplayFormat.FormatString = "n2";
        }
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        _gridView.CloseEditor();
        _gridView.UpdateCurrentRow();

        if (_filas.Count == 0)
        {
            XtraMessageBox.Show(this, "Consulta SAP o busca una lista para editar antes de guardar.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_dtFechaInicio.EditValue is not DateTime fechaInicio)
        {
            XtraMessageBox.Show(this, "Captura la fecha inicio.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_cmbVariedad.EditValue is not int variedadId)
        {
            XtraMessageBox.Show(this, "Selecciona la variedad.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_idsPorItemCode is not null)
        {
            await GuardarEdicionAsync(fechaInicio, variedadId);
            return;
        }

        DateTime? fechaFin = null;
        if (_chkPreciosPorRango.Checked)
        {
            if (_dtFechaFin.EditValue is not DateTime fin)
            {
                XtraMessageBox.Show(this, "Captura la fecha fin del rango.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            fechaFin = fin;
        }

        var dtos = _filas
            .Select(f => new ListaPrecioFrutaDto(0, f.ItemCode, f.ItemName, f.Lista1, f.Lista2, f.Lista3, fechaInicio, fechaFin, true, _productorId, variedadId))
            .ToList();

        try
        {
            await _listaPrecioFrutaService.GuardarListaAsync(dtos);
            XtraMessageBox.Show(this, "Lista de precios guardada correctamente.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, $"No se pudo guardar la lista de precios.\n\n{ex.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task GuardarEdicionAsync(DateTime fecha, int variedadId)
    {
        try
        {
            foreach (var fila in _filas)
            {
                var id = _idsPorItemCode![fila.ItemCode];
                var dto = new ListaPrecioFrutaDto(id, fila.ItemCode, fila.ItemName, fila.Lista1, fila.Lista2, fila.Lista3, fecha, null, true, _productorId, variedadId);
                await _listaPrecioFrutaService.ActualizarAsync(dto);
            }

            XtraMessageBox.Show(this, "Lista de precios actualizada correctamente.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            SalirModoEdicion();
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, $"No se pudo actualizar la lista de precios.\n\n{ex.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Borra toda la lista guardada de la fecha (y productor, si aplica) en pantalla, para
    // poder volver a capturarla desde cero. No usa _dtFechaFin — es siempre una sola fecha,
    // ignorando el checkbox de rango.
    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        if (_dtFechaInicio.EditValue is not DateTime fecha)
        {
            XtraMessageBox.Show(this, "Selecciona la fecha a eliminar.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var descripcion = string.IsNullOrEmpty(_cmbProductor.Text)
            ? $"la lista de precios general del {fecha:dd/MM/yyyy}"
            : $"la lista de precios especial del {fecha:dd/MM/yyyy} para {_cmbProductor.Text}";

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar {descripcion}?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        var variedadIdEliminar = _cmbVariedad.EditValue as int?;

        try
        {
            await _listaPrecioFrutaService.EliminarPorFechaAsync(fecha, _productorId, variedadIdEliminar);
            XtraMessageBox.Show(this, "Lista de precios eliminada.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Después de borrar, se vuelve a consultar SAP para capturar de nuevo esa fecha
            // (esto ya saca del modo edición y reactiva los controles de fecha). El productor
            // seleccionado se conserva, para recapturar la misma lista especial de una vez.
            await ConsultarSapAsync();
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, $"No se pudo eliminar la lista de precios.\n\n{ex.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Trae al form principal los productos ya guardados de una fecha (y productor, si es una
    // lista especial) elegida en el picker de búsqueda, para editar sus precios ahí mismo.
    private async void BtnBuscarListaPrecios_Click(object? sender, EventArgs e)
    {
        using var form = new BuscarListaPrecioFrutaForm(_listaPrecioFrutaService);
        if (form.ShowDialog(this) != DialogResult.OK || form.VigenciaSeleccionada is not { } vigencia)
        {
            return;
        }

        var filas = await _listaPrecioFrutaService.ObtenerPorFechaAsync(vigencia.Fecha, vigencia.ProductorId, vigencia.VariedadId);
        if (filas.Count == 0)
        {
            XtraMessageBox.Show(this, "Esa lista ya no está guardada.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _idsPorItemCode = filas.ToDictionary(f => f.ItemCode, f => f.Id);
        _filas = new BindingList<FilaListaPrecioFruta>(filas
            .Select(f => new FilaListaPrecioFruta { ItemCode = f.ItemCode, ItemName = f.ItemName, Lista1 = f.Lista1, Lista2 = f.Lista2, Lista3 = f.Lista3 })
            .ToList());

        _grid.DataSource = _filas;
        ConfigurarColumnas();

        _chkPreciosPorRango.Checked = false;
        _dtFechaInicio.EditValue = vigencia.Fecha;
        _dtFechaInicio.Enabled = false;
        _chkPreciosPorRango.Enabled = false;

        _productorId = vigencia.ProductorId;
        _cmbProductor.Text = vigencia.ProductorNombre ?? string.Empty;
        _cmbProductor.Enabled = false;

        _cmbVariedad.EditValue = vigencia.VariedadId;
        _cmbVariedad.Enabled = false;
    }

    private void SalirModoEdicion()
    {
        _idsPorItemCode = null;
        _dtFechaInicio.Enabled = true;
        _chkPreciosPorRango.Enabled = true;
        _cmbProductor.Enabled = true;
        _cmbVariedad.Enabled = true;
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
