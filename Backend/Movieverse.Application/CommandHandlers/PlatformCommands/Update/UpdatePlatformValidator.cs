using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Platform;
using Movieverse.Domain.Common;

namespace Movieverse.Application.CommandHandlers.PlatformCommands.Update;

public sealed class UpdatePlatformValidator : AbstractValidator<UpdatePlatformCommand>
{
	public UpdatePlatformValidator()
	{
		RuleFor(p => p.Name)
			.Must(s => s?.Length <= Constants.nameLength)
			.When(p => p.Name != null)
			.WithMessage(PlatformResources.NameTooLong);

		RuleFor(p => p.Price)
			.Must(d => d >= 0)
			.When(p => p.Price != null)
			.WithMessage(PlatformResources.PriceMustBeGreaterThanOrEqualToZero);
	}
}