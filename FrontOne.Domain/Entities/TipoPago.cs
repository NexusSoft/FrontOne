namespace FrontOne.Domain.Entities;

public class TipoPago
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool NecesitaListaPrecios { get; set; }
    public bool Activo { get; set; }
}
