using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;

namespace Movieverse.Application.CommandHandlers.UserCommands.ChangePassword;

public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
	public ChangePasswordValidator()
	{
		RuleFor(u => u.CurrentPassword)
			.NotEmpty().WithMessage(UserResources.EnterPassword);
		
		RuleFor(u => u.NewPassword)
			.NotEmpty().WithMessage(UserResources.EnterPassword)
			.Equal(u => u.ConfirmNewPassword).WithMessage(UserResources.PasswordsMustBeTheSame);

		RuleFor(u => u.ConfirmNewPassword)
			.NotEmpty().WithMessage(UserResources.ConfirmPassword);
	}
}