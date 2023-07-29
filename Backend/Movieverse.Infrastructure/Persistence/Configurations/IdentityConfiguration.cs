using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
	{
		builder.ToTable("Roles");
	}
}

public sealed class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
	{
		builder.ToTable("UserClaims");
	}
}

public sealed class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
	{
		builder.ToTable("UserRoles");
	}
}

public sealed class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
	{
		builder.ToTable("UserLogins");
	}
}

public sealed class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
	{
		builder.ToTable("RoleClaims");
	}
}

public sealed class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
	{
		builder.ToTable("UserTokens");
	}
}