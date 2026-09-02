namespace FrontOne.Domain.DTOs;

// Fila del buscador de pallets del Reempaque, en dos modos:
// - Origen: pallets candidatos a desarmarse — ya armados (Bloqueado), no Neutros, no reempacados
//   antes, no reservados en otro folio abierto. CajasObjetivo/EsMixto vienen en cero/false (el SP
//   de origen no los calcula, no aplican a ese modo).
// - Destino: pallets candidatos a recibir cajas — Vacío o Incompleto (1, 2), no Neutros, no
//   bloqueados, excluyendo los que son origen de este mismo folio. CajasObjetivo indica cuántas
//   caben (null si el pallet es mixto o su producto de encabezado no tiene Cajas por Pallet).
public record ReempaquePalletDisponibleDto(
    int Id,
    string Folio,
    DateTime FechaCreacion,
    byte Estatus,
    int? TotalCajas,
    decimal? TotalKilogramos,
    int? CajasObjetivo,
    bool EsMixto,
    string ProductoDescripcion);
