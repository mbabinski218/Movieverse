using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		ConfigureUserTable(builder);
		ConfigureDataTable(builder);
		ConfigureInformation(builder);
	}
	
	private static void ConfigureUserTable(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");
		
		builder.HasKey(u => u.Id);

		builder.Property(u => u.UserName)
			.HasMaxLength(Constants.nameLength);

		var avatarConverter = new ValueConverter<ContentId?, Guid?>
		(
			x => x == null ? null : x.Value,
			x => x == null ? null : ContentId.Create(x.Value)
		);
		
		builder.Property(u => u.AvatarId)
			.HasConversion(avatarConverter);

		var personConverter = new ValueConverter<PersonId?, Guid?>
		(
			x => x == null ? null : x.Value,
			x => x == null ? null : PersonId.Create(x.Value)
		);
		
		builder.Property(u => u.PersonId)
			.HasConversion(personConverter);

		builder.OwnsOne(u => u.Information);
	}
	
	private static void ConfigureDataTable(EntityTypeBuilder<User> builder)
	{
		builder.OwnsMany(u => u.MediaInfos, dataBuilder =>
		{
			dataBuilder.Property(mi => mi.MediaId)
				.HasConversion(
					x => x.Value, 
					x => MediaId.Create(x));

			dataBuilder.HasKey(nameof(MediaInfo.Id));
		});
	}

	private static void ConfigureInformation(EntityTypeBuilder<User> builder)
	{
		builder.OwnsOne(u => u.Information, informationBuilder =>
		{
			informationBuilder.Property(i => i.FirstName)
				.HasMaxLength(Constants.nameLength);
			
			informationBuilder.Property(i => i.LastName)
				.HasMaxLength(Constants.nameLength);
		});
		
		builder.OwnsOne(u => u.Subscription);
	}
}