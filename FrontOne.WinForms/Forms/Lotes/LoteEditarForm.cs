using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Acopio;
using FrontOne.WinForms.Forms.Catalogos;
using FrontOne.WinForms.Forms.Recepcion;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Lotes;

public partial class LoteEditarForm : XtraForm
{
    private readonly LoteService _loteService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly LoteDto? _loteExistente;
    private readonly SessionContext _sessionContext = null!;

    // Servicios adicionales solo para abrir Acuerdo/Orden/Recepción al hacer clic en su folio
    // (columnas F. Acuerdo, F. OrdenC, F. Recepción del detalle) — mismo criterio que
    // RecepcionesFrutaForm/OrdenesCorteForm.
    private readonly AcuerdoCorteService _acuerdoCorteService = null!;
    private readonly ProductorService _productorService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly ProductoService _productoService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly TipoComercializacionService _tipoComercializacionService = null!;
    private readonly TipoCorteService _tipoCorteService = null!;
    private readonly TipoPagoService _tipoPagoService = null!;
    private readonly MonedaService _monedaService = null!;
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly OrdenCorteService _ordenCorteService = null!;
    private readonly HuertaService _huertaService = null!;
    private readonly FloracionService _floracionService = null!;
    private readonly ListaPrecioAcarreoService _listaPrecioAcarreoService = null!;
    private readonly ZonaService _zonaService = null!;
    private readonly ListaPrecioCorteService _listaPrecioCorteService = null!;
    private readonly JefeAcopioService _jefeAcopioService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PoblacionService _poblacionService = null!;
    private readonly RecepcionFrutaService _recepcionFrutaService = null!;

    private AcuerdoCorteEditarForm? _acuerdoCorteEditarFormDesdeGrid;
    private OrdenCorteEditarForm? _ordenCorteEditarFormDesdeGrid;
    private RecepcionFrutaEditarForm? _recepcionFrutaEditarFormDesdeGrid;

    private static readonly string[] ColumnasFolioClickeable = ["AcuerdoCorteFolio", "OrdenCorteFolio", "RecepcionFrutaFolio"];

    public event EventHandler? Guardado;

    private readonly BindingList<FilaDetalleLote> _filas = [];
    private readonly List<int> _idsEliminados = [];

    public LoteEditarForm()
    {
        InitializeComponent();
    }

    public LoteEditarForm(
        LoteService loteService,
        LineaProduccionService lineaProduccionService,
        LoteDto? loteExistente,
        SessionContext sessionContext,
        AcuerdoCorteService acuerdoCorteService,
        ProductorService productorService,
        PaisService paisService,
        EstadoService estadoService,
        ProductoService productoService,
        VariedadService variedadService,
        TipoComercializacionService tipoComercializacionService,
        TipoCorteService tipoCorteService,
        TipoPagoService tipoPagoService,
        MonedaService monedaService,
        ListaPrecioFrutaService listaPrecioFrutaService,
        OrdenCorteService ordenCorteService,
        HuertaService huertaService,
        FloracionService floracionService,
        ListaPrecioAcarreoService listaPrecioAcarreoService,
        ZonaService zonaService,
        ListaPrecioCorteService listaPrecioCorteService,
        JefeAcopioService jefeAcopioService,
        MunicipioService municipioService,
        PoblacionService poblacionService,
        RecepcionFrutaService recepcionFrutaService)
        : this()
    {
        _loteService = loteService;
        _lineaProduccionService = lineaProduccionService;
        _loteExistente = loteExistente;
        _sessionContext = sessionContext;
        _acuerdoCorteService = acuerdoCorteService;
        _productorService = productorService;
        _paisService = paisService;
        _estadoService = estadoService;
        _productoService = productoService;
        _variedadService = variedadService;
        _tipoComercializacionService = tipoComercializacionService;
        _tipoCorteService = tipoCorteService;
        _tipoPagoService = tipoPagoService;
        _monedaService = monedaService;
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _ordenCorteService = ordenCorteService;
        _huertaService = huertaService;
        _floracionService = floracionService;
        _listaPrecioAcarreoService = listaPrecioAcarreoService;
        _zonaService = zonaService;
        _listaPrecioCorteService = listaPrecioCorteService;
        _jefeAcopioService = jefeAcopioService;
        _municipioService = municipioService;
        _poblacionService = poblacionService;
        _recepcionFrutaService = recepcionFrutaService;

        _gridDetalle.DataSource = _filas;
        ConfigurarColumnasDetalle();
        _gridViewDetalle.MouseMove += GridViewDetalle_MouseMove;
        _gridViewDetalle.RowCellClick += GridViewDetalle_RowCellClick;
        _filas.ListChanged += (_, _) =>
        {
            RecalcularKilogramos();
            RecalcularMateriaSeca();
        };

        Load += async (_, _) => await CargarAsync();
    }

    private async Task CargarAsync()
    {
        await CargarLineasProduccionAsync();

        if (_loteExistente is null)
        {
            _txtFolio.Text = "(se genera al guardar)";
            _txtReferencia.Text = "(se genera al guardar)";
            _dtFecha.EditValue = DateTime.Today;
            RecalcularKilogramos();
            RecalcularMateriaSeca();
            return;
        }

        _txtFolio.Text = _loteExistente.Folio;
        _txtReferencia.Text = _loteExistente.Referencia;
        _dtFecha.EditValue = _loteExistente.Fecha;
        _txtObservaciones.Text = _loteExistente.Observaciones;
        _txtPersonalizado.Text = _loteExistente.Personalizado;
        _cmbLineaProduccion.EditValue = _loteExistente.LineaProduccionId;

        try
        {
            var detalle = await _loteService.ObtenerDetalleAsync(_loteExistente.Id);
            foreach (var linea in detalle)
            {
                _filas.Add(new FilaDetalleLote
                {
                    DetalleId = linea.Id,
                    RecepcionFrutaId = linea.RecepcionFrutaId,
                    RecepcionFrutaFolio = linea.RecepcionFrutaFolio,
                    NumeroTicket = linea.NumeroTicket,
                    Fecha = linea.Fecha,
                    CoprefBico = linea.CoprefBico,
                    PesoNeto = linea.PesoNeto,
                    PorcentajeMateriaSeca = linea.PorcentajeMateriaSeca,
                    OrdenCorteFolio = linea.OrdenCorteFolio,
                    AcuerdoCorteFolio = linea.AcuerdoCorteFolio,
                });
            }
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        RecalcularKilogramos();
    }

    private async Task CargarLineasProduccionAsync()
    {
        var lineas = await _lineaProduccionService.ObtenerAsync();
        _cmbLineaProduccion.Properties.DataSource = lineas.ToList();
        _cmbLineaProduccion.Properties.ValueMember = "Id";
        _cmbLineaProduccion.Properties.DisplayMember = "Nombre";
        _cmbLineaProduccion.Properties.Columns.Clear();
        _cmbLineaProduccion.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 200, "Línea de Producción"));
        _cmbLineaProduccion.Properties.PopupWidth = 230;
    }

    private async void CmbLineaProduccion_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new LineasProduccionForm(_lineaProduccionService);
        form.ShowDialog(this);
        await CargarLineasProduccionAsync();
    }

    private void ConfigurarColumnasDetalle()
    {
        foreach (var nombre in new[] { "DetalleId", "RecepcionFrutaId", "HuertaId", "AcuerdoCorteId", "PagarCorteACardCode" })
        {
            if (_gridViewDetalle.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridViewDetalle.Columns["AcuerdoCorteFolio"] is { } colAcuerdo)
        {
            colAcuerdo.Caption = "F. Acuerdo";
            colAcuerdo.VisibleIndex = 0;
            AplicarEstiloFolioClickeable(colAcuerdo);
        }

        if (_gridViewDetalle.Columns["OrdenCorteFolio"] is { } colOrden)
        {
            colOrden.Caption = "F. OrdenC";
            colOrden.VisibleIndex = 1;
            AplicarEstiloFolioClickeable(colOrden);
        }

        if (_gridViewDetalle.Columns["RecepcionFrutaFolio"] is { } colFolio)
        {
            colFolio.Caption = "F. Recepción";
            colFolio.VisibleIndex = 2;
            AplicarEstiloFolioClickeable(colFolio);
        }

        if (_gridViewDetalle.Columns["NumeroTicket"] is { } colTicket)
        {
            colTicket.Caption = "Ticket";
        }

        if (_gridViewDetalle.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridViewDetalle.Columns["CoprefBico"] is { } colCopref)
        {
            colCopref.Caption = "COPREF/BICO";
        }

        if (_gridViewDetalle.Columns["PesoNeto"] is { } colPesoNeto)
        {
            colPesoNeto.Caption = "Peso Neto";
            colPesoNeto.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colPesoNeto.DisplayFormat.FormatString = "n2";
        }

        if (_gridViewDetalle.Columns["PorcentajeMateriaSeca"] is { } colMateriaSeca)
        {
            colMateriaSeca.Caption = "% Materia Seca";
            colMateriaSeca.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMateriaSeca.DisplayFormat.FormatString = "n2";
        }

        _gridViewDetalle.BestFitColumns();
    }

    // F. Acuerdo, F. OrdenC y F. Recepción se ven como folio clickeable (azul + negritas +
    // subrayado) — al hacer clic abren el módulo correspondiente cargado con ese registro,
    // validando permiso antes (mismo criterio que RecepcionesFrutaForm/OrdenesCorteForm).
    private static void AplicarEstiloFolioClickeable(DevExpress.XtraGrid.Columns.GridColumn columna)
    {
        columna.AppearanceCell.Font = new Font(columna.AppearanceCell.Font, FontStyle.Bold | FontStyle.Underline);
        columna.AppearanceCell.ForeColor = ColorTranslator.FromHtml("#0563C1");
        columna.AppearanceCell.Options.UseFont = true;
        columna.AppearanceCell.Options.UseForeColor = true;
    }

    private void GridViewDetalle_MouseMove(object? sender, MouseEventArgs e)
    {
        var info = _gridViewDetalle.CalcHitInfo(e.Location);
        _gridDetalle.Cursor = info.InRowCell && ColumnasFolioClickeable.Contains(info.Column?.FieldName)
            ? Cursors.Hand
            : Cursors.Default;
    }

    private async void GridViewDetalle_RowCellClick(object? sender, RowCellClickEventArgs e)
    {
        switch (e.Column.FieldName)
        {
            case "AcuerdoCorteFolio":
                await AbrirAcuerdoPorFolioAsync(e.CellValue as string);
                break;
            case "OrdenCorteFolio":
                await AbrirOrdenPorFolioAsync(e.CellValue as string);
                break;
            case "RecepcionFrutaFolio":
                await AbrirRecepcionAsync(_gridViewDetalle.GetRow(e.RowHandle) as FilaDetalleLote);
                break;
        }
    }

    private async Task AbrirAcuerdoPorFolioAsync(string? folio)
    {
        if (string.IsNullOrWhiteSpace(folio))
        {
            return;
        }

        if (!_sessionContext.TienePermiso("Acopio", "AcuerdosCorte", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var acuerdo = await _acuerdoCorteService.ObtenerPorFolioAsync(folio);
        if (acuerdo is null)
        {
            XtraMessageBox.Show(this, "El acuerdo de corte ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_acuerdoCorteEditarFormDesdeGrid is { IsDisposed: false })
        {
            if (_acuerdoCorteEditarFormDesdeGrid.WindowState == FormWindowState.Minimized)
            {
                _acuerdoCorteEditarFormDesdeGrid.WindowState = FormWindowState.Normal;
            }

            _acuerdoCorteEditarFormDesdeGrid.Activate();
            return;
        }

        _acuerdoCorteEditarFormDesdeGrid = new AcuerdoCorteEditarForm(
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService, acuerdo);
        _acuerdoCorteEditarFormDesdeGrid.FormClosed += (_, _) => _acuerdoCorteEditarFormDesdeGrid = null;
        _acuerdoCorteEditarFormDesdeGrid.Show(this);
    }

    private async Task AbrirOrdenPorFolioAsync(string? folio)
    {
        if (string.IsNullOrWhiteSpace(folio))
        {
            return;
        }

        if (!_sessionContext.TienePermiso("Acopio", "OrdenesCorte", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var orden = await _ordenCorteService.ObtenerPorFolioAsync(folio);
        if (orden is null)
        {
            XtraMessageBox.Show(this, "La orden de corte ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_ordenCorteEditarFormDesdeGrid is { IsDisposed: false })
        {
            if (_ordenCorteEditarFormDesdeGrid.WindowState == FormWindowState.Minimized)
            {
                _ordenCorteEditarFormDesdeGrid.WindowState = FormWindowState.Normal;
            }

            _ordenCorteEditarFormDesdeGrid.Activate();
            return;
        }

        _ordenCorteEditarFormDesdeGrid = new OrdenCorteEditarForm(
            _ordenCorteService, _huertaService, _floracionService, _variedadService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _tipoCorteService,
            _paisService, _estadoService, _municipioService, _poblacionService, orden);
        _ordenCorteEditarFormDesdeGrid.FormClosed += (_, _) => _ordenCorteEditarFormDesdeGrid = null;
        _ordenCorteEditarFormDesdeGrid.Show(this);
    }

    private async Task AbrirRecepcionAsync(FilaDetalleLote? fila)
    {
        if (fila is null)
        {
            return;
        }

        if (!_sessionContext.TienePermiso("Recepcion", "RecepcionesFruta", "Consultar"))
        {
            XtraMessageBox.Show(this, "Acceso denegado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var recepcion = await _recepcionFrutaService.ObtenerPorIdAsync(fila.RecepcionFrutaId);
        if (recepcion is null)
        {
            XtraMessageBox.Show(this, "La recepción de fruta ya no existe.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_recepcionFrutaEditarFormDesdeGrid is { IsDisposed: false })
        {
            if (_recepcionFrutaEditarFormDesdeGrid.WindowState == FormWindowState.Minimized)
            {
                _recepcionFrutaEditarFormDesdeGrid.WindowState = FormWindowState.Normal;
            }

            _recepcionFrutaEditarFormDesdeGrid.Activate();
            return;
        }

        _recepcionFrutaEditarFormDesdeGrid = new RecepcionFrutaEditarForm(_recepcionFrutaService, recepcion);
        _recepcionFrutaEditarFormDesdeGrid.FormClosed += (_, _) => _recepcionFrutaEditarFormDesdeGrid = null;
        _recepcionFrutaEditarFormDesdeGrid.Show(this);
    }

    // Kilogramos del encabezado nunca se captura a mano — siempre es la suma en vivo del Peso
    // Neto de las Recepciones agregadas (pedido explícito del usuario).
    private void RecalcularKilogramos()
        => _spnKilogramos.EditValue = _filas.Sum(f => f.PesoNeto);

    // % Materia Seca del encabezado tampoco se captura a mano — se toma de las Recepciones
    // agregadas (promedio simple entre las líneas, pedido explícito del usuario).
    private void RecalcularMateriaSeca()
        => _spnPorcentajeMateriaSeca.EditValue = _filas.Count > 0 ? _filas.Average(f => f.PorcentajeMateriaSeca) : 0m;

    private async void BtnDetalleNuevo_Click(object? sender, EventArgs e)
    {
        // Si ya hay líneas, el picker se filtra en SQL a solo Recepciones compatibles con la
        // primera (misma Huerta/Acuerdo/Proveedor) — si es la primera línea, se muestran todas.
        int? huertaId = null;
        int? acuerdoCorteId = null;
        string? pagarCorteACardCode = null;
        if (_filas.Count > 0)
        {
            huertaId = _filas[0].HuertaId;
            acuerdoCorteId = _filas[0].AcuerdoCorteId;
            pagarCorteACardCode = _filas[0].PagarCorteACardCode;
        }

        using var form = new SeleccionarRecepcionForm(_loteService, huertaId, acuerdoCorteId, pagarCorteACardCode);
        if (form.ShowDialog(this) != DialogResult.OK || form.RecepcionSeleccionada is null)
        {
            return;
        }

        var seleccionada = form.RecepcionSeleccionada;
        _filas.Add(new FilaDetalleLote
        {
            RecepcionFrutaId = seleccionada.Id,
            RecepcionFrutaFolio = seleccionada.Folio,
            NumeroTicket = seleccionada.NumeroTicket,
            Fecha = seleccionada.Fecha,
            PesoNeto = seleccionada.PesoNeto,
            PorcentajeMateriaSeca = seleccionada.PorcentajeMateriaSeca,
            CoprefBico = seleccionada.CoprefBico,
            HuertaId = seleccionada.HuertaId,
            AcuerdoCorteId = seleccionada.AcuerdoCorteId,
            PagarCorteACardCode = seleccionada.PagarCorteACardCode,
            OrdenCorteFolio = seleccionada.OrdenCorteFolio,
            AcuerdoCorteFolio = seleccionada.AcuerdoCorteFolio,
        });
    }

    private void BtnDetalleBorrar_Click(object? sender, EventArgs e)
    {
        var fila = ObtenerFilaSeleccionada();
        if (fila is null)
        {
            XtraMessageBox.Show(this, "Selecciona un renglón del detalle.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Quitar la recepción '{fila.RecepcionFrutaFolio}' de este Lote?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        if (fila.DetalleId.HasValue)
        {
            _idsEliminados.Add(fila.DetalleId.Value);
        }

        _filas.Remove(fila);
    }

    private FilaDetalleLote? ObtenerFilaSeleccionada()
        => _gridViewDetalle.GetFocusedRow() as FilaDetalleLote;

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var dto = new LoteDto(
            _loteExistente?.Id ?? 0,
            _loteExistente?.Folio ?? string.Empty,
            (DateTime)_dtFecha.EditValue,
            _loteExistente?.Referencia ?? string.Empty,
            _txtObservaciones.Text,
            (decimal)_spnKilogramos.EditValue,
            _txtPersonalizado.Text,
            _cmbLineaProduccion.EditValue is int lineaProduccionId ? lineaProduccionId : 0,
            string.Empty,
            (decimal)_spnPorcentajeMateriaSeca.EditValue,
            0,
            0,
            null,
            null);

        try
        {
            if (_loteExistente is null)
            {
                var resultado = await _loteService.CrearAsync(dto);
                foreach (var fila in _filas)
                {
                    await _loteService.AgregarLineaAsync(resultado.Id, fila.RecepcionFrutaId);
                }
            }
            else
            {
                await _loteService.ActualizarAsync(dto);

                foreach (var idEliminado in _idsEliminados)
                {
                    await _loteService.EliminarLineaAsync(idEliminado);
                }

                foreach (var fila in _filas.Where(f => !f.DetalleId.HasValue))
                {
                    await _loteService.AgregarLineaAsync(_loteExistente.Id, fila.RecepcionFrutaId);
                }
            }

            Guardado?.Invoke(this, EventArgs.Empty);
            Close();
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
