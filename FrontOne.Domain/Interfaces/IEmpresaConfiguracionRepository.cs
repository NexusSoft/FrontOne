using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IEmpresaConfiguracionRepository
{
    Task<EmpresaConfiguracion> ObtenerAsync();
    Task ActualizarAsync(EmpresaConfiguracion empresa);
}
