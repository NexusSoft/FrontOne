using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Pallets;

partial class PalletEditarForm
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
    private TextEdit _txtFecha;
    private LabelControl _lblHora;
    private TextEdit _txtHora;
    private LabelControl _lblEstatus;
    private TextEdit _txtEstatus;
    private LabelControl _lblLineaProduccion;
    private LookUpEdit _cmbLineaProduccion;
    private CheckEdit _chkEsMixto;
    private LabelControl _lblPorcentajeMateriaSeca;
    private SpinEdit _spnPorcentajeMateriaSeca;
    private LabelControl _lblPesoReal;
    private SpinEdit _spnPesoReal;
    private SimpleButton _btnTomarLectura;
    private LabelControl _lblNoReempaque;
    private TextEdit _txtNoReempaque;
    private SimpleButton _btnBloquear;
    private SimpleButton _btnImprimirEtiquetas;
    private SimpleButton _btnImprimirPapeleta;
    private LabelControl _lblDetalle;
    private GridControl _gridDetalle;
    private GridView _gridViewDetalle;
    private SimpleButton _btnDetalleNuevo;
    private SimpleButton _btnDetalleEditar;
    private SimpleButton _btnDetalleBorrar;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PalletEditarForm));
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _lblFecha = new LabelControl();
        _txtFecha = new TextEdit();
        _lblHora = new LabelControl();
        _txtHora = new TextEdit();
        _lblEstatus = new LabelControl();
        _txtEstatus = new TextEdit();
        _lblLineaProduccion = new LabelControl();
        _cmbLineaProduccion = new LookUpEdit();
        _chkEsMixto = new CheckEdit();
        _lblPorcentajeMateriaSeca = new LabelControl();
        _spnPorcentajeMateriaSeca = new SpinEdit();
        _lblPesoReal = new LabelControl();
        _spnPesoReal = new SpinEdit();
        _btnTomarLectura = new SimpleButton();
        _lblNoReempaque = new LabelControl();
        _txtNoReempaque = new TextEdit();
        _btnBloquear = new SimpleButton();
        _btnImprimirEtiquetas = new SimpleButton();
        _btnImprimirPapeleta = new SimpleButton();
        _lblDetalle = new LabelControl();
        _gridDetalle = new GridControl();
        _gridViewDetalle = new GridView();
        _btnDetalleNuevo = new SimpleButton();
        _btnDetalleEditar = new SimpleButton();
        _btnDetalleBorrar = new SimpleButton();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFecha.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtHora.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtEstatus.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbLineaProduccion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkEsMixto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMateriaSeca.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoReal.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNoReempaque.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridDetalle).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewDetalle).BeginInit();
        SuspendLayout();
        //
        // _lblFolio
        //
        _lblFolio.Location = new Point(15, 18);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(66, 13);
        _lblFolio.Text = "No. de Pallet:";
        //
        // _txtFolio
        //
        _txtFolio.Location = new Point(130, 15);
        _txtFolio.Name = "_txtFolio";
        _txtFolio.Properties.ReadOnly = true;
        _txtFolio.Size = new Size(130, 20);
        _txtFolio.TabIndex = 0;
        //
        // _lblFecha
        //
        _lblFecha.Location = new Point(275, 18);
        _lblFecha.Name = "_lblFecha";
        _lblFecha.Size = new Size(35, 13);
        _lblFecha.Text = "Fecha:";
        //
        // _txtFecha
        //
        _txtFecha.Location = new Point(330, 15);
        _txtFecha.Name = "_txtFecha";
        _txtFecha.Properties.ReadOnly = true;
        _txtFecha.Size = new Size(100, 20);
        _txtFecha.TabIndex = 1;
        //
        // _lblHora
        //
        _lblHora.Location = new Point(445, 18);
        _lblHora.Name = "_lblHora";
        _lblHora.Size = new Size(28, 13);
        _lblHora.Text = "Hora:";
        //
        // _txtHora
        //
        _txtHora.Location = new Point(490, 15);
        _txtHora.Name = "_txtHora";
        _txtHora.Properties.ReadOnly = true;
        _txtHora.Size = new Size(80, 20);
        _txtHora.TabIndex = 2;
        //
        // _lblEstatus
        //
        _lblEstatus.Location = new Point(590, 18);
        _lblEstatus.Name = "_lblEstatus";
        _lblEstatus.Size = new Size(35, 13);
        _lblEstatus.Text = "Status:";
        //
        // _txtEstatus
        //
        _txtEstatus.Location = new Point(640, 15);
        _txtEstatus.Name = "_txtEstatus";
        _txtEstatus.Properties.ReadOnly = true;
        _txtEstatus.Size = new Size(110, 20);
        _txtEstatus.TabIndex = 3;
        //
        // _lblLineaProduccion
        //
        _lblLineaProduccion.Location = new Point(15, 46);
        _lblLineaProduccion.Name = "_lblLineaProduccion";
        _lblLineaProduccion.Size = new Size(105, 13);
        _lblLineaProduccion.Text = "Línea de Producción:";
        //
        // _cmbLineaProduccion
        //
        _cmbLineaProduccion.Location = new Point(130, 43);
        _cmbLineaProduccion.Name = "_cmbLineaProduccion";
        _cmbLineaProduccion.Properties.NullText = "Seleccionar";
        _cmbLineaProduccion.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbLineaProduccion.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbLineaProduccion.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbLineaProduccion.Size = new Size(200, 20);
        _cmbLineaProduccion.TabIndex = 4;
        _cmbLineaProduccion.ButtonClick += CmbLineaProduccion_ButtonClick;
        //
        // _chkEsMixto
        //
        _chkEsMixto.Location = new Point(345, 43);
        _chkEsMixto.Name = "_chkEsMixto";
        _chkEsMixto.Properties.Caption = "Es Mixto";
        _chkEsMixto.Size = new Size(85, 20);
        _chkEsMixto.TabIndex = 5;
        //
        // _lblPorcentajeMateriaSeca
        //
        _lblPorcentajeMateriaSeca.Location = new Point(445, 46);
        _lblPorcentajeMateriaSeca.Name = "_lblPorcentajeMateriaSeca";
        _lblPorcentajeMateriaSeca.Size = new Size(80, 13);
        _lblPorcentajeMateriaSeca.Text = "% Materia Seca:";
        //
        // _spnPorcentajeMateriaSeca
        //
        _spnPorcentajeMateriaSeca.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPorcentajeMateriaSeca.Location = new Point(640, 43);
        _spnPorcentajeMateriaSeca.Name = "_spnPorcentajeMateriaSeca";
        _spnPorcentajeMateriaSeca.Properties.Mask.EditMask = "N02";
        _spnPorcentajeMateriaSeca.Properties.ReadOnly = true;
        _spnPorcentajeMateriaSeca.Size = new Size(110, 20);
        _spnPorcentajeMateriaSeca.TabIndex = 6;
        //
        // _lblPesoReal
        //
        _lblPesoReal.Location = new Point(15, 74);
        _lblPesoReal.Name = "_lblPesoReal";
        _lblPesoReal.Size = new Size(53, 13);
        _lblPesoReal.Text = "Peso Real:";
        //
        // _spnPesoReal
        //
        _spnPesoReal.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoReal.Location = new Point(130, 71);
        _spnPesoReal.Name = "_spnPesoReal";
        _spnPesoReal.Properties.Mask.EditMask = "N02";
        _spnPesoReal.Properties.MaxValue = new decimal(new int[] { 99999999, 0, 0, 0 });
        _spnPesoReal.Size = new Size(120, 20);
        _spnPesoReal.TabIndex = 7;
        //
        // _btnTomarLectura
        //
        _btnTomarLectura.Location = new Point(256, 70);
        _btnTomarLectura.Name = "_btnTomarLectura";
        _btnTomarLectura.Size = new Size(105, 23);
        _btnTomarLectura.TabIndex = 8;
        _btnTomarLectura.Text = "Tomar Lectura";
        _btnTomarLectura.Click += BtnTomarLectura_Click;
        //
        // _lblNoReempaque
        //
        _lblNoReempaque.Location = new Point(445, 74);
        _lblNoReempaque.Name = "_lblNoReempaque";
        _lblNoReempaque.Size = new Size(95, 13);
        _lblNoReempaque.Text = "No. de Reempaque:";
        //
        // _txtNoReempaque
        //
        _txtNoReempaque.Location = new Point(640, 71);
        _txtNoReempaque.Name = "_txtNoReempaque";
        _txtNoReempaque.Properties.ReadOnly = true;
        _txtNoReempaque.Size = new Size(110, 20);
        _txtNoReempaque.TabIndex = 9;
        //
        // _btnBloquear
        //
        _btnBloquear.Location = new Point(15, 105);
        _btnBloquear.Name = "_btnBloquear";
        _btnBloquear.Size = new Size(120, 23);
        _btnBloquear.TabIndex = 10;
        _btnBloquear.Text = "Bloquear Pallet";
        _btnBloquear.Click += BtnBloquear_Click;
        //
        // _btnImprimirEtiquetas
        //
        _btnImprimirEtiquetas.Location = new Point(141, 105);
        _btnImprimirEtiquetas.Name = "_btnImprimirEtiquetas";
        _btnImprimirEtiquetas.Size = new Size(130, 23);
        _btnImprimirEtiquetas.TabIndex = 11;
        _btnImprimirEtiquetas.Text = "Imprimir Etiquetas";
        _btnImprimirEtiquetas.Click += BtnImprimirEtiquetas_Click;
        //
        // _btnImprimirPapeleta
        //
        _btnImprimirPapeleta.Location = new Point(277, 105);
        _btnImprimirPapeleta.Name = "_btnImprimirPapeleta";
        _btnImprimirPapeleta.Size = new Size(130, 23);
        _btnImprimirPapeleta.TabIndex = 12;
        _btnImprimirPapeleta.Text = "Imprimir Papeleta";
        _btnImprimirPapeleta.Click += BtnImprimirPapeleta_Click;
        //
        // _lblDetalle
        //
        _lblDetalle.Location = new Point(15, 142);
        _lblDetalle.Name = "_lblDetalle";
        _lblDetalle.Size = new Size(40, 13);
        _lblDetalle.Text = "Detalle:";
        //
        // _gridDetalle
        //
        _gridDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _gridDetalle.Location = new Point(15, 160);
        _gridDetalle.MainView = _gridViewDetalle;
        _gridDetalle.Name = "_gridDetalle";
        _gridDetalle.Size = new Size(735, 265);
        _gridDetalle.TabIndex = 13;
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
        _gridViewDetalle.FocusedRowChanged += GridViewDetalle_FocusedRowChanged;
        //
        // _btnDetalleNuevo
        //
        _btnDetalleNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnDetalleNuevo.Location = new Point(15, 435);
        _btnDetalleNuevo.Name = "_btnDetalleNuevo";
        _btnDetalleNuevo.Size = new Size(85, 23);
        _btnDetalleNuevo.TabIndex = 14;
        _btnDetalleNuevo.Text = "Nuevo";
        _btnDetalleNuevo.Click += BtnDetalleNuevo_Click;
        //
        // _btnDetalleEditar
        //
        _btnDetalleEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnDetalleEditar.Location = new Point(106, 435);
        _btnDetalleEditar.Name = "_btnDetalleEditar";
        _btnDetalleEditar.Size = new Size(85, 23);
        _btnDetalleEditar.TabIndex = 15;
        _btnDetalleEditar.Text = "Editar";
        _btnDetalleEditar.Click += BtnDetalleEditar_Click;
        //
        // _btnDetalleBorrar
        //
        _btnDetalleBorrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnDetalleBorrar.Location = new Point(197, 435);
        _btnDetalleBorrar.Name = "_btnDetalleBorrar";
        _btnDetalleBorrar.Size = new Size(85, 23);
        _btnDetalleBorrar.TabIndex = 16;
        _btnDetalleBorrar.Text = "Borrar";
        _btnDetalleBorrar.Click += BtnDetalleBorrar_Click;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(585, 435);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 17;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(670, 435);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 18;
        _btnCancelar.Text = "Cerrar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // PalletEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(765, 470);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_lblFecha);
        Controls.Add(_txtFecha);
        Controls.Add(_lblHora);
        Controls.Add(_txtHora);
        Controls.Add(_lblEstatus);
        Controls.Add(_txtEstatus);
        Controls.Add(_lblLineaProduccion);
        Controls.Add(_cmbLineaProduccion);
        Controls.Add(_chkEsMixto);
        Controls.Add(_lblPorcentajeMateriaSeca);
        Controls.Add(_spnPorcentajeMateriaSeca);
        Controls.Add(_lblPesoReal);
        Controls.Add(_spnPesoReal);
        Controls.Add(_btnTomarLectura);
        Controls.Add(_lblNoReempaque);
        Controls.Add(_txtNoReempaque);
        Controls.Add(_btnBloquear);
        Controls.Add(_btnImprimirEtiquetas);
        Controls.Add(_btnImprimirPapeleta);
        Controls.Add(_lblDetalle);
        Controls.Add(_gridDetalle);
        Controls.Add(_btnDetalleNuevo);
        Controls.Add(_btnDetalleEditar);
        Controls.Add(_btnDetalleBorrar);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        MinimumSize = new Size(785, 509);
        Name = "PalletEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Pallet";
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtHora.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtEstatus.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbLineaProduccion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkEsMixto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMateriaSeca.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoReal.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNoReempaque.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridDetalle).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewDetalle).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
