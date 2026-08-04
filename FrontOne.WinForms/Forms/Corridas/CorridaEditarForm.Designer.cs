using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Corridas;

partial class CorridaEditarForm
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

    private LabelControl _lblLote;
    private LookUpEdit _cmbLote;
    private TextEdit _txtLoteFolio;
    private LabelControl _lblCodigoTrazabilidad;
    private TextEdit _txtCodigoTrazabilidad;
    private LabelControl _lblHuerta;
    private TextEdit _txtHuerta;
    private LabelControl _lblRegistroSagarpa;
    private TextEdit _txtRegistroSagarpa;
    private LabelControl _lblProductor;
    private TextEdit _txtProductor;
    private LabelControl _lblBeneficiario;
    private TextEdit _txtBeneficiario;
    private LabelControl _lblKilogramos;
    private TextEdit _txtKilogramos;
    private LabelControl _lblKilosAProcesar;
    private TextEdit _txtKilosAProcesar;
    private LabelControl _lblKilosProcesados;
    private TextEdit _txtKilosProcesados;
    private LabelControl _lblFechaHoraInicio;
    private TextEdit _txtFechaHoraInicio;
    private LabelControl _lblFechaHoraFin;
    private TextEdit _txtFechaHoraFin;
    private SimpleButton _btnIniciarProceso;
    private SimpleButton _btnFinalizarCorrida;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CorridaEditarForm));
        _lblLote = new LabelControl();
        _cmbLote = new LookUpEdit();
        _txtLoteFolio = new TextEdit();
        _lblCodigoTrazabilidad = new LabelControl();
        _txtCodigoTrazabilidad = new TextEdit();
        _lblHuerta = new LabelControl();
        _txtHuerta = new TextEdit();
        _lblRegistroSagarpa = new LabelControl();
        _txtRegistroSagarpa = new TextEdit();
        _lblProductor = new LabelControl();
        _txtProductor = new TextEdit();
        _lblBeneficiario = new LabelControl();
        _txtBeneficiario = new TextEdit();
        _lblKilogramos = new LabelControl();
        _txtKilogramos = new TextEdit();
        _lblKilosAProcesar = new LabelControl();
        _txtKilosAProcesar = new TextEdit();
        _lblKilosProcesados = new LabelControl();
        _txtKilosProcesados = new TextEdit();
        _lblFechaHoraInicio = new LabelControl();
        _txtFechaHoraInicio = new TextEdit();
        _lblFechaHoraFin = new LabelControl();
        _txtFechaHoraFin = new TextEdit();
        _btnIniciarProceso = new SimpleButton();
        _btnFinalizarCorrida = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbLote.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtLoteFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoTrazabilidad.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtHuerta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtBeneficiario.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilogramos.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosAProcesar.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosProcesados.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFechaHoraInicio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFechaHoraFin.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblLote
        //
        _lblLote.Location = new Point(10, 14);
        _lblLote.Name = "_lblLote";
        _lblLote.Size = new Size(64, 13);
        _lblLote.Text = "No. de Lote:";
        //
        // _cmbLote
        //
        _cmbLote.Location = new Point(150, 11);
        _cmbLote.Name = "_cmbLote";
        _cmbLote.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbLote.Properties.NullText = "Seleccionar";
        _cmbLote.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbLote.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbLote.Size = new Size(260, 20);
        _cmbLote.TabIndex = 0;
        _cmbLote.EditValueChanged += CmbLote_EditValueChanged;
        //
        // _txtLoteFolio
        //
        _txtLoteFolio.Location = new Point(150, 11);
        _txtLoteFolio.Name = "_txtLoteFolio";
        _txtLoteFolio.Properties.ReadOnly = true;
        _txtLoteFolio.Size = new Size(260, 20);
        _txtLoteFolio.TabIndex = 1;
        //
        // _lblCodigoTrazabilidad
        //
        _lblCodigoTrazabilidad.Location = new Point(10, 40);
        _lblCodigoTrazabilidad.Name = "_lblCodigoTrazabilidad";
        _lblCodigoTrazabilidad.Size = new Size(115, 13);
        _lblCodigoTrazabilidad.Text = "Código de Trazabilidad:";
        //
        // _txtCodigoTrazabilidad
        //
        _txtCodigoTrazabilidad.Location = new Point(150, 37);
        _txtCodigoTrazabilidad.Name = "_txtCodigoTrazabilidad";
        _txtCodigoTrazabilidad.Properties.ReadOnly = true;
        _txtCodigoTrazabilidad.Size = new Size(260, 20);
        _txtCodigoTrazabilidad.TabIndex = 2;
        //
        // _lblHuerta
        //
        _lblHuerta.Location = new Point(10, 66);
        _lblHuerta.Name = "_lblHuerta";
        _lblHuerta.Size = new Size(35, 13);
        _lblHuerta.Text = "Huerta:";
        //
        // _txtHuerta
        //
        _txtHuerta.Location = new Point(150, 63);
        _txtHuerta.Name = "_txtHuerta";
        _txtHuerta.Properties.ReadOnly = true;
        _txtHuerta.Size = new Size(260, 20);
        _txtHuerta.TabIndex = 3;
        //
        // _lblRegistroSagarpa
        //
        _lblRegistroSagarpa.Location = new Point(10, 92);
        _lblRegistroSagarpa.Name = "_lblRegistroSagarpa";
        _lblRegistroSagarpa.Size = new Size(85, 13);
        _lblRegistroSagarpa.Text = "Registro Sagarpa:";
        //
        // _txtRegistroSagarpa
        //
        _txtRegistroSagarpa.Location = new Point(150, 89);
        _txtRegistroSagarpa.Name = "_txtRegistroSagarpa";
        _txtRegistroSagarpa.Properties.ReadOnly = true;
        _txtRegistroSagarpa.Size = new Size(260, 20);
        _txtRegistroSagarpa.TabIndex = 4;
        //
        // _lblProductor
        //
        _lblProductor.Location = new Point(10, 118);
        _lblProductor.Name = "_lblProductor";
        _lblProductor.Size = new Size(47, 13);
        _lblProductor.Text = "Productor:";
        //
        // _txtProductor
        //
        _txtProductor.Location = new Point(150, 115);
        _txtProductor.Name = "_txtProductor";
        _txtProductor.Properties.ReadOnly = true;
        _txtProductor.Size = new Size(260, 20);
        _txtProductor.TabIndex = 5;
        //
        // _lblBeneficiario
        //
        _lblBeneficiario.Location = new Point(10, 144);
        _lblBeneficiario.Name = "_lblBeneficiario";
        _lblBeneficiario.Size = new Size(60, 13);
        _lblBeneficiario.Text = "Beneficiario:";
        //
        // _txtBeneficiario
        //
        _txtBeneficiario.Location = new Point(150, 141);
        _txtBeneficiario.Name = "_txtBeneficiario";
        _txtBeneficiario.Properties.ReadOnly = true;
        _txtBeneficiario.Size = new Size(260, 20);
        _txtBeneficiario.TabIndex = 6;
        //
        // _lblKilogramos
        //
        _lblKilogramos.Location = new Point(10, 170);
        _lblKilogramos.Name = "_lblKilogramos";
        _lblKilogramos.Size = new Size(58, 13);
        _lblKilogramos.Text = "Kilogramos:";
        //
        // _txtKilogramos
        //
        _txtKilogramos.Location = new Point(150, 167);
        _txtKilogramos.Name = "_txtKilogramos";
        _txtKilogramos.Properties.Mask.EditMask = "n2";
        _txtKilogramos.Properties.ReadOnly = true;
        _txtKilogramos.Size = new Size(140, 20);
        _txtKilogramos.TabIndex = 7;
        //
        // _lblKilosAProcesar
        //
        _lblKilosAProcesar.Location = new Point(10, 196);
        _lblKilosAProcesar.Name = "_lblKilosAProcesar";
        _lblKilosAProcesar.Size = new Size(80, 13);
        _lblKilosAProcesar.Text = "Kilos a Procesar:";
        //
        // _txtKilosAProcesar
        //
        _txtKilosAProcesar.Location = new Point(150, 193);
        _txtKilosAProcesar.Name = "_txtKilosAProcesar";
        _txtKilosAProcesar.Properties.Mask.EditMask = "n2";
        _txtKilosAProcesar.Properties.ReadOnly = true;
        _txtKilosAProcesar.Size = new Size(140, 20);
        _txtKilosAProcesar.TabIndex = 8;
        //
        // _lblKilosProcesados
        //
        _lblKilosProcesados.Location = new Point(10, 222);
        _lblKilosProcesados.Name = "_lblKilosProcesados";
        _lblKilosProcesados.Size = new Size(80, 13);
        _lblKilosProcesados.Text = "Kilos Procesados:";
        //
        // _txtKilosProcesados
        //
        _txtKilosProcesados.Location = new Point(150, 219);
        _txtKilosProcesados.Name = "_txtKilosProcesados";
        _txtKilosProcesados.Properties.Mask.EditMask = "n2";
        _txtKilosProcesados.Properties.ReadOnly = true;
        _txtKilosProcesados.Size = new Size(140, 20);
        _txtKilosProcesados.TabIndex = 9;
        //
        // _lblFechaHoraInicio
        //
        _lblFechaHoraInicio.Location = new Point(10, 248);
        _lblFechaHoraInicio.Name = "_lblFechaHoraInicio";
        _lblFechaHoraInicio.Size = new Size(90, 13);
        _lblFechaHoraInicio.Text = "Fecha/Hora Inicio:";
        //
        // _txtFechaHoraInicio
        //
        _txtFechaHoraInicio.Location = new Point(150, 245);
        _txtFechaHoraInicio.Name = "_txtFechaHoraInicio";
        _txtFechaHoraInicio.Properties.ReadOnly = true;
        _txtFechaHoraInicio.Size = new Size(180, 20);
        _txtFechaHoraInicio.TabIndex = 10;
        //
        // _lblFechaHoraFin
        //
        _lblFechaHoraFin.Location = new Point(10, 274);
        _lblFechaHoraFin.Name = "_lblFechaHoraFin";
        _lblFechaHoraFin.Size = new Size(80, 13);
        _lblFechaHoraFin.Text = "Fecha/Hora Fin:";
        //
        // _txtFechaHoraFin
        //
        _txtFechaHoraFin.Location = new Point(150, 271);
        _txtFechaHoraFin.Name = "_txtFechaHoraFin";
        _txtFechaHoraFin.Properties.ReadOnly = true;
        _txtFechaHoraFin.Size = new Size(180, 20);
        _txtFechaHoraFin.TabIndex = 11;
        //
        // _btnIniciarProceso
        //
        _btnIniciarProceso.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnIniciarProceso.Location = new Point(12, 312);
        _btnIniciarProceso.Name = "_btnIniciarProceso";
        _btnIniciarProceso.Size = new Size(130, 23);
        _btnIniciarProceso.Text = "Iniciar Proceso";
        _btnIniciarProceso.Click += BtnIniciarProceso_Click;
        //
        // _btnFinalizarCorrida
        //
        _btnFinalizarCorrida.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnFinalizarCorrida.Location = new Point(148, 312);
        _btnFinalizarCorrida.Name = "_btnFinalizarCorrida";
        _btnFinalizarCorrida.Size = new Size(130, 23);
        _btnFinalizarCorrida.Text = "Finalizar Corrida";
        _btnFinalizarCorrida.Click += BtnFinalizarCorrida_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.Location = new Point(330, 312);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cerrar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // CorridaEditarForm
        //
        ClientSize = new Size(422, 347);
        Controls.Add(_lblLote);
        Controls.Add(_cmbLote);
        Controls.Add(_txtLoteFolio);
        Controls.Add(_lblCodigoTrazabilidad);
        Controls.Add(_txtCodigoTrazabilidad);
        Controls.Add(_lblHuerta);
        Controls.Add(_txtHuerta);
        Controls.Add(_lblRegistroSagarpa);
        Controls.Add(_txtRegistroSagarpa);
        Controls.Add(_lblProductor);
        Controls.Add(_txtProductor);
        Controls.Add(_lblBeneficiario);
        Controls.Add(_txtBeneficiario);
        Controls.Add(_lblKilogramos);
        Controls.Add(_txtKilogramos);
        Controls.Add(_lblKilosAProcesar);
        Controls.Add(_txtKilosAProcesar);
        Controls.Add(_lblKilosProcesados);
        Controls.Add(_txtKilosProcesados);
        Controls.Add(_lblFechaHoraInicio);
        Controls.Add(_txtFechaHoraInicio);
        Controls.Add(_lblFechaHoraFin);
        Controls.Add(_txtFechaHoraFin);
        Controls.Add(_btnIniciarProceso);
        Controls.Add(_btnFinalizarCorrida);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = true;
        Name = "CorridaEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Corrida";
        ((System.ComponentModel.ISupportInitialize)_cmbLote.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtLoteFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoTrazabilidad.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtHuerta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtBeneficiario.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilogramos.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosAProcesar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosProcesados.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFechaHoraInicio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFechaHoraFin.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
