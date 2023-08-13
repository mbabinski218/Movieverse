using FluentValidation;

namespace Movieverse.Application.CommandHandlers.UserCommands.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationValidator : AbstractValidator<Contracts.Commands.User.ResendEmailConfirmationCommand>
{
	public ResendEmailConfirmationValidator()
	{
		RuleFor(u => u.Email)
			.NotEmpty().WithMessage("Enter email address")
			.EmailAddress().WithMessage("Wrong email format");
	}
}