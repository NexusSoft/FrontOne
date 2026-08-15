namespace FrontOne.Domain.DTOs;

public record EtiquetaPalletEncabezadoDto(
    string NoPallet,
    DateTime FechaProcesado,
    byte Estatus,
    string NombreProducto,
    decimal? PesoEstandar);
