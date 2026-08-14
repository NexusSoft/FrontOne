using FrontOne.Domain.Enums;

namespace FrontOne.Domain.Entities;

public class Etiqueta
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal AnchoPulgadas { get; set; }
    public decimal AltoPulgadas { get; set; }
    public TipoEtiqueta Tipo { get; set; }
    public string? DefinicionXml { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaModificacion { get; set; }
}
