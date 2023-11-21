using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.Entities;

namespace Movieverse.Infrastructure.Persistence;

public sealed class DatabaseSeeder : IDatabaseSeeder
{
	private readonly ILogger<DatabaseSeeder> _logger;
	private readonly Context _dbContext;
	private readonly IUnitOfWork _unitOfWork;
	private readonly RoleManager<IdentityUserRole> _roleManager;

	public DatabaseSeeder(ILogger<DatabaseSeeder> logger, Context dbContext, RoleManager<IdentityUserRole> roleManager, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_dbContext = dbContext;
		_roleManager = roleManager;
		_unitOfWork = unitOfWork;
	}

	public async Task SeedAsync()
	{
		if (!await _dbContext.Database.CanConnectAsync())
		{
			return;
		}

		await SeedRoles();
		await SeedGenres();

		await _unitOfWork.SaveChangesAsync();
	}

	private async Task SeedRoles()
	{
		foreach (var supportedRole in UserRoleExtensions.GetNames())
		{
			var role = new IdentityUserRole(supportedRole);

			if (await _dbContext.Roles.AnyAsync(r => r.Name == role.Name).ConfigureAwait(false)) continue;
			
			_logger.LogDebug("Seeding role: {supportedRole}.", supportedRole);
			
			await _roleManager.CreateAsync(role);

			var claim = new Claim(ClaimNames.role, supportedRole);
			await _roleManager.AddClaimAsync(role, claim);
		}
	}
	
	private async Task SeedGenres()
	{
		if (_dbContext.Genres.Any())
		{
			return;
		}
		
		var genreNames = new List<string>
		{
			"Action", "Adventure", "Animation", "Biography", "Comedy", 
			"Crime", "Documentary", "Drama", "Family", "Fantasy",
			"Film-Noir", "History", "Horror", "Music", "Musical",
			"Mystery", "Romance", "Sci-Fi", "Sport", "Thriller",
			"War", "Western", "Game-Show", "News", "Reality-TV",
			"Short", "Talk-Show", "Lifestyle", "Cars", "Travel",
			"Food", "Home", "Garden", "Fashion", "Health", "Fitness",
			"DIY", "Crafts", "Finance", "Business", "Technology", "Science",
			"Nature", "Animals"
		};
		
		var genres = genreNames.Select(Genre.Create);
		
		await _dbContext.Genres.AddRangeAsync(genres);
	}
}