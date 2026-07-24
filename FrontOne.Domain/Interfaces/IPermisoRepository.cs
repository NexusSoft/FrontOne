namespace FrontOne.Domain.Interfaces;

public interface IPermisoRepository
{
    Task<IReadOnlyList<(int ModuloId, string ModuloNombre, int PantallaId, string PantallaNombre, int AccionId, string AccionNombre)>> ObtenerEstructuraAsync();
    Task<IReadOnlyList<(int PantallaId, int AccionId)>> ObtenerPorRolAsync(int rolId);
    Task SincronizarAsync(int rolId, IReadOnlyList<(int PantallaId, int AccionId)> otorgados);
}
