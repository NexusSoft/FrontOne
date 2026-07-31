using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Catalogos;

namespace FrontOne.WinForms.Forms.Lotes;

public partial class LoteEditarForm : XtraForm
{
    private readonly LoteService _loteService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly LoteDto? _loteExistente;

    private readonly BindingList<FilaDetalleLote> _filas = [];
    private readonly List<int> _idsEliminados = [];

    public LoteEditarForm()
    {
        InitializeComponent();
    }

    public LoteEditarForm(LoteService loteService, LineaProduccionService lineaProduccionService, LoteDto? loteExistente)
        : this()
    {
        _loteService = loteService;
        _lineaProduccionService = lineaProduccionService;
        _loteExistente = loteExistente;

        _gridDetalle.DataSource = _filas;
        ConfigurarColumnasDetalle();
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

        if (_gridViewDetalle.Columns["RecepcionFrutaFolio"] is { } colFolio)
        {
            colFolio.Caption = "No. Recepción";
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

            DialogResult = DialogResult.OK;
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
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
