using DevExpress.Utils;
using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Embarques;

// Listado de Contenedores. Mismo molde que ReempaquesForm.
public partial class ContenedoresForm : XtraForm
{
    private readonly ContenedorService _contenedorService = null!;
    private readonly PalletService _palletService = null!;

    private ContenedorEditarForm? _contenedorEditarForm;
    private List<ContenedorDto> _contenedores = new();

    public ContenedoresForm()
    {
        InitializeComponent();
    }

    public ContenedoresForm(ContenedorService contenedorService, PalletService palletService) : this()
    {
        _contenedorService = contenedorService;
        _palletService = palletService;

        Shown += async (_, _) => await CargarDatosAsync();
        _grid.SizeChanged += (_, _) => { if (_gridView.Columns.Count > 0) _gridView.BestFitColumns(); };
    }

    private async Task CargarDatosAsync()
    {
        _contenedores = (await _contenedorService.ObtenerAsync()).ToList();
        _grid.DataSource = _contenedores;
        ConfigurarColumnas();
    }

    private void ConfigurarColumnas()
    {
        foreach (var nombre in new[] { "Id", "SapDocEntry", "FechaCreacionRegistro" })
        {
            if (_gridView.Columns[nombre] is { } columna)
            {
                columna.Visible = false;
            }
        }

        if (_gridView.Columns["Folio"] is { } colFolio)
        {
            colFolio.Caption = "Folio";
        }

        if (_gridView.Columns["Fecha"] is { } colFecha)
        {
            colFecha.DisplayFormat.FormatType = FormatType.DateTime;
            colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        if (_gridView.Columns["SapDocNum"] is { } colDocNum)
        {
            colDocNum.Caption = "Pedido SAP";
        }

        if (_gridView.Columns["FolioFronterra"] is { } colFolioFronterra)
        {
            colFolioFronterra.Caption = "Folio Fronterra";
        }

        if (_gridView.Columns["CardCode"] is { } colCliente)
        {
            colCliente.Caption = "Código Cliente";
        }

        if (_gridView.Columns["CardName"] is { } colNombreCliente)
        {
            colNombreCliente.Caption = "Cliente";
        }

        if (_gridView.Columns["TotalPallets"] is { } colPallets)
        {
            colPallets.Caption = "No. Pallets";
        }

        var orden = new[] { "Folio", "Fecha", "SapDocNum", "FolioFronterra", "CardCode", "CardName", "TotalPallets", "Observaciones" };
        for (var i = 0; i < orden.Length; i++)
        {
            if (_gridView.Columns[orden[i]] is { } col)
            {
                col.VisibleIndex = i;
            }
        }

        _gridView.BestFitColumns();
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => BtnEditar_Click(sender, EventArgs.Empty);

    private void BtnNuevo_Click(object? sender, EventArgs e) => AbrirEditarForm(null);

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un contenedor.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AbrirEditarForm(seleccionado);
    }

    private void AbrirEditarForm(ContenedorDto? existente)
    {
        if (_contenedorEditarForm is { IsDisposed: false })
        {
            if (_contenedorEditarForm.WindowState == FormWindowState.Minimized)
            {
                _contenedorEditarForm.WindowState = FormWindowState.Normal;
            }

            _contenedorEditarForm.Activate();
            return;
        }

        _contenedorEditarForm = new ContenedorEditarForm(_contenedorService, _palletService, existente);
        _contenedorEditarForm.Guardado += async (_, _) => await CargarDatosAsync();
        _contenedorEditarForm.FormClosed += (_, _) => _contenedorEditarForm = null;
        _contenedorEditarForm.Show(this);
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona un contenedor.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this,
            $"¿Eliminar el contenedor '{seleccionado.Folio}'?", "FrontOne", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _contenedorService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private ContenedorDto? ObtenerSeleccionado() => _gridView.GetFocusedRow() as ContenedorDto;
}
