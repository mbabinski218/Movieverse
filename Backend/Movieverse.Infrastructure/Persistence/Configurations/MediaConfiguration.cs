using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class MediaConfiguration : IEntityTypeConfiguration<Media>
{
	public void Configure(EntityTypeBuilder<Media> builder)
	{
		ConfigureMediaTable(builder);
		ConfigureMediaPlatformIdsTable(builder);
		ConfigureMediaContentIdsTable(builder);
		ConfigureStaffTable(builder);
		ConfigureReviewsTable(builder);
		ConfigureAdvancedStatisticsTable(builder);
		ConfigureValueObjects(builder);
	}

	private static void ConfigureMediaTable(EntityTypeBuilder<Media> builder)
	{
		builder.UseTphMappingStrategy();
		
		builder.HasKey(m => m.Id);
		
		builder.Property(m => m.Id)
			.HasConversion(
				x => x.Value, 
				x => MediaId.Create(x));
		
		builder.Property(m => m.Title)
			.HasMaxLength(Constants.titleLength);

		var converter = new ValueConverter<ContentId?, Guid?>
		(
			x => x == null ? null : x.Value,
			x => x == null ? null : ContentId.Create(x.Value)
		);
		
		builder.Property(m => m.PosterId)
			.HasConversion(converter);

		builder.Property(m => m.TrailerId)
			.HasConversion(converter);
	}
	
	private static void ConfigureMediaPlatformIdsTable(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsMany(m => m.PlatformIds, platformIdsBuilder =>
		{
			platformIdsBuilder.ToTable($"{nameof(Media)}{nameof(Media.PlatformIds)}");
			
			platformIdsBuilder.WithOwner().HasForeignKey("MediaId");

			platformIdsBuilder.HasKey("Id");

			platformIdsBuilder.Property(x => x.Value)
				.HasColumnName("PlatformId")
				.ValueGeneratedNever();
		});

		builder.Metadata.FindNavigation(nameof(Media.PlatformIds))!
			.SetPropertyAccessMode(PropertyAccessMode.Field);
	}
	
	private static void ConfigureMediaContentIdsTable(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsMany(m => m.ContentIds, contentIdsBuilder =>
		{
			contentIdsBuilder.ToTable($"{nameof(Media)}{nameof(Media.ContentIds)}");
			
			contentIdsBuilder.WithOwner().HasForeignKey("MediaId");

			contentIdsBuilder.HasKey("Id");
			
			contentIdsBuilder.Property(x => x.Value)
				.ValueGeneratedNever()
				.HasColumnName("ContentId");
		});
		
		builder.Metadata.FindNavigation(nameof(Media.ContentIds))!
			.SetPropertyAccessMode(PropertyAccessMode.Field);
	}
	
	private static void ConfigureStaffTable(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsMany(m => m.Staff, staffBuilder =>
		{
			staffBuilder.HasKey(nameof(Staff.Id));
			
			staffBuilder.Property(s => s.PersonId)
				.HasConversion(
					x => x.Value, 
					x => PersonId.Create(x));
		});
	}
	
	private static void ConfigureAdvancedStatisticsTable(EntityTypeBuilder<Media> builder)
	{
		builder.HasOne(m => m.AdvancedStatistics)
			.WithOne(s => s.Media)
			.HasForeignKey<Statistics>("MediaId");
	}
	
	private static void ConfigureReviewsTable(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsMany(m => m.Reviews, reviewBuilder =>
		{
			reviewBuilder.ToTable($"{nameof(Media.Reviews)}");
		
			reviewBuilder.HasKey(nameof(Review.Id));
		
			reviewBuilder.Property(r => r.UserName)
				.HasMaxLength(Constants.nameLength);
		
			reviewBuilder.Property(r => r.Text)
				.HasMaxLength(Constants.reviewLength);
		});
	}
	
	private static void ConfigureValueObjects(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsOne(m => m.Details, detailsConfiguration =>
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
		
		builder.OwnsOne(m => m.TechnicalSpecs, technicalSpecsConfiguration =>
		{
			technicalSpecsConfiguration.Property(t => t.Camera)
				.HasMaxLength(Constants.technicalSpecsLength);

			technicalSpecsConfiguration.Property(t => t.Color)
				.HasMaxLength(Constants.technicalSpecsLength);
			
			technicalSpecsConfiguration.Property(t => t.AspectRatio)
				.HasMaxLength(Constants.technicalSpecsLength);
			
			technicalSpecsConfiguration.Property(t => t.NegativeFormat)
				.HasMaxLength(Constants.technicalSpecsLength);
			
			technicalSpecsConfiguration.Property(t => t.SoundMix)
				.HasMaxLength(Constants.technicalSpecsLength);
		});
	}
}

public sealed class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
	public void Configure(EntityTypeBuilder<Genre> builder)
	{
		builder.HasKey(g => g.Id);
	}
}