using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;
using Movieverse.Infrastructure.Common;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class StatisticsConfiguration : IEntityTypeConfiguration<Statistics>
{
	public void Configure(EntityTypeBuilder<Statistics> builder)
	{
		ConfigureStatisticsTable(builder);
		ConfigurePopularityTable(builder);
		ConfigureValueObjects(builder);
	}

	private static void ConfigureStatisticsTable(EntityTypeBuilder<Statistics> builder)
	{
		builder.HasKey(s => s.Id);
		
		builder.OwnsOne(s => s.BoxOffice);
	}
	
	private static void ConfigurePopularityTable(EntityTypeBuilder<Statistics> builder)
	{
		builder.OwnsMany(s => s.Popularity, popularityBuilder =>
		{
			popularityBuilder.HasKey(nameof(Popularity.Id));

			popularityBuilder.OwnsOne(p => p.BasicStatistics);
		});
	}
	
	private static void ConfigureValueObjects(EntityTypeBuilder<Statistics> builder)
	{
		builder.OwnsOne(s => s.BoxOffice, boxOfficeConfiguration =>
		{
			boxOfficeConfiguration.Property(bo => bo.Budget)
				.HasMaxLength(Constants.precision);
			
			boxOfficeConfiguration.Property(bo => bo.Revenue)
				.HasMaxLength(Constants.precision);
			
			boxOfficeConfiguration.Property(bo => bo.GrossUs)
				.HasMaxLength(Constants.precision);
			
			boxOfficeConfiguration.Property(bo => bo.OpeningWeekendUs)
				.HasMaxLength(Constants.precision);
			
			boxOfficeConfiguration.Property(bo => bo.GrossWorldwide)
				.HasMaxLength(Constants.precision);
			
			boxOfficeConfiguration.Property(bo => bo.OpeningWeekendWorldwide)
				.HasMaxLength(Constants.precision);
		});
	}
}

public sealed class StatisticsAwardConfiguration : IEntityTypeConfiguration<StatisticsAward>
{
	public void Configure(EntityTypeBuilder<StatisticsAward> builder)
	{
		builder
			.HasOne(sa => sa.Award)
			.WithMany(a => a.StatisticsAwards)
			.HasForeignKey("AwardId");

		builder
			.HasOne(sa => sa.Statistics)
			.WithMany(s => s.StatisticsAwards)
			.HasForeignKey("StatisticsId");
		
		builder.HasKey("StatisticsId", "AwardId");
	}
}

public sealed class AwardConfiguration : IEntityTypeConfiguration<Award>
{
	public void Configure(EntityTypeBuilder<Award> builder)
	{
		builder.HasKey(a => a.Id);

		builder.Property(a => a.ImageId)
			.HasConversion(EfExtensions.nullableAggregateRootIdConverter);
		
		builder.Property(a => a.Name)
			.HasMaxLength(Constants.nameLength);
		
		builder.Property(a => a.Description)
			.HasMaxLength(Constants.descriptionLength);
	}
}