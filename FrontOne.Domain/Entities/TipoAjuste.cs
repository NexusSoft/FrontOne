namespace FrontOne.Domain.Entities;

public class TipoAjuste
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public byte TipoGasto { get; set; }
    public byte Signo { get; set; }
    public bool Activo { get; set; }
}
