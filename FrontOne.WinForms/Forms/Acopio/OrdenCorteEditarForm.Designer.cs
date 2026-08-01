using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Acopio;

partial class OrdenCorteEditarForm
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
    private LabelControl _lblFecha;
    private DateEdit _dtFecha;
    private LabelControl _lblAcuerdo;
    private LookUpEdit _cmbAcuerdo;
    private LabelControl _lblTipoPago;
    private TextEdit _txtTipoPago;
    private LabelControl _lblProducto;
    private TextEdit _txtProducto;
    private LabelControl _lblProductor;
    private TextEdit _txtProductor;
    private LabelControl _lblHuerta;
    private LookUpEdit _cmbHuerta;
    private LabelControl _lblFloracion;
    private LookUpEdit _cmbFloracion;
    private LabelControl _lblRegistro;
    private TextEdit _txtRegistro;
    private LabelControl _lblVariedad;
    private LookUpEdit _cmbVariedad;
    private LabelControl _lblPagarCorteA;
    private LookUpEdit _cmbPagarCorteA;
    private LabelControl _lblTransportista;
    private LookUpEdit _cmbTransportista;
    private LabelControl _lblPrecioAcarreo;
    private TextEdit _txtPrecioAcarreo;
    private LabelControl _lblNoCandado;
    private TextEdit _txtNoCandado;
    private LabelControl _lblCajas;
    private ComboBoxEdit _cmbCajas;
    private LabelControl _lblJefeCuadrilla;
    private LookUpEdit _cmbJefeCuadrilla;
    private LabelControl _lblCostoKg;
    private TextEdit _txtCostoKg;
    private LabelControl _lblPagoDia;
    private TextEdit _txtPagoDia;
    private LabelControl _lblCuadrillaApoyo;
    private TextEdit _txtCuadrillaApoyo;
    private LabelControl _lblKgMinimo;
    private TextEdit _txtKgMinimo;
    private LabelControl _lblJefeAcopio;
    private LookUpEdit _cmbJefeAcopio;
    private LabelControl _lblPuntoReunion;
    private TextEdit _txtPuntoReunion;
    private LabelControl _lblObservaciones;
    private TextEdit _txtObservaciones;
    private CheckEdit _chkCancelado;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrdenCorteEditarForm));
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _lblFecha = new LabelControl();
        _dtFecha = new DateEdit();
        _lblAcuerdo = new LabelControl();
        _cmbAcuerdo = new LookUpEdit();
        _lblTipoPago = new LabelControl();
        _txtTipoPago = new TextEdit();
        _lblProducto = new LabelControl();
        _txtProducto = new TextEdit();
        _lblProductor = new LabelControl();
        _txtProductor = new TextEdit();
        _lblHuerta = new LabelControl();
        _cmbHuerta = new LookUpEdit();
        _lblFloracion = new LabelControl();
        _cmbFloracion = new LookUpEdit();
        _lblRegistro = new LabelControl();
        _txtRegistro = new TextEdit();
        _lblVariedad = new LabelControl();
        _cmbVariedad = new LookUpEdit();
        _lblPagarCorteA = new LabelControl();
        _cmbPagarCorteA = new LookUpEdit();
        _lblTransportista = new LabelControl();
        _cmbTransportista = new LookUpEdit();
        _lblPrecioAcarreo = new LabelControl();
        _txtPrecioAcarreo = new TextEdit();
        _lblNoCandado = new LabelControl();
        _txtNoCandado = new TextEdit();
        _lblCajas = new LabelControl();
        _cmbCajas = new ComboBoxEdit();
        _lblJefeCuadrilla = new LabelControl();
        _cmbJefeCuadrilla = new LookUpEdit();
        _lblCostoKg = new LabelControl();
        _txtCostoKg = new TextEdit();
        _lblPagoDia = new LabelControl();
        _txtPagoDia = new TextEdit();
        _lblCuadrillaApoyo = new LabelControl();
        _txtCuadrillaApoyo = new TextEdit();
        _lblKgMinimo = new LabelControl();
        _txtKgMinimo = new TextEdit();
        _lblJefeAcopio = new LabelControl();
        _cmbJefeAcopio = new LookUpEdit();
        _lblPuntoReunion = new LabelControl();
        _txtPuntoReunion = new TextEdit();
        _lblObservaciones = new LabelControl();
        _txtObservaciones = new TextEdit();
        _chkCancelado = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbAcuerdo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtTipoPago.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtProducto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbHuerta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbFloracion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistro.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbVariedad.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPagarCorteA.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTransportista.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPrecioAcarreo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNoCandado.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCajas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbJefeCuadrilla.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCostoKg.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPagoDia.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCuadrillaApoyo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKgMinimo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbJefeAcopio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPuntoReunion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkCancelado.Properties).BeginInit();
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
        _txtFolio.Size = new Size(120, 20);
        _txtFolio.TabIndex = 1;
        //
        // _lblFecha
        //
        _lblFecha.Location = new Point(15, 44);
        _lblFecha.Name = "_lblFecha";
        _lblFecha.Size = new Size(35, 13);
        _lblFecha.TabIndex = 2;
        _lblFecha.Text = "Fecha:";
        //
        // _dtFecha
        //
        _dtFecha.EditValue = null;
        _dtFecha.Location = new Point(170, 41);
        _dtFecha.Name = "_dtFecha";
        _dtFecha.Size = new Size(120, 20);
        _dtFecha.TabIndex = 3;
        _dtFecha.EditValueChanged += DtFecha_EditValueChanged;
        //
        // _lblAcuerdo
        //
        _lblAcuerdo.Location = new Point(15, 70);
        _lblAcuerdo.Name = "_lblAcuerdo";
        _lblAcuerdo.Size = new Size(80, 13);
        _lblAcuerdo.TabIndex = 4;
        _lblAcuerdo.Text = "No. de Acuerdo:";
        //
        // _cmbAcuerdo
        //
        _cmbAcuerdo.Location = new Point(170, 67);
        _cmbAcuerdo.Name = "_cmbAcuerdo";
        _cmbAcuerdo.Properties.NullText = "Seleccionar";
        _cmbAcuerdo.Size = new Size(300, 20);
        _cmbAcuerdo.TabIndex = 5;
        _cmbAcuerdo.EditValueChanged += CmbAcuerdo_EditValueChanged;
        //
        // _lblTipoPago
        //
        _lblTipoPago.Location = new Point(15, 96);
        _lblTipoPago.Name = "_lblTipoPago";
        _lblTipoPago.Size = new Size(60, 13);
        _lblTipoPago.TabIndex = 6;
        _lblTipoPago.Text = "Tipo de Pago:";
        //
        // _txtTipoPago
        //
        _txtTipoPago.Location = new Point(170, 93);
        _txtTipoPago.Name = "_txtTipoPago";
        _txtTipoPago.Properties.ReadOnly = true;
        _txtTipoPago.Size = new Size(300, 20);
        _txtTipoPago.TabIndex = 7;
        //
        // _lblProducto
        //
        _lblProducto.Location = new Point(15, 122);
        _lblProducto.Name = "_lblProducto";
        _lblProducto.Size = new Size(42, 13);
        _lblProducto.TabIndex = 8;
        _lblProducto.Text = "Producto:";
        //
        // _txtProducto
        //
        _txtProducto.Location = new Point(170, 119);
        _txtProducto.Name = "_txtProducto";
        _txtProducto.Properties.ReadOnly = true;
        _txtProducto.Size = new Size(300, 20);
        _txtProducto.TabIndex = 9;
        //
        // _lblProductor
        //
        _lblProductor.Location = new Point(15, 148);
        _lblProductor.Name = "_lblProductor";
        _lblProductor.Size = new Size(50, 13);
        _lblProductor.TabIndex = 10;
        _lblProductor.Text = "Productor:";
        //
        // _txtProductor
        //
        _txtProductor.Location = new Point(170, 145);
        _txtProductor.Name = "_txtProductor";
        _txtProductor.Properties.ReadOnly = true;
        _txtProductor.Size = new Size(300, 20);
        _txtProductor.TabIndex = 11;
        //
        // _lblHuerta
        //
        _lblHuerta.Location = new Point(15, 174);
        _lblHuerta.Name = "_lblHuerta";
        _lblHuerta.Size = new Size(38, 13);
        _lblHuerta.TabIndex = 12;
        _lblHuerta.Text = "Huerta:";
        //
        // _cmbHuerta
        //
        _cmbHuerta.Location = new Point(170, 171);
        _cmbHuerta.Name = "_cmbHuerta";
        _cmbHuerta.Properties.NullText = "Seleccionar";
        _cmbHuerta.Size = new Size(300, 20);
        _cmbHuerta.TabIndex = 13;
        _cmbHuerta.EditValueChanged += CmbHuerta_EditValueChanged;
        //
        // _lblFloracion
        //
        _lblFloracion.Location = new Point(15, 200);
        _lblFloracion.Name = "_lblFloracion";
        _lblFloracion.Size = new Size(51, 13);
        _lblFloracion.TabIndex = 14;
        _lblFloracion.Text = "FloraciÃ³n:";
        //
        // _cmbFloracion
        //
        _cmbFloracion.Location = new Point(170, 197);
        _cmbFloracion.Name = "_cmbFloracion";
        _cmbFloracion.Properties.NullText = "Seleccionar";
        _cmbFloracion.Size = new Size(300, 20);
        _cmbFloracion.TabIndex = 15;
        _cmbFloracion.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbFloracion.ButtonClick += CmbFloracion_ButtonClick;
        //
        // _lblRegistro
        //
        _lblRegistro.Location = new Point(15, 226);
        _lblRegistro.Name = "_lblRegistro";
        _lblRegistro.Size = new Size(48, 13);
        _lblRegistro.TabIndex = 16;
        _lblRegistro.Text = "Registro:";
        //
        // _txtRegistro
        //
        _txtRegistro.Location = new Point(170, 223);
        _txtRegistro.Name = "_txtRegistro";
        _txtRegistro.Properties.ReadOnly = true;
        _txtRegistro.Size = new Size(300, 20);
        _txtRegistro.TabIndex = 17;
        //
        // _lblVariedad
        //
        _lblVariedad.Location = new Point(15, 252);
        _lblVariedad.Name = "_lblVariedad";
        _lblVariedad.Size = new Size(43, 13);
        _lblVariedad.TabIndex = 18;
        _lblVariedad.Text = "Variedad:";
        //
        // _cmbVariedad
        //
        _cmbVariedad.Location = new Point(170, 249);
        _cmbVariedad.Name = "_cmbVariedad";
        _cmbVariedad.Properties.NullText = "Seleccionar";
        _cmbVariedad.Size = new Size(300, 20);
        _cmbVariedad.TabIndex = 19;
        _cmbVariedad.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbVariedad.ButtonClick += CmbVariedad_ButtonClick;
        //
        // _lblCajas
        //
        _lblCajas.Location = new Point(15, 278);
        _lblCajas.Name = "_lblCajas";
        _lblCajas.Size = new Size(90, 13);
        _lblCajas.TabIndex = 20;
        _lblCajas.Text = "Cajas a Entregar:";
        //
        // _cmbCajas
        //
        _cmbCajas.Location = new Point(170, 275);
        _cmbCajas.Name = "_cmbCajas";
        _cmbCajas.Properties.Items.AddRange(new object[] { "300", "400", "500" });
        _cmbCajas.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbCajas.Size = new Size(80, 20);
        _cmbCajas.TabIndex = 21;
        _cmbCajas.EditValueChanged += CmbCajas_EditValueChanged;
        //
        // _lblPagarCorteA
        //
        _lblPagarCorteA.Location = new Point(15, 304);
        _lblPagarCorteA.Name = "_lblPagarCorteA";
        _lblPagarCorteA.Size = new Size(85, 13);
        _lblPagarCorteA.TabIndex = 22;
        _lblPagarCorteA.Text = "Pagar el Corte a:";
        //
        // _cmbPagarCorteA
        //
        _cmbPagarCorteA.Location = new Point(170, 301);
        _cmbPagarCorteA.Name = "_cmbPagarCorteA";
        _cmbPagarCorteA.Properties.NullText = "Seleccionar";
        _cmbPagarCorteA.Size = new Size(300, 20);
        _cmbPagarCorteA.TabIndex = 23;
        //
        // _lblTransportista
        //
        _lblTransportista.Location = new Point(15, 330);
        _lblTransportista.Name = "_lblTransportista";
        _lblTransportista.Size = new Size(63, 13);
        _lblTransportista.TabIndex = 24;
        _lblTransportista.Text = "Transportista:";
        //
        // _cmbTransportista
        //
        _cmbTransportista.Location = new Point(170, 327);
        _cmbTransportista.Name = "_cmbTransportista";
        _cmbTransportista.Properties.NullText = "Seleccionar";
        _cmbTransportista.Size = new Size(300, 20);
        _cmbTransportista.TabIndex = 25;
        //
        // _lblPrecioAcarreo
        //
        _lblPrecioAcarreo.Location = new Point(15, 356);
        _lblPrecioAcarreo.Name = "_lblPrecioAcarreo";
        _lblPrecioAcarreo.Size = new Size(70, 13);
        _lblPrecioAcarreo.TabIndex = 26;
        _lblPrecioAcarreo.Text = "Precio Acarreo:";
        //
        // _txtPrecioAcarreo
        //
        _txtPrecioAcarreo.Location = new Point(170, 353);
        _txtPrecioAcarreo.Name = "_txtPrecioAcarreo";
        _txtPrecioAcarreo.Properties.ReadOnly = true;
        _txtPrecioAcarreo.Size = new Size(120, 20);
        _txtPrecioAcarreo.TabIndex = 27;
        //
        // _lblNoCandado
        //
        _lblNoCandado.Location = new Point(15, 382);
        _lblNoCandado.Name = "_lblNoCandado";
        _lblNoCandado.Size = new Size(74, 13);
        _lblNoCandado.TabIndex = 28;
        _lblNoCandado.Text = "No. de Candado:";
        //
        // _txtNoCandado
        //
        _txtNoCandado.Location = new Point(170, 379);
        _txtNoCandado.Name = "_txtNoCandado";
        _txtNoCandado.Size = new Size(150, 20);
        _txtNoCandado.TabIndex = 29;
        //
        // _lblJefeCuadrilla
        //
        _lblJefeCuadrilla.Location = new Point(15, 408);
        _lblJefeCuadrilla.Name = "_lblJefeCuadrilla";
        _lblJefeCuadrilla.Size = new Size(120, 13);
        _lblJefeCuadrilla.TabIndex = 30;
        _lblJefeCuadrilla.Text = "Jefe de Cuadrilla:";
        //
        // _cmbJefeCuadrilla
        //
        _cmbJefeCuadrilla.Location = new Point(170, 405);
        _cmbJefeCuadrilla.Name = "_cmbJefeCuadrilla";
        _cmbJefeCuadrilla.Properties.NullText = "Seleccionar";
        _cmbJefeCuadrilla.Size = new Size(300, 20);
        _cmbJefeCuadrilla.TabIndex = 31;
        _cmbJefeCuadrilla.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbJefeCuadrilla.EditValueChanged += CmbJefeCuadrilla_EditValueChanged;
        _cmbJefeCuadrilla.ButtonClick += CmbJefeCuadrilla_ButtonClick;
        //
        // _lblCostoKg
        //
        _lblCostoKg.Location = new Point(15, 434);
        _lblCostoKg.Name = "_lblCostoKg";
        _lblCostoKg.Size = new Size(55, 13);
        _lblCostoKg.TabIndex = 32;
        _lblCostoKg.Text = "Costo x kg:";
        //
        // _txtCostoKg
        //
        _txtCostoKg.Location = new Point(170, 431);
        _txtCostoKg.Name = "_txtCostoKg";
        _txtCostoKg.Properties.ReadOnly = true;
        _txtCostoKg.Size = new Size(120, 20);
        _txtCostoKg.TabIndex = 33;
        //
        // _lblPagoDia
        //
        _lblPagoDia.Location = new Point(15, 460);
        _lblPagoDia.Name = "_lblPagoDia";
        _lblPagoDia.Size = new Size(63, 13);
        _lblPagoDia.TabIndex = 34;
        _lblPagoDia.Text = "Pago por dÃ­a:";
        //
        // _txtPagoDia
        //
        _txtPagoDia.Location = new Point(170, 457);
        _txtPagoDia.Name = "_txtPagoDia";
        _txtPagoDia.Properties.ReadOnly = true;
        _txtPagoDia.Size = new Size(120, 20);
        _txtPagoDia.TabIndex = 35;
        //
        // _lblCuadrillaApoyo
        //
        _lblCuadrillaApoyo.Location = new Point(15, 486);
        _lblCuadrillaApoyo.Name = "_lblCuadrillaApoyo";
        _lblCuadrillaApoyo.Size = new Size(90, 13);
        _lblCuadrillaApoyo.TabIndex = 36;
        _lblCuadrillaApoyo.Text = "Cuadrilla de Apoyo:";
        //
        // _txtCuadrillaApoyo
        //
        _txtCuadrillaApoyo.Location = new Point(170, 483);
        _txtCuadrillaApoyo.Name = "_txtCuadrillaApoyo";
        _txtCuadrillaApoyo.Properties.ReadOnly = true;
        _txtCuadrillaApoyo.Size = new Size(120, 20);
        _txtCuadrillaApoyo.TabIndex = 37;
        //
        // _lblKgMinimo
        //
        _lblKgMinimo.Location = new Point(15, 512);
        _lblKgMinimo.Name = "_lblKgMinimo";
        _lblKgMinimo.Size = new Size(68, 13);
        _lblKgMinimo.TabIndex = 38;
        _lblKgMinimo.Text = "Kg MÃ­nimos:";
        //
        // _txtKgMinimo
        //
        _txtKgMinimo.Location = new Point(170, 509);
        _txtKgMinimo.Name = "_txtKgMinimo";
        _txtKgMinimo.Properties.ReadOnly = true;
        _txtKgMinimo.Size = new Size(120, 20);
        _txtKgMinimo.TabIndex = 39;
        //
        // _lblJefeAcopio
        //
        _lblJefeAcopio.Location = new Point(15, 538);
        _lblJefeAcopio.Name = "_lblJefeAcopio";
        _lblJefeAcopio.Size = new Size(78, 13);
        _lblJefeAcopio.TabIndex = 40;
        _lblJefeAcopio.Text = "Jefe de Acopio:";
        //
        // _cmbJefeAcopio
        //
        _cmbJefeAcopio.Location = new Point(170, 535);
        _cmbJefeAcopio.Name = "_cmbJefeAcopio";
        _cmbJefeAcopio.Properties.NullText = "Seleccionar";
        _cmbJefeAcopio.Size = new Size(300, 20);
        _cmbJefeAcopio.TabIndex = 41;
        _cmbJefeAcopio.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbJefeAcopio.ButtonClick += CmbJefeAcopio_ButtonClick;
        //
        // _lblPuntoReunion
        //
        _lblPuntoReunion.Location = new Point(15, 564);
        _lblPuntoReunion.Name = "_lblPuntoReunion";
        _lblPuntoReunion.Size = new Size(80, 13);
        _lblPuntoReunion.TabIndex = 42;
        _lblPuntoReunion.Text = "Punto de ReuniÃ³n:";
        //
        // _txtPuntoReunion
        //
        _txtPuntoReunion.Location = new Point(170, 561);
        _txtPuntoReunion.Name = "_txtPuntoReunion";
        _txtPuntoReunion.Size = new Size(300, 20);
        _txtPuntoReunion.TabIndex = 43;
        //
        // _lblObservaciones
        //
        _lblObservaciones.Location = new Point(15, 590);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(68, 13);
        _lblObservaciones.TabIndex = 44;
        _lblObservaciones.Text = "Observaciones:";
        //
        // _txtObservaciones
        //
        _txtObservaciones.Location = new Point(170, 587);
        _txtObservaciones.Name = "_txtObservaciones";
        _txtObservaciones.Size = new Size(300, 20);
        _txtObservaciones.TabIndex = 45;
        //
        // _chkCancelado
        //
        _chkCancelado.Location = new Point(170, 616);
        _chkCancelado.Name = "_chkCancelado";
        _chkCancelado.Properties.Caption = "Â¿Movimiento cancelado?";
        _chkCancelado.Size = new Size(200, 20);
        _chkCancelado.TabIndex = 46;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(310, 650);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 47;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(400, 650);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 48;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // OrdenCorteEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(500, 690);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_lblFecha);
        Controls.Add(_dtFecha);
        Controls.Add(_lblAcuerdo);
        Controls.Add(_cmbAcuerdo);
        Controls.Add(_lblTipoPago);
        Controls.Add(_txtTipoPago);
        Controls.Add(_lblProducto);
        Controls.Add(_txtProducto);
        Controls.Add(_lblProductor);
        Controls.Add(_txtProductor);
        Controls.Add(_lblHuerta);
        Controls.Add(_cmbHuerta);
        Controls.Add(_lblFloracion);
        Controls.Add(_cmbFloracion);
        Controls.Add(_lblRegistro);
        Controls.Add(_txtRegistro);
        Controls.Add(_lblVariedad);
        Controls.Add(_cmbVariedad);
        Controls.Add(_lblCajas);
        Controls.Add(_cmbCajas);
        Controls.Add(_lblPagarCorteA);
        Controls.Add(_cmbPagarCorteA);
        Controls.Add(_lblTransportista);
        Controls.Add(_cmbTransportista);
        Controls.Add(_lblPrecioAcarreo);
        Controls.Add(_txtPrecioAcarreo);
        Controls.Add(_lblNoCandado);
        Controls.Add(_txtNoCandado);
        Controls.Add(_lblJefeCuadrilla);
        Controls.Add(_cmbJefeCuadrilla);
        Controls.Add(_lblCostoKg);
        Controls.Add(_txtCostoKg);
        Controls.Add(_lblPagoDia);
        Controls.Add(_txtPagoDia);
        Controls.Add(_lblCuadrillaApoyo);
        Controls.Add(_txtCuadrillaApoyo);
        Controls.Add(_lblKgMinimo);
        Controls.Add(_txtKgMinimo);
        Controls.Add(_lblJefeAcopio);
        Controls.Add(_cmbJefeAcopio);
        Controls.Add(_lblPuntoReunion);
        Controls.Add(_txtPuntoReunion);
        Controls.Add(_lblObservaciones);
        Controls.Add(_txtObservaciones);
        Controls.Add(_chkCancelado);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = true;
        Name = "OrdenCorteEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Orden de Corte";
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbAcuerdo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtTipoPago.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtProducto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbHuerta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbFloracion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistro.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbVariedad.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPagarCorteA.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTransportista.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPrecioAcarreo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNoCandado.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCajas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbJefeCuadrilla.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCostoKg.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPagoDia.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCuadrillaApoyo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKgMinimo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbJefeAcopio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPuntoReunion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkCancelado.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
