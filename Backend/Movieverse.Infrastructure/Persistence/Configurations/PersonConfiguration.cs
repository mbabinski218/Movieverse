using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
	public void Configure(EntityTypeBuilder<Person> builder)
	{
		ConfigurePersonTable(builder);
		ConfigurePersonContentIdsTable(builder);
		ConfigureInformation(builder);
		ConfigureValueObjects(builder);
	}
	
	private static void ConfigurePersonTable(EntityTypeBuilder<Person> builder)
	{
		builder.HasKey(p => p.Id);
		
		builder.Property(p => p.FunFacts)
			.HasMaxLength(Constants.maxDescriptionLength);
		
		builder.Property(p => p.Biography)
			.HasMaxLength(Constants.maxDescriptionLength);

		builder.OwnsOne(p => p.Information);

		builder.OwnsOne(p => p.LifeHistory);
	}
	
	private static void ConfigurePersonContentIdsTable(EntityTypeBuilder<Person> builder)
	{
		builder.OwnsMany(p => p.ContentIds, contentIdsBuilder =>
		{
			contentIdsBuilder.ToTable($"{nameof(Person)}{nameof(Person.ContentIds)}");
			
			contentIdsBuilder.WithOwner().HasForeignKey("PersonId");
			
			contentIdsBuilder.HasKey("Id");
			
			contentIdsBuilder.Property(x => x.Value)
				.ValueGeneratedNever()
				.HasColumnName("ContentId");
		});
	}
	
	private static void ConfigureInformation(EntityTypeBuilder<Person> builder)
	{
		builder.OwnsOne(u => u.Information, informationBuilder =>
		{
			informationBuilder.Property(i => i.FirstName)
				.HasMaxLength(Constants.maxNameLength);
			
			informationBuilder.Property(i => i.LastName)
				.HasMaxLength(Constants.maxNameLength);
		});
	}

	private static void ConfigureValueObjects(EntityTypeBuilder<Person> builder)
	{
		builder.OwnsOne(p => p.LifeHistory, lifeHistoryConfiguration =>
		{
			lifeHistoryConfiguration.Property(lh => lh.BirthPlace)
				.HasMaxLength(Constants.maxLocationLength);
			
			lifeHistoryConfiguration.Property(lh => lh.DeathPlace)
				.HasMaxLength(Constants.maxLocationLength);
		});
	}
}