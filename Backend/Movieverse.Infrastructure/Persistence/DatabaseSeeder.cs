using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Infrastructure.Persistence;

public sealed class DatabaseSeeder : IDatabaseSeeder
{
	private readonly ILogger<DatabaseSeeder> _logger;
	private readonly Context _dbContext;
	private readonly IUnitOfWork _unitOfWork;
	private readonly RoleManager<IdentityUserRole> _roleManager;
	private readonly IUserRepository _userRepository;
	private readonly IPlatformRepository _platformRepository;
	private readonly IMediaRepository _mediaRepository;

	public DatabaseSeeder(ILogger<DatabaseSeeder> logger, Context dbContext, RoleManager<IdentityUserRole> roleManager, 
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

		await SeedRoles();
		await SeedUsers();
		await SeedPlatforms();
		await SeedMedias();

		await _unitOfWork.SaveChangesAsync();
	}

	private async Task SeedRoles()
	{
		foreach (var supportedRole in UserRoleExtensions.GetNames())
		{
			var role = new IdentityUserRole(supportedRole);

			if (await _dbContext.Roles.AnyAsync(r => r.Name == role.Name).ConfigureAwait(false)) continue;
			
			_logger.LogInformation("Seeding role: {supportedRole}.", supportedRole);
			
			await _roleManager.CreateAsync(role);

			var claim = new Claim(ClaimNames.role, supportedRole);
			await _roleManager.AddClaimAsync(role, claim);
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
		
		var platform1 = Platform.Create(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"),"Netflix",Guid.Parse("9c902bc9-c9bd-4510-93d2-d8fa9cdabb6b"), 50.99m);
		await _platformRepository.AddAsync(platform1);
		var platform2 = Platform.Create(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"),"HBO",Guid.Parse("fab57010-44d3-4d05-a4be-0a9df4b942cb"), 30m);
		await _platformRepository.AddAsync(platform2);
		var platform3 = Platform.Create(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"),"Amazon Prime",Guid.Parse("fab57010-44d3-4d05-a4be-0a9df4b942cb"), 30m);
		await _platformRepository.AddAsync(platform3);
		var platform4 = Platform.Create(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"),"Disney+",Guid.Parse("fab57010-44d3-4d05-a4be-0a9df4b942cb"), 30m);
		await _platformRepository.AddAsync(platform4);
	}
	
	private async Task SeedMedias()
	{
		if (await _dbContext.Medias.AnyAsync().ConfigureAwait(false)) return;
		
		//Movies
		var movie1 = Movie.Create(Guid.Parse("0efb96b1-9db9-4890-bb8b-8dcdd713fd60"), "John Wick 2");
		movie1.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie1.Details = new Details
		{
			Certificate = 18,
			ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31), TimeSpan.Zero)
		};
		await _mediaRepository.AddMovieAsync(movie1);
		
		var movie2 = Movie.Create(Guid.Parse("0efb96b1-9db9-4890-bb8b-8dcdd713fd69"), "Baby Driver");
		movie2.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie2.Details = new Details
		{
			Certificate = 12,
			ReleaseDate =  new DateTimeOffset(new DateTime(2025, 3, 31), TimeSpan.Zero)
		};
		await _mediaRepository.AddMovieAsync(movie2);
		
		var movie3 = Movie.Create(Guid.NewGuid(), "The Wolf of Wall Street");
		movie3.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie3.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 3, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie3);
		
		var movie4 = Movie.Create(Guid.NewGuid(), "Barbie");
		movie4.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		movie4.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2026, 1, 1 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie4);
		
		var movie5 = Movie.Create(Guid.NewGuid(), "Oppenheimer");
		movie5.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		movie5.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie5);
		
		var movie6 = Movie.Create(Guid.NewGuid(), "The Dark Knight");
		movie6.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		movie6.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie6.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie6);
		
		var movie7 = Movie.Create(Guid.NewGuid(), "Joker");
		movie7.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		movie7.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie7);
		
		var movie8 = Movie.Create(Guid.NewGuid(), "Deadpool");
		movie8.AddPlatform(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		movie8.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie8);
		
		var movie9 = Movie.Create(Guid.NewGuid(), "Need for Speed");
		movie9.AddPlatform(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		movie9.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2028, 3, 3 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie9);
		
		var movie10 = Movie.Create(Guid.NewGuid(), "The Fast and the Furious");
		movie10.AddPlatform(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		movie10.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2028, 3, 3 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie10);
		
		var movie11 = Movie.Create(Guid.NewGuid(), "John Wick 4");
		movie11.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		movie11.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie11);
		
		var movie12 = Movie.Create(Guid.NewGuid(), "Avatar 5");
		movie12.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		movie12.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie12.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2031, 10, 13 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie12);
		
		var movie13 = Movie.Create(Guid.NewGuid(), "Avengers: Endgame");
		movie13.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		movie13.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 10, 13 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie13);
		
		//Series
		//Netflix
		var series1 = Series.Create(Guid.Parse("f557684a-4755-11ee-be56-0242ac120002"), "Witcher", 4);
		series1.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series1.Details = new Details
		{
			Certificate = 18,
			ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6), TimeSpan.Zero)
		};
		await _mediaRepository.AddSeriesAsync(series1);
		
		var series2 = Series.Create(Guid.Parse("5daedcde-5255-11ee-be56-0242ac120009"), "The Queen's Gambit", 1);
		series2.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series2.Details = new Details
		{
			Certificate = 7,
			ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11), TimeSpan.Zero)
		};
		await _mediaRepository.AddSeriesAsync(series2);
		
		var series3 = Series.Create(Guid.NewGuid(), "Peaky Blinders", 6);
		series3.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series3.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series3);
		
		var series4 = Series.Create(Guid.NewGuid(), "Arcane", 1);
		series4.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series4.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series4);
		
		var series5 = Series.Create(Guid.NewGuid(), "Breaking Bad", 5);
		series5.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series5.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series5);
		
		var series6 = Series.Create(Guid.NewGuid(), "Lucifer", 25);
		series6.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series6.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series6);
		
		var series7 = Series.Create(Guid.NewGuid(), "Power Rangers", 1);
		series7.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series7.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2018, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series7);
		
		var series8 = Series.Create(Guid.NewGuid(), "The Days", 2);
		series8.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series8.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series8);
		
		var series9 = Series.Create(Guid.NewGuid(), "Resident Evil", 12);
		series9.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series9.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series9);
		
		var series10 = Series.Create(Guid.NewGuid(), "Flash", 1);
		series10.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series10.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series10);
		
		var series11 = Series.Create(Guid.NewGuid(), "Sherlock", 2);
		series11.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series11.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series11);
		
		var series12 = Series.Create(Guid.NewGuid(), "Stranger Things", 5);
		series12.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series12.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series12);
		
		//HBO
		var series13 = Series.Create(Guid.NewGuid(), "Euphoria", 4);
		series13.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series13.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series13);
		
		var series14 = Series.Create(Guid.NewGuid(), "Succession", 1);
		series14.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series14.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series14);
		
		var series15 = Series.Create(Guid.NewGuid(), "Chernobyl", 1);
		series15.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series15.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series15);
		
		var series16 = Series.Create(Guid.NewGuid(), "The Last of Us", 6);
		series16.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series16.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series16.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series16);
		
		var series17 = Series.Create(Guid.NewGuid(), "Peacemaker", 5);
		series17.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series17.AddPlatform(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series17.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series17);
		
		var series18 = Series.Create(Guid.NewGuid(), "Game of Thrones", 8);
		series18.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series18.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series18);
		
		var series19 = Series.Create(Guid.NewGuid(), "Ninjago", 1);
		series19.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series19.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series19);
		
		var series20 = Series.Create(Guid.NewGuid(), "Billions", 2);
		series20.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series20.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series20);
		
		var series21 = Series.Create(Guid.NewGuid(), "Scooby Doo", 12);
		series21.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series21.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series21);
		
		var series22 = Series.Create(Guid.NewGuid(), "Watchman", 1);
		series22.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series22.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series22);
		
		var series23 = Series.Create(Guid.NewGuid(), "Rick and Morty", 2);
		series23.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series23.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series23);
		
		var series24 = Series.Create(Guid.NewGuid(), "Ballers", 5);
		series24.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series24.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2022, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series24);
		
		//Amazon Prime
		var series25 = Series.Create(Guid.NewGuid(), "The Boys", 4);
		series25.AddPlatform(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series25.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series25);
		
		var series26 = Series.Create(Guid.NewGuid(), "Ninjago", 1);
		series26.AddPlatform(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series26.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series26);
		
		var series27 = Series.Create(Guid.NewGuid(), "Hunters", 2);
		series27.AddPlatform(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series27.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series27);
		
		var series28 = Series.Create(Guid.NewGuid(), "Good Omens", 12);
		series28.AddPlatform(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series28.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series28);
		
		var series29 = Series.Create(Guid.NewGuid(), "Latarnik", 127);
		series29.AddPlatform(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series29.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(1977, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series29);
		
		//Disney+
		var series30 = Series.Create(Guid.NewGuid(), "Loki", 4);
		series30.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series30.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series30);
		
		var series31 = Series.Create(Guid.NewGuid(), "Ahsoka", 1);
		series31.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series31.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series31);
		
		var series32 = Series.Create(Guid.NewGuid(), "The Bear", 2);
		series32.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series32.AddPlatform(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series32.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series32);
		
		var series33 = Series.Create(Guid.NewGuid(), "Moon Knight", 12);
		series33.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series33.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series33);
		
		var series34 = Series.Create(Guid.NewGuid(), "Punisher", 127);
		series34.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series34.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(1977, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series34);
		
		var series35 = Series.Create(Guid.NewGuid(), "Hawkeye", 4);
		series35.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series35.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series35);
		
		var series36 = Series.Create(Guid.NewGuid(), "The Mandalorian", 1);
		series36.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series36.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series36);
		
		var series37 = Series.Create(Guid.NewGuid(), "Secret Invasion", 2);
		series37.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series37.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series37);
		
		var series38 = Series.Create(Guid.NewGuid(), "Daredevil", 12);
		series38.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series38.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series38);
		
		var series39 = Series.Create(Guid.NewGuid(), "Andor", 127);
		series39.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series39.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(1977, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series39);
		
		var series40 = Series.Create(Guid.NewGuid(), "Bones", 127);
		series40.AddPlatform(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series40.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2001, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series40);
	}
}