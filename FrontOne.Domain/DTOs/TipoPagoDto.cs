namespace FrontOne.Domain.DTOs;

public record TipoPagoDto(int Id, string Nombre, bool NecesitaListaPrecios, bool Activo);
