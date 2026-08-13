namespace FrontOne.Domain.DTOs;

public record MovilPermisoPantallaDto(
    string PantallaCodigo,
    string Modulo,
    bool Consultar,
    bool Crear,
    bool Modificar,
    bool Eliminar);
