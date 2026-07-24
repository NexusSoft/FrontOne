using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class OrdenCorteRepository : SqlRepositoryBase, IOrdenCorteRepository
{
    private record InsertResult(int Id, string Folio);

    public OrdenCorteRepository(IConnectionFactory connectionFactory, ILogger<OrdenCorteRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<OrdenCorte>> ObtenerAsync(int? id = null)
        => QueryAsync<OrdenCorte>("Acopio.sp_OrdenCorte_Obtener", new { Id = id });

    public async Task<(int Id, string Folio)> InsertarAsync(OrdenCorte orden)
    {
        var resultado = await QueryFirstAsync<InsertResult>("Acopio.sp_OrdenCorte_Insertar", new
        {
            orden.Fecha,
            orden.AcuerdoCorteId,
            orden.ProductorId,
            orden.HuertaId,
            orden.FloracionId,
            orden.RegistroSagarpa,
            orden.VariedadId,
            orden.PagarCorteACardCode,
            orden.PagarCorteANombre,
            orden.TransportistaCardCode,
            orden.TransportistaNombre,
            orden.PrecioAcarreo,
            orden.NoCandado,
            orden.CajasEntregadas,
            orden.JefeCuadrillaCardCode,
            orden.JefeCuadrillaNombre,
            orden.CostoKg,
            orden.PagoDia,
            orden.CuadrillaApoyo,
            orden.KgMinimo,
            orden.JefeAcopioId,
            orden.JefeAcopioNombre,
            orden.PuntoReunion,
            orden.Observaciones,
            orden.Cancelado,
        });

        return (resultado!.Id, resultado.Folio);
    }

    public Task ActualizarAsync(OrdenCorte orden)
        => ExecuteAsync("Acopio.sp_OrdenCorte_Actualizar", new
        {
            orden.Id,
            orden.Fecha,
            orden.AcuerdoCorteId,
            orden.ProductorId,
            orden.HuertaId,
            orden.FloracionId,
            orden.RegistroSagarpa,
            orden.VariedadId,
            orden.PagarCorteACardCode,
            orden.PagarCorteANombre,
            orden.TransportistaCardCode,
            orden.TransportistaNombre,
            orden.PrecioAcarreo,
            orden.NoCandado,
            orden.CajasEntregadas,
            orden.JefeCuadrillaCardCode,
            orden.JefeCuadrillaNombre,
            orden.CostoKg,
            orden.PagoDia,
            orden.CuadrillaApoyo,
            orden.KgMinimo,
            orden.JefeAcopioId,
            orden.JefeAcopioNombre,
            orden.PuntoReunion,
            orden.Observaciones,
            orden.Cancelado,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_OrdenCorte_Eliminar", new { Id = id });
}
