using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IReportePlantillaRepository
{
    Task<ReportePlantilla?> ObtenerPorCodigoAsync(string codigo);
    Task GuardarAsync(string codigo, string nombre, string? definicionXml);
    Task EliminarAsync(string codigo);
    Task MarcarPredeterminadoAsync(string codigo, string nombre);
    Task QuitarPredeterminadoAsync(string codigo);
}
