using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ProductorRepository : SqlRepositoryBase, IProductorRepository
{
    private record InsertResult(int Id, string Clave);

    public ProductorRepository(IConnectionFactory connectionFactory, ILogger<ProductorRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Productor>> ObtenerAsync(int? id = null)
        => QueryAsync<Productor>("Catalogos.sp_Productor_Obtener", new { Id = id });

    public Task<IReadOnlyList<Productor>> BuscarAsync(string filtro)
        => QueryAsync<Productor>("Catalogos.sp_Productor_Buscar", new { Filtro = filtro });

    public Task<IReadOnlyList<Productor>> ObtenerTop100Async()
        => QueryAsync<Productor>("Catalogos.sp_Productor_ObtenerTop100");

    public Task<Productor?> ObtenerPrimeroAsync()
        => QueryFirstAsync<Productor>("Catalogos.sp_Productor_ObtenerPrimero");

    public Task<Productor?> ObtenerUltimoAsync()
        => QueryFirstAsync<Productor>("Catalogos.sp_Productor_ObtenerUltimo");

    public Task<Productor?> ObtenerSiguienteAsync(int id)
        => QueryFirstAsync<Productor>("Catalogos.sp_Productor_ObtenerSiguiente", new { Id = id });

    public Task<Productor?> ObtenerAnteriorAsync(int id)
        => QueryFirstAsync<Productor>("Catalogos.sp_Productor_ObtenerAnterior", new { Id = id });

    public async Task<(int Id, string Clave)> InsertarAsync(Productor productor)
    {
        var resultado = await QueryFirstAsync<InsertResult>("Catalogos.sp_Productor_Insertar", new
        {
            productor.NombreProductor,
            productor.Domicilio,
            productor.Colonia,
            productor.CodigoPostal,
            productor.PoblacionId,
            productor.MunicipioId,
            productor.EstadoId,
            productor.Rfc,
            productor.Telefono,
            productor.Celular,
            productor.Email,
            productor.Organizacion,
            productor.Observaciones,
            productor.Usuario,
            productor.PasswordEncriptado,
            productor.DiasCredito,
            productor.Activo,
        });

        return (resultado!.Id, resultado.Clave);
    }

    public Task ActualizarAsync(Productor productor)
        => ExecuteAsync("Catalogos.sp_Productor_Actualizar", new
        {
            productor.Id,
            productor.NombreProductor,
            productor.Domicilio,
            productor.Colonia,
            productor.CodigoPostal,
            productor.PoblacionId,
            productor.MunicipioId,
            productor.EstadoId,
            productor.Rfc,
            productor.Telefono,
            productor.Celular,
            productor.Email,
            productor.Organizacion,
            productor.Observaciones,
            productor.Usuario,
            productor.PasswordEncriptado,
            productor.DiasCredito,
            productor.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Productor_Eliminar", new { Id = id });
}
