namespace FrontOne.Web.Constants;

// Mapa pantalla (Codigo de PantallasWebDisponibles) -> ruta del sitio. Compartido por NavMenu.razor
// (arma el árbol) y Breadcrumb.razor (arma "Inicio > Módulo > Pantalla" a partir de la URL
// actual) — una sola fuente para no desincronizar los dos. Cada página nueva agrega su entrada
// aquí, junto con su registro en PantallasWebDisponibles (Domain).
public static class RutasWebPantallas
{
    public static readonly IReadOnlyDictionary<string, string> PorPantalla = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Paises"] = "/catalogos/paises",
        ["OrdenesCorte"] = "/acopio/ordenes-corte",
        ["SimuladorBandas"] = "/acopio/simulador-bandas",
        ["Estimacion"] = "/acopio/estimacion",
        ["AutorizacionEstimaciones"] = "/acopio/autorizacion-estimaciones",
        ["Lotes"] = "/produccion/lotes",
    };
}
