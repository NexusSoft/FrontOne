using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class OrdenesCorteForm : XtraForm
{
    private readonly OrdenCorteService _ordenCorteService = null!;
    private readonly HuertaService _huertaService = null!;
    private readonly FloracionService _floracionService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly ListaPrecioAcarreoService _listaPrecioAcarreoService = null!;
    private readonly ZonaService _zonaService = null!;
    private readonly ListaPrecioCorteService _listaPrecioCorteService = null!;
    private readonly JefeAcopioService _jefeAcopioService = null!;
    private readonly TipoCorteService _tipoCorteService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PoblacionService _poblacionService = null!;

    public OrdenesCorteForm()
    {
        InitializeComponent();
    }

    public OrdenesCorteForm(
        OrdenCorteService ordenCorteService,
        HuertaService huertaService,
        FloracionService floracionService,
        VariedadService variedadService,
        ListaPrecioAcarreoService listaPrecioAcarreoService,
        ZonaService zonaService,
        ListaPrecioCorteService listaPrecioCorteService,
        JefeAcopioService jefeAcopioService,
        TipoCorteService tipoCorteService,
        PaisService paisService,
        EstadoService estadoService,
        MunicipioService municipioService,
        PoblacionService poblacionService)
        : this()
    {
        _ordenCorteService = ordenCorteService;
        _huertaService = huertaService;
        _floracionService = floracionService;
        _variedadService = variedadService;
        _listaPrecioAcarreoService = listaPrecioAcarreoService;
        _zonaService = zonaService;
        _listaPrecioCorteService = listaPrecioCorteService;
        _jefeAcopioService = jefeAcopioService;
        _tipoCorteService = tipoCorteService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;
        _poblacionService = poblacionService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var ordenes = await _ordenCorteService.ObtenerAsync();
        _grid.DataSource = ordenes.ToList();
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[]
        {
            "Id", "AcuerdoCorteId", "ProductorId", "HuertaId", "FloracionId", "VariedadId",
            "PagarCorteACardCode", "TransportistaCardCode", "JefeCuadrillaCardCode", "JefeAcopioId",
            "AcuerdoCorteFolio", "PrecioAcarreo", "NoCandado", "CostoKg", "PagoDia", "CuadrillaApoyo",
            "PuntoReunion", "Observaciones",
        })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["ProductorNombre"] is { } colProductor)
        {
            colProductor.Caption = "Productor";
        }

        if (_gridView.Columns["HuertaNombre"] is { } colHuerta)
        {
            colHuerta.Caption = "Huerta";
        }

        if (_gridView.Columns["FloracionNombre"] is { } colFloracion)
        {
            colFloracion.Caption = "Floración";
        }

        if (_gridView.Columns["RegistroSagarpa"] is { } colRegistro)
        {
            colRegistro.Caption = "Registro";
        }

        if (_gridView.Columns["VariedadNombre"] is { } colVariedad)
        {
            colVariedad.Caption = "Variedad";
        }

        if (_gridView.Columns["TipoPagoNombre"] is { } colTipoPago)
        {
            colTipoPago.Caption = "Tipo de Pago";
        }

        if (_gridView.Columns["TipoCorteNombre"] is { } colTipoCorte)
        {
            colTipoCorte.Caption = "Tipo de Corte";
        }

        if (_gridView.Columns["PagarCorteANombre"] is { } colPagarCorteA)
        {
            colPagarCorteA.Caption = "Pagar el Corte a";
        }

        if (_gridView.Columns["TransportistaNombre"] is { } colTransportista)
        {
            colTransportista.Caption = "Transportista";
        }

        if (_gridView.Columns["CajasEntregadas"] is { } colCajas)
        {
            colCajas.Caption = "Cajas";
        }

        if (_gridView.Columns["JefeCuadrillaNombre"] is { } colJefeCuadrilla)
        {
            colJefeCuadrilla.Caption = "Jefe de Cuadrilla";
        }

        if (_gridView.Columns["KgMinimo"] is { } colKgMinimo)
        {
            colKgMinimo.Caption = "Kg Mínimo";
            colKgMinimo.DisplayFormat.FormatType = FormatType.Numeric;
            colKgMinimo.DisplayFormat.FormatString = "n2";
        }

        if (_gridView.Columns["JefeAcopioNombre"] is { } colJefeAcopio)
        {
            colJefeAcopio.Caption = "Jefe de Acopio";
        }

        if (_gridView.Columns["Cancelado"] is { } colCancelado)
        {
            colCancelado.Caption = "Cancelado";
        }

        _gridView.BestFitColumns();
    }

    private async void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var form = new OrdenCorteEditarForm(
            _ordenCorteService, _huertaService, _floracionService, _variedadService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _tipoCorteService,
            _paisService, _estadoService, _municipioService, _poblacionService, null);
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
            XtraMessageBox.Show(this, "Selecciona una orden de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new OrdenCorteEditarForm(
            _ordenCorteService, _huertaService, _floracionService, _variedadService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _tipoCorteService,
            _paisService, _estadoService, _municipioService, _poblacionService, seleccionado);
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
            XtraMessageBox.Show(this, "Selecciona una orden de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar la orden de corte folio '{seleccionado.Folio}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _ordenCorteService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private OrdenCorteDto? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as OrdenCorteDto;
}
