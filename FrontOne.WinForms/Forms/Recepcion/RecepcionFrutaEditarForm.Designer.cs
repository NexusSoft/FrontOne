using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Recepcion;

partial class RecepcionFrutaEditarForm
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
    private LabelControl _lblChofer;
    private TextEdit _txtChofer;
    private LabelControl _lblPlacas;
    private TextEdit _txtPlacas;
    private LabelControl _lblObservaciones;
    private TextEdit _txtObservaciones;
    private LabelControl _lblNoLote;
    private TextEdit _txtNoLote;
    private CheckEdit _chkDescargaCompleta;
    private GroupControl _grpBascula;
    private LabelControl _lblNumeroTicket;
    private TextEdit _txtNumeroTicket;
    private LabelControl _lblCoprefBico;
    private TextEdit _txtCoprefBico;
    private LabelControl _lblPesoBruto;
    private SpinEdit _spnPesoBruto;
    private LabelControl _lblPesoTara;
    private SpinEdit _spnPesoTara;
    private LabelControl _lblTaraCajas;
    private SpinEdit _spnTaraCajas;
    private LabelControl _lblPesoMuestra;
    private SpinEdit _spnPesoMuestra;
    private LabelControl _lblPesoNeto;
    private SpinEdit _spnPesoNeto;
    private LabelControl _lblPesoProductor;
    private SpinEdit _spnPesoProductor;
    private SimpleButton _btnTicketPesada;
    private SimpleButton _btnQuitarTicketPesada;
    private LabelControl _lblTicketPesadaArchivo;
    private GroupControl _grpCajas;
    private LabelControl _lblCajasPorEntregar;
    private SpinEdit _spnCajasPorEntregar;
    private LabelControl _lblCajasEntregadas;
    private SpinEdit _spnCajasEntregadas;
    private LabelControl _lblCajasCortadas;
    private SpinEdit _spnCajasCortadas;
    private LabelControl _lblCajasRecibidasVacias;
    private SpinEdit _spnCajasRecibidasVacias;
    private LabelControl _lblCajasDiferencia;
    private SpinEdit _spnCajasDiferencia;
    private LabelControl _lblCajasPerdidas;
    private SpinEdit _spnCajasPerdidas;
    private LabelControl _lblPorcentajeMateriaSeca;
    private SpinEdit _spnPorcentajeMateriaSeca;
    private LabelControl _lblDetalle;
    private GridControl _gridDetalle;
    private GridView _gridViewDetalle;
    private SimpleButton _btnDetalleNuevo;
    private SimpleButton _btnDetalleBorrar;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecepcionFrutaEditarForm));
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _lblFecha = new LabelControl();
        _dtFecha = new DateEdit();
        _lblChofer = new LabelControl();
        _txtChofer = new TextEdit();
        _lblPlacas = new LabelControl();
        _txtPlacas = new TextEdit();
        _lblObservaciones = new LabelControl();
        _txtObservaciones = new TextEdit();
        _lblNoLote = new LabelControl();
        _txtNoLote = new TextEdit();
        _chkDescargaCompleta = new CheckEdit();
        _grpBascula = new GroupControl();
        _lblNumeroTicket = new LabelControl();
        _txtNumeroTicket = new TextEdit();
        _lblCoprefBico = new LabelControl();
        _txtCoprefBico = new TextEdit();
        _lblPesoBruto = new LabelControl();
        _spnPesoBruto = new SpinEdit();
        _lblPesoTara = new LabelControl();
        _spnPesoTara = new SpinEdit();
        _lblTaraCajas = new LabelControl();
        _spnTaraCajas = new SpinEdit();
        _lblPesoMuestra = new LabelControl();
        _spnPesoMuestra = new SpinEdit();
        _lblPesoNeto = new LabelControl();
        _spnPesoNeto = new SpinEdit();
        _lblPesoProductor = new LabelControl();
        _spnPesoProductor = new SpinEdit();
        _btnTicketPesada = new SimpleButton();
        _btnQuitarTicketPesada = new SimpleButton();
        _lblTicketPesadaArchivo = new LabelControl();
        _grpCajas = new GroupControl();
        _lblCajasPorEntregar = new LabelControl();
        _spnCajasPorEntregar = new SpinEdit();
        _lblCajasEntregadas = new LabelControl();
        _spnCajasEntregadas = new SpinEdit();
        _lblCajasCortadas = new LabelControl();
        _spnCajasCortadas = new SpinEdit();
        _lblCajasRecibidasVacias = new LabelControl();
        _spnCajasRecibidasVacias = new SpinEdit();
        _lblCajasDiferencia = new LabelControl();
        _spnCajasDiferencia = new SpinEdit();
        _lblCajasPerdidas = new LabelControl();
        _spnCajasPerdidas = new SpinEdit();
        _lblPorcentajeMateriaSeca = new LabelControl();
        _spnPorcentajeMateriaSeca = new SpinEdit();
        _lblDetalle = new LabelControl();
        _gridDetalle = new GridControl();
        _gridViewDetalle = new GridView();
        _btnDetalleNuevo = new SimpleButton();
        _btnDetalleBorrar = new SimpleButton();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtChofer.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPlacas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNoLote.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkDescargaCompleta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpBascula).BeginInit();
        _grpBascula.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroTicket.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCoprefBico.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoBruto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoTara.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnTaraCajas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoMuestra.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoNeto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpCajas).BeginInit();
        _grpCajas.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_spnCajasPorEntregar.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasEntregadas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasCortadas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasRecibidasVacias.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasDiferencia.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasPerdidas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMateriaSeca.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridDetalle).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewDetalle).BeginInit();
        SuspendLayout();
        //
        // _lblFolio
        //
        _lblFolio.Location = new Point(15, 18);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(27, 13);
        _lblFolio.Text = "Folio:";
        //
        // _txtFolio
        //
        _txtFolio.Location = new Point(110, 15);
        _txtFolio.Name = "_txtFolio";
        _txtFolio.Properties.ReadOnly = true;
        _txtFolio.Size = new Size(100, 20);
        _txtFolio.TabIndex = 0;
        //
        // _lblFecha
        //
        _lblFecha.Location = new Point(230, 18);
        _lblFecha.Name = "_lblFecha";
        _lblFecha.Size = new Size(35, 13);
        _lblFecha.Text = "Fecha:";
        //
        // _dtFecha
        //
        _dtFecha.EditValue = null;
        _dtFecha.Location = new Point(280, 15);
        _dtFecha.Name = "_dtFecha";
        _dtFecha.Size = new Size(120, 20);
        _dtFecha.TabIndex = 1;
        //
        // _lblChofer
        //
        _lblChofer.Location = new Point(15, 44);
        _lblChofer.Name = "_lblChofer";
        _lblChofer.Size = new Size(38, 13);
        _lblChofer.Text = "Chofer:";
        //
        // _txtChofer
        //
        _txtChofer.Location = new Point(110, 41);
        _txtChofer.Name = "_txtChofer";
        _txtChofer.Size = new Size(240, 20);
        _txtChofer.TabIndex = 2;
        //
        // _lblPlacas
        //
        _lblPlacas.Location = new Point(360, 44);
        _lblPlacas.Name = "_lblPlacas";
        _lblPlacas.Size = new Size(35, 13);
        _lblPlacas.Text = "Placas:";
        //
        // _txtPlacas
        //
        _txtPlacas.Location = new Point(410, 41);
        _txtPlacas.Name = "_txtPlacas";
        _txtPlacas.Size = new Size(110, 20);
        _txtPlacas.TabIndex = 3;
        //
        // _lblObservaciones
        //
        _lblObservaciones.Location = new Point(15, 70);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(68, 13);
        _lblObservaciones.Text = "Observaciones:";
        //
        // _txtObservaciones
        //
        _txtObservaciones.Location = new Point(110, 67);
        _txtObservaciones.Name = "_txtObservaciones";
        _txtObservaciones.Size = new Size(410, 20);
        _txtObservaciones.TabIndex = 4;
        //
        // _lblNoLote
        //
        _lblNoLote.Location = new Point(15, 96);
        _lblNoLote.Name = "_lblNoLote";
        _lblNoLote.Size = new Size(62, 13);
        _lblNoLote.Text = "No. de Lote:";
        //
        // _txtNoLote
        //
        _txtNoLote.Location = new Point(110, 93);
        _txtNoLote.Name = "_txtNoLote";
        _txtNoLote.Properties.ReadOnly = true;
        _txtNoLote.Size = new Size(110, 20);
        _txtNoLote.TabIndex = 5;
        //
        // _chkDescargaCompleta
        //
        _chkDescargaCompleta.Location = new Point(250, 92);
        _chkDescargaCompleta.Name = "_chkDescargaCompleta";
        _chkDescargaCompleta.Properties.Caption = "¿Descarga Completa?";
        _chkDescargaCompleta.Size = new Size(270, 20);
        _chkDescargaCompleta.TabIndex = 6;
        //
        // _grpBascula
        //
        _grpBascula.Location = new Point(15, 125);
        _grpBascula.Size = new Size(340, 330);
        _grpBascula.Text = "Control de Báscula";
        _grpBascula.Controls.Add(_lblNumeroTicket);
        _grpBascula.Controls.Add(_txtNumeroTicket);
        _grpBascula.Controls.Add(_lblCoprefBico);
        _grpBascula.Controls.Add(_txtCoprefBico);
        _grpBascula.Controls.Add(_lblPesoBruto);
        _grpBascula.Controls.Add(_spnPesoBruto);
        _grpBascula.Controls.Add(_lblPesoTara);
        _grpBascula.Controls.Add(_spnPesoTara);
        _grpBascula.Controls.Add(_lblTaraCajas);
        _grpBascula.Controls.Add(_spnTaraCajas);
        _grpBascula.Controls.Add(_lblPesoMuestra);
        _grpBascula.Controls.Add(_spnPesoMuestra);
        _grpBascula.Controls.Add(_lblPesoNeto);
        _grpBascula.Controls.Add(_spnPesoNeto);
        _grpBascula.Controls.Add(_lblPesoProductor);
        _grpBascula.Controls.Add(_spnPesoProductor);
        _grpBascula.Controls.Add(_btnTicketPesada);
        _grpBascula.Controls.Add(_btnQuitarTicketPesada);
        _grpBascula.Controls.Add(_lblTicketPesadaArchivo);
        //
        // _lblNumeroTicket
        //
        _lblNumeroTicket.Location = new Point(10, 28);
        _lblNumeroTicket.Name = "_lblNumeroTicket";
        _lblNumeroTicket.Size = new Size(60, 13);
        _lblNumeroTicket.Text = "No. Ticket:";
        //
        // _txtNumeroTicket
        //
        _txtNumeroTicket.Location = new Point(110, 25);
        _txtNumeroTicket.Name = "_txtNumeroTicket";
        _txtNumeroTicket.Size = new Size(150, 20);
        _txtNumeroTicket.TabIndex = 0;
        //
        // _lblCoprefBico
        //
        _lblCoprefBico.Location = new Point(10, 58);
        _lblCoprefBico.Name = "_lblCoprefBico";
        _lblCoprefBico.Size = new Size(70, 13);
        _lblCoprefBico.Text = "COPREF/BICO:";
        //
        // _txtCoprefBico
        //
        _txtCoprefBico.Location = new Point(110, 55);
        _txtCoprefBico.Name = "_txtCoprefBico";
        _txtCoprefBico.Size = new Size(150, 20);
        _txtCoprefBico.TabIndex = 1;
        //
        // _lblPesoBruto
        //
        _lblPesoBruto.Location = new Point(10, 88);
        _lblPesoBruto.Name = "_lblPesoBruto";
        _lblPesoBruto.Size = new Size(60, 13);
        _lblPesoBruto.Text = "Peso Bruto:";
        //
        // _spnPesoBruto
        //
        _spnPesoBruto.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoBruto.Location = new Point(110, 85);
        _spnPesoBruto.Name = "_spnPesoBruto";
        _spnPesoBruto.Properties.Mask.EditMask = "N02";
        _spnPesoBruto.Size = new Size(120, 20);
        _spnPesoBruto.TabIndex = 2;
        _spnPesoBruto.EditValueChanged += CamposBascula_EditValueChanged;
        //
        // _lblPesoTara
        //
        _lblPesoTara.Location = new Point(10, 118);
        _lblPesoTara.Name = "_lblPesoTara";
        _lblPesoTara.Size = new Size(55, 13);
        _lblPesoTara.Text = "Peso Tara:";
        //
        // _spnPesoTara
        //
        _spnPesoTara.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoTara.Location = new Point(110, 115);
        _spnPesoTara.Name = "_spnPesoTara";
        _spnPesoTara.Properties.Mask.EditMask = "N02";
        _spnPesoTara.Size = new Size(120, 20);
        _spnPesoTara.TabIndex = 3;
        _spnPesoTara.EditValueChanged += CamposBascula_EditValueChanged;
        //
        // _lblTaraCajas
        //
        _lblTaraCajas.Location = new Point(10, 148);
        _lblTaraCajas.Name = "_lblTaraCajas";
        _lblTaraCajas.Size = new Size(60, 13);
        _lblTaraCajas.Text = "Tara Cajas:";
        //
        // _spnTaraCajas
        //
        _spnTaraCajas.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnTaraCajas.Location = new Point(110, 145);
        _spnTaraCajas.Name = "_spnTaraCajas";
        _spnTaraCajas.Properties.Mask.EditMask = "N02";
        _spnTaraCajas.Size = new Size(120, 20);
        _spnTaraCajas.TabIndex = 4;
        _spnTaraCajas.EditValueChanged += CamposBascula_EditValueChanged;
        //
        // _lblPesoMuestra
        //
        _lblPesoMuestra.Location = new Point(10, 178);
        _lblPesoMuestra.Name = "_lblPesoMuestra";
        _lblPesoMuestra.Size = new Size(75, 13);
        _lblPesoMuestra.Text = "Peso Muestra:";
        //
        // _spnPesoMuestra
        //
        _spnPesoMuestra.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoMuestra.Location = new Point(110, 175);
        _spnPesoMuestra.Name = "_spnPesoMuestra";
        _spnPesoMuestra.Properties.Mask.EditMask = "N02";
        _spnPesoMuestra.Size = new Size(120, 20);
        _spnPesoMuestra.TabIndex = 5;
        _spnPesoMuestra.EditValueChanged += CamposBascula_EditValueChanged;
        //
        // _lblPesoNeto
        //
        _lblPesoNeto.Location = new Point(10, 208);
        _lblPesoNeto.Name = "_lblPesoNeto";
        _lblPesoNeto.Size = new Size(58, 13);
        _lblPesoNeto.Text = "Peso Neto:";
        //
        // _spnPesoNeto
        //
        _spnPesoNeto.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoNeto.Location = new Point(110, 205);
        _spnPesoNeto.Name = "_spnPesoNeto";
        _spnPesoNeto.Properties.Mask.EditMask = "N02";
        _spnPesoNeto.Properties.ReadOnly = true;
        _spnPesoNeto.Size = new Size(120, 20);
        _spnPesoNeto.TabIndex = 6;
        //
        // _lblPesoProductor
        //
        _lblPesoProductor.Location = new Point(10, 238);
        _lblPesoProductor.Name = "_lblPesoProductor";
        _lblPesoProductor.Size = new Size(80, 13);
        _lblPesoProductor.Text = "Peso Productor:";
        //
        // _spnPesoProductor
        //
        _spnPesoProductor.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoProductor.Location = new Point(110, 235);
        _spnPesoProductor.Name = "_spnPesoProductor";
        _spnPesoProductor.Properties.Mask.EditMask = "N02";
        _spnPesoProductor.Size = new Size(120, 20);
        _spnPesoProductor.TabIndex = 7;
        //
        // _btnTicketPesada
        //
        _btnTicketPesada.Location = new Point(10, 265);
        _btnTicketPesada.Name = "_btnTicketPesada";
        _btnTicketPesada.Size = new Size(190, 23);
        _btnTicketPesada.TabIndex = 8;
        _btnTicketPesada.Text = "Agregar Ticket de Pesada";
        _btnTicketPesada.Click += BtnTicketPesada_Click;
        //
        // _btnQuitarTicketPesada
        //
        _btnQuitarTicketPesada.Location = new Point(210, 265);
        _btnQuitarTicketPesada.Name = "_btnQuitarTicketPesada";
        _btnQuitarTicketPesada.Size = new Size(100, 23);
        _btnQuitarTicketPesada.TabIndex = 9;
        _btnQuitarTicketPesada.Text = "Quitar";
        _btnQuitarTicketPesada.Visible = false;
        _btnQuitarTicketPesada.Click += BtnQuitarTicketPesada_Click;
        //
        // _lblTicketPesadaArchivo
        //
        _lblTicketPesadaArchivo.Location = new Point(10, 298);
        _lblTicketPesadaArchivo.Name = "_lblTicketPesadaArchivo";
        _lblTicketPesadaArchivo.Size = new Size(90, 13);
        _lblTicketPesadaArchivo.Text = "Sin ticket adjunto";
        _lblTicketPesadaArchivo.Click += LblTicketPesadaArchivo_Click;
        //
        // _grpCajas
        //
        _grpCajas.Location = new Point(365, 125);
        _grpCajas.Size = new Size(230, 225);
        _grpCajas.Text = "Control de Cajas";
        _grpCajas.Controls.Add(_lblCajasPorEntregar);
        _grpCajas.Controls.Add(_spnCajasPorEntregar);
        _grpCajas.Controls.Add(_lblCajasEntregadas);
        _grpCajas.Controls.Add(_spnCajasEntregadas);
        _grpCajas.Controls.Add(_lblCajasCortadas);
        _grpCajas.Controls.Add(_spnCajasCortadas);
        _grpCajas.Controls.Add(_lblCajasRecibidasVacias);
        _grpCajas.Controls.Add(_spnCajasRecibidasVacias);
        _grpCajas.Controls.Add(_lblCajasDiferencia);
        _grpCajas.Controls.Add(_spnCajasDiferencia);
        _grpCajas.Controls.Add(_lblCajasPerdidas);
        _grpCajas.Controls.Add(_spnCajasPerdidas);
        //
        // _lblCajasPorEntregar
        //
        _lblCajasPorEntregar.Location = new Point(10, 28);
        _lblCajasPorEntregar.Name = "_lblCajasPorEntregar";
        _lblCajasPorEntregar.Size = new Size(68, 13);
        _lblCajasPorEntregar.Text = "Por Entregar:";
        //
        // _spnCajasPorEntregar
        //
        _spnCajasPorEntregar.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCajasPorEntregar.Location = new Point(110, 25);
        _spnCajasPorEntregar.Name = "_spnCajasPorEntregar";
        _spnCajasPorEntregar.Properties.Mask.EditMask = "N00";
        _spnCajasPorEntregar.Properties.ReadOnly = true;
        _spnCajasPorEntregar.Size = new Size(90, 20);
        _spnCajasPorEntregar.TabIndex = 0;
        _spnCajasPorEntregar.EditValueChanged += CamposCajas_EditValueChanged;
        //
        // _lblCajasEntregadas
        //
        _lblCajasEntregadas.Location = new Point(10, 58);
        _lblCajasEntregadas.Name = "_lblCajasEntregadas";
        _lblCajasEntregadas.Size = new Size(65, 13);
        _lblCajasEntregadas.Text = "Entregadas:";
        //
        // _spnCajasEntregadas
        //
        _spnCajasEntregadas.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCajasEntregadas.Location = new Point(110, 55);
        _spnCajasEntregadas.Name = "_spnCajasEntregadas";
        _spnCajasEntregadas.Properties.Mask.EditMask = "N00";
        _spnCajasEntregadas.Size = new Size(90, 20);
        _spnCajasEntregadas.TabIndex = 1;
        _spnCajasEntregadas.EditValueChanged += CamposCajas_EditValueChanged;
        //
        // _lblCajasCortadas
        //
        _lblCajasCortadas.Location = new Point(10, 88);
        _lblCajasCortadas.Name = "_lblCajasCortadas";
        _lblCajasCortadas.Size = new Size(53, 13);
        _lblCajasCortadas.Text = "Cortadas:";
        //
        // _spnCajasCortadas
        //
        _spnCajasCortadas.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCajasCortadas.Location = new Point(110, 85);
        _spnCajasCortadas.Name = "_spnCajasCortadas";
        _spnCajasCortadas.Properties.Mask.EditMask = "N00";
        _spnCajasCortadas.Size = new Size(90, 20);
        _spnCajasCortadas.TabIndex = 2;
        _spnCajasCortadas.EditValueChanged += CamposCajas_EditValueChanged;
        //
        // _lblCajasRecibidasVacias
        //
        _lblCajasRecibidasVacias.Location = new Point(10, 118);
        _lblCajasRecibidasVacias.Name = "_lblCajasRecibidasVacias";
        _lblCajasRecibidasVacias.Size = new Size(93, 13);
        _lblCajasRecibidasVacias.Text = "Recibidas Vacías:";
        //
        // _spnCajasRecibidasVacias
        //
        _spnCajasRecibidasVacias.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCajasRecibidasVacias.Location = new Point(110, 115);
        _spnCajasRecibidasVacias.Name = "_spnCajasRecibidasVacias";
        _spnCajasRecibidasVacias.Properties.Mask.EditMask = "N00";
        _spnCajasRecibidasVacias.Size = new Size(90, 20);
        _spnCajasRecibidasVacias.TabIndex = 3;
        _spnCajasRecibidasVacias.EditValueChanged += CamposCajas_EditValueChanged;
        //
        // _lblCajasDiferencia
        //
        _lblCajasDiferencia.Location = new Point(10, 148);
        _lblCajasDiferencia.Name = "_lblCajasDiferencia";
        _lblCajasDiferencia.Size = new Size(56, 13);
        _lblCajasDiferencia.Text = "Diferencia:";
        //
        // _spnCajasDiferencia
        //
        _spnCajasDiferencia.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCajasDiferencia.Location = new Point(110, 145);
        _spnCajasDiferencia.Name = "_spnCajasDiferencia";
        _spnCajasDiferencia.Properties.Mask.EditMask = "N00";
        _spnCajasDiferencia.Properties.ReadOnly = true;
        _spnCajasDiferencia.Size = new Size(90, 20);
        _spnCajasDiferencia.TabIndex = 4;
        //
        // _lblCajasPerdidas
        //
        _lblCajasPerdidas.Location = new Point(10, 178);
        _lblCajasPerdidas.Name = "_lblCajasPerdidas";
        _lblCajasPerdidas.Size = new Size(65, 13);
        _lblCajasPerdidas.Text = "Perdidas:";
        //
        // _spnCajasPerdidas
        //
        _spnCajasPerdidas.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCajasPerdidas.Location = new Point(110, 175);
        _spnCajasPerdidas.Name = "_spnCajasPerdidas";
        _spnCajasPerdidas.Properties.Mask.EditMask = "N00";
        _spnCajasPerdidas.Properties.ReadOnly = true;
        _spnCajasPerdidas.Size = new Size(90, 20);
        _spnCajasPerdidas.TabIndex = 5;
        //
        // _lblPorcentajeMateriaSeca
        //
        _lblPorcentajeMateriaSeca.Location = new Point(365, 365);
        _lblPorcentajeMateriaSeca.Name = "_lblPorcentajeMateriaSeca";
        _lblPorcentajeMateriaSeca.Size = new Size(80, 13);
        _lblPorcentajeMateriaSeca.Text = "% Materia Seca:";
        //
        // _spnPorcentajeMateriaSeca
        //
        _spnPorcentajeMateriaSeca.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPorcentajeMateriaSeca.Location = new Point(455, 362);
        _spnPorcentajeMateriaSeca.Name = "_spnPorcentajeMateriaSeca";
        _spnPorcentajeMateriaSeca.Properties.Mask.EditMask = "N02";
        _spnPorcentajeMateriaSeca.Size = new Size(90, 20);
        _spnPorcentajeMateriaSeca.TabIndex = 8;
        //
        // _lblDetalle
        //
        _lblDetalle.Location = new Point(15, 495);
        _lblDetalle.Name = "_lblDetalle";
        _lblDetalle.Size = new Size(87, 13);
        _lblDetalle.Text = "Órdenes de Corte:";
        //
        // _gridDetalle
        //
        _gridDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _gridDetalle.Location = new Point(15, 513);
        _gridDetalle.MainView = _gridViewDetalle;
        _gridDetalle.Name = "_gridDetalle";
        _gridDetalle.Size = new Size(580, 120);
        _gridDetalle.TabIndex = 9;
        _gridDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewDetalle });
        //
        // _gridViewDetalle
        //
        _gridViewDetalle.GridControl = _gridDetalle;
        _gridViewDetalle.Name = "_gridViewDetalle";
        _gridViewDetalle.OptionsBehavior.Editable = false;
        _gridViewDetalle.OptionsFind.AlwaysVisible = true;
        _gridViewDetalle.OptionsView.ShowGroupPanel = false;
        _gridViewDetalle.OptionsView.ColumnAutoWidth = false;
        //
        // _btnDetalleNuevo
        //
        _btnDetalleNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        _btnDetalleNuevo.Location = new Point(15, 640);
        _btnDetalleNuevo.Name = "_btnDetalleNuevo";
        _btnDetalleNuevo.Size = new Size(85, 23);
        _btnDetalleNuevo.TabIndex = 10;
        _btnDetalleNuevo.Text = "Nuevo";
        _btnDetalleNuevo.Click += BtnDetalleNuevo_Click;
        //
        // _btnDetalleBorrar
        //
        _btnDetalleBorrar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        _btnDetalleBorrar.Location = new Point(105, 640);
        _btnDetalleBorrar.Name = "_btnDetalleBorrar";
        _btnDetalleBorrar.Size = new Size(85, 23);
        _btnDetalleBorrar.TabIndex = 11;
        _btnDetalleBorrar.Text = "Borrar";
        _btnDetalleBorrar.Click += BtnDetalleBorrar_Click;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(430, 680);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 13;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(515, 680);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 14;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // RecepcionFrutaEditarForm
        //
        ClientSize = new Size(610, 715);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_lblFecha);
        Controls.Add(_dtFecha);
        Controls.Add(_lblChofer);
        Controls.Add(_txtChofer);
        Controls.Add(_lblPlacas);
        Controls.Add(_txtPlacas);
        Controls.Add(_lblObservaciones);
        Controls.Add(_txtObservaciones);
        Controls.Add(_lblNoLote);
        Controls.Add(_txtNoLote);
        Controls.Add(_chkDescargaCompleta);
        Controls.Add(_grpBascula);
        Controls.Add(_grpCajas);
        Controls.Add(_lblPorcentajeMateriaSeca);
        Controls.Add(_spnPorcentajeMateriaSeca);
        Controls.Add(_lblDetalle);
        Controls.Add(_gridDetalle);
        Controls.Add(_btnDetalleNuevo);
        Controls.Add(_btnDetalleBorrar);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = true;
        MinimumSize = new Size(630, 754);
        Name = "RecepcionFrutaEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Recepción de Fruta";
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtChofer.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPlacas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNoLote.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkDescargaCompleta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroTicket.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCoprefBico.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoBruto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoTara.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnTaraCajas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoMuestra.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoNeto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpBascula).EndInit();
        _grpBascula.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_spnCajasPorEntregar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasEntregadas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasCortadas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasRecibidasVacias.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasDiferencia.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasPerdidas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpCajas).EndInit();
        _grpCajas.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMateriaSeca.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridDetalle).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewDetalle).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
