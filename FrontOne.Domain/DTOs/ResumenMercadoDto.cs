namespace FrontOne.Domain.DTOs;

public record ResumenMercadoDto(
    string Mercado,
    decimal Kilogramos,
    decimal Porcentaje,
    decimal ImporteReal,
    decimal ImporteEstimado);
