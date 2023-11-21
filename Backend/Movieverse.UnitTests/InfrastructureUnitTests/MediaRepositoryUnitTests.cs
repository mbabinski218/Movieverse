using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Infrastructure.Repositories;
using Movieverse.UnitTests.InfrastructureUnitTests.Mocks;
using NSubstitute;
using NUnit.Framework;

namespace Movieverse.UnitTests.InfrastructureUnitTests;

public class MediaRepositoryUnitTests
{
	private IMediaRepository _mediaRepository = null!;
	private ILogger<MediaRepository> _logger = null!;
	private Context _dbContext = null!;
	
	[SetUp]
	public void SetUp()
	{
		_logger = Substitute.For<ILogger<MediaRepository>>();
		_dbContext = ContextMock.Get();
           
		_mediaRepository = new MediaRepository(_logger, _dbContext);
	}
	
	[TearDown]
	public void TearDown()
	{
		_dbContext.Database.EnsureDeleted();
	}
	
	[Test]
	public async Task GetAllAsync_MediaExists_ReturnsMedia()
	{
		// Arrange
		var expectedMedia = new List<Media>
		{
			Movie.Create(MediaId.Create(), "Test1"),
			Movie.Create(MediaId.Create(), "Test2"),
			Movie.Create(MediaId.Create(), "Test3")
		}.AsQueryable();
		
		_dbContext.Media = expectedMedia.BuildMockDbSet();

		// Act
		var result = await _mediaRepository.GetAllAsync();

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.That(expectedMedia, Is.EqualTo(result.Value));
	}
	
	[Test]
	public async Task GetAllAsync_MediaNotExists_ReturnsEmpty()
	{
		// Arrange
		var expectedMedia = new List<Media>().AsQueryable();
		_dbContext.Media = expectedMedia.BuildMockDbSet();

		// Act
		var result = await _mediaRepository.GetAllAsync();

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.That(expectedMedia, Is.Empty);
	}
	
	[Test]
	public async Task TitleExistsAsync_TitleExists_ReturnsTrue()
	{
		// Arrange
		const string title = "Existing Title";
		
		var media = new List<Media>
		{
			Movie.Create(MediaId.Create(), "Test1"),
			Movie.Create(MediaId.Create(), title),
			Movie.Create(MediaId.Create(), "Test2"),
			Movie.Create(MediaId.Create(), "Test3")
		}.AsQueryable();
		
		_dbContext.Media = media.BuildMockDbSet();

		// Act
		var result = await _mediaRepository.TitleExistsAsync(title);

		// Assert
		Assert.IsTrue(result);
	}
	
	[Test]
	public async Task TitleExistsAsync_TitleDoesNotExist_ReturnsFalse()
	{
		// Arrange
		const string title = "Non-Existing Title";
		
		var media = new List<Media>
		{
			Movie.Create(MediaId.Create(), "Test1"),
			Movie.Create(MediaId.Create(), "Test2"),
			Movie.Create(MediaId.Create(), "Test3")
		}.AsQueryable();
		
		_dbContext.Media = media.BuildMockDbSet();

		// Act
		var result = await _mediaRepository.TitleExistsAsync(title);

		// Assert
		Assert.IsFalse(result);
	}
	
	[Test]
	public async Task BanReviewsByUserIdAsync_UserHasReviews_BansReviews()
	{
		// Arrange
		var userId = Guid.NewGuid();
		const string userName = "Test User";
		var media = Movie.Create(MediaId.Create(), "Test Media");

		var review1 = Review.Create(media, userId, userName, "Test Review1");
		var review2 = Review.Create(media, userId, userName, "Test Review2");
		var review3 = Review.Create(media, Guid.NewGuid(), "Test1", "Test Review3");
		var review4 = Review.Create(media, Guid.NewGuid(), "Test2", "Test Review4");
		
		media.AddReview(review1);
		media.AddReview(review2);
		media.AddReview(review3);
		media.AddReview(review4);
		
		var allMedia = new List<Media>
		{
			media,
			Movie.Create(MediaId.Create(), "Test Media2"),
			Movie.Create(MediaId.Create(), "Test Media3")
		}.AsQueryable();
		
		_dbContext.Media = allMedia.BuildMockDbSet();

		// Act
		var result = await _mediaRepository.BanReviewsByUserIdAsync(userId);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsTrue(review1.Banned);
		Assert.IsTrue(review2.Banned);
		Assert.IsFalse(review3.Banned);
		Assert.IsFalse(review4.Banned);
	}
	
	[Test]
	public async Task BanReviewsByUserIdAsync_UserHasNoReviews_BansReviews()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var media = Movie.Create(MediaId.Create(), "Test Media");

		var review1 = Review.Create(media, Guid.NewGuid(), "Test1", "Test Review1");
		var review2 = Review.Create(media, Guid.NewGuid(), "Test2", "Test Review2");
		var review3 = Review.Create(media, Guid.NewGuid(), "Test3", "Test Review3");
		
		media.AddReview(review1);
		media.AddReview(review2);
		media.AddReview(review3);
		
		var allMedia = new List<Media>
		{
			media,
			Movie.Create(MediaId.Create(), "Test Media2"),
			Movie.Create(MediaId.Create(), "Test Media3")
		}.AsQueryable();
		
		_dbContext.Media = allMedia.BuildMockDbSet();

		// Act
		var result = await _mediaRepository.BanReviewsByUserIdAsync(userId);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsFalse(review1.Banned);
		Assert.IsFalse(review2.Banned);
		Assert.IsFalse(review3.Banned);
	}
}