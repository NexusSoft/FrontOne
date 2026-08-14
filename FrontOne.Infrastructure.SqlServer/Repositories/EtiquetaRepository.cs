using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class EtiquetaRepository : SqlRepositoryBase, IEtiquetaRepository
{
    public EtiquetaRepository(IConnectionFactory connectionFactory, ILogger<EtiquetaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Etiqueta>> ObtenerTodosAsync()
        => QueryAsync<Etiqueta>("Etiquetado.sp_Etiqueta_ObtenerTodos");

    public Task<IReadOnlyList<Etiqueta>> ObtenerEliminadosAsync()
        => QueryAsync<Etiqueta>("Etiquetado.sp_Etiqueta_ObtenerEliminados");

    public Task<Etiqueta?> ObtenerAsync(int id)
        => QueryFirstAsync<Etiqueta>("Etiquetado.sp_Etiqueta_Obtener", new { Id = id });

    public async Task<bool> ExisteNombreAsync(string nombre, int? idExcluir)
        => await QueryFirstAsync<bool>("Etiquetado.sp_Etiqueta_ValidarNombre", new { Nombre = nombre, IdExcluir = idExcluir });

    public Task<int> InsertarAsync(string nombre, decimal anchoPulgadas, decimal altoPulgadas, byte tipo, string? definicionXml)
        => ExecuteScalarAsync<int>("Etiquetado.sp_Etiqueta_Insertar", new
        {
            Nombre = nombre,
            AnchoPulgadas = anchoPulgadas,
            AltoPulgadas = altoPulgadas,
            Tipo = tipo,
            DefinicionXml = definicionXml,
        })!;

    public Task ActualizarAsync(int id, string nombre, decimal anchoPulgadas, decimal altoPulgadas, byte tipo)
        => ExecuteAsync("Etiquetado.sp_Etiqueta_Actualizar", new
        {
            Id = id,
            Nombre = nombre,
            AnchoPulgadas = anchoPulgadas,
            AltoPulgadas = altoPulgadas,
            Tipo = tipo,
        });

    public Task GuardarLayoutAsync(int id, string definicionXml)
        => ExecuteAsync("Etiquetado.sp_Etiqueta_GuardarLayout", new { Id = id, DefinicionXml = definicionXml });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Etiquetado.sp_Etiqueta_Eliminar", new { Id = id });

    public Task RecuperarAsync(int id)
        => ExecuteAsync("Etiquetado.sp_Etiqueta_Recuperar", new { Id = id });
}
