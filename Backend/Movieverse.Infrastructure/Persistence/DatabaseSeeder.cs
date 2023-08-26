using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Infrastructure.Persistence;

public sealed class DatabaseSeeder : IDatabaseSeeder
{
	private readonly ILogger<DatabaseSeeder> _logger;
	private readonly AppDbContext _dbContext;
	private readonly RoleManager<IdentityUserRole> _roleManager;
	private readonly IUserRepository _userRepository;

	public DatabaseSeeder(ILogger<DatabaseSeeder> logger, AppDbContext dbContext, RoleManager<IdentityUserRole> roleManager, 
		IUserRepository userRepository)
	{
		_logger = logger;
		_dbContext = dbContext;
		_roleManager = roleManager;
		_userRepository = userRepository;
	}

	public async Task SeedAsync()
	{
		if (!await _dbContext.Database.CanConnectAsync().ConfigureAwait(false))
		{
			return;
		}

		await SeedRoles();
		await SeedUsers();
	}

	private async Task SeedRoles()
	{
		foreach (var supportedRole in UserRoleExtensions.GetNames())
		{
			var role = new IdentityUserRole(supportedRole);

			if (await _dbContext.Roles.AnyAsync(r => r.Name == role.Name).ConfigureAwait(false)) continue;
			
			_logger.LogInformation("Seeding role: {supportedRole}.", supportedRole);
			
			await _roleManager.CreateAsync(role).ConfigureAwait(false);

			var claim = new Claim(ClaimNames.role, supportedRole);
			await _roleManager.AddClaimAsync(role, claim).ConfigureAwait(false);
		}
	}
	
	private async Task SeedUsers()
	{
		if (await _dbContext.Users.AnyAsync(u => u.Email == "string1@string.pl").ConfigureAwait(false)) return;
		
		var user = User.Create("string1@string.pl", "AA23B", "Mateusz", "Babiński", 22);
		await _userRepository.RegisterAsync(user, "string");
		user = User.Create("string2@string.pl", "abbc", "Bartosz", "Babiński", 15);
		await _userRepository.RegisterAsync(user, "string");
		user = User.Create("string3@string.pl", "23AAb", "Tomek", "Kowalski", 18);
		await _userRepository.RegisterAsync(user, "string");
		user = User.Create("string4@string.pl", "aa4", "Tomasz", "Kowalski", 60);
		await _userRepository.RegisterAsync(user, "string");
		user = User.Create("string5@string.pl", "AADF!@g2", "Kasia", "Kowalski", 45);
		await _userRepository.RegisterAsync(user, "string");
		user = User.Create("string6@string.pl", "elogdp", "Ola", "Kowalski", 99);
		await _userRepository.RegisterAsync(user, "string");
		user = User.Create("string7@string.pl", "AA31!", "Jan", "Kowalski", 32);
		await _userRepository.RegisterAsync(user, "string");
		user = User.Create("string8@string.pl", "@23fsdaf", "Paweł", "Kowalski", 18);
		await _userRepository.RegisterAsync(user, "string");
	}
}