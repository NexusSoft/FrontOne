namespace FrontOne.Domain.Constants;

// Catálogo de "pantallas" de FrontOne.Web que se pueden otorgar por rol — mismo criterio que
// PantallasMovilDisponibles: el catálogo vive en código, no en tabla (Seguridad.WebPermiso.PantallaCodigo
// no lleva FK a ningún catálogo). AccesoWeb es el gate de "puede iniciar sesión en el sitio" (módulo
// Seguridad); las demás son las páginas reales del sitio (módulo AplicacionWeb). Crece con cada
// página nueva que se agregue a FrontOne.Web.
public static class PantallasWebDisponibles
{
    public sealed record Definicion(string Codigo, string Modulo, string Descripcion);

    public static IReadOnlyList<Definicion> Todas { get; } =
    [
        new("AccesoWeb", "Seguridad", "Permiso de acceso al sitio web FrontOne.Web (sin pantalla propia en escritorio)"),
        new("Paises", "AplicacionWeb", "Página \"Países\" del sitio FrontOne.Web (módulo de ejemplo)"),
    ];
}
