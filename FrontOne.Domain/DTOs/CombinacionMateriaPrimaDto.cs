namespace FrontOne.Domain.DTOs;

// Combinación Categoría×Calibre APEAM capturable en Lista de Precio de Fruta — sale de
// Catalogos.MateriaPrima (solo materias primas activas), ya no de SAP.
public record CombinacionMateriaPrimaDto(int CategoriaId, string CategoriaNombre, int CalibreApeamId, string CalibreApeamNombre);
