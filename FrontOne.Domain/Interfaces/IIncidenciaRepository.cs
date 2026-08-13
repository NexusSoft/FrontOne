using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IIncidenciaRepository
{
    // Grid principal: TODAS las Órdenes de Corte del rango de fecha, tengan o no Incidencia.
    Task<IReadOnlyList<IncidenciaListadoDto>> ObtenerOrdenesConEstatusAsync(DateTime fechaDesde, DateTime fechaHasta);

    // Usado al abrir el formulario de captura — siempre regresa una fila (los campos derivados de
    // Orden de Corte vienen resueltos aunque la Incidencia todavía no exista).
    Task<IncidenciaDto?> ObtenerPorOrdenCorteIdAsync(int ordenCorteId);

    Task<int> InsertarAsync(Incidencia incidencia);
    Task ActualizarAsync(Incidencia incidencia);

    // Fuente de datos del PDF — solo Órdenes de Corte con Incidencia ya capturada.
    Task<IReadOnlyList<IncidenciaReporteDto>> ObtenerParaReporteAsync(DateTime fechaDesde, DateTime fechaHasta);
}
