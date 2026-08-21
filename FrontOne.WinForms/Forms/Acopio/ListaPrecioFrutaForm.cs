using System.ComponentModel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class ListaPrecioFrutaForm : XtraForm
{
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly ProductorService _productorService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private BindingList<FilaListaPrecioFruta> _filas = [];

    // Productor opcional: null = lista general, con valor = lista especial solo para ese
    // productor. Convive con la lista general de la misma combinación/día (no chocan entre sí).
    private int? _productorId;

    // null = modo captura (Guardar inserta). No-null = modo edición (se cargó una fecha ya
    // guardada vía Buscar Lista de Precios, Guardar actualiza esos mismos Id).
    private Dictionary<(int CategoriaId, int CalibreApeamId), int>? _idsPorCombinacion;

    public ListaPrecioFrutaForm()
    {
        InitializeComponent();
    }

    public ListaPrecioFrutaForm(
        ListaPrecioFrutaService listaPrecioFrutaService,
        ProductorService productorService,
        PaisService paisService,
        EstadoService estadoService)
        : this()
    {
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _productorService = productorService;
        _paisService = paisService;
        _estadoService = estadoService;
        _dtFechaInicio.EditValue = DateTime.Today;
        _gridView.RowCellStyle += GridView_RowCellStyle;

        Shown += async (_, _) => await CargarCombinacionesAsync();
    }

    // Pastel por grupo de Categoría, solo en las columnas de identidad (Categoría/Calibre
    // APEAM) — las columnas de precio conservan su propio color por lista (Convencional/
    // Organico/Nacional), fijado en ConfigurarColumnas.
    private static readonly Dictionary<string, Color> ColoresPorCategoria = new()
    {
        ["Cat 1"] = ColorTranslator.FromHtml("#E8DAEF"),
        ["Cat 2"] = ColorTranslator.FromHtml("#FCF3CF"),
        ["Nal"] = ColorTranslator.FromHtml("#FADBD8"),
    };

    private void GridView_RowCellStyle(object? sender, RowCellStyleEventArgs e)
    {
        if (e.Column.FieldName is not ("CategoriaNombre" or "CalibreApeamNombre"))
        {
            return;
        }

        if (_gridView.GetRowCellValue(e.RowHandle, "CategoriaNombre") is string categoria
            && ColoresPorCategoria.TryGetValue(categoria, out var color))
        {
            e.Appearance.BackColor = color;
            e.Appearance.Options.UseBackColor = true;
        }
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

    private async void BtnCargarCombinaciones_Click(object? sender, EventArgs e) => await CargarCombinacionesAsync();

    // Carga unificada: siempre trae el universo completo de combinaciones activas de
    // Catalogos.MateriaPrima (ya no SAP). Si se pasa una vigencia (fechaOverlay/
    // productorIdOverlay, viene de "Buscar Lista de Precios"), sobrepone los precios ya
    // guardados para esa vigencia sobre las combinaciones que hagan match, dejando el resto en
    // 0 — decisión confirmada con el usuario: el grid de edición siempre muestra el universo
    // completo, no solo lo que ya se guardó ese día.
    private async Task CargarCombinacionesAsync(DateTime? fechaOverlay = null, int? productorIdOverlay = null)
    {
        _btnCargarCombinaciones.Enabled = false;
        // useFadeIn: false — evita la carrera de DevExpress donde CloseDefaultWaitForm truena
        // ("Splash Form is not displayed") si la operación termina antes de que el fade-in
        // asíncrono termine de registrar el splash como visible.
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: false, useFadeOut: true, "FrontOne", "Cargando combinaciones...");

        try
        {
            var combinaciones = await _listaPrecioFrutaService.ObtenerCombinacionesActivasAsync();
            var filas = combinaciones.Select(c => new FilaListaPrecioFruta
            {
                CategoriaId = c.CategoriaId,
                CategoriaNombre = c.CategoriaNombre,
                CalibreApeamId = c.CalibreApeamId,
                CalibreApeamNombre = c.CalibreApeamNombre,
            }).ToList();

            if (fechaOverlay is { } fecha)
            {
                var guardadas = await _listaPrecioFrutaService.ObtenerPorFechaAsync(fecha, productorIdOverlay);
                _idsPorCombinacion = guardadas.ToDictionary(g => (g.CategoriaId, g.CalibreApeamId), g => g.Id);

                foreach (var fila in filas)
                {
                    var match = guardadas.FirstOrDefault(g => g.CategoriaId == fila.CategoriaId && g.CalibreApeamId == fila.CalibreApeamId);
                    if (match is not null)
                    {
                        fila.Convencional = match.Convencional;
                        fila.Organico = match.Organico;
                        fila.Nacional = match.Nacional;
                    }
                }
            }
            else
            {
                _idsPorCombinacion = null;
            }

            _filas = new BindingList<FilaListaPrecioFruta>(filas);
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
            _btnCargarCombinaciones.Enabled = true;
        }

        _grid.DataSource = _filas;
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["CalibreApeamNombre"] is { } colCalibreApeam)
        {
            colCalibreApeam.Caption = "Calibre APEAM";
            colCalibreApeam.OptionsColumn.AllowEdit = false;
            colCalibreApeam.VisibleIndex = 0;
        }

        if (_gridView.Columns["CategoriaNombre"] is { } colCategoria)
        {
            colCategoria.Caption = "Categoría";
            colCategoria.OptionsColumn.AllowEdit = false;
            colCategoria.VisibleIndex = 1;
        }

        if (_gridView.Columns["CategoriaId"] is { } colCategoriaId)
        {
            colCategoriaId.Visible = false;
        }

        if (_gridView.Columns["CalibreApeamId"] is { } colCalibreApeamId)
        {
            colCalibreApeamId.Visible = false;
        }

        // Colores pastel de fondo para distinguir cada columna de precio a simple vista.
        var nombresColumna = new[] { "Convencional", "Organico", "Nacional" };
        var coloresPastel = new[]
        {
            ColorTranslator.FromHtml("#D6EAF8"), // Azul
            ColorTranslator.FromHtml("#FDEBD0"), // Naranja
            ColorTranslator.FromHtml("#D5F5E3"), // Verde
        };
        for (var i = 0; i < nombresColumna.Length; i++)
        {
            if (_gridView.Columns[nombresColumna[i]] is not { } columna)
            {
                continue;
            }

            columna.Caption = ListasPrecioFruta.Nombres[i];
            columna.VisibleIndex = 2 + i;
            columna.DisplayFormat.FormatType = FormatType.Numeric;
            columna.DisplayFormat.FormatString = "n2";
            columna.AppearanceCell.BackColor = coloresPastel[i];
            columna.AppearanceCell.Options.UseBackColor = true;
        }

        _gridView.BestFitColumns();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        _gridView.CloseEditor();
        _gridView.UpdateCurrentRow();

        if (_filas.Count == 0)
        {
            XtraMessageBox.Show(this, "Carga las combinaciones o busca una lista para editar antes de guardar.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_dtFechaInicio.EditValue is not DateTime fechaInicio)
        {
            XtraMessageBox.Show(this, "Captura la fecha inicio.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Las filas sin ningún precio capturado se omiten: insertarlas con precio 0 sería
        // indistinguible de "no hay precio capturado" para el cálculo de costo en Gastos.
        var filasConPrecio = _filas.Where(f => f.Convencional != 0 || f.Organico != 0 || f.Nacional != 0).ToList();
        if (filasConPrecio.Count == 0)
        {
            XtraMessageBox.Show(this, "Captura al menos un precio antes de guardar.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_idsPorCombinacion is not null)
        {
            await GuardarEdicionAsync(fechaInicio);
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

        var dtos = filasConPrecio
            .Select(f => new ListaPrecioFrutaDto(0, f.CategoriaId, f.CalibreApeamId, f.Convencional, f.Organico, f.Nacional, fechaInicio, fechaFin, true, _productorId))
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

    private async Task GuardarEdicionAsync(DateTime fecha)
    {
        try
        {
            foreach (var fila in _filas)
            {
                if (!_idsPorCombinacion!.TryGetValue((fila.CategoriaId, fila.CalibreApeamId), out var id))
                {
                    // Combinación sin precio guardado previamente y sin capturar ahora: se
                    // ignora, no se inserta aparte (el alta de nuevas combinaciones en una
                    // vigencia ya guardada se hace desde el modo captura, no desde edición).
                    continue;
                }

                var dto = new ListaPrecioFrutaDto(id, fila.CategoriaId, fila.CalibreApeamId, fila.Convencional, fila.Organico, fila.Nacional, fecha, null, true, _productorId);
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

        try
        {
            await _listaPrecioFrutaService.EliminarPorFechaAsync(fecha, _productorId);
            XtraMessageBox.Show(this, "Lista de precios eliminada.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Después de borrar, se vuelve a cargar el universo de combinaciones para capturar
            // de nuevo esa fecha (esto ya saca del modo edición y reactiva los controles de
            // fecha). El productor seleccionado se conserva, para recapturar la misma lista
            // especial de una vez.
            SalirModoEdicion();
            await CargarCombinacionesAsync();
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

    // Trae al form principal la vigencia (fecha + productor, si aplica) elegida en el picker de
    // búsqueda, mostrando el universo completo de combinaciones con los precios ya guardados
    // prellenados.
    private async void BtnBuscarListaPrecios_Click(object? sender, EventArgs e)
    {
        using var form = new BuscarListaPrecioFrutaForm(_listaPrecioFrutaService);
        if (form.ShowDialog(this) != DialogResult.OK || form.VigenciaSeleccionada is not { } vigencia)
        {
            return;
        }

        await CargarCombinacionesAsync(vigencia.Fecha, vigencia.ProductorId);

        _chkPreciosPorRango.Checked = false;
        _dtFechaInicio.EditValue = vigencia.Fecha;
        _dtFechaInicio.Enabled = false;
        _chkPreciosPorRango.Enabled = false;

        _productorId = vigencia.ProductorId;
        _cmbProductor.Text = vigencia.ProductorNombre ?? string.Empty;
        _cmbProductor.Enabled = false;
    }

    private void SalirModoEdicion()
    {
        _idsPorCombinacion = null;
        _dtFechaInicio.Enabled = true;
        _chkPreciosPorRango.Enabled = true;
        _cmbProductor.Enabled = true;
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
