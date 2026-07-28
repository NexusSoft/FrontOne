using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;

namespace FrontOne.WinForms.Forms.Recepcion;

// Diálogo puro de captura (no toca la base de datos) — solo elige la Orden de Corte. Cajas
// Entregadas y Kilogramos ya no se capturan aquí: el padre (RecepcionFrutaEditarForm) los toma
// automáticamente de la Orden de Corte elegida y del Peso Neto del encabezado, respectivamente.
public partial class RecepcionFrutaDetalleEditarForm : XtraForm
{
    private readonly RecepcionFrutaService _recepcionFrutaService = null!;

    public int OrdenCorteId { get; private set; }
    public string OrdenCorteFolio { get; private set; } = string.Empty;
    public string HuertaNombre { get; private set; } = string.Empty;
    public short CajasCortadas { get; private set; }

    public RecepcionFrutaDetalleEditarForm()
    {
        InitializeComponent();
    }

    public RecepcionFrutaDetalleEditarForm(RecepcionFrutaService recepcionFrutaService)
        : this()
    {
        _recepcionFrutaService = recepcionFrutaService;
    }

    private void CmbOrdenCorte_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        using var form = new SeleccionarOrdenCorteForm(_recepcionFrutaService);
        if (form.ShowDialog(this) != DialogResult.OK || form.OrdenSeleccionada is null)
        {
            return;
        }

        var seleccionada = form.OrdenSeleccionada;
        OrdenCorteId = seleccionada.Id;
        OrdenCorteFolio = seleccionada.Folio;
        HuertaNombre = seleccionada.HuertaNombre;
        CajasCortadas = seleccionada.CajasEntregadas;
        _cmbOrdenCorte.EditValue = $"{seleccionada.Folio} - {seleccionada.HuertaNombre}";
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (OrdenCorteId <= 0)
        {
            XtraMessageBox.Show(this, "Selecciona la orden de corte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
