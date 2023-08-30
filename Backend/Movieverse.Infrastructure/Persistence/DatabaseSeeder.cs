using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Infrastructure.Persistence;

public sealed class DatabaseSeeder : IDatabaseSeeder
{
	private readonly ILogger<DatabaseSeeder> _logger;
	private readonly AppDbContext _dbContext;
	private readonly IUnitOfWork _unitOfWork;
	private readonly RoleManager<IdentityUserRole> _roleManager;
	private readonly IUserRepository _userRepository;
	private readonly IPlatformRepository _platformRepository;
	private readonly IMediaRepository _mediaRepository;

	public DatabaseSeeder(ILogger<DatabaseSeeder> logger, AppDbContext dbContext, RoleManager<IdentityUserRole> roleManager, 
		IUserRepository userRepository, IPlatformRepository platformRepository, IUnitOfWork unitOfWork, IMediaRepository mediaRepository)
	{
		_logger = logger;
		_dbContext = dbContext;
		_roleManager = roleManager;
		_userRepository = userRepository;
		_platformRepository = platformRepository;
		_unitOfWork = unitOfWork;
		_mediaRepository = mediaRepository;
	}

	public async Task SeedAsync()
	{
		if (!await _dbContext.Database.CanConnectAsync().ConfigureAwait(false))
		{
			return;
		}

		await SeedRoles().ConfigureAwait(false);
		await SeedUsers().ConfigureAwait(false);
		await SeedPlatforms().ConfigureAwait(false);
		await SeedMedias().ConfigureAwait(false);

		await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
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
		if (await _dbContext.Users.AnyAsync().ConfigureAwait(false)) return;
		
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
	
	private async Task SeedPlatforms()
	{
		if (await _dbContext.Platforms.AnyAsync().ConfigureAwait(false)) return;
		
		var platform = Platform.Create(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"),"Netflix",Guid.Parse("9c902bc9-c9bd-4510-93d2-d8fa9cdabb6b"), 50.99m);
		await _platformRepository.AddAsync(platform);
		platform = Platform.Create(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"),"HBO",Guid.Parse("fab57010-44d3-4d05-a4be-0a9df4b942cb"), 30m);
		await _platformRepository.AddAsync(platform);
	}
	
	private async Task SeedMedias()
	{
		if (await _dbContext.Medias.AnyAsync().ConfigureAwait(false)) return;
		
		var movie = Movie.Create(Guid.Parse("0efb96b1-9db9-4890-bb8b-8dcdd713fd60"), "John Wick 4");
		await _mediaRepository.AddMovieAsync(movie).ConfigureAwait(false);
		
		var series = Series.Create(Guid.Parse("f557684a-4755-11ee-be56-0242ac120002"), "Oppenheimer", 1);
		await _mediaRepository.AddSeriesAsync(series).ConfigureAwait(false);
	}
}