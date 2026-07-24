using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface IAuditoriaRepository
{
    Task RegistrarAsync(AuditoriaEntryDto entry);
}
