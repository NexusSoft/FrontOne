namespace FrontOne.Domain.Entities;

// CategoriaId/CalibreApeamId son NULLABLE a propósito: una Materia Prima nace únicamente
// de la sincronización con SAP (SincronizarConSapAsync, grupo de artículos "MP"), momento en
// el que todavía no existe ninguna captura de negocio en FrontOne. El usuario completa esos
// campos después, editando el registro desde MateriaPrimaEditarForm.
public class MateriaPrima
{
    public int Id { get; set; }
    public string CodigoSap { get; set; } = string.Empty;
    public string DescripcionSap { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public int? CategoriaId { get; set; }
    public int? CalibreApeamId { get; set; }
    public DateTime FechaCreacion { get; set; }

    // Nombres resueltos por JOIN en el repositorio (Obtener/ObtenerTop1000/Buscar) — solo para
    // mostrarse en el grid del listado, nunca se escriben directo (Insertar/Actualizar no los tocan).
    public string? CategoriaNombre { get; set; }
    public string? CalibreApeamNombre { get; set; }
}
