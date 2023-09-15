using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class PlatformConfiguration : IEntityTypeConfiguration<Platform>
{
	public void Configure(EntityTypeBuilder<Platform> builder)
	{
		ConfigurePlatformTable(builder);
		ConfigurePlatformMediaIdsTable(builder);
	}

	private static void ConfigurePlatformTable(EntityTypeBuilder<Platform> builder)
	{
		builder.HasKey(p => p.Id);
		
		builder.Property(p => p.Id)
			.HasConversion(
				x => x.Value, 
				x => PlatformId.Create(x));

		builder.Property(p => p.Name)
			.HasMaxLength(Constants.nameLength);

		builder.Property(p => p.Price)
			.HasPrecision(Constants.precision);

		builder.Property(p => p.LogoId)
			.HasConversion(
				x => x.Value, 
				x => ContentId.Create(x));
	}

	private static void ConfigurePlatformMediaIdsTable(EntityTypeBuilder<Platform> builder)
	{
		builder.OwnsMany(p => p.MediaIds, mediaIdsBuilder =>
		{
			mediaIdsBuilder.ToTable($"{nameof(Platform)}{nameof(Platform.MediaIds)}");

			mediaIdsBuilder.WithOwner().HasForeignKey("PlatformId");

			mediaIdsBuilder.HasKey("Id");

			mediaIdsBuilder.Property(x => x.Value)
				.ValueGeneratedNever()
				.HasColumnName("MediaId");
		});
    }
}
