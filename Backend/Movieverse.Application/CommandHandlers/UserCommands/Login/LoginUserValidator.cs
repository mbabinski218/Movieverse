using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.CommandHandlers.UserCommands.Login;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
	public LoginUserValidator()
	{
		RuleFor(l=>l.GrantType)
			.NotEmpty().WithMessage(UserResources.EnterAuthenticator)
			.Must(GrantTypeExtensions.IsDefined)
			.WithMessage(UserResources.WrongEmailFormat);

		RuleFor(l => l.Email)
			.Must(x => x is not null)
			.When(x => x.GrantType == GrantType.Password.ToStringFast() || x.Password is not null)
			.WithMessage(UserResources.EnterEmail)
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.IdToken is not null)
			.WithMessage(UserResources.WrongDataFormat)
			.EmailAddress()
			.WithMessage(UserResources.WrongEmailFormat);

		RuleFor(x => x.Password)
			.Must(x => x is not null)
			.When(x => x.GrantType == GrantType.Password.ToStringFast() || x.Email is not null)
			.WithMessage(UserResources.EnterPassword)
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.IdToken is not null)
			.WithMessage(UserResources.WrongDataFormat);
		
		RuleFor(x => x.RefreshToken)
			.Must(x => x is not null)
			.When(x => x.GrantType == GrantType.RefreshToken.ToStringFast())
			.WithMessage(UserResources.EnterRefreshToken)
			.Must(x => x is null)
			.When(x => x.IdToken is not null || x.Password is not null || x.Email is not null)
			.WithMessage(UserResources.WrongDataFormat);

		RuleFor(x => x.IdToken)
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.Password is not null || x.Email is not null)
			.WithMessage(UserResources.WrongDataFormat);
	}
}