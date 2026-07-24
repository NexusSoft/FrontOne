namespace FrontOne.Domain.DTOs;

public record PermisoPantallaDto(
    int PantallaId,
    string ModuloNombre,
    string PantallaNombre,
    bool Consultar,
    bool Crear,
    bool Modificar,
    bool Eliminar);
