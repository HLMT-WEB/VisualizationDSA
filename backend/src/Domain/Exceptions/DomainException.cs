namespace VisualizationDSA.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public string ErrorType { get; }

    protected DomainException(string message, string errorType)
        : base(message)
    {
        ErrorType = errorType;
    }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object id)
        : base($"Không tìm thấy {entityName} với ID: '{id}'.", "NOT_FOUND")
    {
    }

    public NotFoundException(string message)
        : base(message, "NOT_FOUND")
    {
    }
}

public class DomainValidationException : DomainException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public DomainValidationException(string message)
        : base(message, "VALIDATION_ERROR")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public DomainValidationException(IDictionary<string, string[]> errors)
        : base("Dữ liệu đầu vào không hợp lệ.", "VALIDATION_ERROR")
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}

public class AuthenticationException : DomainException
{
    public AuthenticationException(string message)
        : base(message, "AUTHENTICATION_ERROR")
    {
    }
}

public class ConflictException : DomainException
{
    public ConflictException(string message)
        : base(message, "CONFLICT")
    {
    }
}
