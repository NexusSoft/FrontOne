using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Recepcion;

public partial class RecepcionFrutaEditarForm : XtraForm
{
    private readonly RecepcionFrutaService _recepcionFrutaService = null!;
    private readonly RecepcionFrutaDto? _recepcionExistente;

    public event EventHandler? Guardado;

    private readonly BindingList<FilaDetalleRecepcion> _filas = [];
    private readonly List<int> _idsEliminados = [];

    private byte[]? _ticketPesadaArchivo;
    private string? _ticketPesadaNombreArchivo;

    public RecepcionFrutaEditarForm()
    {
        InitializeComponent();
    }

    public RecepcionFrutaEditarForm(RecepcionFrutaService recepcionFrutaService, RecepcionFrutaDto? recepcionExistente)
        : this()
    {
        _recepcionFrutaService = recepcionFrutaService;
        _recepcionExistente = recepcionExistente;

        _gridDetalle.DataSource = _filas;
        ConfigurarColumnasDetalle();

        Load += async (_, _) => await CargarAsync();
    }

    private async Task CargarAsync()
    {
        if (_recepcionExistente is null)
        {
            _txtFolio.Text = "(se genera al guardar)";
            _dtFecha.EditValue = DateTime.Today;
            RecalcularPesoNeto();
            RecalcularDiferenciaCajas();
            return;
        }

        _txtFolio.Text = _recepcionExistente.Folio;
        _txtNoLote.Text = _recepcionExistente.NoLote;
        _dtFecha.EditValue = _recepcionExistente.Fecha;
        _txtChofer.Text = _recepcionExistente.Chofer;
        _txtPlacas.Text = _recepcionExistente.Placas;
        _txtObservaciones.Text = _recepcionExistente.Observaciones;
        _txtNumeroTicket.Text = _recepcionExistente.NumeroTicket;
        _txtCoprefBico.Text = _recepcionExistente.CoprefBico;
        _spnPesoBruto.EditValue = _recepcionExistente.PesoBruto;
        _spnPesoTara.EditValue = _recepcionExistente.PesoTara;
        _spnTaraCajas.EditValue = _recepcionExistente.TaraCajas;
        _spnPesoMuestra.EditValue = _recepcionExistente.PesoMuestra;
        _spnPesoProductor.EditValue = _recepcionExistente.PesoProductor;
        _spnPorcentajeMateriaSeca.EditValue = _recepcionExistente.PorcentajeMateriaSeca;
        _spnCajasPorEntregar.EditValue = (decimal)_recepcionExistente.CajasPorEntregar;
        _spnCajasEntregadas.EditValue = (decimal)_recepcionExistente.CajasEntregadas;
        _spnCajasCortadas.EditValue = (decimal)_recepcionExistente.CajasCortadas;
        _spnCajasRecibidasVacias.EditValue = (decimal)_recepcionExistente.CajasRecibidasVacias;
        _spnCajasPerdidas.EditValue = (decimal)_recepcionExistente.CajasPerdidas;
        _chkCamionDestarado.Checked = _recepcionExistente.CamionDestarado;
        _ticketPesadaArchivo = _recepcionExistente.TicketPesadaArchivo;
        _ticketPesadaNombreArchivo = _recepcionExistente.TicketPesadaNombreArchivo;
        ActualizarEstadoTicketPesada();
        RecalcularPesoNeto();
        RecalcularDiferenciaCajas();

        try
        {
            var detalle = await _recepcionFrutaService.ObtenerDetalleAsync(_recepcionExistente.Id);
            foreach (var linea in detalle)
            {
                _filas.Add(new FilaDetalleRecepcion
                {
                    DetalleId = linea.Id,
                    OrdenCorteId = linea.OrdenCorteId,
                    OrdenCorteFolio = linea.OrdenCorteFolio,
                    HuertaNombre = linea.HuertaNombre,
                    CajasCortadas = linea.CajasCortadas,
                    Kilogramos = linea.Kilogramos,
                });
            }
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        if (_recepcionExistente.EstaEnLote)
        {
            AplicarBloqueoPorLote();
        }
    }

    // Una vez que la Recepción entra a un Lote, se bloquea la edición por completo (regla dura
    // del módulo Lotes — ver RecepcionFrutaService.ActualizarAsync). El servidor ya lo rechaza,
    // pero aquí se deshabilitan los campos para que el usuario ni siquiera pueda intentarlo. Se
    // usa Enabled = false (no Properties.ReadOnly) para que además se vea gris/deshabilitado —
    // ReadOnly no cambia la apariencia del control.
    private void AplicarBloqueoPorLote()
    {
        _dtFecha.Enabled = false;
        _txtChofer.Enabled = false;
        _txtPlacas.Enabled = false;
        _txtObservaciones.Enabled = false;
        _txtNumeroTicket.Enabled = false;
        _txtCoprefBico.Enabled = false;
        _spnPesoBruto.Enabled = false;
        _spnPesoTara.Enabled = false;
        _spnTaraCajas.Enabled = false;
        _spnPesoMuestra.Enabled = false;
        _spnPesoProductor.Enabled = false;
        _spnPorcentajeMateriaSeca.Enabled = false;
        _spnCajasEntregadas.Enabled = false;
        _spnCajasCortadas.Enabled = false;
        _spnCajasRecibidasVacias.Enabled = false;
        _chkCamionDestarado.Enabled = false;

        _btnTicketPesada.Enabled = false;
        _btnQuitarTicketPesada.Enabled = false;
        _btnDetalleNuevo.Enabled = false;
        _btnDetalleBorrar.Enabled = false;
        _btnGuardar.Enabled = false;
    }

    private void ConfigurarColumnasDetalle()
    {
        foreach (var nombre in new[] { "DetalleId", "OrdenCorteId" })
        {
            if (_gridViewDetalle.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridViewDetalle.Columns["OrdenCorteFolio"] is { } colFolio)
        {
            colFolio.Caption = "Orden de Corte";
        }

        if (_gridViewDetalle.Columns["HuertaNombre"] is { } colHuerta)
        {
            colHuerta.Caption = "Huerta";
        }

        if (_gridViewDetalle.Columns["CajasCortadas"] is { } colCajas)
        {
            colCajas.Caption = "Cajas Cortadas";
        }

        if (_gridViewDetalle.Columns["Kilogramos"] is { } colKilos)
        {
            colKilos.DisplayFormat.FormatType = FormatType.Numeric;
            colKilos.DisplayFormat.FormatString = "n2";
        }

        _gridViewDetalle.BestFitColumns();
    }

    private void CamposBascula_EditValueChanged(object? sender, EventArgs e) => RecalcularPesoNeto();

    private void RecalcularPesoNeto()
    {
        var pesoNeto = (decimal)_spnPesoBruto.EditValue - (decimal)_spnPesoTara.EditValue
            - (decimal)_spnTaraCajas.EditValue - (decimal)_spnPesoMuestra.EditValue;
        _spnPesoNeto.EditValue = pesoNeto;

        // Kilogramos de la única línea de detalle = Peso Neto del encabezado (pedido explícito
        // del usuario) — se mantiene sincronizado cada vez que cambia algún peso de báscula.
        foreach (var fila in _filas)
        {
            fila.Kilogramos = pesoNeto;
            _filas.ResetItem(_filas.IndexOf(fila));
        }
    }

    private void CamposCajas_EditValueChanged(object? sender, EventArgs e) => RecalcularDiferenciaCajas();

    // Diferencia: ¿salió del almacén lo que la Orden de Corte comprometía? (Por Entregar viene
    // de la Orden de Corte, Entregadas es lo que realmente salió con la cuadrilla).
    // Perdidas: de lo que salió con la cuadrilla, ¿cuánto no volvió en ninguna forma (ni con
    // fruta ni vacía)? Dispara el ajuste de inventario del módulo Almacenes al guardar.
    private void RecalcularDiferenciaCajas()
    {
        var diferencia = (decimal)_spnCajasPorEntregar.EditValue - (decimal)_spnCajasEntregadas.EditValue;
        _spnCajasDiferencia.EditValue = diferencia;

        var perdidas = (decimal)_spnCajasEntregadas.EditValue
            - (decimal)_spnCajasCortadas.EditValue - (decimal)_spnCajasRecibidasVacias.EditValue;
        _spnCajasPerdidas.EditValue = perdidas;
    }

    // Solo se permite una Orden de Corte por Recepción (pedido explícito del usuario) — "Por
    // Entregar" del encabezado se toma directo de esa orden (ya no se captura a mano; "Entregadas"
    // sí se sigue capturando manual, es lo que realmente llegó) y Kilogramos de la línea se
    // sincroniza con el Peso Neto ya calculado.
    private void BtnDetalleNuevo_Click(object? sender, EventArgs e)
    {
        if (_filas.Count > 0)
        {
            XtraMessageBox.Show(this, "Solo se permite una Orden de Corte por Recepción. Quita la actual antes de agregar otra.",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new RecepcionFrutaDetalleEditarForm(_recepcionFrutaService);
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _filas.Add(new FilaDetalleRecepcion
        {
            OrdenCorteId = form.OrdenCorteId,
            OrdenCorteFolio = form.OrdenCorteFolio,
            HuertaNombre = form.HuertaNombre,
            CajasCortadas = form.CajasCortadas,
            Kilogramos = (decimal)_spnPesoNeto.EditValue,
        });

        _spnCajasPorEntregar.EditValue = (decimal)form.CajasCortadas;
    }

    private void BtnDetalleBorrar_Click(object? sender, EventArgs e)
    {
        var fila = ObtenerFilaSeleccionada();
        if (fila is null)
        {
            XtraMessageBox.Show(this, "Selecciona un renglón del detalle.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Quitar la orden de corte '{fila.OrdenCorteFolio}' de esta Recepción?", "FrontOne",
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
        _spnCajasPorEntregar.EditValue = 0m;
    }

    private FilaDetalleRecepcion? ObtenerFilaSeleccionada()
        => _gridViewDetalle.GetFocusedRow() as FilaDetalleRecepcion;

    // El ticket se guarda en memoria (VARBINARY(MAX) en Recepcion.RecepcionFruta) y se persiste
    // junto con el resto del encabezado al hacer clic en Guardar — mismo criterio que el detalle.
    private void BtnTicketPesada_Click(object? sender, EventArgs e)
    {
        using var dialogo = new OpenFileDialog
        {
            Title = "Seleccionar ticket de pesada",
            Filter = "Imágenes y PDF (*.jpg;*.jpeg;*.png;*.pdf)|*.jpg;*.jpeg;*.png;*.pdf|Todos los archivos (*.*)|*.*",
        };

        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            _ticketPesadaArchivo = File.ReadAllBytes(dialogo.FileName);
            _ticketPesadaNombreArchivo = Path.GetFileName(dialogo.FileName);
            ActualizarEstadoTicketPesada();
        }
        catch (IOException ex)
        {
            XtraMessageBox.Show(this, $"No se pudo leer el archivo: {ex.Message}", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnQuitarTicketPesada_Click(object? sender, EventArgs e)
    {
        _ticketPesadaArchivo = null;
        _ticketPesadaNombreArchivo = null;
        ActualizarEstadoTicketPesada();
    }

    private void LblTicketPesadaArchivo_Click(object? sender, EventArgs e)
    {
        if (_ticketPesadaArchivo is null || _ticketPesadaNombreArchivo is null)
        {
            return;
        }

        try
        {
            var rutaTemporal = Path.Combine(Path.GetTempPath(), _ticketPesadaNombreArchivo);
            File.WriteAllBytes(rutaTemporal, _ticketPesadaArchivo);
            Process.Start(new ProcessStartInfo(rutaTemporal) { UseShellExecute = true });
        }
        catch (Exception ex) when (ex is IOException or System.ComponentModel.Win32Exception)
        {
            XtraMessageBox.Show(this, $"No se pudo abrir el archivo: {ex.Message}", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ActualizarEstadoTicketPesada()
    {
        if (_ticketPesadaArchivo is null)
        {
            _lblTicketPesadaArchivo.Text = "Sin ticket adjunto";
            _lblTicketPesadaArchivo.Cursor = Cursors.Default;
            _btnTicketPesada.Text = "Agregar Ticket de Pesada";
            _btnQuitarTicketPesada.Visible = false;
            return;
        }

        _lblTicketPesadaArchivo.Text = _ticketPesadaNombreArchivo;
        _lblTicketPesadaArchivo.Cursor = Cursors.Hand;
        _btnTicketPesada.Text = "Reemplazar Ticket de Pesada";
        _btnQuitarTicketPesada.Visible = true;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var dto = new RecepcionFrutaDto(
            _recepcionExistente?.Id ?? 0,
            _recepcionExistente?.Folio ?? string.Empty,
            _txtNoLote.Text,
            (DateTime)_dtFecha.EditValue,
            _txtChofer.Text,
            _txtPlacas.Text,
            _txtObservaciones.Text,
            _txtNumeroTicket.Text,
            _txtCoprefBico.Text,
            (decimal)_spnPesoBruto.EditValue,
            (decimal)_spnPesoTara.EditValue,
            (decimal)_spnTaraCajas.EditValue,
            (decimal)_spnPesoMuestra.EditValue,
            (decimal)_spnPesoNeto.EditValue,
            (decimal)_spnPesoProductor.EditValue,
            (decimal)_spnPorcentajeMateriaSeca.EditValue,
            (short)(decimal)_spnCajasPorEntregar.EditValue,
            (short)(decimal)_spnCajasEntregadas.EditValue,
            (short)(decimal)_spnCajasCortadas.EditValue,
            (short)(decimal)_spnCajasRecibidasVacias.EditValue,
            (short)(decimal)_spnCajasDiferencia.EditValue,
            (short)(decimal)_spnCajasPerdidas.EditValue,
            _chkCamionDestarado.Checked,
            _ticketPesadaArchivo,
            _ticketPesadaNombreArchivo,
            null,
            _recepcionExistente?.EstaEnLote ?? false,
            null,
            null);

        try
        {
            if (_recepcionExistente is null)
            {
                var resultado = await _recepcionFrutaService.CrearAsync(dto);
                foreach (var fila in _filas)
                {
                    await _recepcionFrutaService.AgregarLineaAsync(resultado.Id, fila.OrdenCorteId, fila.Kilogramos);
                }
            }
            else
            {
                await _recepcionFrutaService.ActualizarAsync(dto);

                foreach (var idEliminado in _idsEliminados)
                {
                    await _recepcionFrutaService.EliminarLineaAsync(idEliminado, _recepcionExistente.Id);
                }

                foreach (var fila in _filas)
                {
                    if (fila.DetalleId.HasValue)
                    {
                        await _recepcionFrutaService.ActualizarLineaAsync(fila.DetalleId.Value, fila.Kilogramos);
                    }
                    else
                    {
                        await _recepcionFrutaService.AgregarLineaAsync(_recepcionExistente.Id, fila.OrdenCorteId, fila.Kilogramos);
                    }
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
