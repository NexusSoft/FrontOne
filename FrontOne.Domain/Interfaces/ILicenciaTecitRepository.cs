using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ILicenciaTecitRepository
{
    Task<LicenciaTecit> ObtenerAsync();
    Task ActualizarAsync(LicenciaTecit licencia);
}
