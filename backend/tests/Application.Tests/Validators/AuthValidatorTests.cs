using FluentAssertions;
using FluentValidation.TestHelper;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Validators;

namespace Application.Tests.Validators;

public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _validator = new();

    [Fact]
    public void Valid_ShouldPass()
    {
        var request = new RegisterRequest
        {
            Email = "user@example.com",
            Username = "testuser",
            Password = "Password1"
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EmptyEmail_ShouldFail()
    {
        var request = new RegisterRequest { Email = "", Username = "user", Password = "Password1" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void InvalidEmail_ShouldFail()
    {
        var request = new RegisterRequest { Email = "not-an-email", Username = "user", Password = "Password1" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ShortUsername_ShouldFail()
    {
        var request = new RegisterRequest { Email = "a@b.com", Username = "ab", Password = "Password1" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void SpecialCharsUsername_ShouldFail()
    {
        var request = new RegisterRequest { Email = "a@b.com", Username = "user@name!", Password = "Password1" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void ValidUsernameWithDash_ShouldPass()
    {
        var request = new RegisterRequest { Email = "a@b.com", Username = "user-name_1", Password = "Password1" };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void ShortPassword_ShouldFail()
    {
        var request = new RegisterRequest { Email = "a@b.com", Username = "user", Password = "Pass1" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void NoUppercasePassword_ShouldFail()
    {
        var request = new RegisterRequest { Email = "a@b.com", Username = "user", Password = "password1" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void NoLowercasePassword_ShouldFail()
    {
        var request = new RegisterRequest { Email = "a@b.com", Username = "user", Password = "PASSWORD1" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void NoDigitPassword_ShouldFail()
    {
        var request = new RegisterRequest { Email = "a@b.com", Username = "user", Password = "Passwordx" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator = new();

    [Fact]
    public void Valid_ShouldPass()
    {
        var request = new LoginRequest { Email = "user@example.com", Password = "anypassword" };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EmptyEmail_ShouldFail()
    {
        var request = new LoginRequest { Email = "", Password = "pass" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void EmptyPassword_ShouldFail()
    {
        var request = new LoginRequest { Email = "a@b.com", Password = "" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}

public class RefreshTokenRequestValidatorTests
{
    private readonly RefreshTokenRequestValidator _validator = new();

    [Fact]
    public void EmptyToken_ShouldFail()
    {
        var request = new RefreshTokenRequest { RefreshToken = "" };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
    }

    [Fact]
    public void ValidToken_ShouldPass()
    {
        var request = new RefreshTokenRequest { RefreshToken = "some_valid_token_string" };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
