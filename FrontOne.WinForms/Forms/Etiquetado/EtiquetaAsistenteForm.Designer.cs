using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraWizard;

namespace FrontOne.WinForms.Forms.Etiquetado;

partial class EtiquetaAsistenteForm
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

    private WizardControl _wizard;
    private WizardPage _pageNombre;
    private WizardPage _pageTamano;
    private WizardPage _pageTipo;
    private LabelControl _lblNombre;
    private TextEdit _txtNombre;
    private LabelControl _lblAncho;
    private SpinEdit _spnAncho;
    private LabelControl _lblAlto;
    private SpinEdit _spnAlto;
    private LabelControl _lblMargen;
    private LabelControl _lblTipo;
    private RadioGroup _rdgTipo;

    private void InitializeComponent()
    {
        _wizard = new WizardControl();
        _pageNombre = new WizardPage();
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _pageTamano = new WizardPage();
        _lblAncho = new LabelControl();
        _spnAncho = new SpinEdit();
        _lblAlto = new LabelControl();
        _spnAlto = new SpinEdit();
        _lblMargen = new LabelControl();
        _pageTipo = new WizardPage();
        _lblTipo = new LabelControl();
        _rdgTipo = new RadioGroup();
        ((System.ComponentModel.ISupportInitialize)_wizard).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnAncho.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnAlto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_rdgTipo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(30, 30);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(120, 13);
        _lblNombre.Text = "Nombre de la etiqueta:";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(30, 50);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Size = new Size(300, 20);
        //
        // _pageNombre
        //
        _pageNombre.Controls.Add(_txtNombre);
        _pageNombre.Controls.Add(_lblNombre);
        _pageNombre.Name = "_pageNombre";
        _pageNombre.Text = "Nombre de la etiqueta";
        _pageNombre.DescriptionText = "Captura un nombre único que identifique esta plantilla de etiqueta.";
        _pageNombre.PageValidating += PageNombre_PageValidating;
        //
        // _lblAncho
        //
        _lblAncho.Location = new Point(30, 30);
        _lblAncho.Name = "_lblAncho";
        _lblAncho.Size = new Size(80, 13);
        _lblAncho.Text = "Ancho (pulgadas):";
        //
        // _spnAncho
        //
        _spnAncho.Location = new Point(30, 50);
        _spnAncho.Name = "_spnAncho";
        _spnAncho.Properties.Mask.EditMask = "n2";
        _spnAncho.Properties.MinValue = 0.5m;
        _spnAncho.Properties.MaxValue = 50m;
        _spnAncho.Properties.Increment = 0.1m;
        _spnAncho.Size = new Size(120, 20);
        //
        // _lblAlto
        //
        _lblAlto.Location = new Point(30, 90);
        _lblAlto.Name = "_lblAlto";
        _lblAlto.Size = new Size(68, 13);
        _lblAlto.Text = "Alto (pulgadas):";
        //
        // _spnAlto
        //
        _spnAlto.Location = new Point(30, 110);
        _spnAlto.Name = "_spnAlto";
        _spnAlto.Properties.Mask.EditMask = "n2";
        _spnAlto.Properties.MinValue = 0.5m;
        _spnAlto.Properties.MaxValue = 50m;
        _spnAlto.Properties.Increment = 0.1m;
        _spnAlto.Size = new Size(120, 20);
        //
        // _lblMargen
        //
        _lblMargen.Location = new Point(30, 150);
        _lblMargen.Name = "_lblMargen";
        _lblMargen.Size = new Size(310, 13);
        _lblMargen.Text = "El margen de impresión siempre es de 5 mm en los 4 lados (no editable).";
        //
        // _pageTamano
        //
        _pageTamano.Controls.Add(_lblMargen);
        _pageTamano.Controls.Add(_spnAlto);
        _pageTamano.Controls.Add(_lblAlto);
        _pageTamano.Controls.Add(_spnAncho);
        _pageTamano.Controls.Add(_lblAncho);
        _pageTamano.Name = "_pageTamano";
        _pageTamano.Text = "Tamaño de la etiqueta";
        _pageTamano.DescriptionText = "Define el ancho y el alto en pulgadas.";
        _pageTamano.PageValidating += PageTamano_PageValidating;
        //
        // _lblTipo
        //
        _lblTipo.Location = new Point(30, 30);
        _lblTipo.Name = "_lblTipo";
        _lblTipo.Size = new Size(69, 13);
        _lblTipo.Text = "Tipo de etiqueta:";
        //
        // _rdgTipo
        //
        _rdgTipo.Location = new Point(30, 50);
        _rdgTipo.Name = "_rdgTipo";
        _rdgTipo.Size = new Size(300, 90);
        //
        // _pageTipo
        //
        _pageTipo.Controls.Add(_rdgTipo);
        _pageTipo.Controls.Add(_lblTipo);
        _pageTipo.Name = "_pageTipo";
        _pageTipo.Text = "Tipo de etiqueta";
        _pageTipo.DescriptionText = "Selecciona el origen de datos que va a usar esta etiqueta.";
        _pageTipo.PageValidating += PageTipo_PageValidating;
        //
        // _wizard
        //
        _wizard.Dock = DockStyle.Fill;
        _wizard.Name = "_wizard";
        _wizard.Pages.AddRange(new BaseWizardPage[] { _pageNombre, _pageTamano, _pageTipo });
        _wizard.FinishClick += Wizard_FinishClick;
        _wizard.CancelClick += Wizard_CancelClick;
        //
        // EtiquetaAsistenteForm
        //
        ClientSize = new Size(520, 420);
        Controls.Add(_wizard);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EtiquetaAsistenteForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Nueva etiqueta";
        ((System.ComponentModel.ISupportInitialize)_wizard).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnAncho.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnAlto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_rdgTipo.Properties).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
