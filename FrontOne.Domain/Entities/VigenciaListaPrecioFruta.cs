namespace FrontOne.Domain.Entities;

// Proyección de sp_ListaPrecioFruta_ObtenerFechas: una combinación única de fecha +
// productor-o-general, navegable desde el buscador. No es una fila de la tabla en sí.
public class VigenciaListaPrecioFruta
{
    public DateTime FechaInicio { get; set; }
    public int? ProductorId { get; set; }
    public string? ProductorNombre { get; set; }
}
