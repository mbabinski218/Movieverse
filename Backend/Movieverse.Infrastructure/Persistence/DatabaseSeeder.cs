using Microsoft.AspNetCore.Identity;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Infrastructure.Persistence;

public sealed class DatabaseSeeder : IDatabaseSeeder
{
	private readonly AppDbContext _dbContext;
	private readonly RoleManager<IdentityUserRole> _roleManager;

	public DatabaseSeeder(AppDbContext dbContext, RoleManager<IdentityUserRole> roleManager)
	{
		_dbContext = dbContext;
		_roleManager = roleManager;
	}

	public async Task SeedAsync()
	{
		if (!await _dbContext.Database.CanConnectAsync())
		{
			return;
		}

		await SeedRoles();
	}

	private async Task SeedRoles()
	{
		foreach (var supportedRole in Enum.GetNames(typeof(UserRole)))
		{
			var role = new IdentityUserRole(supportedRole);

			if (!_dbContext.Roles.Contains(role))
			{
				await _roleManager.CreateAsync(role);
			}
		}
	}
}