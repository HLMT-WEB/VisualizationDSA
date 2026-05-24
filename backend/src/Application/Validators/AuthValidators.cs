using FluentValidation;
using VisualizationDSA.Application.DTOs;

namespace VisualizationDSA.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không đúng định dạng.")
            .MaximumLength(255).WithMessage("Email không được vượt quá 255 ký tự.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username không được để trống.")
            .MinimumLength(3).WithMessage("Username phải có ít nhất 3 ký tự.")
            .MaximumLength(100).WithMessage("Username không được vượt quá 100 ký tự.")
            .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Username chỉ chấp nhận chữ cái, số, dấu gạch ngang và gạch dưới.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống.")
            .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự.")
            .MaximumLength(128).WithMessage("Mật khẩu không được vượt quá 128 ký tự.")
            .Matches(@"[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ hoa.")
            .Matches(@"[a-z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ thường.")
            .Matches(@"[0-9]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ số.");
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không đúng định dạng.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống.");
    }
}

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token không được để trống.");
    }
}
