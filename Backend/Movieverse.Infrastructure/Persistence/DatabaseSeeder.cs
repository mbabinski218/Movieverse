using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Infrastructure.Persistence;

public sealed class DatabaseSeeder : IDatabaseSeeder
{
	private readonly ILogger<DatabaseSeeder> _logger;
	private readonly AppDbContext _dbContext;
	private readonly RoleManager<IdentityUserRole> _roleManager;

	public DatabaseSeeder(ILogger<DatabaseSeeder> logger, AppDbContext dbContext, RoleManager<IdentityUserRole> roleManager)
	{
		_logger = logger;
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

			if (await _dbContext.Roles.AnyAsync(r => r.Name == role.Name)) continue;
			
			_logger.LogInformation("Seeding role: {supportedRole}.", supportedRole);
			await _roleManager.CreateAsync(role);
		}
	}
}