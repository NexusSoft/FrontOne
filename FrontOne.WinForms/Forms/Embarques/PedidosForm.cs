using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Embarques;

public partial class PedidosForm : XtraForm
{
    private readonly PedidoService _pedidoService = null!;

    public PedidosForm()
    {
        InitializeComponent();
    }

    public PedidosForm(PedidoService pedidoService) : this()
    {
        _pedidoService = pedidoService;
    }

    private async void PedidosForm_Load(object? sender, EventArgs e) => await CargarAsync();

    private async Task CargarAsync()
    {
        // useFadeIn: false — evita la carrera de DevExpress donde CloseDefaultWaitForm truena
        // ("Splash Form is not displayed") si la operación termina antes de que el fade-in
        // asíncrono termine de registrar el splash como visible.
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: false, useFadeOut: true, "FrontOne", "Consultando SAP...");
        try
        {
            var pedidos = await _pedidoService.ObtenerTop500Async();
            _grid.DataSource = pedidos.ToList();
            Text = "FrontOne - Pedidos (500 más recientes)";
        }
        catch (SapException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e) => await CargarAsync();

    private void AbrirDetalle()
    {
        if (ObtenerSeleccionado() is not { } pedido)
        {
            return;
        }

        using var form = new PedidoDetalleForm(_pedidoService, pedido.DocEntry);
        form.ShowDialog(this);
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => AbrirDetalle();

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private SapPedidoDto? ObtenerSeleccionado() => _gridView.GetFocusedRow() as SapPedidoDto;
}
