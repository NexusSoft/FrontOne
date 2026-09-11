using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Embarques;

public partial class PedidoDetalleForm : XtraForm
{
    private readonly PedidoService _pedidoService = null!;
    private readonly int _docEntry;

    public PedidoDetalleForm()
    {
        InitializeComponent();
    }

    public PedidoDetalleForm(PedidoService pedidoService, int docEntry) : this()
    {
        _pedidoService = pedidoService;
        _docEntry = docEntry;
    }

    private async void PedidoDetalleForm_Load(object? sender, EventArgs e)
    {
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: false, useFadeOut: true, "FrontOne", "Consultando SAP...");
        SapPedidoDetalleDto? pedido;
        try
        {
            pedido = await _pedidoService.ObtenerDetalleAsync(_docEntry);
        }
        catch (SapException ex)
        {
            SplashScreenManager.CloseDefaultWaitForm();
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }
        SplashScreenManager.CloseDefaultWaitForm();

        if (pedido is null)
        {
            XtraMessageBox.Show(this, "El pedido ya no existe en SAP.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
            return;
        }

        MostrarEncabezado(pedido);
        _grid.DataSource = pedido.Lineas.ToList();
        Text = $"FrontOne - Pedido {pedido.DocNum}";
    }

    private void MostrarEncabezado(SapPedidoDetalleDto pedido)
    {
        _txtDocNum.Text = pedido.DocNum.ToString();
        _txtEstatus.Text = pedido.Estatus;
        _txtCardCode.Text = pedido.CardCode;
        _txtCardName.Text = pedido.CardName;
        _txtNumAtCard.Text = pedido.NumAtCard;
        _txtDocCurrency.Text = pedido.DocCurrency;
        _txtDocDate.Text = pedido.DocDate.ToString("dd/MM/yyyy");
        _txtDocDueDate.Text = pedido.DocDueDate.ToString("dd/MM/yyyy");
        _txtTaxDate.Text = pedido.TaxDate?.ToString("dd/MM/yyyy");
        _txtDocRate.Text = pedido.DocRate.ToString("N4");
        _txtDiscountPercent.Text = pedido.DiscountPercent.ToString("N2");
        _txtVatSum.Text = pedido.VatSum.ToString("N2");
        _txtDocTotal.Text = pedido.DocTotal.ToString("N2");
        _txtVendedor.Text = pedido.VendedorCodigo;
        _txtFolioFronterra.Text = pedido.FolioFronterra;
        _txtDireccion.Text = pedido.Direccion;
        _memComentarios.Text = pedido.Comentarios;
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
