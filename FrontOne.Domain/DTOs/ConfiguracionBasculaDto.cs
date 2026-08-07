namespace FrontOne.Domain.DTOs;

public record ConfiguracionBasculaDto(
    string Puerto,
    int BaudRate,
    byte Parity,
    byte DataBits,
    byte StopBits,
    string? PatronLectura);
