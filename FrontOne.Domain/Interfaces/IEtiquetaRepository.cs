using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IEtiquetaRepository
{
    Task<IReadOnlyList<Etiqueta>> ObtenerTodosAsync();
    Task<IReadOnlyList<Etiqueta>> ObtenerEliminadosAsync();
    Task<Etiqueta?> ObtenerAsync(int id);
    Task<bool> ExisteNombreAsync(string nombre, int? idExcluir);
    Task<int> InsertarAsync(string nombre, decimal anchoPulgadas, decimal altoPulgadas, byte tipo, string? definicionXml);
    Task ActualizarAsync(int id, string nombre, decimal anchoPulgadas, decimal altoPulgadas, byte tipo);
    Task GuardarLayoutAsync(int id, string definicionXml);
    Task EliminarAsync(int id);
    Task RecuperarAsync(int id);
}
