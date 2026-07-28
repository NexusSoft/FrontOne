using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IRecepcionFrutaRepository
{
    Task<IReadOnlyList<RecepcionFruta>> ObtenerAsync(int? id = null);
    Task<(int Id, string Folio)> InsertarAsync(RecepcionFruta recepcion);
    Task ActualizarAsync(RecepcionFruta recepcion);
    Task EliminarAsync(int id);

    Task<IReadOnlyList<RecepcionFrutaOrdenCorte>> ObtenerDetalleAsync(int recepcionFrutaId);
    Task<int> InsertarDetalleAsync(RecepcionFrutaOrdenCorte linea);
    Task ActualizarDetalleAsync(int id, decimal kilogramos);
    Task EliminarDetalleAsync(int id);
}
