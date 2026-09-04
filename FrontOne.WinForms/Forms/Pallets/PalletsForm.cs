using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Reempaques;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Pallets;

// Listado de Pallets ya capturados, para editarlos/completarlos. Mismo molde que CorridasForm:
// tab MDI singleton con filtro por Status y carga en Shown (no en Load: en un tab MDI el grid
// todavía no tiene su tamaño real durante Load).
public partial class PalletsForm : XtraForm
{
    private readonly PalletService _palletService = null!;
    private readonly ReempaqueService _reempaqueService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly ProductoTerminadoService _productoTerminadoService = null!;

    // Servicios que solo hacen falta para el botón `+` del combo de Producto Terminado dentro de
    // PalletDetalleCapturaForm (abre ProductosTerminadosForm, que los pide todos) — mismo criterio
    // de plomería que ya usa LotesForm con AcuerdoCorte/OrdenCorte.
    private readonly CategoriaService _categoriaService = null!;
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly MarcaService _marcaService = null!;
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly PaisService _paisService = null!;
    private readonly VariedadService _variedadService = null!;

    private readonly ConfiguracionBasculaService _configuracionBasculaService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly EtiquetaService _etiquetaService = null!;
    private readonly LicenciaTecitService _licenciaTecitService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly SqlOptions _sqlOptions = null!;

    private PalletEditarForm? _palletEditarForm;
    private List<PalletDto> _pallets = new();

    // Huella de la última carga (Total + fecha de modificación más reciente) contra la que se
    // compara cada tick del timer para saber si el grid quedó desactualizado — ver
    // Produccion.sp_Pallet_ObtenerUltimaModificacion. Colores de la app móvil (verde
    // "Completo"/gris "Vacío") para que el criterio visual sea el mismo en ambas plataformas.
    private static readonly Color ColorActualizacionDisponible = Color.FromArgb(47, 158, 110);
    private PalletUltimaModificacionDto? _ultimaHuellaConocida;

    public PalletsForm()
    {
        InitializeComponent();
    }

    public PalletsForm(
        PalletService palletService,
        ReempaqueService reempaqueService,
        LineaProduccionService lineaProduccionService,
        ProductoTerminadoService productoTerminadoService,
        CategoriaService categoriaService,
        TipoProductoService tipoProductoService,
        CalibreApeamService calibreApeamService,
        MarcaService marcaService,
        PesoEstandarService pesoEstandarService,
        PaisService paisService,
        VariedadService variedadService,
        ConfiguracionBasculaService configuracionBasculaService,
        EmpresaConfiguracionService empresaConfiguracionService,
        EtiquetaService etiquetaService,
        LicenciaTecitService licenciaTecitService,
        SessionContext sessionContext,
        SqlOptions sqlOptions)
        : this()
    {
        _palletService = palletService;
        _reempaqueService = reempaqueService;
        _lineaProduccionService = lineaProduccionService;
        _productoTerminadoService = productoTerminadoService;
        _categoriaService = categoriaService;
        _tipoProductoService = tipoProductoService;
        _calibreApeamService = calibreApeamService;
        _marcaService = marcaService;
        _pesoEstandarService = pesoEstandarService;
        _paisService = paisService;
        _variedadService = variedadService;
        _configuracionBasculaService = configuracionBasculaService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _etiquetaService = etiquetaService;
        _licenciaTecitService = licenciaTecitService;
        _sessionContext = sessionContext;
        _sqlOptions = sqlOptions;

        // El orden de estas entradas es el valor del Estatus (índice 0 = "Todos"), así el filtro
        // se resuelve comparando contra el índice sin tabla de traducción aparte.
        _cmbFiltroStatus.Properties.Items.AddRange(new object[]
        {
            "Todos", "Vacío", "Incompleto", "Completo", "Excedido", "Empacado", "Reempacado", "En Proceso",
        });
        _cmbFiltroStatus.SelectedIndex = 0;

        Shown += async (_, _) =>
        {
            await CargarDatosAsync();
            _timerActualizacion.Start();
        };
        FormClosed += (_, _) => _timerActualizacion.Stop();
        _grid.SizeChanged += (_, _) => AjustarAnchoColumnas();
    }

    private async Task CargarDatosAsync()
    {
        _pallets = (await _palletService.ObtenerAsync()).ToList();
        AplicarFiltro();

        // Cualquier carga manual (inicial, tras Nuevo/Editar/Eliminar, o al presionar Actualizar)
        // deja el grid al día — se vuelve a fijar la huella conocida para que el próximo tick del
        // timer compare contra este momento, no contra la carga anterior.
        try
        {
            _ultimaHuellaConocida = await _palletService.ObtenerUltimaModificacionAsync();
        }
        catch (SqlRepositoryException)
        {
            // Si falla, se deja la huella anterior — el próximo tick del timer lo vuelve a intentar.
        }

        MarcarComoActualizado();
    }

    // Compara la huella actual contra la última conocida sin traer el grid completo — Total
    // cambia con cualquier alta/baja, UltimaModificacion con cualquier edición (encabezado, línea
    // de detalle, o bloqueo). Cualquiera de los dos que cambie prende el botón en verde.
    private async void TimerActualizacion_Tick(object? sender, EventArgs e)
    {
        try
        {
            var huellaActual = await _palletService.ObtenerUltimaModificacionAsync();
            if (_ultimaHuellaConocida is null ||
                huellaActual.Total != _ultimaHuellaConocida.Total ||
                huellaActual.UltimaModificacion != _ultimaHuellaConocida.UltimaModificacion)
            {
                MarcarComoDesactualizado();
            }
        }
        catch (SqlRepositoryException)
        {
            // Silencioso a propósito: una falla de un tick de fondo no debe interrumpir al usuario
            // con un mensaje de error por algo que no fue una acción suya — el próximo tick reintenta.
        }
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e)
    {
        if (!_btnActualizar.Enabled)
        {
            return;
        }

        await CargarDatosAsync();
    }

    private void MarcarComoDesactualizado()
    {
        _btnActualizar.Enabled = true;
        _btnActualizar.Text = "Actualizar";
        _btnActualizar.Appearance.BackColor = ColorActualizacionDisponible;
        _btnActualizar.Appearance.ForeColor = Color.White;
        _btnActualizar.Appearance.Options.UseBackColor = true;
        _btnActualizar.Appearance.Options.UseForeColor = true;
    }

    private void MarcarComoActualizado()
    {
        _btnActualizar.Enabled = false;
        _btnActualizar.Text = "Todo actualizado";
        _btnActualizar.Appearance.Options.UseBackColor = false;
        _btnActualizar.Appearance.Options.UseForeColor = false;
    }

    private void AplicarFiltro()
    {
        var indice = _cmbFiltroStatus.SelectedIndex;
        IEnumerable<PalletDto> filtrados = indice <= 0
            ? _pallets
            : _pallets.Where(p => p.Estatus == indice);

        _grid.DataSource = filtrados.ToList();
        ConfigurarColumnas();
    }

    private void CmbFiltroStatus_SelectedIndexChanged(object? sender, EventArgs e) => AplicarFiltro();

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[]
        {
            "Id", "LineaProduccionId", "FechaCreacionRegistro", "FechaBloqueo", "PrimeraCorrida",
            "PorcentajeMateriaSeca", "PesoReal", "EsMixto", "HoraCreacion",
            "ProductoTerminadoId",
        })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Folio"] is { } colFolio)
        {
            colFolio.Caption = "No. de Pallet";
        }

        if (_gridView.Columns["Estatus"] is { } colEstatus)
        {
            colEstatus.Caption = "Status";
        }

        if (_gridView.Columns["LineaProduccionNombre"] is { } colLinea)
        {
            colLinea.Caption = "Línea de Producción";
        }

        if (_gridView.Columns["FechaCreacion"] is { } colFecha)
        {
            colFecha.Caption = "Fecha";
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["TotalCajas"] is { } colCajas)
        {
            colCajas.Caption = "Cajas";
        }

        if (_gridView.Columns["TotalKilogramos"] is { } colKilos)
        {
            colKilos.Caption = "Kilogramos";
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["ProductoCodigoSap"] is { } colCodigoSap)
        {
            colCodigoSap.Caption = "Código SAP";
        }

        if (_gridView.Columns["ProductoDescripcion"] is { } colProducto)
        {
            colProducto.Caption = "Producto";
        }

        if (_gridView.Columns["Bloqueado"] is { } colBloqueado)
        {
            colBloqueado.Caption = "Bloqueado";
        }

        // Folio del reempaque como hipervínculo (mismo patrón que AplicarEstiloFolioClickeable
        // en LoteEditarForm/GastoLoteForm/RecepcionesFrutaForm/OrdenesCorteForm): abre el módulo
        // de Reempaques con ese folio.
        if (_gridView.Columns["NoReempaque"] is { } colNoReempaque)
        {
            colNoReempaque.Caption = "No. de Reempaque";
            AplicarEstiloFolioClickeable(colNoReempaque);
        }

        // Orden explícito: asignar VisibleIndex uno por uno es la única forma confiable de fijar
        // el orden de columnas en DevExpress (mismo criterio ya usado en CorridasForm).
        var ordenColumnas = new[]
        {
            "Folio", "LineaProduccionNombre", "FechaCreacion", "TotalCajas", "TotalKilogramos",
            "ProductoCodigoSap", "ProductoDescripcion", "Bloqueado", "Estatus", "NoReempaque",
        };
        for (var i = 0; i < ordenColumnas.Length; i++)
        {
            if (_gridView.Columns[ordenColumnas[i]] is { } columnaOrdenada)
            {
                columnaOrdenada.VisibleIndex = i;
            }
        }

        AjustarAnchoColumnas();
    }

    private void AjustarAnchoColumnas()
    {
        if (_gridView.Columns.Count == 0)
        {
            return;
        }

        _gridView.BestFitColumns();
    }

    private void GridView_CustomColumnDisplayText(object? sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "Estatus" && e.Value is byte estatus)
        {
            e.DisplayText = NombreEstatus(estatus);
            return;
        }

        // Un pallet mixto no tiene un producto único de encabezado: en vez de mostrar la columna
        // vacía, se rotula explícitamente como "PALLET MIXTO".
        if (e.Column.FieldName == "ProductoDescripcion" && _gridView.GetRow(e.ListSourceRowIndex) is PalletDto fila && fila.EsMixto)
        {
            e.DisplayText = "PALLET MIXTO";
        }
    }

    internal static string NombreEstatus(byte estatus) => estatus switch
    {
        1 => "Vacío",
        2 => "Incompleto",
        3 => "Completo",
        4 => "Excedido",
        5 => "Empacado",
        6 => "Reempacado",
        7 => "En Proceso",
        8 => "Embarcado",
        _ => string.Empty,
    };

    private static readonly string[] ColumnasFolioClickeable = ["NoReempaque"];

    private static void AplicarEstiloFolioClickeable(DevExpress.XtraGrid.Columns.GridColumn columna)
    {
        columna.AppearanceCell.Font = new Font(columna.AppearanceCell.Font, FontStyle.Bold | FontStyle.Underline);
        columna.AppearanceCell.ForeColor = ColorTranslator.FromHtml("#0563C1");
        columna.AppearanceCell.Options.UseFont = true;
        columna.AppearanceCell.Options.UseForeColor = true;
    }

    private void GridView_MouseMove(object? sender, MouseEventArgs e)
    {
        var info = _gridView.CalcHitInfo(e.Location);
        _grid.Cursor = info.InRowCell && ColumnasFolioClickeable.Contains(info.Column?.FieldName)
            ? Cursors.Hand
            : Cursors.Default;
    }

    private async void GridView_RowCellClick(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
    {
        if (e.Column.FieldName != "NoReempaque" || _gridView.GetRow(e.RowHandle) is not PalletDto fila || string.IsNullOrWhiteSpace(fila.NoReempaque))
        {
            return;
        }

        await AbrirReempaquePorFolioAsync(fila.NoReempaque);
    }

    private async Task AbrirReempaquePorFolioAsync(string folio)
    {
        if (!_sessionContext.TienePermiso("Reempaques", "Reempaques", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var reempaque = await _reempaqueService.ObtenerPorFolioAsync(folio);
        if (reempaque is null)
        {
            XtraMessageBox.Show(this, "El reempaque ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new ReempaqueEditarForm(
            _reempaqueService, _palletService, _lineaProduccionService, _productoTerminadoService,
            _categoriaService, _tipoProductoService, _calibreApeamService, _marcaService,
            _pesoEstandarService, _paisService, _variedadService,
            _configuracionBasculaService, _empresaConfiguracionService,
            _etiquetaService, _licenciaTecitService, _sessionContext, _sqlOptions, reempaque);
        form.Guardado += async (_, _) => await CargarDatosAsync();
        form.ShowDialog(this);
    }

    private void GridView_DoubleClick(object? sender, EventArgs e)
    {
        var punto = _grid.PointToClient(Cursor.Position);
        var info = _gridView.CalcHitInfo(punto);
        if (!info.InRowCell)
        {
            return;
        }

        BtnEditar_Click(sender, EventArgs.Empty);
    }

    private void BtnNuevo_Click(object? sender, EventArgs e) => AbrirEditarForm(null);

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un pallet.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(PalletDto? palletExistente)
    {
        if (_palletEditarForm is { IsDisposed: false })
        {
            if (_palletEditarForm.WindowState == FormWindowState.Minimized)
            {
                _palletEditarForm.WindowState = FormWindowState.Normal;
            }

            _palletEditarForm.Activate();
            return;
        }

        _palletEditarForm = new PalletEditarForm(
            _palletService, _reempaqueService, _lineaProduccionService, _productoTerminadoService,
            _categoriaService, _tipoProductoService, _calibreApeamService, _marcaService,
            _pesoEstandarService, _paisService, _variedadService,
            _configuracionBasculaService, _empresaConfiguracionService,
            _etiquetaService, _licenciaTecitService, _sessionContext, _sqlOptions, palletExistente);
        _palletEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _palletEditarForm.FormClosed += (_, _) => _palletEditarForm = null;
        _palletEditarForm.Show(this);
    }

    // Eliminar un Pallet completo revierte, del lado del SP, los Kilogramos de todas sus líneas
    // contra la Corrida correspondiente. Un Pallet bloqueado no se puede eliminar (lo rechaza el
    // propio SP con THROW 50000, y aquí además se avisa antes de intentarlo).
    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un pallet.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (seleccionado.Bloqueado)
        {
            XtraMessageBox.Show(this, "Este pallet ya está bloqueado: no se puede eliminar.", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Eliminar el pallet '{seleccionado.Folio}'? Los kilogramos de sus líneas se devolverán al saldo de la corrida correspondiente.",
            "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _palletService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private PalletDto? ObtenerSeleccionado() => _gridView.GetFocusedRow() as PalletDto;
}
