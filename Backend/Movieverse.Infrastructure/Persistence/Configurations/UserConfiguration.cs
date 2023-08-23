using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Entities;
using Movieverse.Infrastructure.Common;

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

		builder.Property(u => u.AvatarId)
			.HasConversion(EfExtensions.nullableAggregateRootIdConverter);

		builder.Property(u => u.PersonId)
			.HasConversion(EfExtensions.nullableAggregateRootIdConverter);

		builder.OwnsOne(u => u.Information);
	}
	
	private static void ConfigureDataTable(EntityTypeBuilder<User> builder)
	{
		builder.OwnsMany(u => u.MediaInfos, dataBuilder =>
		{
			dataBuilder.Property(mi => mi.MediaId)
				.HasConversion(EfExtensions.aggregateRootIdConverter);

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
	}
}