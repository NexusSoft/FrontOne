namespace FrontOne.Shared.Exceptions;

public class SqlRepositoryException : Exception
{
    public string? StoredProcedure { get; }

    public SqlRepositoryException(string message, string? storedProcedure = null)
        : base(message)
    {
        StoredProcedure = storedProcedure;
    }

    public SqlRepositoryException(string message, Exception innerException, string? storedProcedure = null)
        : base(message, innerException)
    {
        StoredProcedure = storedProcedure;
    }
}
