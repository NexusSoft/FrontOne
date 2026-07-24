using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IListaPrecioFrutaRepository
{
    Task<IReadOnlyList<ListaPrecioFruta>> ObtenerAsync(int? id = null);
    Task<bool> ExisteTraslapeAsync(string itemCode, DateTime fechaInicio, DateTime? fechaFin, int? productorId, int? variedadId, int? idExcluir = null);
    Task<int> InsertarAsync(ListaPrecioFruta lista);
    Task ActualizarAsync(ListaPrecioFruta lista);
    Task<IReadOnlyList<VigenciaListaPrecioFruta>> ObtenerFechasAsync();
    Task<IReadOnlyList<VigenciaListaPrecioFruta>> ObtenerFechasPorProductorYRangoAsync(int productorId, DateTime fechaInicio, DateTime fechaFin);
    Task<IReadOnlyList<ListaPrecioFruta>> ObtenerPorFechaAsync(DateTime fecha, int? productorId, int? variedadId);
    Task EliminarPorFechaAsync(DateTime fecha, int? productorId, int? variedadId);
}
