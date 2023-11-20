using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using Movieverse.Application.Common.Extensions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
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
	
	//TODO In progress
	[Test]
	public async Task FindUpcomingMoviesAsync_NoPagination_ReturnsAllMediaDtos()
	{
		// Arrange
		var platformId = PlatformId.Create();
		const short totalCount = 2;

		var media = new List<Media>();
		
		var item1 = Movie.Create(MediaId.Create(), "Test1");
		item1.AddPlatform(platformId);
		item1.BasicStatistics = new BasicStatistics();
		item1.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(1)
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
			ReleaseDate = DateTime.Now.AddDays(100)
		};
		media.Add(item3);
		
		var item4 = Series.Create(MediaId.Create(), "Test4");
		item4.AddPlatform(platformId);
		item4.BasicStatistics = new BasicStatistics();
		item4.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(10)
		};
		media.Add(item4);
		
		var item5 = Series.Create(MediaId.Create(), "Test5");
		item5.BasicStatistics = new BasicStatistics();
		item5.Details = new Details
		{
			ReleaseDate = DateTime.Now.AddDays(-1)
		};
		media.Add(item5);
		
		_dbContext.Media = media.AsQueryable().BuildMockDbSet();
		
		var expectedResult = new List<Media>
		{
			
		};

		// Act
		var result = await _mediaRepository.FindUpcomingMoviesAsync(null, null);

		// Assert
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(totalCount, result.Value.Items.Count);
		Assert.That(result.Value.Items.Select(x => x.Id), Is.EqualTo(expectedResult.Select(x => x.Id.Value)));
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
		
		return config;
	}
}