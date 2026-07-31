using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ReportePlantillaRepository : SqlRepositoryBase, IReportePlantillaRepository
{
    public ReportePlantillaRepository(IConnectionFactory connectionFactory, ILogger<ReportePlantillaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<ReportePlantilla?> ObtenerPorCodigoAsync(string codigo)
        => QueryFirstAsync<ReportePlantilla>("Configuracion.sp_ReportePlantilla_ObtenerPorCodigo", new { Codigo = codigo });

    public Task GuardarAsync(string codigo, string nombre, string? definicionXml)
        => ExecuteAsync("Configuracion.sp_ReportePlantilla_Guardar", new { Codigo = codigo, Nombre = nombre, DefinicionXml = definicionXml });

    public Task EliminarAsync(string codigo)
        => ExecuteAsync("Configuracion.sp_ReportePlantilla_Eliminar", new { Codigo = codigo });

    public Task MarcarPredeterminadoAsync(string codigo, string nombre)
        => ExecuteAsync("Configuracion.sp_ReportePlantilla_MarcarPredeterminado", new { Codigo = codigo, Nombre = nombre });

    public Task QuitarPredeterminadoAsync(string codigo)
        => ExecuteAsync("Configuracion.sp_ReportePlantilla_QuitarPredeterminado", new { Codigo = codigo });
}
