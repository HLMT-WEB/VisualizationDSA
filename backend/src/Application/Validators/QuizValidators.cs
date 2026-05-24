using FluentValidation;
using VisualizationDSA.Application.DTOs;

namespace VisualizationDSA.Application.Validators;

public class QuizAttemptRequestValidator : AbstractValidator<QuizAttemptRequest>
{
    public QuizAttemptRequestValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("QuizId không được để trống.");

        RuleFor(x => x.Answers)
            .NotNull().WithMessage("Danh sách câu trả lời không được null.")
            .Must(a => a != null && a.Length > 0).WithMessage("Phải có ít nhất 1 câu trả lời.");
    }
}

public class XPAwardRequestValidator : AbstractValidator<XPAwardRequest>
{
    public XPAwardRequestValidator()
    {
        RuleFor(x => x.Amount)
            .InclusiveBetween(1, 200).WithMessage("Số XP phải nằm trong khoảng 1-200.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Lý do không được vượt quá 500 ký tự.");
    }
}
