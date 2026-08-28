namespace FrontOne.Domain.Constants;

// Catálogo de "pantallas" de FrontOne.Web que se pueden otorgar por rol — mismo criterio que
// PantallasMovilDisponibles: el catálogo vive en código, no en tabla (Seguridad.WebPermiso.PantallaCodigo
// no lleva FK a ningún catálogo). AccesoWeb es el gate de "puede iniciar sesión en el sitio" (módulo
// Seguridad); las demás son las páginas reales del sitio. El campo Modulo aquí es solo la etiqueta
// del grupo bajo el que aparece la pantalla en el árbol del menú (NavMenu.razor) — usa el nombre
// del módulo de negocio real (ej. "Acopio") cuando la pantalla pertenece a uno; "AplicacionWeb" es
// el genérico para páginas de ejemplo sin módulo de negocio propio (ej. Países). Crece con cada
// página nueva que se agregue a FrontOne.Web.
public static class PantallasWebDisponibles
{
    public sealed record Definicion(string Codigo, string Modulo, string Descripcion);

    public static IReadOnlyList<Definicion> Todas { get; } =
    [
        new("AccesoWeb", "Seguridad", "Permiso de acceso al sitio web FrontOne.Web (sin pantalla propia en escritorio)"),
        new("Paises", "AplicacionWeb", "Página \"Países\" del sitio FrontOne.Web (módulo de ejemplo)"),
        new("SimuladorBandas", "Acopio", "Página \"Simulador de Bandas\" del sitio FrontOne.Web"),
        new("Lotes", "Producción", "Página \"Lotes\" (listado de Producción) del sitio FrontOne.Web"),
    ];
}
