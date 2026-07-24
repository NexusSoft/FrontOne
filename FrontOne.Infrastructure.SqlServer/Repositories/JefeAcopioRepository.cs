using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class JefeAcopioRepository : SqlRepositoryBase, IJefeAcopioRepository
{
    private record InsertResult(int Id, string Clave);

    public JefeAcopioRepository(IConnectionFactory connectionFactory, ILogger<JefeAcopioRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<JefeAcopio>> ObtenerAsync(int? id = null)
        => QueryAsync<JefeAcopio>("Acopio.sp_JefeAcopio_Obtener", new { Id = id });

    public Task<IReadOnlyList<JefeAcopio>> BuscarAsync(string filtro)
        => QueryAsync<JefeAcopio>("Acopio.sp_JefeAcopio_Buscar", new { Filtro = filtro });

    public Task<IReadOnlyList<JefeAcopio>> ObtenerTop100Async()
        => QueryAsync<JefeAcopio>("Acopio.sp_JefeAcopio_ObtenerTop100");

    public Task<JefeAcopio?> ObtenerPrimeroAsync()
        => QueryFirstAsync<JefeAcopio>("Acopio.sp_JefeAcopio_ObtenerPrimero");

    public Task<JefeAcopio?> ObtenerUltimoAsync()
        => QueryFirstAsync<JefeAcopio>("Acopio.sp_JefeAcopio_ObtenerUltimo");

    public Task<JefeAcopio?> ObtenerSiguienteAsync(int id)
        => QueryFirstAsync<JefeAcopio>("Acopio.sp_JefeAcopio_ObtenerSiguiente", new { Id = id });

    public Task<JefeAcopio?> ObtenerAnteriorAsync(int id)
        => QueryFirstAsync<JefeAcopio>("Acopio.sp_JefeAcopio_ObtenerAnterior", new { Id = id });

    public async Task<(int Id, string Clave)> InsertarAsync(JefeAcopio jefeAcopio)
    {
        var resultado = await QueryFirstAsync<InsertResult>("Acopio.sp_JefeAcopio_Insertar", new
        {
            jefeAcopio.Nombre,
            jefeAcopio.Domicilio,
            jefeAcopio.Colonia,
            jefeAcopio.CodigoPostal,
            jefeAcopio.PoblacionId,
            jefeAcopio.MunicipioId,
            jefeAcopio.EstadoId,
            jefeAcopio.Telefono,
            jefeAcopio.Celular,
            jefeAcopio.Email,
            jefeAcopio.Observaciones,
            jefeAcopio.Activo,
        });

        return (resultado!.Id, resultado.Clave);
    }

    public Task ActualizarAsync(JefeAcopio jefeAcopio)
        => ExecuteAsync("Acopio.sp_JefeAcopio_Actualizar", new
        {
            jefeAcopio.Id,
            jefeAcopio.Nombre,
            jefeAcopio.Domicilio,
            jefeAcopio.Colonia,
            jefeAcopio.CodigoPostal,
            jefeAcopio.PoblacionId,
            jefeAcopio.MunicipioId,
            jefeAcopio.EstadoId,
            jefeAcopio.Telefono,
            jefeAcopio.Celular,
            jefeAcopio.Email,
            jefeAcopio.Observaciones,
            jefeAcopio.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_JefeAcopio_Eliminar", new { Id = id });
}
