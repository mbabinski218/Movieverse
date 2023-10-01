using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Platform;
using Movieverse.Domain.Common;

namespace Movieverse.Application.CommandHandlers.PlatformCommands.Add;

public sealed class AddPlatformValidator : AbstractValidator<AddPlatformCommand>
{
	public AddPlatformValidator()
	{
		RuleFor(p => p.Name)
			.NotEmpty().WithMessage(PlatformResources.EnterName)
			.MaximumLength(Constants.nameLength).WithMessage(PlatformResources.NameTooLong);

		RuleFor(p => p.Price)
			.NotEmpty().WithMessage(PlatformResources.EnterPrice)
			.GreaterThanOrEqualTo(0).WithMessage(PlatformResources.PriceMustBeGreaterThanOrEqualToZero);

		RuleFor(p => p.Image)
			.NotNull().WithMessage(PlatformResources.PickImage);
	}
}