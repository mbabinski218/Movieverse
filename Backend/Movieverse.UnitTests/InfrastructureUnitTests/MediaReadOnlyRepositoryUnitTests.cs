using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using Movieverse.Application.Common.Extensions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Infrastructure.Repositories;
using Movieverse.UnitTests.InfrastructureUnitTests.Mocks;
using NSubstitute;
using NUnit.Framework;

namespace Movieverse.UnitTests.InfrastructureUnitTests;

public class MediaReadOnlyRepositoryUnitTests
{
	private IMediaReadOnlyRepository _mediaRepository = null!;
	private ILogger<MediaReadOnlyRepository> _logger = null!;
	private IMapper _mapper = null!;
	private ReadOnlyContext _dbContext = null!;
	
	[SetUp]
	public void SetUp()
	{
		_logger = Substitute.For<ILogger<MediaReadOnlyRepository>>();
		_mapper = new Mapper(GetConfig());
		_dbContext = ReadOnlyContextMock.Get();
           
		_mediaRepository = new MediaReadOnlyRepository(_logger, _dbContext, _mapper);
	}
	
	[TearDown]
	public void TearDown()
	{
		_dbContext.Database.EnsureDeleted();
	}
	
	[Test]
	public async Task GetLatestMediaAsync_WithValidData_ReturnsPaginatedMediaDtos()
	{
		// Arrange
		var platformId = PlatformId.Create();
		const short pageNumber = 2;
		const short pageSize = 2;
		const short totalPages = 3;

		var media = new List<Media>();
		
		var item1 = Movie.Create(MediaId.Create(), "Test1");
		item1.AddPlatform(platformId);
		item1.BasicStatistics = new BasicStatistics();
		item1.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item1);
		
		var item2 = Movie.Create(MediaId.Create(), "Test2");
		item2.AddPlatform(platformId);
		item2.BasicStatistics = new BasicStatistics();
		item2.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-10)
		};
		media.Add(item2);
		
		var item3 = Movie.Create(MediaId.Create(), "Test3");
		item3.AddPlatform(platformId);
		item3.BasicStatistics = new BasicStatistics();
		item3.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(1)
		};
		media.Add(item3);
		
		var item4 = Movie.Create(MediaId.Create(), "Test4");
		item4.AddPlatform(platformId);
		item4.BasicStatistics = new BasicStatistics();
		item4.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(10)
		};
		media.Add(item4);
		
		var item5 = Movie.Create(MediaId.Create(), "Test5");
		item5.BasicStatistics = new BasicStatistics();
		item5.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item5);
		
		var item6 = Movie.Create(MediaId.Create(), "Test6");
		item6.AddPlatform(platformId);
		item6.BasicStatistics = new BasicStatistics();
		item6.Details = new Details
		{
			ReleaseDate = DateTime.Now
		};
		media.Add(item6);
		
		var item7 = Movie.Create(MediaId.Create(), "Test7");
		item7.AddPlatform(platformId);
		item7.BasicStatistics = new BasicStatistics();
		item7.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item7);
		
		var item8 = Movie.Create(MediaId.Create(), "Test8");
		item8.BasicStatistics = new BasicStatistics();
		item8.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item8);
		
		var item9 = Movie.Create(MediaId.Create(), "Test9");
		item9.AddPlatform(platformId);
		item9.BasicStatistics = new BasicStatistics();
		item9.Details = new Details
		{
			ReleaseDate = DateTime.Now
		};
		media.Add(item9);
		
		_dbContext.Media = media.AsQueryable().BuildMockDbSet();
		
		var expectedResult = new List<Media>
		{
			item7, item1
		};

		// Act
		var result = await _mediaRepository.GetLatestMediaAsync(platformId, pageNumber, pageSize);

		// Assert
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(pageSize, result.Value.Items.Count);
		Assert.AreEqual(totalPages, result.Value.TotalPages);
		Assert.That(result.Value.Items.Select(x => x.Id), Is.EqualTo(expectedResult.Select(x => x.Id.Value)));
	}
	
	[Test]
	public async Task GetLatestMediaAsync_NoPagination_ReturnsAllMediaDtos()
	{
		// Arrange
		var platformId = PlatformId.Create();
		const short totalCount = 5;

		var media = new List<Media>();
		
		var item1 = Movie.Create(MediaId.Create(), "Test1");
		item1.AddPlatform(platformId);
		item1.BasicStatistics = new BasicStatistics();
		item1.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item1);
		
		var item2 = Movie.Create(MediaId.Create(), "Test2");
		item2.AddPlatform(platformId);
		item2.BasicStatistics = new BasicStatistics();
		item2.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-10)
		};
		media.Add(item2);
		
		var item3 = Movie.Create(MediaId.Create(), "Test3");
		item3.AddPlatform(platformId);
		item3.BasicStatistics = new BasicStatistics();
		item3.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(1)
		};
		media.Add(item3);
		
		var item4 = Movie.Create(MediaId.Create(), "Test4");
		item4.AddPlatform(platformId);
		item4.BasicStatistics = new BasicStatistics();
		item4.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(10)
		};
		media.Add(item4);
		
		var item5 = Movie.Create(MediaId.Create(), "Test5");
		item5.BasicStatistics = new BasicStatistics();
		item5.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item5);
		
		var item6 = Movie.Create(MediaId.Create(), "Test6");
		item6.AddPlatform(platformId);
		item6.BasicStatistics = new BasicStatistics();
		item6.Details = new Details
		{
			ReleaseDate = DateTime.Now
		};
		media.Add(item6);
		
		var item7 = Movie.Create(MediaId.Create(), "Test7");
		item7.AddPlatform(platformId);
		item7.BasicStatistics = new BasicStatistics();
		item7.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item7);
		
		var item8 = Movie.Create(MediaId.Create(), "Test8");
		item8.BasicStatistics = new BasicStatistics();
		item8.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item8);
		
		var item9 = Movie.Create(MediaId.Create(), "Test9");
		item9.AddPlatform(platformId);
		item9.BasicStatistics = new BasicStatistics();
		item9.Details = new Details
		{
			ReleaseDate = DateTime.Now
		};
		media.Add(item9);
		
		_dbContext.Media = media.AsQueryable().BuildMockDbSet();
		
		var expectedResult = new List<Media>
		{
			item9, item6, item7, item1, item2
		};

		// Act
		var result = await _mediaRepository.GetLatestMediaAsync(platformId, null, null);

		// Assert
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(totalCount, result.Value.Items.Count);
		Assert.IsNull(result.Value.TotalPages);
		Assert.IsNull(result.Value.PageNumber);
		Assert.That(result.Value.Items.Select(x => x.Id), Is.EqualTo(expectedResult.Select(x => x.Id.Value)));
	}
	
	[Test]
	public async Task FindUpcomingMoviesAsync_NoPagination_ReturnsAllMediaDtos()
	{
		// Arrange
		var platformId = PlatformId.Create();
		const short totalCount = 2;

		var movies = new List<Movie>();
		var series = new List<Series>();
		
		var item1 = Movie.Create(MediaId.Create(), "Test1");
		item1.AddPlatform(platformId);
		item1.BasicStatistics = new BasicStatistics();
		item1.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(1)
		};
		movies.Add(item1);
		
		var item2 = Movie.Create(MediaId.Create(), "Test2");
		item2.AddPlatform(platformId);
		item2.BasicStatistics = new BasicStatistics();
		item2.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-10)
		};
		movies.Add(item2);
		
		var item3 = Movie.Create(MediaId.Create(), "Test3");
		item3.AddPlatform(platformId);
		item3.BasicStatistics = new BasicStatistics();
		item3.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(100)
		};
		movies.Add(item3);
		
		var item4 = Series.Create(MediaId.Create(), "Test4");
		item4.AddPlatform(platformId);
		item4.BasicStatistics = new BasicStatistics();
		item4.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(10)
		};
		series.Add(item4);
		
		var item5 = Series.Create(MediaId.Create(), "Test5");
		item5.BasicStatistics = new BasicStatistics();
		item5.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		series.Add(item5);
		
		_dbContext.Movies = movies.AsQueryable().BuildMockDbSet();
		_dbContext.Series = series.AsQueryable().BuildMockDbSet();
		
		var expectedResult = new List<Movie>
		{
			item1, item3
		};

		// Act
		var result = await _mediaRepository.FindUpcomingMoviesAsync(null, null);

		// Assert
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(totalCount, result.Value.Items.Count);
		Assert.That(result.Value.Items.Select(x => x.Id), Is.EqualTo(expectedResult.Select(x => x.Id.Value)));
	}
	
	[Test]
	public async Task GetAllGenresAsync_WhenGenresExist_ReturnsAllGenres()
	{
		// Arrange
		var genres = new List<Genre>
		{
			Genre.Create("Test1"),
			Genre.Create("Test2"),
			Genre.Create("Test3"),
			Genre.Create("Test4"),
			Genre.Create("Test5")
		};
		
		_dbContext.Genres = genres.AsQueryable().BuildMockDbSet();

		// Act
		var result = await _mediaRepository.GetAllGenresAsync();

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.That(result.Value.Select(x => x.Name), Is.EqualTo(genres.Select(x => x.Name)));
	}
	
	[Test]
	public async Task GetAllGenresAsync_WhenNoGenresExist_ReturnsEmptyList()
	{
		// Arrange
		_dbContext.Genres = new List<Genre>().AsQueryable().BuildMockDbSet();

		// Act
		var result = await _mediaRepository.GetAllGenresAsync();

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.IsEmpty(result.Value);
	}
	
	[Test]
	public async Task SearchMediaAsync_WithValidSearchString_ReturnsPaginatedMediaDtos()
	{
		// Arrange
		const string searchString = "Test";
		const short pageNumber = 2;
		const short pageSize = 1;
		const short totalPages = 3;
		
		var media = new List<Media>();
		
		var item1 = Movie.Create(MediaId.Create(), "Test1");
		media.Add(item1);
		
		var item2 = Movie.Create(MediaId.Create(), "Not1");
		media.Add(item2);
		
		var item3 = Movie.Create(MediaId.Create(), "Test2");
		media.Add(item3);
		
		var item4 = Movie.Create(MediaId.Create(), "Not2");
		media.Add(item4);
		
		var item5 = Movie.Create(MediaId.Create(), "Not3");
		media.Add(item5);
		
		var item6 = Movie.Create(MediaId.Create(), "Test3");
		media.Add(item6);
		
		_dbContext.Media = media.AsQueryable().BuildMockDbSet();
		
		// Act
		var result = await _mediaRepository.SearchMediaAsync(searchString, pageNumber, pageSize);

		// Assert
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(pageSize, result.Value.Items.Count);
		Assert.AreEqual(pageNumber, result.Value.PageNumber);
		Assert.AreEqual(totalPages, result.Value.TotalPages);
		Assert.That(result.Value.Items.Select(x => x.Title), Is.EqualTo(new List<string> { item3.Title }));
	}
	
	[Test]
	public async Task SearchMediaAsync_WithNoMatchingMedia_ReturnsEmptyList()
	{
		// Arrange
		const string searchString = "NotFound";
		
		var media = new List<Media>();
		
		var item1 = Movie.Create(MediaId.Create(), "Test1");
		media.Add(item1);
		
		var item2 = Movie.Create(MediaId.Create(), "Test2");
		media.Add(item2);
		
		_dbContext.Media = media.AsQueryable().BuildMockDbSet();
		
		// Act
		var result = await _mediaRepository.SearchMediaAsync(searchString, null, null);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.IsEmpty(result.Value.Items);
	}
	
	[Test]
	public async Task SearchSeriesWithFiltersAsync_WithValidFilters_ReturnsPaginatedMediaDtos()
	{
		// Arrange
		const string searchString = "Test";
		const int genreId = 1;
		const int count = 2;

		var genre1 = Genre.Create(genreId, "Test1");
		var genre2 = Genre.Create(2, "Test2");
		
		var series = new List<Series>();
		var movies = new List<Movie>();
		
		var found1 = Series.Create("Test1");
		found1.AddGenre(genre1);
		series.Add(found1);
		
		var found2 = Series.Create("Test2");
		found2.AddGenre(genre1);
		found2.AddGenre(genre2);
		series.Add(found2);
		
		var notFound1 = Series.Create("Not1");
		notFound1.AddGenre(genre1);
		series.Add(notFound1);
		
		var notFound2 = Series.Create("Test3");
		series.Add(notFound2);
		
		var movie1 = Movie.Create("Test1");
		movie1.AddGenre(genre1);
		movies.Add(movie1);
		
		_dbContext.Series = series.AsQueryable().BuildMockDbSet();
		_dbContext.Movies = movies.AsQueryable().BuildMockDbSet();
		
		// Act
		var result = await _mediaRepository.SearchSeriesWithFiltersAsync(searchString, genreId, null, null);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(count, result.Value.Items.Count);
		Assert.That(result.Value.Items.Select(x => x.Title), Is.EqualTo(new List<string> { found1.Title, found2.Title }));
	}
	
	[Test]
	public async Task SearchSeriesWithFiltersAsync_NoFilters_ReturnsPaginatedMediaDtos()
	{
		// Arrange
		const int count = 4;

		var genre1 = Genre.Create(1, "Test1");
		var genre2 = Genre.Create(2, "Test2");
		
		var series = new List<Series>();
		var movies = new List<Movie>();
		
		var found1 = Series.Create("Test1");
		found1.AddGenre(genre1);
		series.Add(found1);
		
		var found2 = Series.Create("Test2");
		found2.AddGenre(genre1);
		found2.AddGenre(genre2);
		series.Add(found2);
		
		var found3 = Series.Create("Not1");
		found3.AddGenre(genre1);
		series.Add(found3);
		
		var found4 = Series.Create("Test3");
		series.Add(found4);
		
		var movie1 = Movie.Create("Test1");
		movie1.AddGenre(genre1);
		movies.Add(movie1);
		
		_dbContext.Series = series.AsQueryable().BuildMockDbSet();
		_dbContext.Movies = movies.AsQueryable().BuildMockDbSet();
		
		// Act
		var result = await _mediaRepository.SearchSeriesWithFiltersAsync(null, null, null, null);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(count, result.Value.Items.Count);
		Assert.That(result.Value.Items.Select(x => x.Title), 
			Is.EqualTo(new List<string> { found1.Title, found2.Title, found3.Title, found4.Title }));
	}
	
	[Test]
	public async Task SearchSeriesWithFiltersAsync_InvalidFilters_ReturnsPaginatedMediaDtos()
	{
		// Arrange
		const string searchString = "Test";
		const int genreId = 1;
		
		var genre1 = Genre.Create(genreId, "Test1");
		
		var genre2 = Genre.Create(2, "Test2");
		
		var series = new List<Series>();
		var movies = new List<Movie>();
		
		var notFound1 = Series.Create("Test1");
		series.Add(notFound1);
		
		var notFound2 = Series.Create("Test2");
		notFound2.AddGenre(genre2);
		series.Add(notFound2);
		
		var found3 = Series.Create("Not1");
		series.Add(found3);
		
		var movie1 = Movie.Create("Test1");
		movie1.AddGenre(genre1);
		movies.Add(movie1);
		
		_dbContext.Series = series.AsQueryable().BuildMockDbSet();
		_dbContext.Movies = movies.AsQueryable().BuildMockDbSet();
		
		// Act
		var result = await _mediaRepository.SearchSeriesWithFiltersAsync(searchString, genreId, null, null);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.IsEmpty(result.Value.Items);
	}
	
	// Helper methods
	private static TypeAdapterConfig GetConfig()
	{
		var config = new TypeAdapterConfig();
		
		config.NewConfig<Media, MediaDemoDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue())
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue())
			.Map(dest => dest.Rating, src => src.BasicStatistics.Rating);
		
		config.NewConfig<Media, SearchMediaDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue())
			.Map(dest => dest.Title, src => src.Title)
			.Map(dest => dest.ReleaseDate, src => src.Details.ReleaseDate)
			.Map(dest => dest.Poster, src => src.PosterId.GetValue().ToString())
			.Map(dest => dest.Description, src => src.Details.Storyline);
		
		return config;
	}
}