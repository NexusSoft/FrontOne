using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Recepcion;

partial class RecepcionFrutaDetalleEditarForm
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

    private LabelControl _lblOrdenCorte;
    private ButtonEdit _cmbOrdenCorte;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecepcionFrutaDetalleEditarForm));
        _lblOrdenCorte = new LabelControl();
        _cmbOrdenCorte = new ButtonEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbOrdenCorte.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblOrdenCorte
        //
        _lblOrdenCorte.Location = new Point(15, 18);
        _lblOrdenCorte.Name = "_lblOrdenCorte";
        _lblOrdenCorte.Size = new Size(72, 13);
        _lblOrdenCorte.TabIndex = 0;
        _lblOrdenCorte.Text = "Orden de Corte:";
        //
        // _cmbOrdenCorte
        //
        _cmbOrdenCorte.Location = new Point(140, 15);
        _cmbOrdenCorte.Name = "_cmbOrdenCorte";
        _cmbOrdenCorte.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Search) });
        _cmbOrdenCorte.Properties.NullValuePrompt = "Buscar orden de corte...";
        _cmbOrdenCorte.Properties.ReadOnly = true;
        _cmbOrdenCorte.Size = new Size(300, 20);
        _cmbOrdenCorte.TabIndex = 1;
        _cmbOrdenCorte.ButtonClick += CmbOrdenCorte_ButtonClick;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(280, 45);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 2;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(370, 45);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 3;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // RecepcionFrutaDetalleEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(465, 81);
        Controls.Add(_lblOrdenCorte);
        Controls.Add(_cmbOrdenCorte);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "RecepcionFrutaDetalleEditarForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Línea de Orden de Corte";
        ((System.ComponentModel.ISupportInitialize)_cmbOrdenCorte.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
