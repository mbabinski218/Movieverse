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
	}
}