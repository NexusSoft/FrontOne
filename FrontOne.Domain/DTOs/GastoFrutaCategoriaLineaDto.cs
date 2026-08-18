namespace FrontOne.Domain.DTOs;

public record GastoFrutaCategoriaLineaDto(
    string MateriaPrimaItemCode,
    string MateriaPrimaNombre,
    string Mercado,
    decimal KilogramosSeleccionados,
    decimal Porcentaje,
    decimal KilogramosComprados,
    decimal? CostoRealUnitario,
    bool CostoRealEsManual,
    decimal? CostoEstimadoUnitario,
    bool CostoEstimadoEsManual)
{
    public decimal ImporteReal => KilogramosSeleccionados * (CostoRealUnitario ?? 0);
    public decimal ImporteEstimado => KilogramosSeleccionados * (CostoEstimadoUnitario ?? 0);
}
