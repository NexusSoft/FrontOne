namespace FrontOne.Shared.Exceptions;

public class SapException : Exception
{
    public string? SapErrorCode { get; }
    public int? HttpStatusCode { get; }

    public SapException(string message, string? sapErrorCode = null, int? httpStatusCode = null)
        : base(message)
    {
        SapErrorCode = sapErrorCode;
        HttpStatusCode = httpStatusCode;
    }

    public SapException(string message, Exception innerException, string? sapErrorCode = null, int? httpStatusCode = null)
        : base(message, innerException)
    {
        SapErrorCode = sapErrorCode;
        HttpStatusCode = httpStatusCode;
    }
}
