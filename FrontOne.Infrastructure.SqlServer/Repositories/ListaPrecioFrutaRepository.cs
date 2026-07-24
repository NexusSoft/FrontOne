using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ListaPrecioFrutaRepository : SqlRepositoryBase, IListaPrecioFrutaRepository
{
    public ListaPrecioFrutaRepository(IConnectionFactory connectionFactory, ILogger<ListaPrecioFrutaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<ListaPrecioFruta>> ObtenerAsync(int? id = null)
        => QueryAsync<ListaPrecioFruta>("Acopio.sp_ListaPrecioFruta_Obtener", new { Id = id });

    public Task<bool> ExisteTraslapeAsync(string itemCode, DateTime fechaInicio, DateTime? fechaFin, int? productorId, int? variedadId, int? idExcluir = null)
        => QueryFirstAsync<bool>("Acopio.sp_ListaPrecioFruta_ExisteTraslape", new
        {
            ItemCode = itemCode,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            ProductorId = productorId,
            VariedadId = variedadId,
            IdExcluir = idExcluir,
        });

    public Task<int> InsertarAsync(ListaPrecioFruta lista)
        => QueryFirstAsync<int>("Acopio.sp_ListaPrecioFruta_Insertar", new
        {
            lista.ItemCode,
            lista.ItemName,
            lista.Lista1,
            lista.Lista2,
            lista.Lista3,
            lista.FechaInicio,
            lista.FechaFin,
            lista.ProductorId,
            lista.VariedadId,
            lista.Activo,
        });

    public Task ActualizarAsync(ListaPrecioFruta lista)
        => ExecuteAsync("Acopio.sp_ListaPrecioFruta_Actualizar", new
        {
            lista.Id,
            lista.Lista1,
            lista.Lista2,
            lista.Lista3,
            lista.FechaInicio,
            lista.FechaFin,
            lista.Activo,
        });

    public Task<IReadOnlyList<VigenciaListaPrecioFruta>> ObtenerFechasAsync()
        => QueryAsync<VigenciaListaPrecioFruta>("Acopio.sp_ListaPrecioFruta_ObtenerFechas");

    public Task<IReadOnlyList<VigenciaListaPrecioFruta>> ObtenerFechasPorProductorYRangoAsync(int productorId, DateTime fechaInicio, DateTime fechaFin)
        => QueryAsync<VigenciaListaPrecioFruta>("Acopio.sp_ListaPrecioFruta_ObtenerFechasPorProductorYRango", new
        {
            ProductorId = productorId,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
        });

    public Task<IReadOnlyList<ListaPrecioFruta>> ObtenerPorFechaAsync(DateTime fecha, int? productorId, int? variedadId)
        => QueryAsync<ListaPrecioFruta>("Acopio.sp_ListaPrecioFruta_ObtenerPorFecha", new { Fecha = fecha, ProductorId = productorId, VariedadId = variedadId });

    public Task EliminarPorFechaAsync(DateTime fecha, int? productorId, int? variedadId)
        => ExecuteAsync("Acopio.sp_ListaPrecioFruta_EliminarPorFecha", new { Fecha = fecha, ProductorId = productorId, VariedadId = variedadId });
}
