using FluentValidation;

namespace Movieverse.Application.Commands.UserCommands.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationValidator : AbstractValidator<ResendEmailConfirmationCommand>
{
	public ResendEmailConfirmationValidator()
	{
		RuleFor(u => u.Email)
			.NotEmpty().WithMessage("Enter email address")
			.EmailAddress().WithMessage("Wrong email format");
	}
}