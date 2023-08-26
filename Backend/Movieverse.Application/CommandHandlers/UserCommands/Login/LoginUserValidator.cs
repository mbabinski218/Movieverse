using FluentValidation;
using Movieverse.Contracts.Commands.User;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.CommandHandlers.UserCommands.Login;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
	public LoginUserValidator()
	{
		RuleFor(l=>l.GrantType)
			.NotEmpty().WithMessage("Enter authenticator")
			.Must(GrantTypeExtensions.IsDefined)
			.WithMessage("Invalid provider");

		RuleFor(l => l.Email)
			.Must(x => x is not null)
			.When(x => x.GrantType == GrantType.Password.ToStringFast() || x.Password is not null)
			.WithMessage("Enter email")
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.IdToken is not null)
			.WithMessage("Wrong data format")
			.EmailAddress()
			.WithMessage("Wrong email format");

		RuleFor(x => x.Password)
			.Must(x => x is not null)
			.When(x => x.GrantType == GrantType.Password.ToStringFast() || x.Email is not null)
			.WithMessage("Enter password")
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.IdToken is not null)
			.WithMessage("Wrong data format");
		
		RuleFor(x => x.RefreshToken)
			.Must(x => x is not null)
			.When(x => x.GrantType == GrantType.RefreshToken.ToStringFast())
			.WithMessage("Enter refresh token")
			.Must(x => x is null)
			.When(x => x.IdToken is not null || x.Password is not null || x.Email is not null)
			.WithMessage("Wrong data format");

		RuleFor(x => x.IdToken)
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.Password is not null || x.Email is not null)
			.WithMessage("Wrong data format");
	}
}