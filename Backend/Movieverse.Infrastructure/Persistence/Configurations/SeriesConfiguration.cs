﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;
using Movieverse.Infrastructure.Common;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class SeriesConfiguration : IEntityTypeConfiguration<Series>
{
	public void Configure(EntityTypeBuilder<Series> builder)
	{
		ConfigureSeasonsTable(builder);
	}
	
	private static void ConfigureSeasonsTable(EntityTypeBuilder<Series> builder)
	{
		builder.OwnsOne(m => m.BasicStatistics);
		
		builder.OwnsOne(m => m.Details);
		
		builder.OwnsOne(m => m.TechnicalSpecs);
		
		builder.OwnsMany(s => s.Seasons, seasonsBuilder =>
		{
			seasonsBuilder.HasKey(nameof(Season.Id));

			seasonsBuilder.OwnsMany(se => se.Episodes, episodesBuilder =>
			{
				episodesBuilder.HasKey(nameof(Episode.Id));

				episodesBuilder.Property(e => e.Title)
					.HasMaxLength(Constants.maxTitleLength);

				episodesBuilder.OwnsOne(e => e.BasicStatistics);
				
				episodesBuilder.OwnsOne(e => e.Details);
				
				episodesBuilder.OwnsMany(e => e.ContentIds, contentIdsBuilder =>
				{
					contentIdsBuilder.ToTable($"{nameof(Episode)}{nameof(Media.ContentIds)}");
			
					contentIdsBuilder.WithOwner().HasForeignKey("EpisodeId");

					contentIdsBuilder.Property<int>("Id");

					contentIdsBuilder.HasKey("Id");
			
					contentIdsBuilder.Property(x => x.Value)
						.ValueGeneratedNever()
						.HasColumnName("ContentId");
				});
				
				episodesBuilder.OwnsMany(e => e.Reviews, reviewBuilder =>
				{
					reviewBuilder.ToTable($"{nameof(Episode)}{nameof(Episode.Reviews)}");
					
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
			});
		});
	}
}