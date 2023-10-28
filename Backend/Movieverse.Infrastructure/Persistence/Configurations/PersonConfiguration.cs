using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
	public void Configure(EntityTypeBuilder<Person> builder)
	{
		ConfigurePersonTable(builder);
		ConfigurePersonContentIdsTable(builder);
		ConfigurePersonMediaIdsTable(builder);
		ConfigureInformation(builder);
		ConfigureValueObjects(builder);
	}
	
	private static void ConfigurePersonTable(EntityTypeBuilder<Person> builder)
	{
		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id)
			.HasConversion(
				x => x.Value,
				x => PersonId.Create(x));
		
		var converter = new ValueConverter<ContentId?, Guid?>
		(
			x => x == null ? null : x.Value,
			x => x == null ? null : ContentId.Create(x.Value)
		);
		
		builder.Property(p => p.PictureId)
			.HasConversion(converter);
		
		builder.Property(p => p.FunFacts)
			.HasMaxLength(Constants.descriptionLength);
		
		builder.Property(p => p.Biography)
			.HasMaxLength(Constants.descriptionLength);

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
		
		builder.Metadata.FindNavigation(nameof(Person.ContentIds))!
			.SetPropertyAccessMode(PropertyAccessMode.Field);
	}
	
	private static void ConfigurePersonMediaIdsTable(EntityTypeBuilder<Person> builder)
	{
		builder.OwnsMany(p => p.MediaIds, contentIdsBuilder =>
		{
			contentIdsBuilder.ToTable($"{nameof(Person)}{nameof(Person.MediaIds)}");
			
			contentIdsBuilder.WithOwner().HasForeignKey("PersonId");
			
			contentIdsBuilder.HasKey("Id");
			
			contentIdsBuilder.Property(x => x.Value)
				.ValueGeneratedNever()
				.HasColumnName("MediaId");
		});
		
		builder.Metadata.FindNavigation(nameof(Person.MediaIds))!
			.SetPropertyAccessMode(PropertyAccessMode.Field);
	}
	
	private static void ConfigureInformation(EntityTypeBuilder<Person> builder)
	{
		builder.OwnsOne(u => u.Information, informationBuilder =>
		{
			informationBuilder.Property(i => i.FirstName)
				.HasMaxLength(Constants.nameLength);
			
			informationBuilder.Property(i => i.LastName)
				.HasMaxLength(Constants.nameLength);
		});
	}

	private static void ConfigureValueObjects(EntityTypeBuilder<Person> builder)
	{
		builder.OwnsOne(p => p.LifeHistory, lifeHistoryConfiguration =>
		{
			lifeHistoryConfiguration.Property(lh => lh.BirthPlace)
				.HasMaxLength(Constants.locationLength);
			
			lifeHistoryConfiguration.Property(lh => lh.DeathPlace)
				.HasMaxLength(Constants.locationLength);
		});
	}
}