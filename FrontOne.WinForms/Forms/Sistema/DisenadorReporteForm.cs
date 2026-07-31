using System.Drawing.Design;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using FrontOne.Application.Services;
using FrontOne.WinForms.Reports.Controles;

namespace FrontOne.WinForms.Forms.Sistema;

// Envuelve el Diseñador de Reportes de DevExpress (XRDesignForm — el mismo editor completo que
// usa Visual Studio, pero corriendo dentro de la app) para un reporte identificado por Codigo.
// Al cerrar la ventana pregunta si se guarda el layout en Configuracion.ReportePlantilla — así
// el cambio queda disponible sin recompilar, para cualquier otra máquina que corra la app.
public static class DisenadorReporteForm
{
    private const string RecursoIconoBarcode = "FrontOne.WinForms.Resources.Icons.barcode.png";

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

        // El servicio de toolbox solo está disponible dentro de este evento (se dispara cada
        // vez que el Diseñador carga un panel de reporte) — pedirlo justo después de OpenReport
        // es demasiado pronto y GetService regresa null en silencio.
        designForm.DesignMdiController.DesignPanelLoaded += (_, e) =>
        {
            // Se usa el mismo servicio (el concreto de DevExpress, que también implementa
            // IToolboxService) para AddToolboxItem y AddToolBoxImage — pedirlos por separado
            // con dos GetService() distintos corre el riesgo de resolver instancias distintas.
            // GetService solo resuelve por el tipo con el que el servicio quedó registrado
            // (la interfaz IToolboxService) — pedirlo directo por la clase concreta
            // (XRToolboxService) regresa null en silencio. Se pide por la interfaz y se
            // castea aparte para llegar a AddToolBoxImage (no expuesto en la interfaz).
            if (e.DesignerHost.GetService(typeof(IToolboxService)) is IToolboxService toolboxService)
            {
                var itemBarcode = new System.Drawing.Design.ToolboxItem(typeof(XRBarcodeControl)) { DisplayName = "Código TEC-IT" };
                toolboxService.AddToolboxItem(itemBarcode, "Código de Barras");

                // El ícono es cosmético — si algo falla al registrarlo (formato de imagen,
                // firma de API distinta a la esperada, etc.) NUNCA debe tumbar el registro del
                // control en sí, que es lo indispensable.
                try
                {
                    if (toolboxService is DevExpress.XtraReports.UserDesigner.Native.XRToolboxService toolboxServiceDx)
                    {
                        using var streamIcono = typeof(DisenadorReporteForm).Assembly.GetManifestResourceStream(RecursoIconoBarcode);
                        if (streamIcono is not null)
                        {
                            // AddToolBoxImage no reescala — hay que mandar un bitmap ya al
                            // tamaño exacto de cada slot (el original es 93x96px, muy grande
                            // para el de 16px si se manda tal cual).
                            using var original = System.Drawing.Image.FromStream(streamIcono);
                            toolboxServiceDx.AddToolBoxImage(typeof(XRBarcodeControl), DevExpress.XtraReports.UserDesigner.Native.ImageSize.Size16, new System.Drawing.Bitmap(original, 16, 16));
                            toolboxServiceDx.AddToolBoxImage(typeof(XRBarcodeControl), DevExpress.XtraReports.UserDesigner.Native.ImageSize.Size24, new System.Drawing.Bitmap(original, 24, 24));
                            toolboxServiceDx.AddToolBoxImage(typeof(XRBarcodeControl), DevExpress.XtraReports.UserDesigner.Native.ImageSize.Size32, new System.Drawing.Bitmap(original, 32, 32));
                        }
                    }
                }
                catch
                {
                    // Ícono no disponible — el control se queda con el ícono genérico, no es
                    // crítico.
                }
            }
        };

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
