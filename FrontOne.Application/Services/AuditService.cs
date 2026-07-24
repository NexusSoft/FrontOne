using System.Net;
using System.Net.Sockets;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;

namespace FrontOne.Application.Services;

public class AuditService
{
    private readonly IAuditoriaRepository _auditoriaRepository;

    public AuditService(IAuditoriaRepository auditoriaRepository)
    {
        _auditoriaRepository = auditoriaRepository;
    }

    public Task RegistrarAsync(
        string usuario,
        TipoAccionAuditoria accion,
        string modulo,
        string? valoresAnteriores = null,
        string? valoresNuevos = null)
    {
        var entry = new AuditoriaEntryDto(
            usuario,
            DateTime.UtcNow,
            ObtenerEquipo(),
            ObtenerIp(),
            accion.ToString(),
            modulo,
            valoresAnteriores,
            valoresNuevos);

        return _auditoriaRepository.RegistrarAsync(entry);
    }

    private static string ObtenerEquipo() => Environment.MachineName;

    private static string ObtenerIp()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
            return ip?.ToString() ?? "0.0.0.0";
        }
        catch (SocketException)
        {
            return "0.0.0.0";
        }
    }
}
