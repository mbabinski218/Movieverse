using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;

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
				.HasPrecision(Constants.pricePrecision);
			
			boxOfficeConfiguration.Property(bo => bo.Revenue)
				.HasPrecision(Constants.pricePrecision);
			
			boxOfficeConfiguration.Property(bo => bo.GrossUs)
				.HasPrecision(Constants.pricePrecision);
			
			boxOfficeConfiguration.Property(bo => bo.OpeningWeekendUs)
				.HasPrecision(Constants.pricePrecision);
			
			boxOfficeConfiguration.Property(bo => bo.GrossWorldwide)
				.HasPrecision(Constants.pricePrecision);
			
			boxOfficeConfiguration.Property(bo => bo.OpeningWeekendWorldwide)
				.HasPrecision(Constants.pricePrecision);
		});
	}
}