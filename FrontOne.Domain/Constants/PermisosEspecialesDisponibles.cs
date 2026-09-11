namespace FrontOne.Domain.Constants;

// Catálogo de permisos especiales que se pueden otorgar por rol, independiente de los permisos
// de pantallas web. Vive en código: Seguridad.PermisoEspecial.Codigo no lleva FK a un catálogo.
// Cada definición identifica la acción que se incorpora a los permisos de la sesión web.
public static class PermisosEspecialesDisponibles
{
    public sealed record Definicion(string Codigo, string Descripcion, string Modulo, string Pantalla, string Accion);

    public static IReadOnlyList<Definicion> Todas =
    [
        new("AutorizarEstimaciones", "Autorizar Estimaciones", "Acopio", "AutorizacionEstimaciones", "Autorizar"),
    ];
}
