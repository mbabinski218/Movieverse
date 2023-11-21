using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class SeriesConfiguration : IEntityTypeConfiguration<Series>
{
	public void Configure(EntityTypeBuilder<Series> builder)
	{
		ConfigureSeasonsTable(builder);
		ConfigureValueObjects(builder);
	}
	
	private static void ConfigureSeasonsTable(EntityTypeBuilder<Series> builder)
	{
		builder.OwnsOne(m => m.BasicStatistics, basicStatisticsBuilder =>
		{
			basicStatisticsBuilder.Property(bs => bs.Rating)
				.HasPrecision(Constants.ratingPrecision, Constants.ratingScale);
		});
		
		builder.OwnsOne(m => m.Details);
		
		builder.OwnsOne(m => m.TechnicalSpecs);
		
		builder.OwnsMany(s => s.Seasons, seasonsBuilder =>
		{
			seasonsBuilder.HasKey(nameof(Season.Id));

			seasonsBuilder.OwnsMany(se => se.Episodes, episodesBuilder =>
			{
				episodesBuilder.HasKey(nameof(Episode.Id));

				episodesBuilder.Property(e => e.Title)
					.HasMaxLength(Constants.titleLength);

				episodesBuilder.OwnsOne(e => e.Details);
			});
		});
	}

	private static void ConfigureValueObjects(EntityTypeBuilder<Series> builder)
	{
		builder.OwnsOne(s => s.Details, detailsConfiguration =>
		{
			detailsConfiguration.Property(d => d.Language)
				.HasMaxLength(Constants.languageLength);

			detailsConfiguration.Property(d => d.FilmingLocations)
				.HasMaxLength(Constants.locationLength);

			detailsConfiguration.Property(d => d.Storyline)
				.HasMaxLength(Constants.descriptionLength);

			detailsConfiguration.Property(d => d.Tagline)
				.HasMaxLength(Constants.descriptionLength);

			detailsConfiguration.Property(d => d.CountryOfOrigin)
				.HasMaxLength(Constants.locationLength);
		});
	}
}