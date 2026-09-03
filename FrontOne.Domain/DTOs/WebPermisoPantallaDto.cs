namespace FrontOne.Domain.DTOs;

public record WebPermisoPantallaDto(
    string PantallaCodigo,
    string Modulo,
    bool Consultar,
    bool Crear,
    bool Modificar,
    bool Eliminar);
