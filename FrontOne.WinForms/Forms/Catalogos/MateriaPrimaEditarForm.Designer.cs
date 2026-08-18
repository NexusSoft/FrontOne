using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class MateriaPrimaEditarForm
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

    private LabelControl _lblCodigoSap;
    private TextEdit _txtCodigoSap;
    private LabelControl _lblDescripcionSap;
    private TextEdit _txtDescripcionSap;
    private LabelControl _lblCategoria;
    private LookUpEdit _cmbCategoria;
    private LabelControl _lblCalibreApeam;
    private LookUpEdit _cmbCalibreApeam;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MateriaPrimaEditarForm));
        _lblCodigoSap = new LabelControl();
        _txtCodigoSap = new TextEdit();
        _lblDescripcionSap = new LabelControl();
        _txtDescripcionSap = new TextEdit();
        _lblCategoria = new LabelControl();
        _cmbCategoria = new LookUpEdit();
        _lblCalibreApeam = new LabelControl();
        _cmbCalibreApeam = new LookUpEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoSap.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcionSap.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCategoria.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCalibreApeam.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblCodigoSap
        //
        _lblCodigoSap.Location = new Point(20, 20);
        _lblCodigoSap.Name = "_lblCodigoSap";
        _lblCodigoSap.Size = new Size(59, 13);
        _lblCodigoSap.TabIndex = 0;
        _lblCodigoSap.Text = "Código SAP:";
        //
        // _txtCodigoSap
        //
        _txtCodigoSap.Location = new Point(140, 18);
        _txtCodigoSap.Name = "_txtCodigoSap";
        _txtCodigoSap.Properties.ReadOnly = true;
        _txtCodigoSap.Size = new Size(150, 20);
        _txtCodigoSap.TabIndex = 1;
        //
        // _lblDescripcionSap
        //
        _lblDescripcionSap.Location = new Point(20, 52);
        _lblDescripcionSap.Name = "_lblDescripcionSap";
        _lblDescripcionSap.Size = new Size(61, 13);
        _lblDescripcionSap.TabIndex = 2;
        _lblDescripcionSap.Text = "Descripción:";
        //
        // _txtDescripcionSap
        //
        _txtDescripcionSap.Location = new Point(140, 50);
        _txtDescripcionSap.Name = "_txtDescripcionSap";
        _txtDescripcionSap.Properties.ReadOnly = true;
        _txtDescripcionSap.Size = new Size(320, 20);
        _txtDescripcionSap.TabIndex = 3;
        //
        // _lblCategoria
        //
        _lblCategoria.Location = new Point(20, 84);
        _lblCategoria.Name = "_lblCategoria";
        _lblCategoria.Size = new Size(50, 13);
        _lblCategoria.TabIndex = 4;
        _lblCategoria.Text = "Categoría:";
        //
        // _cmbCategoria
        //
        _cmbCategoria.Location = new Point(140, 82);
        _cmbCategoria.Name = "_cmbCategoria";
        _cmbCategoria.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbCategoria.Properties.NullText = "Seleccionar";
        _cmbCategoria.Properties.SearchMode = SearchMode.AutoFilter;
        _cmbCategoria.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbCategoria.Size = new Size(270, 20);
        _cmbCategoria.TabIndex = 5;
        _cmbCategoria.ButtonClick += CmbCategoria_ButtonClick;
        //
        // _lblCalibreApeam
        //
        _lblCalibreApeam.Location = new Point(20, 116);
        _lblCalibreApeam.Name = "_lblCalibreApeam";
        _lblCalibreApeam.Size = new Size(79, 13);
        _lblCalibreApeam.TabIndex = 6;
        _lblCalibreApeam.Text = "Calibre APEAM:";
        //
        // _cmbCalibreApeam
        //
        _cmbCalibreApeam.Location = new Point(140, 114);
        _cmbCalibreApeam.Name = "_cmbCalibreApeam";
        _cmbCalibreApeam.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbCalibreApeam.Properties.NullText = "Seleccionar";
        _cmbCalibreApeam.Properties.SearchMode = SearchMode.AutoFilter;
        _cmbCalibreApeam.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbCalibreApeam.Size = new Size(270, 20);
        _cmbCalibreApeam.TabIndex = 7;
        _cmbCalibreApeam.ButtonClick += CmbCalibreApeam_ButtonClick;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(250, 155);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 8;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(340, 155);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 9;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // MateriaPrimaEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(480, 195);
        Controls.Add(_lblCodigoSap);
        Controls.Add(_txtCodigoSap);
        Controls.Add(_lblDescripcionSap);
        Controls.Add(_txtDescripcionSap);
        Controls.Add(_lblCategoria);
        Controls.Add(_cmbCategoria);
        Controls.Add(_lblCalibreApeam);
        Controls.Add(_cmbCalibreApeam);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MateriaPrimaEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Editar materia prima";
        ((System.ComponentModel.ISupportInitialize)_txtCodigoSap.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcionSap.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCategoria.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCalibreApeam.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
