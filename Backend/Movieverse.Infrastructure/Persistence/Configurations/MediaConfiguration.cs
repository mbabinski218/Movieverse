using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;
using Movieverse.Infrastructure.Common;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class MediaConfiguration : IEntityTypeConfiguration<Media>
{
	public void Configure(EntityTypeBuilder<Media> builder)
	{
		ConfigureMediaTable(builder);
		ConfigureMediaPlatformIdsTable(builder);
		ConfigureMediaContentIdsTable(builder);
		ConfigureMediaGenreIdsTable(builder);
		ConfigureReviewsTable(builder);
		ConfigureStaffTable(builder);
		ConfigureAdvancedStatisticsTable(builder);
	}

	private static void ConfigureMediaTable(EntityTypeBuilder<Media> builder)
	{
		builder.UseTphMappingStrategy();
		
		builder.HasKey(m => m.Id);

		builder.Property(m => m.Title)
			.HasMaxLength(Constants.maxTitleLength);

		builder.Property(m => m.PosterId)
			.HasConversion(EfExtensions.nullableAggregateRootIdConverter);

		builder.Property(m => m.TrailerId)
			.HasConversion(EfExtensions.nullableAggregateRootIdConverter);
	}
	
	private static void ConfigureMediaPlatformIdsTable(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsMany(m => m.PlatformIds, platformIdsBuilder =>
		{
			platformIdsBuilder.ToTable($"{nameof(Media)}{nameof(Media.PlatformIds)}");
			
			platformIdsBuilder.WithOwner().HasForeignKey("MediaId");

			platformIdsBuilder.HasKey("Id");
			
			platformIdsBuilder.Property(x => x.Value)
				.ValueGeneratedNever()
				.HasColumnName("PlatformId");
		});
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
	}
	
	private static void ConfigureMediaGenreIdsTable(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsMany(m => m.GenreIds, genreIdsBuilder =>
		{
			genreIdsBuilder.ToTable($"{nameof(Media)}{nameof(Media.GenreIds)}");
			
			genreIdsBuilder.WithOwner().HasForeignKey("MediaId");

			genreIdsBuilder.HasKey("Id");
			
			genreIdsBuilder.Property(x => x.Value)
				.ValueGeneratedNever()
				.HasColumnName("GenreId");
		});
	}
	
	private static void ConfigureReviewsTable(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsMany(m => m.Reviews, reviewBuilder =>
		{
			reviewBuilder.ToTable($"{nameof(Media)}{nameof(Media.Reviews)}");
			
			reviewBuilder.HasKey(nameof(Review.Id));
			
			reviewBuilder.Property(r => r.UserId)
				.HasConversion(EfExtensions.aggregateRootIdConverter);
			
			reviewBuilder.Property(r => r.UserName)
				.HasMaxLength(Constants.maxNameLength);
			
			reviewBuilder.Property(r => r.Title)
				.HasMaxLength(Constants.maxTitleLength);
			
			reviewBuilder.Property(r => r.Content)
				.HasMaxLength(Constants.maxReviewLength);
		});
	}
	
	private static void ConfigureStaffTable(EntityTypeBuilder<Media> builder)
	{
		builder.OwnsMany(m => m.Staff, staffBuilder =>
		{
			staffBuilder.HasKey(nameof(Staff.Id));
			
			staffBuilder.Property(s => s.PersonId)
				.HasConversion(EfExtensions.aggregateRootIdConverter);
		});
	}
	
	private static void ConfigureAdvancedStatisticsTable(EntityTypeBuilder<Media> builder)
	{
		builder.HasOne(m => m.AdvancedStatistics)
			.WithOne(s => s.Media)
			.HasForeignKey<Statistics>("MediaId");
	}
}