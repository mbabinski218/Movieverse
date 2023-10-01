using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

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
			.HasMaxLength(Constants.titleLength);
		
		builder.Property(m => m.PrequelTitle)
			.HasMaxLength(Constants.titleLength);

		var converter = new ValueConverter<MediaId?, Guid?>
		(
			x => x == null ? null : x.Value,
			x => x == null ? null : MediaId.Create(x.Value)
		);
		
		builder.Property(m => m.SequelId)
			.HasConversion(converter);

		builder.Property(m => m.PrequelId)
			.HasConversion(converter);

		builder.OwnsOne(m => m.BasicStatistics);
		
		builder.OwnsOne(m => m.Details);
		
		builder.OwnsOne(m => m.TechnicalSpecs);
	}
}