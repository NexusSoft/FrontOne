using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class AuditoriaRepository : SqlRepositoryBase, IAuditoriaRepository
{
    public AuditoriaRepository(IConnectionFactory connectionFactory, ILogger<AuditoriaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task RegistrarAsync(AuditoriaEntryDto entry)
        => ExecuteAsync("Auditoria.sp_Auditoria_Registrar", new
        {
            entry.Usuario,
            entry.Fecha,
            entry.Equipo,
            entry.Ip,
            entry.Accion,
            entry.Modulo,
            entry.ValoresAnteriores,
            entry.ValoresNuevos,
        });
}
