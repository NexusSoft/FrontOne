using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IConfiguracionBasculaRepository
{
    Task<ConfiguracionBascula> ObtenerAsync();
    Task ActualizarAsync(ConfiguracionBascula bascula);
}
