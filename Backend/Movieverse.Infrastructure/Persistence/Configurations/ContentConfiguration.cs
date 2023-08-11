using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class ContentConfiguration : IEntityTypeConfiguration<Content>
{
	public void Configure(EntityTypeBuilder<Content> builder)
	{
		ConfigureContentTable(builder);
	}

	private static void ConfigureContentTable(EntityTypeBuilder<Content> builder)
	{
		builder.HasKey(c => c.Id);

		builder.Property(c => c.Title)
			.HasMaxLength(Constants.maxTitleLength);

		builder.Property(c => c.Path)
			.HasMaxLength(Constants.maxPathLength);

		builder.Property(c => c.ContentType)
			.HasMaxLength(Constants.maxContentTypeLenght);
	}
}