using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;

namespace FrontOne.WinForms.Forms.Acopio;

partial class JefeAcopioEditarForm
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

    // Encabezado
    private SimpleButton _btnInicio;
    private SimpleButton _btnAnterior;
    private SimpleButton _btnSiguiente;
    private SimpleButton _btnFin;
    private LabelControl _lblClave;
    private TextEdit _txtClave;
    private LabelControl _lblNombre;
    private ButtonEdit _txtNombre;

    private XtraTabControl _tabControl;
    private XtraTabPage _tabGeneral;

    private LabelControl _lblDomicilio;
    private TextEdit _txtDomicilio;
    private LabelControl _lblColonia;
    private TextEdit _txtColonia;
    private LabelControl _lblCodigoPostal;
    private TextEdit _txtCodigoPostal;
    private LabelControl _lblPais;
    private LookUpEdit _cmbPais;
    private LabelControl _lblEstado;
    private LookUpEdit _cmbEstado;
    private LabelControl _lblMunicipio;
    private LookUpEdit _cmbMunicipio;
    private LabelControl _lblPoblacion;
    private LookUpEdit _cmbPoblacion;
    private LabelControl _lblTelefono;
    private TextEdit _txtTelefono;
    private LabelControl _lblCelular;
    private TextEdit _txtCelular;
    private LabelControl _lblEmail;
    private TextEdit _txtEmail;
    private LabelControl _lblObservaciones;
    private TextEdit _txtObservaciones;
    private CheckEdit _chkActivo;

    private SimpleButton _btnNuevo;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JefeAcopioEditarForm));
        _btnInicio = new SimpleButton();
        _btnAnterior = new SimpleButton();
        _btnSiguiente = new SimpleButton();
        _btnFin = new SimpleButton();
        _lblClave = new LabelControl();
        _txtClave = new TextEdit();
        _lblNombre = new LabelControl();
        _txtNombre = new ButtonEdit();

        _tabControl = new XtraTabControl();
        _tabGeneral = new XtraTabPage();

        _lblDomicilio = new LabelControl();
        _txtDomicilio = new TextEdit();
        _lblColonia = new LabelControl();
        _txtColonia = new TextEdit();
        _lblCodigoPostal = new LabelControl();
        _txtCodigoPostal = new TextEdit();
        _lblPais = new LabelControl();
        _cmbPais = new LookUpEdit();
        _lblEstado = new LabelControl();
        _cmbEstado = new LookUpEdit();
        _lblMunicipio = new LabelControl();
        _cmbMunicipio = new LookUpEdit();
        _lblPoblacion = new LabelControl();
        _cmbPoblacion = new LookUpEdit();
        _lblTelefono = new LabelControl();
        _txtTelefono = new TextEdit();
        _lblCelular = new LabelControl();
        _txtCelular = new TextEdit();
        _lblEmail = new LabelControl();
        _txtEmail = new TextEdit();
        _lblObservaciones = new LabelControl();
        _txtObservaciones = new TextEdit();
        _chkActivo = new CheckEdit();

        _btnNuevo = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();

        ((System.ComponentModel.ISupportInitialize)_txtClave.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_tabControl).BeginInit();
        _tabControl.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_txtDomicilio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtColonia.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoPostal.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMunicipio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPoblacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtTelefono.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCelular.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtEmail.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // Encabezado: navegación
        //
        _btnInicio.Location = new Point(20, 8);
        _btnInicio.Name = "_btnInicio";
        _btnInicio.Size = new Size(28, 23);
        _btnInicio.Text = "";
        _btnInicio.ToolTip = "Inicio";
        _btnInicio.Click += BtnInicio_Click;
        _btnAnterior.Location = new Point(52, 8);
        _btnAnterior.Name = "_btnAnterior";
        _btnAnterior.Size = new Size(28, 23);
        _btnAnterior.Text = "";
        _btnAnterior.ToolTip = "Anterior";
        _btnAnterior.Click += BtnAnterior_Click;
        _btnSiguiente.Location = new Point(84, 8);
        _btnSiguiente.Name = "_btnSiguiente";
        _btnSiguiente.Size = new Size(28, 23);
        _btnSiguiente.Text = "";
        _btnSiguiente.ToolTip = "Siguiente";
        _btnSiguiente.Click += BtnSiguiente_Click;
        _btnFin.Location = new Point(116, 8);
        _btnFin.Name = "_btnFin";
        _btnFin.Size = new Size(28, 23);
        _btnFin.Text = "";
        _btnFin.ToolTip = "Fin";
        _btnFin.Click += BtnFin_Click;
        //
        // Encabezado: Clave
        //
        _lblClave.Location = new Point(20, 45);
        _lblClave.Name = "_lblClave";
        _lblClave.Size = new Size(31, 13);
        _lblClave.Text = "Clave:";
        _txtClave.Location = new Point(150, 43);
        _txtClave.Name = "_txtClave";
        _txtClave.Properties.ReadOnly = true;
        _txtClave.Size = new Size(90, 20);
        //
        // Encabezado: Nombre
        //
        _lblNombre.Location = new Point(20, 73);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(45, 13);
        _lblNombre.Text = "Nombre:";
        _txtNombre.Location = new Point(150, 71);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Search));
        _txtNombre.Size = new Size(300, 20);
        _txtNombre.ButtonClick += TxtNombre_ButtonClick;
        //
        // _tabControl
        //
        _tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _tabControl.Location = new Point(10, 103);
        _tabControl.Name = "_tabControl";
        _tabControl.SelectedTabPage = _tabGeneral;
        _tabControl.Size = new Size(620, 350);
        _tabControl.TabPages.AddRange(new XtraTabPage[] { _tabGeneral });
        //
        // _tabGeneral
        //
        _tabGeneral.Name = "_tabGeneral";
        _tabGeneral.Text = "General";
        _tabGeneral.Controls.Add(_lblDomicilio);
        _tabGeneral.Controls.Add(_txtDomicilio);
        _tabGeneral.Controls.Add(_lblColonia);
        _tabGeneral.Controls.Add(_txtColonia);
        _tabGeneral.Controls.Add(_lblCodigoPostal);
        _tabGeneral.Controls.Add(_txtCodigoPostal);
        _tabGeneral.Controls.Add(_lblPais);
        _tabGeneral.Controls.Add(_cmbPais);
        _tabGeneral.Controls.Add(_lblEstado);
        _tabGeneral.Controls.Add(_cmbEstado);
        _tabGeneral.Controls.Add(_lblMunicipio);
        _tabGeneral.Controls.Add(_cmbMunicipio);
        _tabGeneral.Controls.Add(_lblPoblacion);
        _tabGeneral.Controls.Add(_cmbPoblacion);
        _tabGeneral.Controls.Add(_lblTelefono);
        _tabGeneral.Controls.Add(_txtTelefono);
        _tabGeneral.Controls.Add(_lblCelular);
        _tabGeneral.Controls.Add(_txtCelular);
        _tabGeneral.Controls.Add(_lblEmail);
        _tabGeneral.Controls.Add(_txtEmail);
        _tabGeneral.Controls.Add(_lblObservaciones);
        _tabGeneral.Controls.Add(_txtObservaciones);
        _tabGeneral.Controls.Add(_chkActivo);
        //
        // Domicilio
        //
        _lblDomicilio.Location = new Point(10, 10);
        _lblDomicilio.Name = "_lblDomicilio";
        _lblDomicilio.Size = new Size(48, 13);
        _lblDomicilio.Text = "Domicilio:";
        _txtDomicilio.Location = new Point(140, 8);
        _txtDomicilio.Name = "_txtDomicilio";
        _txtDomicilio.Size = new Size(300, 20);
        //
        // Colonia
        //
        _lblColonia.Location = new Point(10, 38);
        _lblColonia.Name = "_lblColonia";
        _lblColonia.Size = new Size(40, 13);
        _lblColonia.Text = "Colonia:";
        _txtColonia.Location = new Point(140, 36);
        _txtColonia.Name = "_txtColonia";
        _txtColonia.Size = new Size(300, 20);
        //
        // Código Postal
        //
        _lblCodigoPostal.Location = new Point(10, 66);
        _lblCodigoPostal.Name = "_lblCodigoPostal";
        _lblCodigoPostal.Size = new Size(72, 13);
        _lblCodigoPostal.Text = "Código Postal:";
        _txtCodigoPostal.Location = new Point(140, 64);
        _txtCodigoPostal.Name = "_txtCodigoPostal";
        _txtCodigoPostal.Properties.MaxLength = 10;
        _txtCodigoPostal.Size = new Size(90, 20);
        //
        // País
        //
        _lblPais.Location = new Point(10, 94);
        _lblPais.Name = "_lblPais";
        _lblPais.Size = new Size(21, 13);
        _lblPais.Text = "País:";
        _cmbPais.Location = new Point(140, 92);
        _cmbPais.Name = "_cmbPais";
        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbPais.Properties.NullText = "Seleccionar";
        _cmbPais.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbPais.Size = new Size(260, 20);
        _cmbPais.ButtonClick += CmbPais_ButtonClick;
        //
        // Estado
        //
        _lblEstado.Location = new Point(10, 122);
        _lblEstado.Name = "_lblEstado";
        _lblEstado.Size = new Size(37, 13);
        _lblEstado.Text = "Estado:";
        _cmbEstado.Location = new Point(140, 120);
        _cmbEstado.Name = "_cmbEstado";
        _cmbEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbEstado.Properties.NullText = "Seleccionar";
        _cmbEstado.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbEstado.Size = new Size(260, 20);
        _cmbEstado.ButtonClick += CmbEstado_ButtonClick;
        //
        // Municipio
        //
        _lblMunicipio.Location = new Point(10, 150);
        _lblMunicipio.Name = "_lblMunicipio";
        _lblMunicipio.Size = new Size(53, 13);
        _lblMunicipio.Text = "Municipio:";
        _cmbMunicipio.Location = new Point(140, 148);
        _cmbMunicipio.Name = "_cmbMunicipio";
        _cmbMunicipio.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbMunicipio.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbMunicipio.Properties.NullText = "Seleccionar";
        _cmbMunicipio.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbMunicipio.Size = new Size(260, 20);
        _cmbMunicipio.ButtonClick += CmbMunicipio_ButtonClick;
        //
        // Población
        //
        _lblPoblacion.Location = new Point(10, 178);
        _lblPoblacion.Name = "_lblPoblacion";
        _lblPoblacion.Size = new Size(55, 13);
        _lblPoblacion.Text = "Población:";
        _cmbPoblacion.Location = new Point(140, 176);
        _cmbPoblacion.Name = "_cmbPoblacion";
        _cmbPoblacion.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbPoblacion.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbPoblacion.Properties.NullText = "Seleccionar";
        _cmbPoblacion.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbPoblacion.Size = new Size(260, 20);
        _cmbPoblacion.ButtonClick += CmbPoblacion_ButtonClick;
        //
        // Teléfono
        //
        _lblTelefono.Location = new Point(10, 206);
        _lblTelefono.Name = "_lblTelefono";
        _lblTelefono.Size = new Size(44, 13);
        _lblTelefono.Text = "Teléfono:";
        _txtTelefono.Location = new Point(140, 204);
        _txtTelefono.Name = "_txtTelefono";
        _txtTelefono.Size = new Size(200, 20);
        //
        // Celular
        //
        _lblCelular.Location = new Point(10, 234);
        _lblCelular.Name = "_lblCelular";
        _lblCelular.Size = new Size(38, 13);
        _lblCelular.Text = "Celular:";
        _txtCelular.Location = new Point(140, 232);
        _txtCelular.Name = "_txtCelular";
        _txtCelular.Size = new Size(200, 20);
        //
        // e-mail
        //
        _lblEmail.Location = new Point(10, 262);
        _lblEmail.Name = "_lblEmail";
        _lblEmail.Size = new Size(35, 13);
        _lblEmail.Text = "e-mail:";
        _txtEmail.Location = new Point(140, 260);
        _txtEmail.Name = "_txtEmail";
        _txtEmail.Size = new Size(300, 20);
        //
        // Observaciones
        //
        _lblObservaciones.Location = new Point(10, 290);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(73, 13);
        _lblObservaciones.Text = "Observaciones:";
        _txtObservaciones.Location = new Point(140, 288);
        _txtObservaciones.Name = "_txtObservaciones";
        _txtObservaciones.Size = new Size(300, 20);
        //
        // Activo
        //
        _chkActivo.Location = new Point(140, 316);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        //
        // _btnNuevo
        //
        _btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevo.Location = new Point(20, 470);
        _btnNuevo.Name = "_btnNuevo";
        _btnNuevo.ImageOptions.Image = (Image)resources.GetObject("_btnNuevo.ImageOptions.Image");
        _btnNuevo.Size = new Size(90, 23);
        _btnNuevo.Text = "Nuevo";
        _btnNuevo.Click += BtnNuevo_Click;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.Location = new Point(116, 470);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnEliminar
        //
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.Location = new Point(202, 470);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Size = new Size(90, 23);
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Click += BtnEliminar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.Location = new Point(550, 470);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // JefeAcopioEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(640, 510);
        Controls.Add(_btnInicio);
        Controls.Add(_btnAnterior);
        Controls.Add(_btnSiguiente);
        Controls.Add(_btnFin);
        Controls.Add(_lblClave);
        Controls.Add(_txtClave);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_tabControl);
        Controls.Add(_btnNuevo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = true;
        MinimumSize = new Size(560, 420);
        Name = "JefeAcopioEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Jefe de Acopio";
        ((System.ComponentModel.ISupportInitialize)_txtClave.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDomicilio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtColonia.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoPostal.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMunicipio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPoblacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtTelefono.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCelular.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtEmail.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_tabControl).EndInit();
        _tabControl.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
