using DevExpress.XtraReports.UI;
using FrontOne.WinForms.Reports.Controles;
using TECIT.TBarCode;

namespace FrontOne.WinForms.Reports;

// Reporte piloto para probar el control XRBarcodeControl (TECIT TBarCode) desde el Diseñador —
// sin datos de negocio todavía, solo un valor fijo para validar que el control genera y se
// puede reposicionar. Base para las futuras pantallas de etiquetas de caja/pallet.
partial class ReportePruebaCodigoBarras
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private TopMarginBand _topMarginBand;
    private DetailBand _detailBand;
    private BottomMarginBand _bottomMarginBand;

    private XRLabel _lblTitulo;
    private XRBarcodeControl _barcode;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand();
        _detailBand = new DetailBand();
        _bottomMarginBand = new BottomMarginBand();
        _lblTitulo = new XRLabel();
        _barcode = new XRBarcodeControl();

        //
        // _topMarginBand
        //
        _topMarginBand.HeightF = 39F;
        _topMarginBand.Name = "_topMarginBand";
        //
        // _lblTitulo
        //
        _lblTitulo.LocationFloat = new DevExpress.Utils.PointFloat(0, 0);
        _lblTitulo.Name = "_lblTitulo";
        _lblTitulo.SizeF = new System.Drawing.SizeF(400, 30);
        _lblTitulo.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
        _lblTitulo.Text = "Prueba de Código de Barras";
        //
        // _barcode
        //
        _barcode.LocationFloat = new DevExpress.Utils.PointFloat(0, 40);
        _barcode.Name = "_barcode";
        _barcode.SizeF = new System.Drawing.SizeF(300, 100);
        _barcode.CampoDato = "123456789012";
        _barcode.Simbologia = BarcodeType.GS1_128;
        //
        // _detailBand
        //
        _detailBand.HeightF = 150F;
        _detailBand.Name = "_detailBand";
        _detailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { _lblTitulo, _barcode });
        //
        // _bottomMarginBand
        //
        _bottomMarginBand.HeightF = 39F;
        _bottomMarginBand.Name = "_bottomMarginBand";
        //
        // ReportePruebaCodigoBarras
        //
        Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { _topMarginBand, _detailBand, _bottomMarginBand });
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
        PageWidth = 850;
        PageHeight = 1100;
        Version = "26.1";
    }
}
