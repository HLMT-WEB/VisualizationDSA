using FluentAssertions;
using VisualizationDSA.Domain.Exceptions;

namespace Domain.Tests.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithEntityAndId_ShouldFormatMessage()
    {
        var id = Guid.NewGuid();
        var ex = new NotFoundException("User", id);

        ex.Message.Should().Contain("User");
        ex.Message.Should().Contain(id.ToString());
        ex.ErrorType.Should().Be("NOT_FOUND");
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        var ex = new NotFoundException("Custom message");

        ex.Message.Should().Be("Custom message");
        ex.ErrorType.Should().Be("NOT_FOUND");
    }
}

public class DomainValidationExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetDefaults()
    {
        var ex = new DomainValidationException("Invalid data");

        ex.Message.Should().Be("Invalid data");
        ex.ErrorType.Should().Be("VALIDATION_ERROR");
        ex.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithErrors_ShouldStoreErrorsDictionary()
    {
        var errors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Email is required" } },
            { "Password", new[] { "Min 8 chars", "Must contain uppercase" } }
        };

        var ex = new DomainValidationException(errors);

        ex.ErrorType.Should().Be("VALIDATION_ERROR");
        ex.Errors.Should().HaveCount(2);
        ex.Errors["Email"].Should().Contain("Email is required");
        ex.Errors["Password"].Should().HaveCount(2);
    }
}

public class AuthenticationExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetMessageAndErrorType()
    {
        var ex = new AuthenticationException("Invalid credentials");

        ex.Message.Should().Be("Invalid credentials");
        ex.ErrorType.Should().Be("AUTHENTICATION_ERROR");
    }
}

public class ConflictExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetMessageAndErrorType()
    {
        var ex = new ConflictException("Email already exists");

        ex.Message.Should().Be("Email already exists");
        ex.ErrorType.Should().Be("CONFLICT");
    }
}

public class DomainException_InheritanceTests
{
    [Fact]
    public void AllCustomExceptions_ShouldExtendDomainException()
    {
        var notFound = new NotFoundException("test");
        var validation = new DomainValidationException("test");
        var auth = new AuthenticationException("test");
        var conflict = new ConflictException("test");

        notFound.Should().BeAssignableTo<DomainException>();
        validation.Should().BeAssignableTo<DomainException>();
        auth.Should().BeAssignableTo<DomainException>();
        conflict.Should().BeAssignableTo<DomainException>();
    }

    [Fact]
    public void AllCustomExceptions_ShouldBeAssignableToException()
    {
        var notFound = new NotFoundException("test");

        notFound.Should().BeAssignableTo<Exception>();
    }
}
