using System.ComponentModel;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Acopio;

// Simulador (no persiste) para calcular la Banda de precio por Categoría/Calibre APEAM: mismo
// universo de combinaciones que Lista de Precios de Fruta, sin las 3 columnas de lista. Se
// exporta a Imagen/PDF/Excel en vez de guardarse — ver contexto/acopio.md.
public partial class SimuladorBandasForm : XtraForm
{
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private BindingList<FilaSimuladorBanda> _filas = [];

    public SimuladorBandasForm()
    {
        InitializeComponent();
    }

    public SimuladorBandasForm(ListaPrecioFrutaService listaPrecioFrutaService)
        : this()
    {
        _listaPrecioFrutaService = listaPrecioFrutaService;

        _cmbListaPrecio.Properties.Items.AddRange(ListasPrecioFruta.Nombres.Cast<object>().ToArray());
        _cmbListaPrecio.SelectedIndex = 0;

        var menu = new DXPopupMenu();
        menu.Items.Add(new DXMenuItem("Imagen (PNG)", BtnExportarImagen_Click));
        menu.Items.Add(new DXMenuItem("PDF", BtnExportarPdf_Click));
        menu.Items.Add(new DXMenuItem("Excel", BtnExportarExcel_Click));
        _btnExportar.DropDownControl = menu;

        _gridView.RowCellStyle += GridView_RowCellStyle;

        Load += async (_, _) => await CargarCombinacionesAsync();
    }

    // Mismos colores pastel por Categoría que ListaPrecioFrutaForm.ColoresPorCategoria, solo en
    // las columnas de identidad (Categoría/Calibre APEAM) — Precio/Porcentaje/Banda conservan su
    // propio color fijo por columna (definido en el Designer).
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

    private async Task CargarCombinacionesAsync()
    {
        // useFadeIn: false — mismo criterio que ListaPrecioFrutaForm: evita la carrera de
        // DevExpress donde CloseDefaultWaitForm truena si la operación termina antes de que el
        // fade-in asíncrono termine de registrar el splash como visible.
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: false, useFadeOut: true, "FrontOne", "Cargando combinaciones...");

        try
        {
            var combinaciones = await _listaPrecioFrutaService.ObtenerCombinacionesActivasAsync();
            var filas = combinaciones.Select(c => new FilaSimuladorBanda
            {
                CategoriaId = c.CategoriaId,
                CategoriaNombre = c.CategoriaNombre,
                CalibreApeamId = c.CalibreApeamId,
                CalibreApeamNombre = c.CalibreApeamNombre,
            }).ToList();

            _filas = new BindingList<FilaSimuladorBanda>(filas);
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }

        _grid.DataSource = _filas;
        ActualizarAvisoSuma();
    }

    private void GridView_CellValueChanged(object? sender, CellValueChangedEventArgs e) => ActualizarAvisoSuma();

    // Avisa sin bloquear: el usuario puede guardar/exportar aunque la suma no dé 100% (pedido
    // explícito — hay curvas legítimas en construcción, o con MERMA que hace que el total no
    // siempre cierre exacto al capturar a medias).
    private void ActualizarAvisoSuma()
    {
        var suma = _filas.Sum(f => f.Porcentaje);
        if (Math.Abs(suma - 100m) > 0.005m)
        {
            _lblAvisoSuma.Text = $"La suma de porcentajes es {suma:n2}%, debería ser 100%.";
            _lblAvisoSuma.Visible = true;
        }
        else
        {
            _lblAvisoSuma.Visible = false;
        }
    }

    // Precarga la columna Precio desde una vigencia ya guardada en Lista de Precios de Fruta —
    // solo agiliza la captura de Precio, nunca toca los Porcentajes que el usuario ya haya
    // capturado. Reemplaza toda la columna (las combinaciones sin precio en esa vigencia quedan
    // en 0) para no mezclar precios de listas distintas.
    private async void BtnCargarPrecios_Click(object? sender, EventArgs e)
    {
        using var form = new BuscarListaPrecioFrutaForm(_listaPrecioFrutaService);
        if (form.ShowDialog(this) != DialogResult.OK || form.VigenciaSeleccionada is not { } vigencia)
        {
            return;
        }

        var guardadas = await _listaPrecioFrutaService.ObtenerPorFechaAsync(vigencia.Fecha, vigencia.ProductorId);
        var indiceLista = _cmbListaPrecio.SelectedIndex;
        var encontradas = 0;

        foreach (var fila in _filas)
        {
            var match = guardadas.FirstOrDefault(g => g.CategoriaId == fila.CategoriaId && g.CalibreApeamId == fila.CalibreApeamId);
            fila.Precio = match is null ? 0m : PrecioDeLista(match, indiceLista);
            if (match is not null)
            {
                encontradas++;
            }
        }

        var descripcion = vigencia.ProductorNombre is null
            ? $"{vigencia.Fecha:dd/MM/yyyy} (general)"
            : $"{vigencia.Fecha:dd/MM/yyyy} — {vigencia.ProductorNombre}";
        _lblListaCargada.Text = $"Precios cargados de: {descripcion} ({ListasPrecioFruta.Nombres[indiceLista]})";
        _lblListaCargada.Visible = true;

        XtraMessageBox.Show(this, $"Se cargó el precio de {encontradas} de {_filas.Count} combinaciones.", "FrontOne",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static decimal PrecioDeLista(ListaPrecioFrutaDto dto, int indiceLista) => indiceLista switch
    {
        0 => dto.Convencional,
        1 => dto.Organico,
        _ => dto.Nacional,
    };

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        if (_filas.Count == 0)
        {
            return;
        }

        var confirmar = XtraMessageBox.Show(this, "¿Limpiar los precios y porcentajes capturados?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        foreach (var fila in _filas)
        {
            fila.Precio = 0m;
            fila.Porcentaje = 0m;
        }

        _lblListaCargada.Visible = false;
    }

    private void BtnExportarExcel_Click(object? sender, EventArgs e)
    {
        _gridView.CloseEditor();

        using var dialogo = new SaveFileDialog { Title = "Exportar a Excel", Filter = "Excel (*.xlsx)|*.xlsx" };
        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _grid.ExportToXlsx(dialogo.FileName);
        XtraMessageBox.Show(this, $"Exportado a:\n{dialogo.FileName}", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnExportarPdf_Click(object? sender, EventArgs e)
    {
        _gridView.CloseEditor();

        using var dialogo = new SaveFileDialog { Title = "Exportar a PDF", Filter = "PDF (*.pdf)|*.pdf" };
        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _grid.ExportToPdf(dialogo.FileName);
        XtraMessageBox.Show(this, $"Exportado a:\n{dialogo.FileName}", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // GridControl no tiene ExportToImage propio (a diferencia de ExportToXlsx/ExportToPdf) —
    // implementa IBasePrintable, así que se exporta a través de un PrintableComponentLink.
    private void BtnExportarImagen_Click(object? sender, EventArgs e)
    {
        _gridView.CloseEditor();

        using var dialogo = new SaveFileDialog { Title = "Exportar a Imagen", Filter = "Imagen PNG (*.png)|*.png" };
        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        // PrintingSystem no se auto-crea fuera del diseñador — sin asignarlo, CreateDocument()
        // truena con NullReferenceException (confirmado por reflexión: la propiedad viene null
        // por defecto en un PrintableComponentLink construido a mano).
        using var link = new PrintableComponentLink { Component = _grid, PrintingSystem = new PrintingSystem() };
        link.CreateDocument();
        link.ExportToImage(dialogo.FileName);
        XtraMessageBox.Show(this, $"Exportado a:\n{dialogo.FileName}", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
