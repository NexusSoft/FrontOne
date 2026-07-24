using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class HuertaRepository : SqlRepositoryBase, IHuertaRepository
{
    public HuertaRepository(IConnectionFactory connectionFactory, ILogger<HuertaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Huerta>> ObtenerAsync(int? id = null, int? productorId = null)
        => QueryAsync<Huerta>("Catalogos.sp_Huerta_Obtener", new { Id = id, ProductorId = productorId });

    public Task<IReadOnlyList<HuertaBusquedaDto>> BuscarAsync(string filtro)
        => QueryAsync<HuertaBusquedaDto>("Catalogos.sp_Huerta_Buscar", new { Filtro = filtro });

    public Task<IReadOnlyList<HuertaBusquedaDto>> ObtenerTop100Async()
        => QueryAsync<HuertaBusquedaDto>("Catalogos.sp_Huerta_ObtenerTop100");

    public Task<Huerta?> ObtenerPrimeroAsync()
        => QueryFirstAsync<Huerta>("Catalogos.sp_Huerta_ObtenerPrimero");

    public Task<Huerta?> ObtenerUltimoAsync()
        => QueryFirstAsync<Huerta>("Catalogos.sp_Huerta_ObtenerUltimo");

    public Task<Huerta?> ObtenerSiguienteAsync(int id)
        => QueryFirstAsync<Huerta>("Catalogos.sp_Huerta_ObtenerSiguiente", new { Id = id });

    public Task<Huerta?> ObtenerAnteriorAsync(int id)
        => QueryFirstAsync<Huerta>("Catalogos.sp_Huerta_ObtenerAnterior", new { Id = id });

    public Task<int> InsertarAsync(Huerta huerta)
        => ExecuteScalarAsync<int>("Catalogos.sp_Huerta_Insertar", new
        {
            huerta.Nombre,
            huerta.ProductorId,
            huerta.Ubicacion,
            huerta.PoblacionId,
            huerta.MunicipioId,
            huerta.EstadoId,
            huerta.ProductoId,
            huerta.EncargadoNombre,
            huerta.EncargadoTelefono,
            huerta.Observaciones,
            huerta.RegistroSagarpa,
            huerta.CertificadoGlobalGap,
            huerta.RegistroFda,
            huerta.NumeroGlobalGap,
            huerta.Superficie,
            huerta.Altura,
            huerta.NumeroArboles,
            huerta.EdadArboles,
            huerta.SistemaRiegoId,
            huerta.PorcentajeMecanizacion,
            huerta.StatusHuertaId,
            huerta.FechaCambioStatus,
            huerta.FechaVencimiento,
            huerta.Latitud,
            huerta.Longitud,
            huerta.Activo,
        })!;

    public Task ActualizarAsync(Huerta huerta)
        => ExecuteAsync("Catalogos.sp_Huerta_Actualizar", new
        {
            huerta.Id,
            huerta.Nombre,
            huerta.ProductorId,
            huerta.Ubicacion,
            huerta.PoblacionId,
            huerta.MunicipioId,
            huerta.EstadoId,
            huerta.ProductoId,
            huerta.EncargadoNombre,
            huerta.EncargadoTelefono,
            huerta.Observaciones,
            huerta.RegistroSagarpa,
            huerta.CertificadoGlobalGap,
            huerta.RegistroFda,
            huerta.NumeroGlobalGap,
            huerta.Superficie,
            huerta.Altura,
            huerta.NumeroArboles,
            huerta.EdadArboles,
            huerta.SistemaRiegoId,
            huerta.PorcentajeMecanizacion,
            huerta.StatusHuertaId,
            huerta.FechaCambioStatus,
            huerta.FechaVencimiento,
            huerta.Latitud,
            huerta.Longitud,
            huerta.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_Huerta_Eliminar", new { Id = id });

    public Task<bool> ExisteRegistroSagarpaAsync(string registroSagarpa, int? idExcluir)
        => ExecuteScalarAsync<bool>("Catalogos.sp_Huerta_ExisteRegistroSagarpa", new { RegistroSagarpa = registroSagarpa, IdExcluir = idExcluir });
}
