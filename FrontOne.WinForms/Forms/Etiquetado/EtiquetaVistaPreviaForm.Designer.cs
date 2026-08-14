using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Etiquetado;

partial class EtiquetaVistaPreviaForm
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

    #region Windows Form Designer generated code

    private LabelControl _lblFolio;
    private TextEdit _txtFolio;
    private SimpleButton _btnVer;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _btnVer = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblFolio
        //
        _lblFolio.Location = new Point(15, 18);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(58, 13);
        _lblFolio.Text = "No. Pallet:";
        //
        // _txtFolio
        //
        _txtFolio.Location = new Point(90, 15);
        _txtFolio.Name = "_txtFolio";
        _txtFolio.Size = new Size(150, 20);
        //
        // _btnVer
        //
        _btnVer.Location = new Point(90, 50);
        _btnVer.Name = "_btnVer";
        _btnVer.Size = new Size(150, 23);
        _btnVer.Text = "Ver vista previa";
        _btnVer.Click += BtnVer_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Location = new Point(90, 80);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(150, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // EtiquetaVistaPreviaForm
        //
        AcceptButton = _btnVer;
        ClientSize = new Size(280, 120);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_btnVer);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EtiquetaVistaPreviaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Vista previa de etiqueta";
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
