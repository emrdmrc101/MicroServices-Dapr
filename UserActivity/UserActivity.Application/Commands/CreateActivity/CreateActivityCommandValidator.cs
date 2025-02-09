using FluentValidation;

namespace UserActivity.Application.Commands.CreateActivity;

public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
{
    public CreateActivityCommandValidator()
    {
        RuleFor(x => x.ActivityType).NotNull().NotEmpty();
        RuleFor(x => x.ContentType).NotNull().NotEmpty();
    }
}