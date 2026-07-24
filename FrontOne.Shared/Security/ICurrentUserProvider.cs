namespace FrontOne.Shared.Security;

public interface ICurrentUserProvider
{
    string? NombreUsuario { get; }
}
