using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IAcuerdoCorteRepository
{
    Task<IReadOnlyList<AcuerdoCorte>> ObtenerAsync(int? id = null);
    Task<(int Id, string Folio)> InsertarAsync(AcuerdoCorte acuerdo);
    Task ActualizarAsync(AcuerdoCorte acuerdo);
    Task EliminarAsync(int id);
}
