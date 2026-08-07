using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Sistema;

partial class ConfiguracionBasculaForm
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

    private GroupControl _grpPuerto;
    private LabelControl _lblPuerto;
    private ComboBoxEdit _cmbPuerto;
    private SimpleButton _btnActualizarPuertos;
    private LabelControl _lblBaudRate;
    private SpinEdit _spnBaudRate;
    private LabelControl _lblParity;
    private ComboBoxEdit _cmbParity;
    private LabelControl _lblDataBits;
    private SpinEdit _spnDataBits;
    private LabelControl _lblStopBits;
    private ComboBoxEdit _cmbStopBits;
    private LabelControl _lblPatronLectura;
    private TextEdit _txtPatronLectura;
    private LabelControl _lblAyudaPatron;
    private SimpleButton _btnProbarLectura;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfiguracionBasculaForm));
        _grpPuerto = new GroupControl();
        _lblPuerto = new LabelControl();
        _cmbPuerto = new ComboBoxEdit();
        _btnActualizarPuertos = new SimpleButton();
        _lblBaudRate = new LabelControl();
        _spnBaudRate = new SpinEdit();
        _lblParity = new LabelControl();
        _cmbParity = new ComboBoxEdit();
        _lblDataBits = new LabelControl();
        _spnDataBits = new SpinEdit();
        _lblStopBits = new LabelControl();
        _cmbStopBits = new ComboBoxEdit();
        _lblPatronLectura = new LabelControl();
        _txtPatronLectura = new TextEdit();
        _lblAyudaPatron = new LabelControl();
        _btnProbarLectura = new SimpleButton();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grpPuerto).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPuerto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnBaudRate.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbParity.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnDataBits.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbStopBits.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPatronLectura.Properties).BeginInit();
        _grpPuerto.SuspendLayout();
        SuspendLayout();
        //
        // _grpPuerto
        //
        _grpPuerto.Controls.Add(_lblPuerto);
        _grpPuerto.Controls.Add(_cmbPuerto);
        _grpPuerto.Controls.Add(_btnActualizarPuertos);
        _grpPuerto.Controls.Add(_lblBaudRate);
        _grpPuerto.Controls.Add(_spnBaudRate);
        _grpPuerto.Controls.Add(_lblParity);
        _grpPuerto.Controls.Add(_cmbParity);
        _grpPuerto.Controls.Add(_lblDataBits);
        _grpPuerto.Controls.Add(_spnDataBits);
        _grpPuerto.Controls.Add(_lblStopBits);
        _grpPuerto.Controls.Add(_cmbStopBits);
        _grpPuerto.Location = new Point(12, 12);
        _grpPuerto.Name = "_grpPuerto";
        _grpPuerto.Size = new Size(450, 130);
        _grpPuerto.TabIndex = 0;
        _grpPuerto.Text = "Puerto serie";
        //
        // _lblPuerto
        //
        _lblPuerto.Location = new Point(15, 32);
        _lblPuerto.Name = "_lblPuerto";
        _lblPuerto.Size = new Size(38, 13);
        _lblPuerto.Text = "Puerto:";
        //
        // _cmbPuerto
        //
        _cmbPuerto.Location = new Point(100, 29);
        _cmbPuerto.Name = "_cmbPuerto";
        _cmbPuerto.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbPuerto.Size = new Size(110, 20);
        _cmbPuerto.TabIndex = 0;
        //
        // _btnActualizarPuertos
        //
        _btnActualizarPuertos.Location = new Point(216, 28);
        _btnActualizarPuertos.Name = "_btnActualizarPuertos";
        _btnActualizarPuertos.Size = new Size(90, 22);
        _btnActualizarPuertos.TabIndex = 1;
        _btnActualizarPuertos.Text = "Actualizar";
        _btnActualizarPuertos.Click += BtnActualizarPuertos_Click;
        //
        // _lblBaudRate
        //
        _lblBaudRate.Location = new Point(15, 62);
        _lblBaudRate.Name = "_lblBaudRate";
        _lblBaudRate.Size = new Size(53, 13);
        _lblBaudRate.Text = "Baud rate:";
        //
        // _spnBaudRate
        //
        _spnBaudRate.EditValue = new decimal(new int[] { 9600, 0, 0, 0 });
        _spnBaudRate.Location = new Point(100, 59);
        _spnBaudRate.Name = "_spnBaudRate";
        _spnBaudRate.Properties.Mask.EditMask = "N00";
        _spnBaudRate.Properties.MaxValue = new decimal(new int[] { 921600, 0, 0, 0 });
        _spnBaudRate.Properties.MinValue = new decimal(new int[] { 110, 0, 0, 0 });
        _spnBaudRate.Size = new Size(110, 20);
        _spnBaudRate.TabIndex = 2;
        //
        // _lblParity
        //
        _lblParity.Location = new Point(230, 62);
        _lblParity.Name = "_lblParity";
        _lblParity.Size = new Size(44, 13);
        _lblParity.Text = "Paridad:";
        //
        // _cmbParity
        //
        _cmbParity.Location = new Point(310, 59);
        _cmbParity.Name = "_cmbParity";
        _cmbParity.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbParity.Size = new Size(120, 20);
        _cmbParity.TabIndex = 3;
        //
        // _lblDataBits
        //
        _lblDataBits.Location = new Point(15, 92);
        _lblDataBits.Name = "_lblDataBits";
        _lblDataBits.Size = new Size(71, 13);
        _lblDataBits.Text = "Bits de datos:";
        //
        // _spnDataBits
        //
        _spnDataBits.EditValue = new decimal(new int[] { 8, 0, 0, 0 });
        _spnDataBits.Location = new Point(100, 89);
        _spnDataBits.Name = "_spnDataBits";
        _spnDataBits.Properties.Mask.EditMask = "N00";
        _spnDataBits.Properties.MaxValue = new decimal(new int[] { 8, 0, 0, 0 });
        _spnDataBits.Properties.MinValue = new decimal(new int[] { 5, 0, 0, 0 });
        _spnDataBits.Size = new Size(110, 20);
        _spnDataBits.TabIndex = 4;
        //
        // _lblStopBits
        //
        _lblStopBits.Location = new Point(230, 92);
        _lblStopBits.Name = "_lblStopBits";
        _lblStopBits.Size = new Size(70, 13);
        _lblStopBits.Text = "Bits de parada:";
        //
        // _cmbStopBits
        //
        _cmbStopBits.Location = new Point(310, 89);
        _cmbStopBits.Name = "_cmbStopBits";
        _cmbStopBits.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbStopBits.Size = new Size(120, 20);
        _cmbStopBits.TabIndex = 5;
        //
        // _lblPatronLectura
        //
        _lblPatronLectura.Location = new Point(12, 158);
        _lblPatronLectura.Name = "_lblPatronLectura";
        _lblPatronLectura.Size = new Size(97, 13);
        _lblPatronLectura.Text = "Patrón de lectura:";
        //
        // _txtPatronLectura
        //
        _txtPatronLectura.Location = new Point(115, 155);
        _txtPatronLectura.Name = "_txtPatronLectura";
        _txtPatronLectura.Size = new Size(347, 20);
        _txtPatronLectura.TabIndex = 1;
        //
        // _lblAyudaPatron
        //
        _lblAyudaPatron.Location = new Point(115, 180);
        _lblAyudaPatron.Name = "_lblAyudaPatron";
        _lblAyudaPatron.Size = new Size(347, 26);
        _lblAyudaPatron.Text = "Expresión regular con un grupo que aísle el número, por ejemplo:  ST,GS,\\s*([0-9.]+)\r\nSi se deja vacío, se toma el primer número que venga en la trama.";
        //
        // _btnProbarLectura
        //
        _btnProbarLectura.Location = new Point(12, 218);
        _btnProbarLectura.Name = "_btnProbarLectura";
        _btnProbarLectura.Size = new Size(110, 23);
        _btnProbarLectura.TabIndex = 2;
        _btnProbarLectura.Text = "Probar lectura";
        _btnProbarLectura.Click += BtnProbarLectura_Click;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(292, 218);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 3;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(382, 218);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 4;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // ConfiguracionBasculaForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(474, 253);
        Controls.Add(_grpPuerto);
        Controls.Add(_lblPatronLectura);
        Controls.Add(_txtPatronLectura);
        Controls.Add(_lblAyudaPatron);
        Controls.Add(_btnProbarLectura);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ConfiguracionBasculaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Configuración de báscula";
        ((System.ComponentModel.ISupportInitialize)_cmbPuerto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnBaudRate.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbParity.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnDataBits.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbStopBits.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPatronLectura.Properties).EndInit();
        _grpPuerto.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_grpPuerto).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
