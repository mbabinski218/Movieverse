using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Infrastructure.Common;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
	public void Configure(EntityTypeBuilder<Movie> builder)
	{
		ConfigureMovieTable(builder);
	}
	
	private static void ConfigureMovieTable(EntityTypeBuilder<Movie> builder)
	{
		builder.Property(m => m.SequelTitle)
			.HasMaxLength(Constants.maxTitleLength);
		
		builder.Property(m => m.PrequelTitle)
			.HasMaxLength(Constants.maxTitleLength);

		builder.Property(m => m.SequelId)
			.HasConversion(EfExtensions.nullableAggregateRootIdConverter);

		builder.Property(m => m.PrequelId)
			.HasConversion(EfExtensions.nullableAggregateRootIdConverter);

		builder.OwnsOne(m => m.BasicStatistics);
		
		builder.OwnsOne(m => m.Details);
		
		builder.OwnsOne(m => m.TechnicalSpecs);
	}
}