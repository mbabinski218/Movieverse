using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityUserRole>
{
	public void Configure(EntityTypeBuilder<IdentityUserRole> builder)
	{
		builder.ToTable("Roles");
	}
}

public sealed class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
	{
		builder.ToTable("RoleClaims");
	}
}

public sealed class UserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
	{
		builder.ToTable("UserClaims");
	}
}

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
	{
		builder.ToTable("UserRoles");
	}
}

public sealed class UserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
	{
		builder.ToTable("UserTokens");
	}
}