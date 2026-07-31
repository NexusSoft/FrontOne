using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Lotes;

partial class LoteEditarForm
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
    private LabelControl _lblReferencia;
    private TextEdit _txtReferencia;
    private LabelControl _lblLineaProduccion;
    private LookUpEdit _cmbLineaProduccion;
    private LabelControl _lblKilogramos;
    private SpinEdit _spnKilogramos;
    private LabelControl _lblPorcentajeMateriaSeca;
    private SpinEdit _spnPorcentajeMateriaSeca;
    private LabelControl _lblPersonalizado;
    private TextEdit _txtPersonalizado;
    private LabelControl _lblObservaciones;
    private TextEdit _txtObservaciones;
    private LabelControl _lblDetalle;
    private GridControl _gridDetalle;
    private GridView _gridViewDetalle;
    private SimpleButton _btnDetalleNuevo;
    private SimpleButton _btnDetalleBorrar;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoteEditarForm));
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _lblFecha = new LabelControl();
        _dtFecha = new DateEdit();
        _lblReferencia = new LabelControl();
        _txtReferencia = new TextEdit();
        _lblLineaProduccion = new LabelControl();
        _cmbLineaProduccion = new LookUpEdit();
        _lblKilogramos = new LabelControl();
        _spnKilogramos = new SpinEdit();
        _lblPorcentajeMateriaSeca = new LabelControl();
        _spnPorcentajeMateriaSeca = new SpinEdit();
        _lblPersonalizado = new LabelControl();
        _txtPersonalizado = new TextEdit();
        _lblObservaciones = new LabelControl();
        _txtObservaciones = new TextEdit();
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
        ((System.ComponentModel.ISupportInitialize)_txtReferencia.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbLineaProduccion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilogramos.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMateriaSeca.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPersonalizado.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).BeginInit();
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
        _txtFolio.Size = new Size(120, 20);
        _txtFolio.TabIndex = 0;
        //
        // _lblFecha
        //
        _lblFecha.Location = new Point(250, 18);
        _lblFecha.Name = "_lblFecha";
        _lblFecha.Size = new Size(35, 13);
        _lblFecha.Text = "Fecha:";
        //
        // _dtFecha
        //
        _dtFecha.EditValue = null;
        _dtFecha.Location = new Point(300, 15);
        _dtFecha.Name = "_dtFecha";
        _dtFecha.Size = new Size(120, 20);
        _dtFecha.TabIndex = 1;
        //
        // _lblReferencia
        //
        _lblReferencia.Location = new Point(15, 44);
        _lblReferencia.Name = "_lblReferencia";
        _lblReferencia.Size = new Size(58, 13);
        _lblReferencia.Text = "Referencia:";
        //
        // _txtReferencia
        //
        _txtReferencia.Location = new Point(110, 41);
        _txtReferencia.Name = "_txtReferencia";
        _txtReferencia.Properties.ReadOnly = true;
        _txtReferencia.Size = new Size(160, 20);
        _txtReferencia.TabIndex = 2;
        //
        // _lblLineaProduccion
        //
        _lblLineaProduccion.Location = new Point(300, 44);
        _lblLineaProduccion.Name = "_lblLineaProduccion";
        _lblLineaProduccion.Size = new Size(105, 13);
        _lblLineaProduccion.Text = "Línea de Producción:";
        //
        // _cmbLineaProduccion
        //
        _cmbLineaProduccion.Location = new Point(415, 41);
        _cmbLineaProduccion.Name = "_cmbLineaProduccion";
        _cmbLineaProduccion.Properties.NullText = "Seleccionar";
        _cmbLineaProduccion.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbLineaProduccion.Size = new Size(170, 20);
        _cmbLineaProduccion.TabIndex = 3;
        _cmbLineaProduccion.ButtonClick += CmbLineaProduccion_ButtonClick;
        //
        // _lblKilogramos
        //
        _lblKilogramos.Location = new Point(15, 70);
        _lblKilogramos.Name = "_lblKilogramos";
        _lblKilogramos.Size = new Size(63, 13);
        _lblKilogramos.Text = "Kilogramos:";
        //
        // _spnKilogramos
        //
        _spnKilogramos.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnKilogramos.Location = new Point(110, 67);
        _spnKilogramos.Name = "_spnKilogramos";
        _spnKilogramos.Properties.Mask.EditMask = "N02";
        _spnKilogramos.Properties.ReadOnly = true;
        _spnKilogramos.Size = new Size(120, 20);
        _spnKilogramos.TabIndex = 4;
        //
        // _lblPorcentajeMateriaSeca
        //
        _lblPorcentajeMateriaSeca.Location = new Point(300, 70);
        _lblPorcentajeMateriaSeca.Name = "_lblPorcentajeMateriaSeca";
        _lblPorcentajeMateriaSeca.Size = new Size(80, 13);
        _lblPorcentajeMateriaSeca.Text = "% Materia Seca:";
        //
        // _spnPorcentajeMateriaSeca
        //
        _spnPorcentajeMateriaSeca.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPorcentajeMateriaSeca.Location = new Point(415, 67);
        _spnPorcentajeMateriaSeca.Name = "_spnPorcentajeMateriaSeca";
        _spnPorcentajeMateriaSeca.Properties.Mask.EditMask = "N02";
        _spnPorcentajeMateriaSeca.Properties.ReadOnly = true;
        _spnPorcentajeMateriaSeca.Size = new Size(90, 20);
        _spnPorcentajeMateriaSeca.TabIndex = 5;
        //
        // _lblPersonalizado
        //
        _lblPersonalizado.Location = new Point(15, 96);
        _lblPersonalizado.Name = "_lblPersonalizado";
        _lblPersonalizado.Size = new Size(68, 13);
        _lblPersonalizado.Text = "Personalizado:";
        //
        // _txtPersonalizado
        //
        _txtPersonalizado.Location = new Point(110, 93);
        _txtPersonalizado.Name = "_txtPersonalizado";
        _txtPersonalizado.Size = new Size(475, 20);
        _txtPersonalizado.TabIndex = 6;
        //
        // _lblObservaciones
        //
        _lblObservaciones.Location = new Point(15, 122);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(68, 13);
        _lblObservaciones.Text = "Observaciones:";
        //
        // _txtObservaciones
        //
        _txtObservaciones.Location = new Point(110, 119);
        _txtObservaciones.Name = "_txtObservaciones";
        _txtObservaciones.Size = new Size(475, 20);
        _txtObservaciones.TabIndex = 7;
        //
        // _lblDetalle
        //
        _lblDetalle.Location = new Point(15, 150);
        _lblDetalle.Name = "_lblDetalle";
        _lblDetalle.Size = new Size(58, 13);
        _lblDetalle.Text = "Recepciones:";
        //
        // _gridDetalle
        //
        _gridDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _gridDetalle.Location = new Point(15, 168);
        _gridDetalle.MainView = _gridViewDetalle;
        _gridDetalle.Name = "_gridDetalle";
        _gridDetalle.Size = new Size(590, 220);
        _gridDetalle.TabIndex = 8;
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
        _btnDetalleNuevo.Location = new Point(15, 395);
        _btnDetalleNuevo.Name = "_btnDetalleNuevo";
        _btnDetalleNuevo.Size = new Size(85, 23);
        _btnDetalleNuevo.TabIndex = 9;
        _btnDetalleNuevo.Text = "Nuevo";
        _btnDetalleNuevo.Click += BtnDetalleNuevo_Click;
        //
        // _btnDetalleBorrar
        //
        _btnDetalleBorrar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        _btnDetalleBorrar.Location = new Point(105, 395);
        _btnDetalleBorrar.Name = "_btnDetalleBorrar";
        _btnDetalleBorrar.Size = new Size(85, 23);
        _btnDetalleBorrar.TabIndex = 10;
        _btnDetalleBorrar.Text = "Borrar";
        _btnDetalleBorrar.Click += BtnDetalleBorrar_Click;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(440, 435);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 11;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(525, 435);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 12;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // LoteEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(620, 470);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_lblFecha);
        Controls.Add(_dtFecha);
        Controls.Add(_lblReferencia);
        Controls.Add(_txtReferencia);
        Controls.Add(_lblLineaProduccion);
        Controls.Add(_cmbLineaProduccion);
        Controls.Add(_lblKilogramos);
        Controls.Add(_spnKilogramos);
        Controls.Add(_lblPorcentajeMateriaSeca);
        Controls.Add(_spnPorcentajeMateriaSeca);
        Controls.Add(_lblPersonalizado);
        Controls.Add(_txtPersonalizado);
        Controls.Add(_lblObservaciones);
        Controls.Add(_txtObservaciones);
        Controls.Add(_lblDetalle);
        Controls.Add(_gridDetalle);
        Controls.Add(_btnDetalleNuevo);
        Controls.Add(_btnDetalleBorrar);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        MinimumSize = new Size(640, 509);
        Name = "LoteEditarForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Lote";
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtReferencia.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbLineaProduccion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilogramos.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMateriaSeca.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPersonalizado.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridDetalle).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewDetalle).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
