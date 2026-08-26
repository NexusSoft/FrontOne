using Microsoft.AspNetCore.Authorization;

namespace FrontOne.Web.Security;

/// <summary>Requisito de autorización: el usuario debe tener el claim "permisoWeb" = "{Pantalla}/{Accion}".</summary>
public class PermisoRequirement : IAuthorizationRequirement
{
    public string Pantalla { get; }
    public string Accion { get; }

    public PermisoRequirement(string pantalla, string accion)
    {
        Pantalla = pantalla;
        Accion = accion;
    }
}
