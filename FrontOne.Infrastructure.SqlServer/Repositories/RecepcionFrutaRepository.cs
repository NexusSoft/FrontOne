using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class RecepcionFrutaRepository : SqlRepositoryBase, IRecepcionFrutaRepository
{
    private record InsertResult(int Id, string Folio);

    public RecepcionFrutaRepository(IConnectionFactory connectionFactory, ILogger<RecepcionFrutaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<RecepcionFruta>> ObtenerAsync(int? id = null)
        => QueryAsync<RecepcionFruta>("Recepcion.sp_RecepcionFruta_Obtener", new { Id = id });

    public async Task<(int Id, string Folio)> InsertarAsync(RecepcionFruta recepcion)
    {
        var resultado = await QueryFirstAsync<InsertResult>("Recepcion.sp_RecepcionFruta_Insertar", new
        {
            recepcion.NoLote,
            recepcion.Fecha,
            recepcion.Chofer,
            recepcion.Placas,
            recepcion.Observaciones,
            recepcion.NumeroTicket,
            recepcion.CoprefBico,
            recepcion.PesoBruto,
            recepcion.PesoTara,
            recepcion.TaraCajas,
            recepcion.PesoMuestra,
            recepcion.PesoNeto,
            recepcion.PesoProductor,
            recepcion.PorcentajeMateriaSeca,
            recepcion.CajasPorEntregar,
            recepcion.CajasEntregadas,
            recepcion.CajasCortadas,
            recepcion.CajasRecibidasVacias,
            recepcion.CajasDiferencia,
            recepcion.CajasPerdidas,
            recepcion.DescargaCompleta,
            recepcion.TicketPesadaArchivo,
            recepcion.TicketPesadaNombreArchivo,
        });

        return (resultado!.Id, resultado.Folio);
    }

    public Task ActualizarAsync(RecepcionFruta recepcion)
        => ExecuteAsync("Recepcion.sp_RecepcionFruta_Actualizar", new
        {
            recepcion.Id,
            recepcion.NoLote,
            recepcion.Fecha,
            recepcion.Chofer,
            recepcion.Placas,
            recepcion.Observaciones,
            recepcion.NumeroTicket,
            recepcion.CoprefBico,
            recepcion.PesoBruto,
            recepcion.PesoTara,
            recepcion.TaraCajas,
            recepcion.PesoMuestra,
            recepcion.PesoNeto,
            recepcion.PesoProductor,
            recepcion.PorcentajeMateriaSeca,
            recepcion.CajasPorEntregar,
            recepcion.CajasEntregadas,
            recepcion.CajasCortadas,
            recepcion.CajasRecibidasVacias,
            recepcion.CajasDiferencia,
            recepcion.CajasPerdidas,
            recepcion.DescargaCompleta,
            recepcion.TicketPesadaArchivo,
            recepcion.TicketPesadaNombreArchivo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Recepcion.sp_RecepcionFruta_Eliminar", new { Id = id });

    public Task<IReadOnlyList<RecepcionFrutaOrdenCorte>> ObtenerDetalleAsync(int recepcionFrutaId)
        => QueryAsync<RecepcionFrutaOrdenCorte>("Recepcion.sp_RecepcionFrutaOrdenCorte_Obtener", new { RecepcionFrutaId = recepcionFrutaId });

    public async Task<int> InsertarDetalleAsync(RecepcionFrutaOrdenCorte linea)
    {
        var resultado = await QueryFirstAsync<int>("Recepcion.sp_RecepcionFrutaOrdenCorte_Insertar", new
        {
            linea.RecepcionFrutaId,
            linea.OrdenCorteId,
            linea.Kilogramos,
        });

        return resultado;
    }

    public Task ActualizarDetalleAsync(int id, decimal kilogramos)
        => ExecuteAsync("Recepcion.sp_RecepcionFrutaOrdenCorte_Actualizar", new { Id = id, Kilogramos = kilogramos });

    public Task EliminarDetalleAsync(int id)
        => ExecuteAsync("Recepcion.sp_RecepcionFrutaOrdenCorte_Eliminar", new { Id = id });

    public Task<RecepcionFrutaReporteDto?> ObtenerParaReporteAsync(int id)
        => QueryFirstAsync<RecepcionFrutaReporteDto>("Recepcion.sp_RecepcionFruta_ObtenerParaReporte", new { Id = id });

    public Task<IReadOnlyList<RecepcionDisponibleParaLote>> ObtenerTop100ParaLoteAsync(int? huertaId, int? acuerdoCorteId, string? pagarCorteACardCode)
        => QueryAsync<RecepcionDisponibleParaLote>("Lotes.sp_RecepcionFruta_ObtenerTop100ParaLote", new
        {
            HuertaId = huertaId,
            AcuerdoCorteId = acuerdoCorteId,
            PagarCorteACardCode = pagarCorteACardCode,
        });

    public Task<IReadOnlyList<RecepcionDisponibleParaLote>> BuscarParaLoteAsync(string filtro, int? huertaId, int? acuerdoCorteId, string? pagarCorteACardCode)
        => QueryAsync<RecepcionDisponibleParaLote>("Lotes.sp_RecepcionFruta_BuscarParaLote", new
        {
            Filtro = filtro,
            HuertaId = huertaId,
            AcuerdoCorteId = acuerdoCorteId,
            PagarCorteACardCode = pagarCorteACardCode,
        });

    public Task<RecepcionDisponibleParaLote?> ObtenerParaLotePorIdAsync(int recepcionFrutaId)
        => QueryFirstAsync<RecepcionDisponibleParaLote?>("Lotes.sp_RecepcionFruta_ObtenerParaLote", new { RecepcionFrutaId = recepcionFrutaId });

    public Task ActualizarNoLoteAsync(int id, string? noLote)
        => ExecuteAsync("Recepcion.sp_RecepcionFruta_ActualizarNoLote", new { Id = id, NoLote = noLote });
}
