namespace FrontOne.Domain.DTOs;

// Resultado ligero de sp_Huerta_Buscar (picker de búsqueda, TOP 500).
public record HuertaBusquedaDto(
    int Id,
    string Nombre,
    string? RegistroSagarpa,
    int ProductorId,
    string NombreProductor,
    bool Activo);
