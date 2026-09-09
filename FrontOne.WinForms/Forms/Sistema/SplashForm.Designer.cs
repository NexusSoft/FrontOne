namespace FrontOne.WinForms.Forms.Sistema
{
    partial class SplashForm
    {
        private System.ComponentModel.IContainer components = null;
        private DevExpress.XtraEditors.PictureEdit _picLogo;
        private DevExpress.XtraEditors.LabelControl _lblNombre;
        private DevExpress.XtraEditors.LabelControl _lblEstado;
        private DevExpress.XtraEditors.LabelControl _lblVersion;
        private DevExpress.XtraEditors.MarqueeProgressBarControl _progreso;
        private System.Windows.Forms.Timer _timerAnimacion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            _picLogo = new DevExpress.XtraEditors.PictureEdit();
            _lblNombre = new DevExpress.XtraEditors.LabelControl();
            _lblEstado = new DevExpress.XtraEditors.LabelControl();
            _lblVersion = new DevExpress.XtraEditors.LabelControl();
            _progreso = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            _timerAnimacion = new System.Windows.Forms.Timer(components);

            ((System.ComponentModel.ISupportInitialize)_picLogo.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_progreso.Properties).BeginInit();
            SuspendLayout();

            _picLogo.Location = new System.Drawing.Point(150, 30);
            _picLogo.Size = new System.Drawing.Size(120, 90);
            _picLogo.Properties.AllowFocused = false;
            _picLogo.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            _picLogo.Properties.Appearance.Options.UseBackColor = true;
            _picLogo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            _picLogo.Properties.ShowMenu = false;
            _picLogo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            _picLogo.TabStop = false;
            _picLogo.Visible = false;
            _picLogo.Paint += PicLogo_Paint;

            _lblNombre.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            _lblNombre.Appearance.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            _lblNombre.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            _lblNombre.Text = "FrontOne";
            _lblNombre.Location = new System.Drawing.Point(0, 130);
            _lblNombre.Size = new System.Drawing.Size(420, 40);

            _lblEstado.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            _lblEstado.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            _lblEstado.Text = string.Empty;
            _lblEstado.Location = new System.Drawing.Point(0, 210);
            _lblEstado.Size = new System.Drawing.Size(420, 20);

            _progreso.EditValue = 0;
            _progreso.Location = new System.Drawing.Point(110, 178);
            _progreso.Size = new System.Drawing.Size(200, 12);
            _progreso.TabStop = false;

            _lblVersion.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            _lblVersion.Text = "v" + (System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version?.ToString() ?? "1.0");
            _lblVersion.Location = new System.Drawing.Point(300, 235);
            _lblVersion.Size = new System.Drawing.Size(110, 16);

            _timerAnimacion.Interval = 16;
            _timerAnimacion.Tick += TimerAnimacion_Tick;

            ClientSize = new System.Drawing.Size(420, 260);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Controls.Add(_lblVersion);
            Controls.Add(_progreso);
            Controls.Add(_lblEstado);
            Controls.Add(_lblNombre);
            Controls.Add(_picLogo);
            Name = "SplashForm";
            Text = "FrontOne";
            Load += SplashForm_Load;

            ((System.ComponentModel.ISupportInitialize)_picLogo.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_progreso.Properties).EndInit();
            ResumeLayout(false);
        }
    }
}
