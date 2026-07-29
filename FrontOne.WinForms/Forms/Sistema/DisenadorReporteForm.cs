using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using FrontOne.Application.Services;

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
        XtraReport reporteDefault)
    {
        var plantilla = await reportePlantillaService.ObtenerPorCodigoAsync(codigo);
        if (!string.IsNullOrWhiteSpace(plantilla?.DefinicionXml))
        {
            using var streamCarga = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(plantilla.DefinicionXml));
            reporteDefault.LoadLayoutFromXml(streamCarga);
        }

        using var designForm = new XRDesignForm();
        designForm.OpenReport(reporteDefault);

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
                using var streamGuarda = new MemoryStream();
                reporteActual.SaveLayoutToXml(streamGuarda);
                var xml = System.Text.Encoding.UTF8.GetString(streamGuarda.ToArray());
                await reportePlantillaService.GuardarAsync(codigo, nombre, xml);
            }
        };

        designForm.ShowDialog(propietario);
    }
}
