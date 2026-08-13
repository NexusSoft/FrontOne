using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ISupervisorHuertaRepository
{
    Task<IReadOnlyList<SupervisorHuerta>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(SupervisorHuerta supervisorHuerta);
    Task ActualizarAsync(SupervisorHuerta supervisorHuerta);
    Task EliminarAsync(int id);
}
