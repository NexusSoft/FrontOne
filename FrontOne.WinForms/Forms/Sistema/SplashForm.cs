using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using DevExpress.XtraSplashScreen;

namespace FrontOne.WinForms.Forms.Sistema;

public enum SplashCommand
{
    SetLogo,
    SetDescripcion,
    FadeOut,
}

public partial class SplashForm : SplashScreen
{
    private const double DuracionBounceMs = 600;
    private const double DuracionShimmerMs = 300;
    private const double DuracionFadeOutMs = 350;

    private readonly Stopwatch _cronometro = new();
    private System.Drawing.Size _tamanoFinalLogo;
    private bool _logoListo;
    private bool _cerrando;

    public SplashForm()
    {
        InitializeComponent();
    }

    public override void ProcessCommand(Enum cmd, object arg)
    {
        base.ProcessCommand(cmd, arg);

        switch ((SplashCommand)cmd)
        {
            case SplashCommand.SetLogo:
                SetLogoInterno((byte[])arg);
                break;
            case SplashCommand.SetDescripcion:
                _lblEstado.Text = (string)arg;
                break;
            case SplashCommand.FadeOut:
                _cerrando = true;
                _cronometro.Restart();
                break;
        }
    }

    // ponytail: un solo timer/cronómetro maneja opacidad de ventana (fade), tamaño del logo
    // (bounce EaseOutBack) y banda de shimmer — evita armar un motor de animación reusable
    // que solo esta pantalla necesita.
    private void SplashForm_Load(object? sender, EventArgs e)
    {
        Opacity = 0;
        _tamanoFinalLogo = _picLogo.Size;
        _cronometro.Start();
        _timerAnimacion.Start();
    }

    private void SetLogoInterno(byte[] bytes)
    {
        using var ms = new MemoryStream(bytes);
        _picLogo.Image = System.Drawing.Image.FromStream(ms);
        _picLogo.Visible = true;
        _logoListo = true;
        _cronometro.Restart();
    }

    private void TimerAnimacion_Tick(object? sender, EventArgs e)
    {
        if (_cerrando)
        {
            var tSalida = Math.Min(1.0, _cronometro.Elapsed.TotalMilliseconds / DuracionFadeOutMs);
            Opacity = Math.Max(0, 1 - tSalida);
            return;
        }

        var t = Math.Min(1.0, _cronometro.Elapsed.TotalMilliseconds / DuracionBounceMs);

        if (Opacity < 1)
        {
            Opacity = Math.Min(1, t);
        }

        if (_logoListo && t < 1)
        {
            var escala = EaseOutBack(t);
            _picLogo.Size = new System.Drawing.Size(
                (int)(_tamanoFinalLogo.Width * (0.85 + 0.15 * escala)),
                (int)(_tamanoFinalLogo.Height * (0.85 + 0.15 * escala)));
            _picLogo.Location = new System.Drawing.Point(
                150 + (_tamanoFinalLogo.Width - _picLogo.Width) / 2,
                30 + (_tamanoFinalLogo.Height - _picLogo.Height) / 2);
        }
        else if (_logoListo && _picLogo.Size != _tamanoFinalLogo)
        {
            _picLogo.Size = _tamanoFinalLogo;
            _picLogo.Location = new System.Drawing.Point(150, 30);
        }

        if (_logoListo)
        {
            _picLogo.Invalidate();
        }
    }

    private double ShimmerProgreso()
    {
        var msDesdeBounce = _cronometro.Elapsed.TotalMilliseconds - DuracionBounceMs;
        return msDesdeBounce < 0 ? -1 : Math.Min(1.0, msDesdeBounce / DuracionShimmerMs);
    }

    private void PicLogo_Paint(object? sender, System.Windows.Forms.PaintEventArgs e)
    {
        var shimmer = ShimmerProgreso();
        if (shimmer < 0 || shimmer > 1)
        {
            return;
        }

        var ancho = _picLogo.Width;
        var x = (int)(-ancho * 0.4 + (ancho * 1.8) * shimmer);

        using var brush = new LinearGradientBrush(
            new System.Drawing.Rectangle(x, 0, ancho / 3, _picLogo.Height),
            System.Drawing.Color.FromArgb(0, System.Drawing.Color.White),
            System.Drawing.Color.FromArgb(90, System.Drawing.Color.White),
            LinearGradientMode.Horizontal);
        e.Graphics.FillRectangle(brush, x, 0, ancho / 3, _picLogo.Height);
    }

    // Easing "back": leve rebote más allá del 100% antes de asentar (Penner easing functions).
    private static double EaseOutBack(double t)
    {
        const double c1 = 1.70158;
        const double c3 = c1 + 1;
        var p = t - 1;
        return 1 + c3 * Math.Pow(p, 3) + c1 * Math.Pow(p, 2);
    }
}
