namespace FrontOne.Shared.Constants;

// Las 4 acciones del modelo de permisos de reportes (Seguridad.ReportePermiso) — separado del
// modelo genérico Pantalla/Accion/Permiso porque un reporte no es una "pantalla" con CRUD.
public static class AccionReporte
{
    public const string VistaPrevia = "VistaPrevia";
    public const string Impresion = "Impresion";
    public const string Exportacion = "Exportacion";
    public const string Diseno = "Diseno";
}
