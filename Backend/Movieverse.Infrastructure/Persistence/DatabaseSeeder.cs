using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects;

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
		movie1.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie1.Details = new Details
		{
			Certificate = 18,
			ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31), TimeSpan.Zero)
		};
		await _mediaRepository.AddMovieAsync(movie1).ConfigureAwait(false);
		
		var movie2 = Movie.Create(Guid.Parse("0efb96b1-9db9-4890-bb8b-8dcdd713fd69"), "Baby Driver");
		movie2.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie2.Details = new Details
		{
			Certificate = 12,
			ReleaseDate =  new DateTimeOffset(new DateTime(2025, 3, 31), TimeSpan.Zero)
		};
		await _mediaRepository.AddMovieAsync(movie2).ConfigureAwait(false);
		
		var movie3 = Movie.Create(Guid.NewGuid(), "The Wolf of Wall Street");
		movie3.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie3.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 3, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie3).ConfigureAwait(false);
		
		var movie4 = Movie.Create(Guid.NewGuid(), "Barbie");
		movie4.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		movie4.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2026, 1, 1 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie4).ConfigureAwait(false);
		
		var movie5 = Movie.Create(Guid.NewGuid(), "Oppenheimer");
		movie5.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		movie5.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie5).ConfigureAwait(false);
		
		var movie6 = Movie.Create(Guid.NewGuid(), "The Dark Knight");
		movie6.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		movie6.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie6.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie6).ConfigureAwait(false);
		
		var movie7 = Movie.Create(Guid.NewGuid(), "Joker");
		movie7.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		movie7.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie7).ConfigureAwait(false);
		
		var movie8 = Movie.Create(Guid.NewGuid(), "Deadpool");
		movie8.AddPlatformId(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		movie8.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie8).ConfigureAwait(false);
		
		var movie9 = Movie.Create(Guid.NewGuid(), "Need for Speed");
		movie9.AddPlatformId(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		movie9.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2028, 3, 3 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie9).ConfigureAwait(false);
		
		var movie10 = Movie.Create(Guid.NewGuid(), "The Fast and the Furious");
		movie10.AddPlatformId(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		movie10.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2028, 3, 3 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie10).ConfigureAwait(false);
		
		var movie11 = Movie.Create(Guid.NewGuid(), "John Wick 4");
		movie11.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		movie11.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 12, 31 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie11).ConfigureAwait(false);
		
		var movie12 = Movie.Create(Guid.NewGuid(), "Avatar 5");
		movie12.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		movie12.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		movie12.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2031, 10, 13 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie12).ConfigureAwait(false);
		
		var movie13 = Movie.Create(Guid.NewGuid(), "Avengers: Endgame");
		movie13.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		movie13.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 10, 13 ), TimeSpan.Zero) };
		await _mediaRepository.AddMovieAsync(movie13).ConfigureAwait(false);
		
		//Series
		//Netflix
		var series1 = Series.Create(Guid.Parse("f557684a-4755-11ee-be56-0242ac120002"), "Witcher", 4);
		series1.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series1.Details = new Details
		{
			Certificate = 18,
			ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6), TimeSpan.Zero)
		};
		await _mediaRepository.AddSeriesAsync(series1).ConfigureAwait(false);
		
		var series2 = Series.Create(Guid.Parse("5daedcde-5255-11ee-be56-0242ac120009"), "The Queen's Gambit", 1);
		series2.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series2.Details = new Details
		{
			Certificate = 7,
			ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11), TimeSpan.Zero)
		};
		await _mediaRepository.AddSeriesAsync(series2).ConfigureAwait(false);
		
		var series3 = Series.Create(Guid.NewGuid(), "Peaky Blinders", 6);
		series3.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series3.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series3).ConfigureAwait(false);
		
		var series4 = Series.Create(Guid.NewGuid(), "Arcane", 1);
		series4.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series4.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series4).ConfigureAwait(false);
		
		var series5 = Series.Create(Guid.NewGuid(), "Breaking Bad", 5);
		series5.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series5.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series5).ConfigureAwait(false);
		
		var series6 = Series.Create(Guid.NewGuid(), "Lucifer", 25);
		series6.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series6.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series6).ConfigureAwait(false);
		
		var series7 = Series.Create(Guid.NewGuid(), "Power Rangers", 1);
		series7.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series7.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2018, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series7).ConfigureAwait(false);
		
		var series8 = Series.Create(Guid.NewGuid(), "The Days", 2);
		series8.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series8.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series8).ConfigureAwait(false);
		
		var series9 = Series.Create(Guid.NewGuid(), "Resident Evil", 12);
		series9.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series9.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series9).ConfigureAwait(false);
		
		var series10 = Series.Create(Guid.NewGuid(), "Flash", 1);
		series10.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series10.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series10).ConfigureAwait(false);
		
		var series11 = Series.Create(Guid.NewGuid(), "Sherlock", 2);
		series11.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series11.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series11).ConfigureAwait(false);
		
		var series12 = Series.Create(Guid.NewGuid(), "Stranger Things", 5);
		series12.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series12.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series12).ConfigureAwait(false);
		
		//HBO
		var series13 = Series.Create(Guid.NewGuid(), "Euphoria", 4);
		series13.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series13.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series13).ConfigureAwait(false);
		
		var series14 = Series.Create(Guid.NewGuid(), "Succession", 1);
		series14.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series14.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series14).ConfigureAwait(false);
		
		var series15 = Series.Create(Guid.NewGuid(), "Chernobyl", 1);
		series15.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series15.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series15).ConfigureAwait(false);
		
		var series16 = Series.Create(Guid.NewGuid(), "The Last of Us", 6);
		series16.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series16.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series16.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series16).ConfigureAwait(false);
		
		var series17 = Series.Create(Guid.NewGuid(), "Peacemaker", 5);
		series17.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series17.AddPlatformId(Guid.Parse("8d789796-2980-422b-b69b-c27a732c23b1"));
		series17.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series17).ConfigureAwait(false);
		
		var series18 = Series.Create(Guid.NewGuid(), "Game of Thrones", 8);
		series18.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series18.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series18).ConfigureAwait(false);
		
		var series19 = Series.Create(Guid.NewGuid(), "Ninjago", 1);
		series19.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series19.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series19).ConfigureAwait(false);
		
		var series20 = Series.Create(Guid.NewGuid(), "Billions", 2);
		series20.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series20.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series20).ConfigureAwait(false);
		
		var series21 = Series.Create(Guid.NewGuid(), "Scooby Doo", 12);
		series21.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series21.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series21).ConfigureAwait(false);
		
		var series22 = Series.Create(Guid.NewGuid(), "Watchman", 1);
		series22.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series22.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series22).ConfigureAwait(false);
		
		var series23 = Series.Create(Guid.NewGuid(), "Rick and Morty", 2);
		series23.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series23.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series23).ConfigureAwait(false);
		
		var series24 = Series.Create(Guid.NewGuid(), "Ballers", 5);
		series24.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series24.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2022, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series24).ConfigureAwait(false);
		
		//Amazon Prime
		var series25 = Series.Create(Guid.NewGuid(), "The Boys", 4);
		series25.AddPlatformId(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series25.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series25).ConfigureAwait(false);
		
		var series26 = Series.Create(Guid.NewGuid(), "Ninjago", 1);
		series26.AddPlatformId(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series26.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series26).ConfigureAwait(false);
		
		var series27 = Series.Create(Guid.NewGuid(), "Hunters", 2);
		series27.AddPlatformId(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series27.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series27).ConfigureAwait(false);
		
		var series28 = Series.Create(Guid.NewGuid(), "Good Omens", 12);
		series28.AddPlatformId(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series28.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series28).ConfigureAwait(false);
		
		var series29 = Series.Create(Guid.NewGuid(), "Latarnik", 127);
		series29.AddPlatformId(Guid.Parse("5ab8ff73-3866-4fff-b74a-4a17a858bec9"));
		series29.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(1977, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series29).ConfigureAwait(false);
		
		//Disney+
		var series30 = Series.Create(Guid.NewGuid(), "Loki", 4);
		series30.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series30.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series30).ConfigureAwait(false);
		
		var series31 = Series.Create(Guid.NewGuid(), "Ahsoka", 1);
		series31.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series31.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series31).ConfigureAwait(false);
		
		var series32 = Series.Create(Guid.NewGuid(), "The Bear", 2);
		series32.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series32.AddPlatformId(Guid.Parse("21680ab0-6640-4a69-b4c0-c6cc7976431c"));
		series32.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series32).ConfigureAwait(false);
		
		var series33 = Series.Create(Guid.NewGuid(), "Moon Knight", 12);
		series33.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series33.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series33).ConfigureAwait(false);
		
		var series34 = Series.Create(Guid.NewGuid(), "Punisher", 127);
		series34.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series34.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(1977, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series34).ConfigureAwait(false);
		
		var series35 = Series.Create(Guid.NewGuid(), "Hawkeye", 4);
		series35.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series35.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 6, 6 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series35).ConfigureAwait(false);
		
		var series36 = Series.Create(Guid.NewGuid(), "The Mandalorian", 1);
		series36.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series36.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2024, 11, 11 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series36).ConfigureAwait(false);
		
		var series37 = Series.Create(Guid.NewGuid(), "Secret Invasion", 2);
		series37.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series37.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series37).ConfigureAwait(false);
		
		var series38 = Series.Create(Guid.NewGuid(), "Daredevil", 12);
		series38.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series38.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2025, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series38).ConfigureAwait(false);
		
		var series39 = Series.Create(Guid.NewGuid(), "Andor", 127);
		series39.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series39.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(1977, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series39).ConfigureAwait(false);
		
		var series40 = Series.Create(Guid.NewGuid(), "Bones", 127);
		series40.AddPlatformId(Guid.Parse("cfe0504b-1877-49c9-9167-f9d5782bbf02"));
		series40.Details = new Details { ReleaseDate =  new DateTimeOffset(new DateTime(2001, 2, 20 ), TimeSpan.Zero) };
		await _mediaRepository.AddSeriesAsync(series40).ConfigureAwait(false);
	}
}