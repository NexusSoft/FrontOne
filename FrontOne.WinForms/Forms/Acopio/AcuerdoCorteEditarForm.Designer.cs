using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Acopio;

partial class AcuerdoCorteEditarForm
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
    private LabelControl _lblProductor;
    private ButtonEdit _cmbProductor;
    private LabelControl _lblFechaInicio;
    private DateEdit _dtFechaInicio;
    private LabelControl _lblFechaFin;
    private DateEdit _dtFechaFin;
    private LabelControl _lblProducto;
    private LookUpEdit _cmbProducto;
    private LabelControl _lblVariedad;
    private LookUpEdit _cmbVariedad;
    private LabelControl _lblTipoComercializacion;
    private LookUpEdit _cmbTipoComercializacion;
    private LabelControl _lblTipoCorte;
    private LookUpEdit _cmbTipoCorte;
    private LabelControl _lblPrecio;
    private SpinEdit _spnPrecio;
    private LabelControl _lblListaPrecios;
    private ButtonEdit _cmbListaPrecios;
    private LabelControl _lblListaNro;
    private ComboBoxEdit _cmbListaPrecioNumero;
    private SimpleButton _btnVerListaPrecios;
    private LabelControl _lblMoneda;
    private LookUpEdit _cmbMoneda;
    private LabelControl _lblObservaciones;
    private TextEdit _txtObservaciones;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AcuerdoCorteEditarForm));
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _lblProductor = new LabelControl();
        _cmbProductor = new ButtonEdit();
        _lblFechaInicio = new LabelControl();
        _dtFechaInicio = new DateEdit();
        _lblFechaFin = new LabelControl();
        _dtFechaFin = new DateEdit();
        _lblProducto = new LabelControl();
        _cmbProducto = new LookUpEdit();
        _lblVariedad = new LabelControl();
        _cmbVariedad = new LookUpEdit();
        _lblTipoComercializacion = new LabelControl();
        _cmbTipoComercializacion = new LookUpEdit();
        _lblTipoCorte = new LabelControl();
        _cmbTipoCorte = new LookUpEdit();
        _lblPrecio = new LabelControl();
        _spnPrecio = new SpinEdit();
        _lblListaPrecios = new LabelControl();
        _cmbListaPrecios = new ButtonEdit();
        _lblListaNro = new LabelControl();
        _cmbListaPrecioNumero = new ComboBoxEdit();
        _btnVerListaPrecios = new SimpleButton();
        _lblMoneda = new LabelControl();
        _cmbMoneda = new LookUpEdit();
        _lblObservaciones = new LabelControl();
        _txtObservaciones = new TextEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaInicio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaInicio.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaFin.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaFin.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbVariedad.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoComercializacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoCorte.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPrecio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbListaPrecios.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbListaPrecioNumero.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMoneda.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblFolio
        //
        _lblFolio.Location = new Point(15, 18);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(27, 13);
        _lblFolio.TabIndex = 0;
        _lblFolio.Text = "Folio:";
        //
        // _txtFolio
        //
        _txtFolio.Location = new Point(170, 15);
        _txtFolio.Name = "_txtFolio";
        _txtFolio.Properties.ReadOnly = true;
        _txtFolio.Size = new Size(100, 20);
        _txtFolio.TabIndex = 1;
        //
        // _lblProductor
        //
        _lblProductor.Location = new Point(15, 44);
        _lblProductor.Name = "_lblProductor";
        _lblProductor.Size = new Size(50, 13);
        _lblProductor.TabIndex = 2;
        _lblProductor.Text = "Productor:";
        //
        // _cmbProductor
        //
        _cmbProductor.Location = new Point(170, 41);
        _cmbProductor.Name = "_cmbProductor";
        _cmbProductor.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Search) });
        _cmbProductor.Properties.NullValuePrompt = "Buscar productor...";
        _cmbProductor.Properties.ReadOnly = true;
        _cmbProductor.Size = new Size(320, 20);
        _cmbProductor.TabIndex = 3;
        _cmbProductor.ButtonClick += CmbProductor_ButtonClick;
        //
        // _lblFechaInicio
        //
        _lblFechaInicio.Location = new Point(15, 70);
        _lblFechaInicio.Name = "_lblFechaInicio";
        _lblFechaInicio.Size = new Size(35, 13);
        _lblFechaInicio.TabIndex = 4;
        _lblFechaInicio.Text = "Fecha:";
        //
        // _dtFechaInicio
        //
        _dtFechaInicio.EditValue = null;
        _dtFechaInicio.Location = new Point(170, 67);
        _dtFechaInicio.Name = "_dtFechaInicio";
        _dtFechaInicio.Size = new Size(120, 20);
        _dtFechaInicio.TabIndex = 5;
        //
        // _lblFechaFin
        //
        _lblFechaFin.Location = new Point(305, 70);
        _lblFechaFin.Name = "_lblFechaFin";
        _lblFechaFin.Size = new Size(115, 13);
        _lblFechaFin.TabIndex = 6;
        _lblFechaFin.Text = "Caducidad del Acuerdo:";
        //
        // _dtFechaFin
        //
        _dtFechaFin.EditValue = null;
        _dtFechaFin.Location = new Point(425, 67);
        _dtFechaFin.Name = "_dtFechaFin";
        _dtFechaFin.Size = new Size(120, 20);
        _dtFechaFin.TabIndex = 7;
        //
        // _lblProducto
        //
        _lblProducto.Location = new Point(15, 96);
        _lblProducto.Name = "_lblProducto";
        _lblProducto.Size = new Size(42, 13);
        _lblProducto.TabIndex = 8;
        _lblProducto.Text = "Producto:";
        //
        // _cmbProducto
        //
        _cmbProducto.Location = new Point(170, 93);
        _cmbProducto.Name = "_cmbProducto";
        _cmbProducto.Properties.NullText = "Seleccionar";
        _cmbProducto.Size = new Size(300, 20);
        _cmbProducto.TabIndex = 9;
        _cmbProducto.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbProducto.ButtonClick += CmbProducto_ButtonClick;
        //
        // _lblVariedad
        //
        _lblVariedad.Location = new Point(15, 122);
        _lblVariedad.Name = "_lblVariedad";
        _lblVariedad.Size = new Size(43, 13);
        _lblVariedad.TabIndex = 10;
        _lblVariedad.Text = "Variedad:";
        //
        // _cmbVariedad
        //
        _cmbVariedad.Location = new Point(170, 119);
        _cmbVariedad.Name = "_cmbVariedad";
        _cmbVariedad.Properties.NullText = "Seleccionar";
        _cmbVariedad.Size = new Size(300, 20);
        _cmbVariedad.TabIndex = 11;
        _cmbVariedad.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbVariedad.ButtonClick += CmbVariedad_ButtonClick;
        //
        // _lblTipoComercializacion
        //
        _lblTipoComercializacion.Location = new Point(15, 148);
        _lblTipoComercializacion.Name = "_lblTipoComercializacion";
        _lblTipoComercializacion.Size = new Size(122, 13);
        _lblTipoComercializacion.TabIndex = 12;
        _lblTipoComercializacion.Text = "Tipo de comercialización:";
        //
        // _cmbTipoComercializacion
        //
        _cmbTipoComercializacion.Location = new Point(170, 145);
        _cmbTipoComercializacion.Name = "_cmbTipoComercializacion";
        _cmbTipoComercializacion.Properties.NullText = "Seleccionar";
        _cmbTipoComercializacion.Size = new Size(300, 20);
        _cmbTipoComercializacion.TabIndex = 13;
        _cmbTipoComercializacion.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbTipoComercializacion.ButtonClick += CmbTipoComercializacion_ButtonClick;
        //
        // _lblTipoCorte
        //
        _lblTipoCorte.Location = new Point(15, 174);
        _lblTipoCorte.Name = "_lblTipoCorte";
        _lblTipoCorte.Size = new Size(60, 13);
        _lblTipoCorte.TabIndex = 14;
        _lblTipoCorte.Text = "Tipo de corte:";
        //
        // _cmbTipoCorte
        //
        _cmbTipoCorte.Location = new Point(170, 171);
        _cmbTipoCorte.Name = "_cmbTipoCorte";
        _cmbTipoCorte.Properties.NullText = "Seleccionar";
        _cmbTipoCorte.Size = new Size(300, 20);
        _cmbTipoCorte.TabIndex = 15;
        _cmbTipoCorte.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbTipoCorte.ButtonClick += CmbTipoCorte_ButtonClick;
        _cmbTipoCorte.EditValueChanged += CmbTipoCorte_EditValueChanged;
        //
        // _lblPrecio
        //
        _lblPrecio.Location = new Point(15, 200);
        _lblPrecio.Name = "_lblPrecio";
        _lblPrecio.Size = new Size(30, 13);
        _lblPrecio.TabIndex = 16;
        _lblPrecio.Text = "Precio:";
        //
        // _spnPrecio
        //
        _spnPrecio.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPrecio.Location = new Point(170, 197);
        _spnPrecio.Name = "_spnPrecio";
        _spnPrecio.Properties.Mask.EditMask = "N02";
        _spnPrecio.Size = new Size(120, 20);
        _spnPrecio.TabIndex = 17;
        //
        // _lblListaPrecios
        //
        _lblListaPrecios.Location = new Point(15, 200);
        _lblListaPrecios.Name = "_lblListaPrecios";
        _lblListaPrecios.Size = new Size(75, 13);
        _lblListaPrecios.TabIndex = 18;
        _lblListaPrecios.Text = "Lista de Precios:";
        //
        // _cmbListaPrecios
        //
        _cmbListaPrecios.Location = new Point(170, 197);
        _cmbListaPrecios.Name = "_cmbListaPrecios";
        _cmbListaPrecios.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Search) });
        _cmbListaPrecios.Properties.NullValuePrompt = "Buscar lista de precios...";
        _cmbListaPrecios.Properties.ReadOnly = true;
        _cmbListaPrecios.Size = new Size(300, 20);
        _cmbListaPrecios.TabIndex = 19;
        _cmbListaPrecios.ButtonClick += CmbListaPrecios_ButtonClick;
        //
        // _lblListaNro
        //
        _lblListaNro.Location = new Point(15, 226);
        _lblListaNro.Name = "_lblListaNro";
        _lblListaNro.Size = new Size(53, 13);
        _lblListaNro.TabIndex = 18;
        _lblListaNro.Text = "Lista Nro:";
        //
        // _cmbListaPrecioNumero
        //
        _cmbListaPrecioNumero.Location = new Point(170, 223);
        _cmbListaPrecioNumero.Name = "_cmbListaPrecioNumero";
        _cmbListaPrecioNumero.Properties.Items.AddRange(new object[] { "1", "2", "3" });
        _cmbListaPrecioNumero.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbListaPrecioNumero.Size = new Size(60, 20);
        _cmbListaPrecioNumero.TabIndex = 19;
        //
        // _btnVerListaPrecios
        //
        _btnVerListaPrecios.Location = new Point(240, 222);
        _btnVerListaPrecios.Name = "_btnVerListaPrecios";
        _btnVerListaPrecios.Size = new Size(100, 23);
        _btnVerListaPrecios.TabIndex = 20;
        _btnVerListaPrecios.Text = "Ver Precios";
        _btnVerListaPrecios.Click += BtnVerListaPrecios_Click;
        //
        // _lblMoneda
        //
        _lblMoneda.Location = new Point(15, 252);
        _lblMoneda.Name = "_lblMoneda";
        _lblMoneda.Size = new Size(41, 13);
        _lblMoneda.TabIndex = 21;
        _lblMoneda.Text = "Moneda:";
        //
        // _cmbMoneda
        //
        _cmbMoneda.Location = new Point(170, 249);
        _cmbMoneda.Name = "_cmbMoneda";
        _cmbMoneda.Properties.NullText = "Seleccionar";
        _cmbMoneda.Size = new Size(200, 20);
        _cmbMoneda.TabIndex = 22;
        _cmbMoneda.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbMoneda.ButtonClick += CmbMoneda_ButtonClick;
        //
        // _lblObservaciones
        //
        _lblObservaciones.Location = new Point(15, 278);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(68, 13);
        _lblObservaciones.TabIndex = 23;
        _lblObservaciones.Text = "Observaciones:";
        //
        // _txtObservaciones
        //
        _txtObservaciones.Location = new Point(170, 275);
        _txtObservaciones.Name = "_txtObservaciones";
        _txtObservaciones.Size = new Size(375, 20);
        _txtObservaciones.TabIndex = 24;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(370, 311);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 25;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(460, 311);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 26;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // AcuerdoCorteEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(560, 351);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_lblProductor);
        Controls.Add(_cmbProductor);
        Controls.Add(_lblFechaInicio);
        Controls.Add(_dtFechaInicio);
        Controls.Add(_lblFechaFin);
        Controls.Add(_dtFechaFin);
        Controls.Add(_lblProducto);
        Controls.Add(_cmbProducto);
        Controls.Add(_lblVariedad);
        Controls.Add(_cmbVariedad);
        Controls.Add(_lblTipoComercializacion);
        Controls.Add(_cmbTipoComercializacion);
        Controls.Add(_lblTipoCorte);
        Controls.Add(_cmbTipoCorte);
        Controls.Add(_lblPrecio);
        Controls.Add(_spnPrecio);
        Controls.Add(_lblListaPrecios);
        Controls.Add(_cmbListaPrecios);
        Controls.Add(_lblListaNro);
        Controls.Add(_cmbListaPrecioNumero);
        Controls.Add(_btnVerListaPrecios);
        Controls.Add(_lblMoneda);
        Controls.Add(_cmbMoneda);
        Controls.Add(_lblObservaciones);
        Controls.Add(_txtObservaciones);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = true;
        Name = "AcuerdoCorteEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Acuerdo de Corte";
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaInicio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaInicio.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaFin.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaFin.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbVariedad.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoComercializacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoCorte.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPrecio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbListaPrecios.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbListaPrecioNumero.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMoneda.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
