namespace FrontOne.Domain.Constants;

// Catálogo de "pantallas" de FrontOne.Android que se pueden otorgar por rol — mismo criterio que
// ReportesDisponibles: el catálogo vive en código, no en tabla (Seguridad.MovilPermiso.PantallaCodigo
// no lleva FK a ningún catálogo). AccesoMovil es el gate de "puede entrar a la app" (módulo
// Seguridad); las otras 8 son las tarjetas de módulo que se ven en Inicio (módulo AplicacionMovil).
public static class PantallasMovilDisponibles
{
    public sealed record Definicion(string Codigo, string Modulo, string Descripcion);

    public static IReadOnlyList<Definicion> Todas { get; } =
    [
        new("AccesoMovil", "Seguridad", "Permiso de acceso a la aplicación móvil FrontOne.Android (sin pantalla propia en escritorio)"),
        new("Pallets", "AplicacionMovil", "Tarjeta \"Pallets\" en la pantalla de Inicio de FrontOne.Android"),
        new("Embarques", "AplicacionMovil", "Tarjeta \"Embarques\" en la pantalla de Inicio de FrontOne.Android"),
        new("Acopio", "AplicacionMovil", "Tarjeta \"Acopio\" en la pantalla de Inicio de FrontOne.Android"),
        new("Cajas de campo", "AplicacionMovil", "Tarjeta \"Cajas de campo\" en la pantalla de Inicio de FrontOne.Android"),
        new("Báscula", "AplicacionMovil", "Tarjeta \"Báscula\" en la pantalla de Inicio de FrontOne.Android"),
        new("Inocuidad", "AplicacionMovil", "Tarjeta \"Inocuidad\" en la pantalla de Inicio de FrontOne.Android"),
        new("Calidad", "AplicacionMovil", "Tarjeta \"Calidad\" en la pantalla de Inicio de FrontOne.Android"),
        new("Reportes", "AplicacionMovil", "Tarjeta \"Reportes\" en la pantalla de Inicio de FrontOne.Android"),
    ];
}
