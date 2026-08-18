namespace FrontOne.Domain.DTOs;

public record GastoRecepcionAjusteDto(
    int Id,
    int GastoLoteId,
    int LoteRecepcionId,
    int RecepcionFrutaId,
    string RecepcionFolio,
    decimal PesoNeto,
    decimal PesoProductor,
    int OrdenCorteId,
    string OrdenCorteFolio,
    int TipoAjusteId,
    string TipoAjusteNombre,
    byte Signo,
    decimal Monto,
    byte CargoA)
{
    // Signo 2 = En Contra: resta al total en vez de sumar.
    public decimal Importe => Signo == 2 ? -Monto : Monto;
}
