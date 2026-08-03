namespace FrontOne.Domain.DTOs;

public record PesoEstandarDto(int Id, string Codigo, string Descripcion, decimal PesoNeto, decimal PesoPromedio, bool Activo);
