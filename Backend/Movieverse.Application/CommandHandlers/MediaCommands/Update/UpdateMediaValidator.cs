using FluentValidation;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.CommandHandlers.MediaCommands.Update;

public sealed class UpdateMediaValidator : AbstractValidator<UpdateMediaCommand>
{
	public UpdateMediaValidator()
	{
		RuleFor(m => m.Title)
			.Must(s => s!.Length > 0)
			.When(m => m.Title != null)
			.WithMessage(MediaResources.TitleTooShort)
			.Must(s => s!.Length <= Constants.titleLength)
			.When(m => m.Title != null)
			.WithMessage(MediaResources.TitleTooLong);

		RuleFor(m => m.Details!.Storyline)
			.Must(s => s!.Length <= Constants.descriptionLength)
			.When(m => m.Details != null && m.Details.Storyline != null)
			.WithMessage(MediaResources.StorylineTooLong);

		RuleFor(m => m.Details!.Tagline)
			.Must(s => s!.Length <= Constants.descriptionLength)
			.When(m => m.Details != null && m.Details.Tagline != null)
			.WithMessage(MediaResources.DetailsTooLong);
		
		RuleFor(m => m.Details!.Language)
			.Must(s => s!.Length <= Constants.languageLength)
			.When(m => m.Details != null && m.Details.Language != null)
			.WithMessage(MediaResources.DetailsTooLong);
		
		RuleFor(m => m.Details!.CountryOfOrigin)
			.Must(s => s!.Length <= Constants.locationLength)
			.When(m => m.Details != null && m.Details.CountryOfOrigin != null)
			.WithMessage(MediaResources.DetailsTooLong);
		
		RuleFor(m => m.Details!.FilmingLocations)
			.Must(s => s!.Length <= Constants.locationLength)
			.When(m => m.Details != null && m.Details.FilmingLocations != null)
			.WithMessage(MediaResources.DetailsTooLong);
		
		RuleFor(m => m.TechnicalSpecs!.Camera)
			.Must(s => s!.Length <= Constants.technicalSpecsLength)
			.When(m => m.TechnicalSpecs != null && m.TechnicalSpecs.Camera != null)
			.WithMessage(MediaResources.TechnicalSpecsTooLong);
		
		RuleFor(m => m.TechnicalSpecs!.Color)
			.Must(s => s!.Length <= Constants.technicalSpecsLength)
			.When(m => m.TechnicalSpecs != null && m.TechnicalSpecs.Color != null)
			.WithMessage(MediaResources.TechnicalSpecsTooLong);
		
		RuleFor(m => m.TechnicalSpecs!.AspectRatio)
			.Must(s => s!.Length <= Constants.technicalSpecsLength)
			.When(m => m.TechnicalSpecs != null && m.TechnicalSpecs.AspectRatio != null)
			.WithMessage(MediaResources.TechnicalSpecsTooLong);
		
		RuleFor(m => m.TechnicalSpecs!.NegativeFormat)
			.Must(s => s!.Length <= Constants.technicalSpecsLength)
			.When(m => m.TechnicalSpecs != null && m.TechnicalSpecs.NegativeFormat != null)
			.WithMessage(MediaResources.TechnicalSpecsTooLong);
		
		RuleFor(m => m.TechnicalSpecs!.SoundMix)
			.Must(s => s!.Length <= Constants.technicalSpecsLength)
			.When(m => m.TechnicalSpecs != null && m.TechnicalSpecs.SoundMix != null)
			.WithMessage(MediaResources.TechnicalSpecsTooLong);
		
		RuleFor(m => m.Staff)
			.Must(s =>
			{
				var roles = RoleExtensions.GetValues();
				return s!.Select(x => x.Role).All(role => roles.Contains(role));
			})
			.When(m => m.Staff != null && m.Staff.Any())
			.WithMessage(MediaResources.RoleDoesNotExist);
	}
}