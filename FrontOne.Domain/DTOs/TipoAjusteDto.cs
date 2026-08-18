namespace FrontOne.Domain.DTOs;

public record TipoAjusteDto(int Id, string Nombre, byte TipoGasto, byte Signo, bool Activo)
{
    public string TipoGastoTexto => TipoGasto == 1 ? "Cosecha" : "Acarreo";
    public string SignoTexto => Signo == 1 ? "A Favor" : "En Contra";
}
