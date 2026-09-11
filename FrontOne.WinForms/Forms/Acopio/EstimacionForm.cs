using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using FrontOne.Application.Services;
using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Acopio;

// Estimación de precio sugerido por Huerta/Categoría/Calibre — cruza porcentajes capturados a
// mano contra una vigencia de Lista de Precios de Fruta ya guardada. A diferencia del Simulador
// de Bandas, sí persiste (guarda un Folio) para poder ligarse después a una Orden de Corte
// (OrdenCorteEditarForm._beEstimacion). Fórmula y precedentes documentados en contexto/acopio.md.
public partial class EstimacionForm : XtraForm
{
    private readonly HuertaService _huertaService = null!;
    private readonly ProductorService _productorService = null!;
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly EstimacionService _estimacionService = null!;
    private readonly JefeAcopioService _jefeAcopioService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PoblacionService _poblacionService = null!;

    // Mismos colores pastel por Categoría que ListaPrecioFrutaForm.ColoresPorCategoria — solo en
    // las columnas de identidad (Calibre/Categoría); las columnas de precio llevan su propio
    // color fijo por lista (Convencional/Orgánica/Nacional), definido en el Designer.
    private static readonly Dictionary<string, Color> ColoresPorCategoria = new()
    {
        ["Cat 1"] = ColorTranslator.FromHtml("#E8DAEF"),
        ["Cat 2"] = ColorTranslator.FromHtml("#FCF3CF"),
        ["Nal"] = ColorTranslator.FromHtml("#FADBD8"),
    };

    private Dictionary<(string Categoria, string Calibre), (int CategoriaId, int CalibreApeamId)> _combinaciones = new();
    private IReadOnlyList<ListaPrecioFrutaDto> _preciosVigencia = [];
    private VigenciaListaPrecioFrutaDto? _vigenciaSeleccionada;
    private int? _huertaId;
    private int? _estimacionId;
    private bool _cerrada;
    private bool _autorizada;
    private bool _cargandoDatos;
    private decimal _precioSugeridoActual;

    public EstimacionForm()
    {
        InitializeComponent();
    }

    public EstimacionForm(
        HuertaService huertaService,
        ProductorService productorService,
        ListaPrecioFrutaService listaPrecioFrutaService,
        EstimacionService estimacionService,
        JefeAcopioService jefeAcopioService,
        PaisService paisService,
        EstadoService estadoService,
        MunicipioService municipioService,
        PoblacionService poblacionService)
        : this()
    {
        _huertaService = huertaService;
        _productorService = productorService;
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _estimacionService = estimacionService;
        _jefeAcopioService = jefeAcopioService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;
        _poblacionService = poblacionService;

        _cmbTipoLista.Properties.Items.AddRange(ListasPrecioFruta.Nombres.Cast<object>().ToArray());
        _cmbTipoLista.SelectedIndex = 0;
        _dtFecha.EditValue = DateTime.Today;
        _gridViewPrecios.RowCellStyle += GridViewPrecios_RowCellStyle;

        Load += async (_, _) =>
        {
            await CargarCombinacionesAsync();
            await CargarAcopiadoresAsync();
        };
    }

    private async Task CargarCombinacionesAsync()
    {
        var combinaciones = await _listaPrecioFrutaService.ObtenerCombinacionesActivasAsync();
        _combinaciones = combinaciones.ToDictionary(
            c => (c.CategoriaNombre, c.CalibreApeamNombre),
            c => (c.CategoriaId, c.CalibreApeamId));
    }

    private async Task CargarAcopiadoresAsync()
    {
        var acopiadores = await _jefeAcopioService.ObtenerAsync();
        _cmbAcopiador.Properties.DataSource = acopiadores.ToList();
        _cmbAcopiador.Properties.ValueMember = "Id";
        _cmbAcopiador.Properties.DisplayMember = "Nombre";
        _cmbAcopiador.Properties.Columns.Clear();
        _cmbAcopiador.Properties.Columns.Add(new LookUpColumnInfo("Clave", 70, "Clave"));
        _cmbAcopiador.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Nombre"));
        _cmbAcopiador.Properties.PopupWidth = 320;
    }

    private async void CmbAcopiador_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new JefeAcopioEditarForm(_jefeAcopioService, _paisService, _estadoService, _municipioService, _poblacionService);
        form.ShowDialog(this);
        await CargarAcopiadoresAsync();
    }

    private void GridViewPrecios_RowCellStyle(object? sender, RowCellStyleEventArgs e)
    {
        if (e.Column.FieldName is not ("CategoriaNombre" or "CalibreApeamNombre"))
        {
            return;
        }

        if (_gridViewPrecios.GetRowCellValue(e.RowHandle, "CategoriaNombre") is string categoria
            && ColoresPorCategoria.TryGetValue(categoria, out var color))
        {
            e.Appearance.BackColor = color;
            e.Appearance.Options.UseBackColor = true;
        }
    }

    private void BeHuerta_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Search)
        {
            return;
        }

        using var buscador = new HuertasForm(_huertaService, _productorService);
        if (buscador.ShowDialog(this) != DialogResult.OK || buscador.HuertaSeleccionada is not { } huerta)
        {
            return;
        }

        _huertaId = huerta.Id;
        _beHuerta.Text = huerta.Nombre;
        _txtRegistroSagarpa.Text = huerta.RegistroSagarpa ?? string.Empty;
    }

    private async void BeListaPrecio_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Search)
        {
            return;
        }

        using var buscador = new BuscarListaPrecioFrutaForm(_listaPrecioFrutaService);
        if (buscador.ShowDialog(this) != DialogResult.OK || buscador.VigenciaSeleccionada is not { } vigencia)
        {
            return;
        }

        await AplicarVigenciaSeleccionadaAsync(vigencia);
    }

    private async Task AplicarVigenciaSeleccionadaAsync(VigenciaListaPrecioFrutaDto vigencia)
    {
        _vigenciaSeleccionada = vigencia;
        _preciosVigencia = await _listaPrecioFrutaService.ObtenerPorFechaAsync(vigencia.Fecha, vigencia.ProductorId);

        _beListaPrecio.Text = vigencia.ProductorNombre is null
            ? $"{vigencia.Fecha:dd/MM/yyyy} (general)"
            : $"{vigencia.Fecha:dd/MM/yyyy} — {vigencia.ProductorNombre}";
        _lblListaCargada.Text = $"Precios cargados: {_preciosVigencia.Count} combinaciones ({ListasPrecioFruta.Nombres[_cmbTipoLista.SelectedIndex]}).";
        _lblListaCargada.Visible = true;
        _gridPrecios.DataSource = _preciosVigencia.ToList();

        RecalcularPrecioSugerido();
    }

    private async void ChkListaMasReciente_CheckedChanged(object? sender, EventArgs e)
    {
        if (_cargandoDatos)
        {
            return;
        }

        _beListaPrecio.Enabled = !_chkListaMasReciente.Checked;

        if (!_chkListaMasReciente.Checked)
        {
            _vigenciaSeleccionada = null;
            _preciosVigencia = [];
            _beListaPrecio.Text = string.Empty;
            _lblListaCargada.Visible = false;
            _gridPrecios.DataSource = null;
            RecalcularPrecioSugerido();
            return;
        }

        var vigencias = await _listaPrecioFrutaService.ObtenerFechasAsync();
        var masReciente = vigencias.OrderByDescending(v => v.Fecha).FirstOrDefault();
        if (masReciente is null)
        {
            XtraMessageBox.Show(this, "No hay listas de precios capturadas.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _chkListaMasReciente.Checked = false;
            return;
        }

        await AplicarVigenciaSeleccionadaAsync(masReciente);
    }

    private void Recalcular_EditValueChanged(object? sender, EventArgs e)
    {
        ActualizarAvisos();
        RecalcularPrecioSugerido();
    }

    // Avisa sin bloquear (mismo criterio que SimuladorBandasForm.ActualizarAvisoSuma) — cada
    // sección puede sumar 100% de forma independiente.
    private void ActualizarAvisos()
    {
        ActualizarAviso(_lblAvisoCategorias, (decimal)_spnCat1.Value + (decimal)_spnCat2.Value + (decimal)_spnNal.Value);

        var sumaExport = (decimal)_spnCalibre32.Value + (decimal)_spnCalibre36.Value + (decimal)_spnCalibre40.Value + (decimal)_spnCalibre48.Value
            + (decimal)_spnCalibre60.Value + (decimal)_spnCalibre70.Value + (decimal)_spnCalibre84.Value + (decimal)_spnCalibre90.Value;
        ActualizarAviso(_lblAvisoCalibresExport, sumaExport);

        var sumaNacional = (decimal)_spnBorona.Value + (decimal)_spnCanica.Value + (decimal)_spnCuarta.Value + (decimal)_spnDesecho.Value + (decimal)_spnProceso.Value;
        ActualizarAviso(_lblAvisoCalibresNacional, sumaNacional);
    }

    private static void ActualizarAviso(LabelControl aviso, decimal suma)
    {
        if (Math.Abs(suma - 100m) > 0.005m)
        {
            aviso.Text = $"La suma de porcentajes es {suma:n2}%, debería ser 100%.";
            aviso.Visible = true;
        }
        else
        {
            aviso.Visible = false;
        }
    }

    // Precio Sugerido = %Cat1 * Σ(%calibreExport * PrecioCat1Calibre) + %Cat2 * Σ(%calibreExport *
    // PrecioCat2Calibre) + %Nal * Σ(%calibreNacional * PrecioNalCalibre) — validado contra el
    // ejercicio real del usuario (contexto/acopio.md), coincide exacto con la celda AU del Excel.
    private void RecalcularPrecioSugerido()
    {
        if (_preciosVigencia.Count == 0)
        {
            _precioSugeridoActual = 0;
            _lblPrecioSugerido.Text = "Precio Sugerido: —";
            return;
        }

        var indiceLista = _cmbTipoLista.SelectedIndex;

        var pctCat1 = (decimal)_spnCat1.Value / 100m;
        var pctCat2 = (decimal)_spnCat2.Value / 100m;
        var pctNal = (decimal)_spnNal.Value / 100m;

        var sumaCat1 = SumaCalibresExport("Cat 1", indiceLista);
        var sumaCat2 = SumaCalibresExport("Cat 2", indiceLista);
        var sumaNal = SumaCalibresNacional(indiceLista);

        _precioSugeridoActual = (pctCat1 * sumaCat1) + (pctCat2 * sumaCat2) + (pctNal * sumaNal);
        _lblPrecioSugerido.Text = $"Precio Sugerido: {_precioSugeridoActual:c2}";
    }

    private decimal SumaCalibresExport(string categoria, int indiceLista) =>
        ((decimal)_spnCalibre32.Value / 100m * PrecioCombo(categoria, "32", indiceLista))
        + ((decimal)_spnCalibre36.Value / 100m * PrecioCombo(categoria, "36", indiceLista))
        + ((decimal)_spnCalibre40.Value / 100m * PrecioCombo(categoria, "40", indiceLista))
        + ((decimal)_spnCalibre48.Value / 100m * PrecioCombo(categoria, "48", indiceLista))
        + ((decimal)_spnCalibre60.Value / 100m * PrecioCombo(categoria, "60", indiceLista))
        + ((decimal)_spnCalibre70.Value / 100m * PrecioCombo(categoria, "70", indiceLista))
        + ((decimal)_spnCalibre84.Value / 100m * PrecioCombo(categoria, "84", indiceLista))
        + ((decimal)_spnCalibre90.Value / 100m * PrecioCombo(categoria, "90", indiceLista));

    private decimal SumaCalibresNacional(int indiceLista) =>
        ((decimal)_spnBorona.Value / 100m * PrecioCombo("Nal", "Borona", indiceLista))
        + ((decimal)_spnCanica.Value / 100m * PrecioCombo("Nal", "Canica", indiceLista))
        + ((decimal)_spnCuarta.Value / 100m * PrecioCombo("Nal", "Cuarta", indiceLista))
        + ((decimal)_spnDesecho.Value / 100m * PrecioCombo("Nal", "Desecho", indiceLista))
        + ((decimal)_spnProceso.Value / 100m * PrecioCombo("Nal", "Proceso", indiceLista));

    // Combos sin precio en la vigencia elegida cuentan como 0 — mismo criterio que SimuladorBandasForm.
    private decimal PrecioCombo(string categoria, string calibre, int indiceLista)
    {
        if (!_combinaciones.TryGetValue((categoria, calibre), out var ids))
        {
            return 0m;
        }

        var precio = _preciosVigencia.FirstOrDefault(p => p.CategoriaId == ids.CategoriaId && p.CalibreApeamId == ids.CalibreApeamId);
        return precio is null ? 0m : PrecioDeLista(precio, indiceLista);
    }

    private static decimal PrecioDeLista(ListaPrecioFrutaDto dto, int indiceLista) => indiceLista switch
    {
        0 => dto.Convencional,
        1 => dto.Organico,
        _ => dto.Nacional,
    };

    private async void BtnBuscar_Click(object? sender, EventArgs e)
    {
        using var buscador = new BuscarEstimacionForm(_estimacionService);
        if (buscador.ShowDialog(this) != DialogResult.OK || buscador.EstimacionSeleccionada is not { } seleccionada)
        {
            return;
        }

        var dto = await _estimacionService.ObtenerPorIdAsync(seleccionada.Id);
        if (dto is null)
        {
            return;
        }

        await CargarEstimacionAsync(dto);
    }

    private async Task CargarEstimacionAsync(EstimacionDto dto)
    {
        _cargandoDatos = true;

        _estimacionId = dto.Id;
        _cerrada = dto.Cerrada;
        _autorizada = dto.Autorizada;
        _chkListaMasReciente.Checked = dto.UsarListaMasReciente;

        _lblFolio.Text = $"Folio: {dto.Folio}";
        _lblAutorizacion.Text = _autorizada ? "Autorización: Autorizada" : "Autorización: Pendiente";
        _dtFecha.EditValue = dto.Fecha;
        _huertaId = dto.HuertaId;
        _beHuerta.Text = dto.HuertaNombre;
        _txtRegistroSagarpa.Text = dto.RegistroSagarpa ?? string.Empty;
        _spnKilos.EditValue = dto.Kilos;
        _cmbAcopiador.EditValue = dto.AcopiadorId;

        _spnCat1.EditValue = dto.PorcentajeCat1;
        _spnCat2.EditValue = dto.PorcentajeCat2;
        _spnNal.EditValue = dto.PorcentajeNal;

        _spnCalibre32.EditValue = dto.PorcentajeCalibre32;
        _spnCalibre36.EditValue = dto.PorcentajeCalibre36;
        _spnCalibre40.EditValue = dto.PorcentajeCalibre40;
        _spnCalibre48.EditValue = dto.PorcentajeCalibre48;
        _spnCalibre60.EditValue = dto.PorcentajeCalibre60;
        _spnCalibre70.EditValue = dto.PorcentajeCalibre70;
        _spnCalibre84.EditValue = dto.PorcentajeCalibre84;
        _spnCalibre90.EditValue = dto.PorcentajeCalibre90;

        _spnBorona.EditValue = dto.PorcentajeBorona;
        _spnCanica.EditValue = dto.PorcentajeCanica;
        _spnCuarta.EditValue = dto.PorcentajeCuarta;
        _spnDesecho.EditValue = dto.PorcentajeDesecho;
        _spnProceso.EditValue = dto.PorcentajeProceso;

        _cmbTipoLista.SelectedIndex = dto.TipoLista;
        _vigenciaSeleccionada = new VigenciaListaPrecioFrutaDto(dto.ListaPrecioFecha, dto.ListaPrecioProductorId, null);
        _preciosVigencia = await _listaPrecioFrutaService.ObtenerPorFechaAsync(dto.ListaPrecioFecha, dto.ListaPrecioProductorId);
        _beListaPrecio.Text = $"{dto.ListaPrecioFecha:dd/MM/yyyy}";
        _lblListaCargada.Text = $"Precios cargados: {_preciosVigencia.Count} combinaciones ({ListasPrecioFruta.Nombres[dto.TipoLista]}).";
        _lblListaCargada.Visible = true;
        _gridPrecios.DataSource = _preciosVigencia.ToList();

        ActualizarAvisos();
        RecalcularPrecioSugerido();

        _cargandoDatos = false;

        if (_cerrada || _autorizada)
        {
            AplicarSoloLectura();
            var mensaje = _autorizada
                ? "Esta estimación ya está autorizada y no se puede editar. Desautorízala primero."
                : "Esta estimación ya está asignada a una Orden de Corte y no se puede editar.";
            XtraMessageBox.Show(this, mensaje, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void AplicarSoloLectura()
    {
        _dtFecha.Enabled = false;
        _beHuerta.Enabled = false;
        _spnKilos.Enabled = false;
        _cmbAcopiador.Enabled = false;
        _beListaPrecio.Enabled = false;
        _chkListaMasReciente.Enabled = false;
        _cmbTipoLista.Enabled = false;
        _spnCat1.Enabled = false;
        _spnCat2.Enabled = false;
        _spnNal.Enabled = false;
        _spnCalibre32.Enabled = false;
        _spnCalibre36.Enabled = false;
        _spnCalibre40.Enabled = false;
        _spnCalibre48.Enabled = false;
        _spnCalibre60.Enabled = false;
        _spnCalibre70.Enabled = false;
        _spnCalibre84.Enabled = false;
        _spnCalibre90.Enabled = false;
        _spnBorona.Enabled = false;
        _spnCanica.Enabled = false;
        _spnCuarta.Enabled = false;
        _spnDesecho.Enabled = false;
        _spnProceso.Enabled = false;
        _btnGuardar.Enabled = false;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_huertaId is not { } huertaId)
        {
            XtraMessageBox.Show(this, "Busca y selecciona la huerta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_vigenciaSeleccionada is null || _preciosVigencia.Count == 0)
        {
            XtraMessageBox.Show(this, "Busca y carga una Lista de Precios.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var dto = new EstimacionDto(
            _estimacionId ?? 0,
            string.Empty,
            (DateTime)_dtFecha.EditValue,
            huertaId,
            _beHuerta.Text,
            string.IsNullOrWhiteSpace(_txtRegistroSagarpa.Text) ? null : _txtRegistroSagarpa.Text,
            (decimal)_spnKilos.Value,
            _cmbAcopiador.EditValue as int?,
            _cmbAcopiador.EditValue is int ? _cmbAcopiador.Text : null,
            (decimal)_spnCat1.Value,
            (decimal)_spnCat2.Value,
            (decimal)_spnNal.Value,
            (decimal)_spnCalibre32.Value,
            (decimal)_spnCalibre36.Value,
            (decimal)_spnCalibre40.Value,
            (decimal)_spnCalibre48.Value,
            (decimal)_spnCalibre60.Value,
            (decimal)_spnCalibre70.Value,
            (decimal)_spnCalibre84.Value,
            (decimal)_spnCalibre90.Value,
            (decimal)_spnBorona.Value,
            (decimal)_spnCanica.Value,
            (decimal)_spnCuarta.Value,
            (decimal)_spnDesecho.Value,
            (decimal)_spnProceso.Value,
            _vigenciaSeleccionada.Fecha,
            _vigenciaSeleccionada.ProductorId,
            (byte)_cmbTipoLista.SelectedIndex,
            _precioSugeridoActual,
            false,
            false,
            _chkListaMasReciente.Checked);

        try
        {
            if (_estimacionId is null)
            {
                var resultado = await _estimacionService.GuardarAsync(dto);
                _estimacionId = resultado.Id;
                _lblFolio.Text = $"Folio: {resultado.Folio}";
                XtraMessageBox.Show(this, $"Estimación guardada con folio {resultado.Folio}.", "FrontOne",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                await _estimacionService.ActualizarAsync(dto with { Id = _estimacionId.Value });
                XtraMessageBox.Show(this, "Estimación actualizada.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
