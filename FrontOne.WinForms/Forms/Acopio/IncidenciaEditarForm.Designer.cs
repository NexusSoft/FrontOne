using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Acopio;

partial class IncidenciaEditarForm
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

    private LabelControl _lblSeccionOrdenCorte;
    private LabelControl _lblFolio;
    private TextEdit _txtFolio;
    private LabelControl _lblFecha;
    private TextEdit _txtFecha;
    private LabelControl _lblAcopiador;
    private TextEdit _txtAcopiador;
    private LabelControl _lblRegistroSagarpa;
    private TextEdit _txtRegistroSagarpa;
    private LabelControl _lblHuerta;
    private TextEdit _txtHuerta;
    private LabelControl _lblCajasEntregadas;
    private TextEdit _txtCajasEntregadas;
    private LabelControl _lblProductor;
    private TextEdit _txtProductor;
    private LabelControl _lblBeneficiario;
    private TextEdit _txtBeneficiario;
    private LabelControl _lblTipoCorte;
    private TextEdit _txtTipoCorte;
    private LabelControl _lblTipoPago;
    private TextEdit _txtTipoPago;
    private LabelControl _lblCostoKg;
    private TextEdit _txtCostoKg;
    private LabelControl _lblJefeCuadrilla;
    private TextEdit _txtJefeCuadrilla;
    private LabelControl _lblTransportista;
    private TextEdit _txtTransportista;
    private LabelControl _lblFloracion;
    private TextEdit _txtFloracion;
    private LabelControl _lblMunicipio;
    private TextEdit _txtMunicipio;
    private LabelControl _lblPoblacion;
    private TextEdit _txtPoblacion;

    private LabelControl _lblSeccionIncidencia;
    private LabelControl _lblSupervisorHuerta;
    private LookUpEdit _cmbSupervisorHuerta;
    private LabelControl _lblHoraLlegada;
    private TimeEdit _timeHoraLlegada;
    private LabelControl _lblOdcSapCosecha;
    private TextEdit _txtOdcSapCosecha;
    private LabelControl _lblOdcSapFlete;
    private TextEdit _txtOdcSapFlete;
    private LabelControl _lblNumeroTelefono;
    private TextEdit _txtNumeroTelefono;
    private LabelControl _lblPlacas;
    private TextEdit _txtPlacas;
    private LabelControl _lblBascula;
    private TextEdit _txtBascula;
    private LabelControl _lblCajasCosechadas;
    private SpinEdit _spnCajasCosechadas;
    private LabelControl _lblPuntoReunion;
    private TextEdit _txtPuntoReunion;
    private LabelControl _lblCajaPorCuadrilla;
    private TextEdit _txtCajaPorCuadrilla;

    private LabelControl _lblObservaciones;
    private MemoEdit _memoObservaciones;
    private LabelControl _lblIncidencias;
    private MemoEdit _memoIncidencias;
    private LabelControl _lblAjuste;
    private MemoEdit _memoAjuste;

    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncidenciaEditarForm));
        _lblSeccionOrdenCorte = new LabelControl();
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _lblFecha = new LabelControl();
        _txtFecha = new TextEdit();
        _lblAcopiador = new LabelControl();
        _txtAcopiador = new TextEdit();
        _lblRegistroSagarpa = new LabelControl();
        _txtRegistroSagarpa = new TextEdit();
        _lblHuerta = new LabelControl();
        _txtHuerta = new TextEdit();
        _lblCajasEntregadas = new LabelControl();
        _txtCajasEntregadas = new TextEdit();
        _lblProductor = new LabelControl();
        _txtProductor = new TextEdit();
        _lblBeneficiario = new LabelControl();
        _txtBeneficiario = new TextEdit();
        _lblTipoCorte = new LabelControl();
        _txtTipoCorte = new TextEdit();
        _lblTipoPago = new LabelControl();
        _txtTipoPago = new TextEdit();
        _lblCostoKg = new LabelControl();
        _txtCostoKg = new TextEdit();
        _lblJefeCuadrilla = new LabelControl();
        _txtJefeCuadrilla = new TextEdit();
        _lblTransportista = new LabelControl();
        _txtTransportista = new TextEdit();
        _lblFloracion = new LabelControl();
        _txtFloracion = new TextEdit();
        _lblMunicipio = new LabelControl();
        _txtMunicipio = new TextEdit();
        _lblPoblacion = new LabelControl();
        _txtPoblacion = new TextEdit();

        _lblSeccionIncidencia = new LabelControl();
        _lblSupervisorHuerta = new LabelControl();
        _cmbSupervisorHuerta = new LookUpEdit();
        _lblHoraLlegada = new LabelControl();
        _timeHoraLlegada = new TimeEdit();
        _lblOdcSapCosecha = new LabelControl();
        _txtOdcSapCosecha = new TextEdit();
        _lblOdcSapFlete = new LabelControl();
        _txtOdcSapFlete = new TextEdit();
        _lblNumeroTelefono = new LabelControl();
        _txtNumeroTelefono = new TextEdit();
        _lblPlacas = new LabelControl();
        _txtPlacas = new TextEdit();
        _lblBascula = new LabelControl();
        _txtBascula = new TextEdit();
        _lblCajasCosechadas = new LabelControl();
        _spnCajasCosechadas = new SpinEdit();
        _lblPuntoReunion = new LabelControl();
        _txtPuntoReunion = new TextEdit();
        _lblCajaPorCuadrilla = new LabelControl();
        _txtCajaPorCuadrilla = new TextEdit();

        _lblObservaciones = new LabelControl();
        _memoObservaciones = new MemoEdit();
        _lblIncidencias = new LabelControl();
        _memoIncidencias = new MemoEdit();
        _lblAjuste = new LabelControl();
        _memoAjuste = new MemoEdit();

        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();

        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFecha.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtAcopiador.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtHuerta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCajasEntregadas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtBeneficiario.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtTipoCorte.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtTipoPago.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCostoKg.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtJefeCuadrilla.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtTransportista.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFloracion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtMunicipio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPoblacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbSupervisorHuerta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_timeHoraLlegada.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtOdcSapCosecha.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtOdcSapFlete.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroTelefono.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPlacas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtBascula.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasCosechadas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPuntoReunion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCajaPorCuadrilla.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_memoObservaciones.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_memoIncidencias.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_memoAjuste.Properties).BeginInit();
        SuspendLayout();
        //
        // Sección Orden de Corte (solo lectura) — cuadrícula de 2 columnas: etiqueta 150px + edit 230px,
        // columna A en X=20, columna B en X=430, fila cada 30px.
        //
        _lblSeccionOrdenCorte.Location = new Point(20, 15);
        _lblSeccionOrdenCorte.Name = "_lblSeccionOrdenCorte";
        _lblSeccionOrdenCorte.Size = new Size(320, 13);
        _lblSeccionOrdenCorte.Text = "Datos de la Orden de Corte (solo lectura)";

        _lblFolio.Location = new Point(20, 49);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(150, 13);
        _lblFolio.Text = "Folio";
        _txtFolio.Location = new Point(180, 45);
        _txtFolio.Name = "_txtFolio";
        _txtFolio.Properties.ReadOnly = true;
        _txtFolio.Size = new Size(230, 20);
        _txtFolio.TabStop = false;

        _lblFecha.Location = new Point(430, 49);
        _lblFecha.Name = "_lblFecha";
        _lblFecha.Size = new Size(150, 13);
        _lblFecha.Text = "Fecha";
        _txtFecha.Location = new Point(590, 45);
        _txtFecha.Name = "_txtFecha";
        _txtFecha.Properties.ReadOnly = true;
        _txtFecha.Size = new Size(230, 20);
        _txtFecha.TabStop = false;

        _lblAcopiador.Location = new Point(20, 79);
        _lblAcopiador.Name = "_lblAcopiador";
        _lblAcopiador.Size = new Size(150, 13);
        _lblAcopiador.Text = "Acopiador";
        _txtAcopiador.Location = new Point(180, 75);
        _txtAcopiador.Name = "_txtAcopiador";
        _txtAcopiador.Properties.ReadOnly = true;
        _txtAcopiador.Size = new Size(230, 20);
        _txtAcopiador.TabStop = false;

        _lblRegistroSagarpa.Location = new Point(430, 79);
        _lblRegistroSagarpa.Name = "_lblRegistroSagarpa";
        _lblRegistroSagarpa.Size = new Size(150, 13);
        _lblRegistroSagarpa.Text = "Registro Sagarpa";
        _txtRegistroSagarpa.Location = new Point(590, 75);
        _txtRegistroSagarpa.Name = "_txtRegistroSagarpa";
        _txtRegistroSagarpa.Properties.ReadOnly = true;
        _txtRegistroSagarpa.Size = new Size(230, 20);
        _txtRegistroSagarpa.TabStop = false;

        _lblHuerta.Location = new Point(20, 109);
        _lblHuerta.Name = "_lblHuerta";
        _lblHuerta.Size = new Size(150, 13);
        _lblHuerta.Text = "Huerta";
        _txtHuerta.Location = new Point(180, 105);
        _txtHuerta.Name = "_txtHuerta";
        _txtHuerta.Properties.ReadOnly = true;
        _txtHuerta.Size = new Size(230, 20);
        _txtHuerta.TabStop = false;

        _lblCajasEntregadas.Location = new Point(430, 109);
        _lblCajasEntregadas.Name = "_lblCajasEntregadas";
        _lblCajasEntregadas.Size = new Size(150, 13);
        _lblCajasEntregadas.Text = "Cajas Entregadas";
        _txtCajasEntregadas.Location = new Point(590, 105);
        _txtCajasEntregadas.Name = "_txtCajasEntregadas";
        _txtCajasEntregadas.Properties.ReadOnly = true;
        _txtCajasEntregadas.Size = new Size(230, 20);
        _txtCajasEntregadas.TabStop = false;

        _lblProductor.Location = new Point(20, 139);
        _lblProductor.Name = "_lblProductor";
        _lblProductor.Size = new Size(150, 13);
        _lblProductor.Text = "Productor";
        _txtProductor.Location = new Point(180, 135);
        _txtProductor.Name = "_txtProductor";
        _txtProductor.Properties.ReadOnly = true;
        _txtProductor.Size = new Size(230, 20);
        _txtProductor.TabStop = false;

        _lblBeneficiario.Location = new Point(430, 139);
        _lblBeneficiario.Name = "_lblBeneficiario";
        _lblBeneficiario.Size = new Size(150, 13);
        _lblBeneficiario.Text = "Beneficiario";
        _txtBeneficiario.Location = new Point(590, 135);
        _txtBeneficiario.Name = "_txtBeneficiario";
        _txtBeneficiario.Properties.ReadOnly = true;
        _txtBeneficiario.Size = new Size(230, 20);
        _txtBeneficiario.TabStop = false;

        _lblTipoCorte.Location = new Point(20, 169);
        _lblTipoCorte.Name = "_lblTipoCorte";
        _lblTipoCorte.Size = new Size(150, 13);
        _lblTipoCorte.Text = "Tipo de Corte";
        _txtTipoCorte.Location = new Point(180, 165);
        _txtTipoCorte.Name = "_txtTipoCorte";
        _txtTipoCorte.Properties.ReadOnly = true;
        _txtTipoCorte.Size = new Size(230, 20);
        _txtTipoCorte.TabStop = false;

        _lblTipoPago.Location = new Point(430, 169);
        _lblTipoPago.Name = "_lblTipoPago";
        _lblTipoPago.Size = new Size(150, 13);
        _lblTipoPago.Text = "Tipo de Pago";
        _txtTipoPago.Location = new Point(590, 165);
        _txtTipoPago.Name = "_txtTipoPago";
        _txtTipoPago.Properties.ReadOnly = true;
        _txtTipoPago.Size = new Size(230, 20);
        _txtTipoPago.TabStop = false;

        _lblCostoKg.Location = new Point(20, 199);
        _lblCostoKg.Name = "_lblCostoKg";
        _lblCostoKg.Size = new Size(150, 13);
        _lblCostoKg.Text = "Precio (Costo/Kg)";
        _txtCostoKg.Location = new Point(180, 195);
        _txtCostoKg.Name = "_txtCostoKg";
        _txtCostoKg.Properties.ReadOnly = true;
        _txtCostoKg.Size = new Size(230, 20);
        _txtCostoKg.TabStop = false;

        _lblJefeCuadrilla.Location = new Point(430, 199);
        _lblJefeCuadrilla.Name = "_lblJefeCuadrilla";
        _lblJefeCuadrilla.Size = new Size(150, 13);
        _lblJefeCuadrilla.Text = "Jefe de Cuadrilla";
        _txtJefeCuadrilla.Location = new Point(590, 195);
        _txtJefeCuadrilla.Name = "_txtJefeCuadrilla";
        _txtJefeCuadrilla.Properties.ReadOnly = true;
        _txtJefeCuadrilla.Size = new Size(230, 20);
        _txtJefeCuadrilla.TabStop = false;

        _lblTransportista.Location = new Point(20, 229);
        _lblTransportista.Name = "_lblTransportista";
        _lblTransportista.Size = new Size(150, 13);
        _lblTransportista.Text = "Transportista";
        _txtTransportista.Location = new Point(180, 225);
        _txtTransportista.Name = "_txtTransportista";
        _txtTransportista.Properties.ReadOnly = true;
        _txtTransportista.Size = new Size(230, 20);
        _txtTransportista.TabStop = false;

        _lblFloracion.Location = new Point(430, 229);
        _lblFloracion.Name = "_lblFloracion";
        _lblFloracion.Size = new Size(150, 13);
        _lblFloracion.Text = "Floración";
        _txtFloracion.Location = new Point(590, 225);
        _txtFloracion.Name = "_txtFloracion";
        _txtFloracion.Properties.ReadOnly = true;
        _txtFloracion.Size = new Size(230, 20);
        _txtFloracion.TabStop = false;

        _lblMunicipio.Location = new Point(20, 259);
        _lblMunicipio.Name = "_lblMunicipio";
        _lblMunicipio.Size = new Size(150, 13);
        _lblMunicipio.Text = "Municipio";
        _txtMunicipio.Location = new Point(180, 255);
        _txtMunicipio.Name = "_txtMunicipio";
        _txtMunicipio.Properties.ReadOnly = true;
        _txtMunicipio.Size = new Size(230, 20);
        _txtMunicipio.TabStop = false;

        _lblPoblacion.Location = new Point(430, 259);
        _lblPoblacion.Name = "_lblPoblacion";
        _lblPoblacion.Size = new Size(150, 13);
        _lblPoblacion.Text = "Localidad";
        _txtPoblacion.Location = new Point(590, 255);
        _txtPoblacion.Name = "_txtPoblacion";
        _txtPoblacion.Properties.ReadOnly = true;
        _txtPoblacion.Size = new Size(230, 20);
        _txtPoblacion.TabStop = false;
        //
        // Sección Incidencia (captura) — misma cuadrícula de 2 columnas.
        //
        _lblSeccionIncidencia.Location = new Point(20, 299);
        _lblSeccionIncidencia.Name = "_lblSeccionIncidencia";
        _lblSeccionIncidencia.Size = new Size(320, 13);
        _lblSeccionIncidencia.Text = "Captura de Incidencia";

        _lblSupervisorHuerta.Location = new Point(20, 333);
        _lblSupervisorHuerta.Name = "_lblSupervisorHuerta";
        _lblSupervisorHuerta.Size = new Size(150, 13);
        _lblSupervisorHuerta.Text = "Supervisor de Huerta";

        _cmbSupervisorHuerta.Location = new Point(180, 329);
        _cmbSupervisorHuerta.Name = "_cmbSupervisorHuerta";
        _cmbSupervisorHuerta.Properties.NullText = "Seleccionar";
        _cmbSupervisorHuerta.Properties.SearchMode = SearchMode.AutoFilter;
        _cmbSupervisorHuerta.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbSupervisorHuerta.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbSupervisorHuerta.Properties.Columns.AddRange(new LookUpColumnInfo[] { new LookUpColumnInfo("Nombre", 220, "Nombre") });
        _cmbSupervisorHuerta.Properties.PopupWidth = 260;
        _cmbSupervisorHuerta.Properties.ValueMember = "Id";
        _cmbSupervisorHuerta.Properties.DisplayMember = "Nombre";
        _cmbSupervisorHuerta.Size = new Size(230, 20);
        _cmbSupervisorHuerta.ButtonClick += CmbSupervisorHuerta_ButtonClick;

        _lblHoraLlegada.Location = new Point(430, 333);
        _lblHoraLlegada.Name = "_lblHoraLlegada";
        _lblHoraLlegada.Size = new Size(150, 13);
        _lblHoraLlegada.Text = "Hora de Llegada a Huerta";

        _timeHoraLlegada.EditValue = null;
        _timeHoraLlegada.Location = new Point(590, 329);
        _timeHoraLlegada.Name = "_timeHoraLlegada";
        _timeHoraLlegada.Properties.Mask.EditMask = "HH:mm";
        _timeHoraLlegada.Properties.Mask.UseMaskAsDisplayFormat = true;
        _timeHoraLlegada.Size = new Size(230, 20);

        _lblOdcSapCosecha.Location = new Point(20, 363);
        _lblOdcSapCosecha.Name = "_lblOdcSapCosecha";
        _lblOdcSapCosecha.Size = new Size(150, 13);
        _lblOdcSapCosecha.Text = "No. ODC SAP Cosecha";
        _txtOdcSapCosecha.Location = new Point(180, 359);
        _txtOdcSapCosecha.Name = "_txtOdcSapCosecha";
        _txtOdcSapCosecha.Size = new Size(230, 20);

        _lblOdcSapFlete.Location = new Point(430, 363);
        _lblOdcSapFlete.Name = "_lblOdcSapFlete";
        _lblOdcSapFlete.Size = new Size(150, 13);
        _lblOdcSapFlete.Text = "No. ODC SAP Flete";
        _txtOdcSapFlete.Location = new Point(590, 359);
        _txtOdcSapFlete.Name = "_txtOdcSapFlete";
        _txtOdcSapFlete.Size = new Size(230, 20);

        _lblNumeroTelefono.Location = new Point(20, 393);
        _lblNumeroTelefono.Name = "_lblNumeroTelefono";
        _lblNumeroTelefono.Size = new Size(150, 13);
        _lblNumeroTelefono.Text = "No. de Teléfono";
        _txtNumeroTelefono.Location = new Point(180, 389);
        _txtNumeroTelefono.Name = "_txtNumeroTelefono";
        _txtNumeroTelefono.Size = new Size(230, 20);

        _lblPlacas.Location = new Point(430, 393);
        _lblPlacas.Name = "_lblPlacas";
        _lblPlacas.Size = new Size(150, 13);
        _lblPlacas.Text = "Placas";
        _txtPlacas.Location = new Point(590, 389);
        _txtPlacas.Name = "_txtPlacas";
        _txtPlacas.Size = new Size(230, 20);

        _lblBascula.Location = new Point(20, 423);
        _lblBascula.Name = "_lblBascula";
        _lblBascula.Size = new Size(150, 13);
        _lblBascula.Text = "Báscula";
        _txtBascula.Location = new Point(180, 419);
        _txtBascula.Name = "_txtBascula";
        _txtBascula.Size = new Size(230, 20);

        _lblCajasCosechadas.Location = new Point(430, 423);
        _lblCajasCosechadas.Name = "_lblCajasCosechadas";
        _lblCajasCosechadas.Size = new Size(150, 13);
        _lblCajasCosechadas.Text = "Cajas Cosechadas";

        _spnCajasCosechadas.EditValue = null;
        _spnCajasCosechadas.Location = new Point(590, 419);
        _spnCajasCosechadas.Name = "_spnCajasCosechadas";
        _spnCajasCosechadas.Properties.IsFloatValue = false;
        _spnCajasCosechadas.Properties.Mask.EditMask = "N00";
        _spnCajasCosechadas.Properties.MaxValue = 9999;
        _spnCajasCosechadas.Properties.MinValue = 0;
        _spnCajasCosechadas.Size = new Size(230, 20);

        _lblPuntoReunion.Location = new Point(20, 453);
        _lblPuntoReunion.Name = "_lblPuntoReunion";
        _lblPuntoReunion.Size = new Size(150, 13);
        _lblPuntoReunion.Text = "Punto de Reunión";
        _txtPuntoReunion.Location = new Point(180, 449);
        _txtPuntoReunion.Name = "_txtPuntoReunion";
        _txtPuntoReunion.Size = new Size(230, 20);

        _lblCajaPorCuadrilla.Location = new Point(430, 453);
        _lblCajaPorCuadrilla.Name = "_lblCajaPorCuadrilla";
        _lblCajaPorCuadrilla.Size = new Size(150, 13);
        _lblCajaPorCuadrilla.Text = "Caja por Cuadrilla";
        _txtCajaPorCuadrilla.Location = new Point(590, 449);
        _txtCajaPorCuadrilla.Name = "_txtCajaPorCuadrilla";
        _txtCajaPorCuadrilla.Size = new Size(230, 20);
        //
        // Observaciones / Incidencias / Ajuste (memos, ancho completo del contenido: X=20..820)
        //
        _lblObservaciones.Location = new Point(20, 487);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(150, 13);
        _lblObservaciones.Text = "Observaciones";

        _memoObservaciones.Location = new Point(20, 503);
        _memoObservaciones.Name = "_memoObservaciones";
        _memoObservaciones.Size = new Size(800, 50);

        _lblIncidencias.Location = new Point(20, 561);
        _lblIncidencias.Name = "_lblIncidencias";
        _lblIncidencias.Size = new Size(150, 13);
        _lblIncidencias.Text = "Incidencias";

        _memoIncidencias.Location = new Point(20, 577);
        _memoIncidencias.Name = "_memoIncidencias";
        _memoIncidencias.Size = new Size(800, 50);

        _lblAjuste.Location = new Point(20, 635);
        _lblAjuste.Name = "_lblAjuste";
        _lblAjuste.Size = new Size(150, 13);
        _lblAjuste.Text = "Ajuste";

        _memoAjuste.Location = new Point(20, 651);
        _memoAjuste.Name = "_memoAjuste";
        _memoAjuste.Size = new Size(800, 50);
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(650, 711);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(740, 711);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // IncidenciaEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(840, 760);
        Controls.Add(_lblSeccionOrdenCorte);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_lblFecha);
        Controls.Add(_txtFecha);
        Controls.Add(_lblAcopiador);
        Controls.Add(_txtAcopiador);
        Controls.Add(_lblRegistroSagarpa);
        Controls.Add(_txtRegistroSagarpa);
        Controls.Add(_lblHuerta);
        Controls.Add(_txtHuerta);
        Controls.Add(_lblCajasEntregadas);
        Controls.Add(_txtCajasEntregadas);
        Controls.Add(_lblProductor);
        Controls.Add(_txtProductor);
        Controls.Add(_lblBeneficiario);
        Controls.Add(_txtBeneficiario);
        Controls.Add(_lblTipoCorte);
        Controls.Add(_txtTipoCorte);
        Controls.Add(_lblTipoPago);
        Controls.Add(_txtTipoPago);
        Controls.Add(_lblCostoKg);
        Controls.Add(_txtCostoKg);
        Controls.Add(_lblJefeCuadrilla);
        Controls.Add(_txtJefeCuadrilla);
        Controls.Add(_lblTransportista);
        Controls.Add(_txtTransportista);
        Controls.Add(_lblFloracion);
        Controls.Add(_txtFloracion);
        Controls.Add(_lblMunicipio);
        Controls.Add(_txtMunicipio);
        Controls.Add(_lblPoblacion);
        Controls.Add(_txtPoblacion);
        Controls.Add(_lblSeccionIncidencia);
        Controls.Add(_lblSupervisorHuerta);
        Controls.Add(_cmbSupervisorHuerta);
        Controls.Add(_lblHoraLlegada);
        Controls.Add(_timeHoraLlegada);
        Controls.Add(_lblOdcSapCosecha);
        Controls.Add(_txtOdcSapCosecha);
        Controls.Add(_lblOdcSapFlete);
        Controls.Add(_txtOdcSapFlete);
        Controls.Add(_lblNumeroTelefono);
        Controls.Add(_txtNumeroTelefono);
        Controls.Add(_lblPlacas);
        Controls.Add(_txtPlacas);
        Controls.Add(_lblBascula);
        Controls.Add(_txtBascula);
        Controls.Add(_lblCajasCosechadas);
        Controls.Add(_spnCajasCosechadas);
        Controls.Add(_lblPuntoReunion);
        Controls.Add(_txtPuntoReunion);
        Controls.Add(_lblCajaPorCuadrilla);
        Controls.Add(_txtCajaPorCuadrilla);
        Controls.Add(_lblObservaciones);
        Controls.Add(_memoObservaciones);
        Controls.Add(_lblIncidencias);
        Controls.Add(_memoIncidencias);
        Controls.Add(_lblAjuste);
        Controls.Add(_memoAjuste);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "IncidenciaEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Incidencia";
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtAcopiador.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtHuerta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCajasEntregadas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtBeneficiario.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtTipoCorte.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtTipoPago.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCostoKg.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtJefeCuadrilla.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtTransportista.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFloracion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtMunicipio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPoblacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbSupervisorHuerta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_timeHoraLlegada.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtOdcSapCosecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtOdcSapFlete.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroTelefono.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPlacas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtBascula.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasCosechadas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPuntoReunion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCajaPorCuadrilla.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_memoObservaciones.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_memoIncidencias.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_memoAjuste.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
