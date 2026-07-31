using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.UserDesigner.Native;
using FrontOne.Application.Services;
using FrontOne.WinForms.Reports.Controles;

namespace FrontOne.WinForms.Forms.Sistema;

// Envuelve el Diseñador de Reportes de DevExpress (XRDesignForm — el mismo editor completo que
// usa Visual Studio, pero corriendo dentro de la app) para un reporte identificado por Codigo.
// Al cerrar la ventana pregunta si se guarda el layout en Configuracion.ReportePlantilla — así
// el cambio queda disponible sin recompilar, para cualquier otra máquina que corra la app.
public static class DisenadorReporteForm
{
    public static async Task MostrarAsync(
        IWin32Window propietario,
        ReportePlantillaService reportePlantillaService,
        string codigo,
        string nombre,
        XtraReport reporteDefault,
        Action<XtraReport>? conectarOrigenDatos = null,
        Action<XtraReport>? desconectarOrigenDatos = null,
        LicenciaTecitService? licenciaTecitService = null)
    {
        var plantilla = await reportePlantillaService.ObtenerPorCodigoAsync(codigo);
        if (!string.IsNullOrWhiteSpace(plantilla?.DefinicionXml))
        {
            using var streamCarga = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(plantilla.DefinicionXml));
            reporteDefault.LoadLayoutFromXml(streamCarga);
        }

        // El origen de datos (si el reporte lo tiene) se conecta después de LoadLayoutFromXml y
        // se desconecta antes de SaveLayoutToXml — nunca debe quedar serializado en el layout
        // guardado (llevaría la contraseña de conexión), solo las expresiones de binding.
        conectarOrigenDatos?.Invoke(reporteDefault);

        // XRBarcodeControl (Reports/Controles) lee la licencia de TECIT desde este estático al
        // imprimir/previsualizar dentro del propio Diseñador — se carga una sola vez por sesión
        // del Diseñador y se limpia al cerrar, nunca queda pegada al reporte serializado.
        if (licenciaTecitService is not null)
        {
            XRBarcodeControl.LicenciaActual = await licenciaTecitService.ObtenerAsync();
        }

        using var designForm = new XRDesignForm();
        designForm.OpenReport(reporteDefault);

        if (designForm.ActiveDesignPanel?.GetService(typeof(XRToolboxService)) is XRToolboxService toolboxService)
        {
            toolboxService.AddToolboxItem(new System.Drawing.Design.ToolboxItem(typeof(XRBarcodeControl)), "Código de Barras");
        }

        designForm.FormClosing += async (_, e) =>
        {
            var reporteActual = designForm.ActiveDesignPanel?.Report;
            if (reporteActual is null)
            {
                return;
            }

            var confirmar = XtraMessageBox.Show(designForm, "¿Guardar los cambios de diseño de este reporte?", "FrontOne",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (confirmar == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            if (confirmar == DialogResult.Yes)
            {
                desconectarOrigenDatos?.Invoke(reporteActual);

                using var streamGuarda = new MemoryStream();
                reporteActual.SaveLayoutToXml(streamGuarda);
                var xml = System.Text.Encoding.UTF8.GetString(streamGuarda.ToArray());
                await reportePlantillaService.GuardarAsync(codigo, nombre, xml);
            }
        };

        designForm.ShowDialog(propietario);
        XRBarcodeControl.LicenciaActual = null;
    }
}
