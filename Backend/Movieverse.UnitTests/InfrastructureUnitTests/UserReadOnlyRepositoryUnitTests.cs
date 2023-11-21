using MapsterMapper;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Infrastructure.Repositories;
using Movieverse.UnitTests.InfrastructureUnitTests.Mocks;
using NSubstitute;
using NUnit.Framework;

namespace Movieverse.UnitTests.InfrastructureUnitTests;

[TestFixture]
public class UserReadOnlyRepositoryUnitTests
{
	private IUserReadOnlyRepository _userRepository = null!;
	private ILogger<UserReadOnlyRepository> _logger = null!;
	private IMapper _mapper = null!;
	private ReadOnlyContext _dbContext = null!;

	[SetUp]
	public void SetUp()
	{
		_logger = Substitute.For<ILogger<UserReadOnlyRepository>>();
		_mapper = Substitute.For<IMapper>();
		_dbContext = ReadOnlyContextMock.Get();
           
		_userRepository = new UserReadOnlyRepository(_logger, _dbContext, _mapper);
	}
	
	[TearDown]
	public void TearDown()
	{
		_dbContext.Database.EnsureDeleted();
	}
	
	[Test]
	public async Task FindAsync_UserExists_ReturnsUserDto()
	{
		// Arrange
		var userId = Guid.NewGuid();
		const string email = "test@test.com";
		const string userName = "Test";
		var cancellationToken = new CancellationToken();
		
		var user = User.Create(userId, email, userName, "Test", "Test", 20);
		user.PasswordHash = "Hash";
		var expectedUser = new UserDto
		{
			Email = email,
			UserName = userName,
			Banned = false
		};
		
		_dbContext.Users.FindAsync(Arg.Is<object[]>(x => x.Contains(userId)), cancellationToken).Returns(user);
		_mapper.Map<UserDto>(Arg.Any<User>()).Returns(expectedUser);

		// Act
		var result = await _userRepository.FindAsync(userId, cancellationToken);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.IsFalse(result.Value.Banned);
		Assert.IsTrue(result.Value.CanChangePassword);
		Assert.That(result.Value.Email, Is.EqualTo(expectedUser.Email));
		Assert.That(result.Value.UserName, Is.EqualTo(expectedUser.UserName));
	}
	
	[Test]
	public async Task FindAsync_UserDoesNotExist_ReturnsNotFoundError()
	{
		// Arrange
		var userId = Guid.NewGuid();
		_dbContext.Users.FindAsync(Arg.Is<object[]>(x => x.Contains(userId))).Returns((User?)null);

		// Act
		var result = await _userRepository.FindAsync(userId);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { UserResources.UserDoesNotExist }, result.Error.Messages);
	}
	
	[Test]
	public async Task GetWatchlistAsync_UserHasItemsInWatchlist_ReturnsMediaIds()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var user = User.Create(userId, "test@test.com", "Test", "Test", "Test", 20);
		
		user.AddMediaInfo(MediaInfo.Create(user, MediaId.Create(), true, 8));  // On watchlist
		user.AddMediaInfo(MediaInfo.Create(user, MediaId.Create(), false, 8)); // Not on watchlist
		user.AddMediaInfo(MediaInfo.Create(user, MediaId.Create(), true, 0));  // On watchlist
		
		var users = new List<User>
		{
			user,
			User.Create(Guid.NewGuid(), "test1@test.com", "Test", "Test", "Test", 20),
			User.Create(Guid.NewGuid(), "test2@test.com", "Test", "Test", "Test", 20)
		}.AsQueryable();
		
		var mockUsersDbSet = users.BuildMockDbSet();
		_dbContext.Users = mockUsersDbSet;

		// Act
		var result = await _userRepository.GetWatchlistAsync(userId);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.AreEqual(2, result.Value.Count());
	}
	
	[Test]
	public async Task GetWatchlistAsync_UserHasNoItemsInWatchlist_ReturnsEmptyCollection()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var user = User.Create(userId, "test@test.com", "Test", "Test", "Test", 20);
		
		var users = new List<User>
		{
			user,
			User.Create(Guid.NewGuid(), "test1@test.com", "Test", "Test", "Test", 20),
			User.Create(Guid.NewGuid(), "test2@test.com", "Test", "Test", "Test", 20)
		}.AsQueryable();
		
		var mockUsersDbSet = users.BuildMockDbSet();
		_dbContext.Users = mockUsersDbSet;

		// Act
		var result = await _userRepository.GetWatchlistAsync(userId);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsEmpty(result.Value);
	}
	
	[Test]
	public async Task GetWatchlistAsync_InvalidUserId_ReturnsEmptyCollection()
	{
		// Arrange
		var invalidUserId = Guid.NewGuid();
		
		var users = new List<User>
		{
			User.Create(Guid.NewGuid(), "test1@test.com", "Test", "Test", "Test", 20),
			User.Create(Guid.NewGuid(), "test2@test.com", "Test", "Test", "Test", 20)
		}.AsQueryable();
		
		var mockUsersDbSet = users.BuildMockDbSet();
		_dbContext.Users = mockUsersDbSet;

		// Act
		var result = await _userRepository.GetWatchlistAsync(invalidUserId);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsEmpty(result.Value);
	}
	
	[Test]
	public async Task GetMediaInfoAsync_UserHasMediaInfo_ReturnsMediaInfoDto()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var mediaId = MediaId.Create();
		const bool isOnWatchlist = true;
		const ushort rating = 8;
		
		var user = User.Create(userId, "test@test.com", "Test", "Test", "Test", 20);
		
		user.AddMediaInfo(MediaInfo.Create(user, mediaId, isOnWatchlist, rating)); // On watchlist, valid MediaId
		user.AddMediaInfo(MediaInfo.Create(user, MediaId.Create(), false, 8)); // Not on watchlist
		user.AddMediaInfo(MediaInfo.Create(user, MediaId.Create(), true, 0)); // On watchlist
		
		var users = new List<User>
		{
			user,
			User.Create(Guid.NewGuid(), "test1@test.com", "Test", "Test", "Test", 20),
			User.Create(Guid.NewGuid(), "test2@test.com", "Test", "Test", "Test", 20)
		}.AsQueryable();
		
		_dbContext.Users = users.BuildMockDbSet();

		// Act
		var result = await _userRepository.GetMediaInfoAsync(userId, mediaId);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.IsNotNull(result.Value);
		Assert.IsInstanceOf<MediaInfoDto>(result.Value);
		Assert.That(result.Value.IsOnWatchlist, Is.EqualTo(isOnWatchlist));
		Assert.That(result.Value.Rating, Is.EqualTo(rating));
	}
	
	[Test]
	public async Task GetMediaInfoAsync_NoMediaInfoFound_ReturnsNotFoundError()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var mediaId = MediaId.Create();
		
		var user = User.Create(userId, "test@test.com", "Test", "Test", "Test", 20);
		
		user.AddMediaInfo(MediaInfo.Create(user, MediaId.Create(), false, 8)); // Not on watchlist
		user.AddMediaInfo(MediaInfo.Create(user, MediaId.Create(), true, 0)); // On watchlist
		
		var users = new List<User>
		{
			user,
			User.Create(Guid.NewGuid(), "test1@test.com", "Test", "Test", "Test", 20),
			User.Create(Guid.NewGuid(), "test2@test.com", "Test", "Test", "Test", 20)
		}.AsQueryable();
		
		_dbContext.Users = users.BuildMockDbSet();

		// Act
		var result = await _userRepository.GetMediaInfoAsync(userId, mediaId);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.IsNull(result.Value);
	}
}