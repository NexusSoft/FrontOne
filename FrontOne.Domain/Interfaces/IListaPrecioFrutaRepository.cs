using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IListaPrecioFrutaRepository
{
    Task<IReadOnlyList<ListaPrecioFruta>> ObtenerAsync(int? id = null);
    Task<bool> ExisteTraslapeAsync(int categoriaId, int calibreApeamId, DateTime fechaInicio, DateTime? fechaFin, int? productorId, int? idExcluir = null);
    Task<int> InsertarAsync(ListaPrecioFruta lista);
    Task ActualizarAsync(ListaPrecioFruta lista);
    Task<IReadOnlyList<VigenciaListaPrecioFruta>> ObtenerFechasAsync();
    Task<IReadOnlyList<VigenciaListaPrecioFruta>> ObtenerFechasPorProductorYRangoAsync(int productorId, DateTime fechaInicio, DateTime fechaFin);
    Task<IReadOnlyList<ListaPrecioFruta>> ObtenerPorFechaAsync(DateTime fecha, int? productorId);
    Task EliminarPorFechaAsync(DateTime fecha, int? productorId);
    Task<bool> ExisteVinculoAcuerdoCorteAsync(DateTime fecha, int? productorId);

    // Universo de combinaciones capturables (Catalogos.MateriaPrima activas) — ya no se
    // consulta SAP para llenar el grid de captura.
    Task<IReadOnlyList<CombinacionMateriaPrimaDto>> ObtenerCombinacionesActivasAsync();
}
