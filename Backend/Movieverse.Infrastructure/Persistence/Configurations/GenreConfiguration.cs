﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
	public void Configure(EntityTypeBuilder<Genre> builder)
	{
		ConfigureGenreTable(builder);
		ConfigureGenreMediaIdsTable(builder);
	}
	
	private static void ConfigureGenreTable(EntityTypeBuilder<Genre> builder)
	{
		builder.HasKey(g => g.Id);
		
		builder.Property(g => g.Id)
			.HasConversion(
				id => id.Value,
				value => GenreId.Create(value));

		builder.Property(g => g.Name)
			.HasMaxLength(Constants.nameLength);
		
		builder.Property(g => g.Description)
			.HasMaxLength(Constants.descriptionLength);
	}
	
	private static void ConfigureGenreMediaIdsTable(EntityTypeBuilder<Genre> builder)
	{
		builder.OwnsMany(g => g.MediaIds, mediaIdsBuilder =>
		{
			mediaIdsBuilder.ToTable($"{nameof(Genre)}{nameof(Genre.MediaIds)}");

			mediaIdsBuilder.WithOwner().HasForeignKey("GenreId");
			
			mediaIdsBuilder.HasKey("Id");
			
			mediaIdsBuilder.Property(x => x.Value)
				.ValueGeneratedNever()
				.HasColumnName("MediaId");
		});
	}
}