using FluentValidation;
using Movieverse.Application.Resources;

namespace Movieverse.Application.CommandHandlers.UserCommands.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationValidator : AbstractValidator<Contracts.Commands.User.ResendEmailConfirmationCommand>
{
	public ResendEmailConfirmationValidator()
	{
		RuleFor(u => u.Email)
			.NotEmpty().WithMessage(UserResources.EnterEmail)
			.EmailAddress().WithMessage(UserResources.WrongEmailFormat);
	}
}