using NUnit.Framework;
using NSubstitute;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Movieverse.Infrastructure.Persistence;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Infrastructure.Repositories;
using Movieverse.UnitTests.InfrastructureUnitTests.Mocks;

namespace Movieverse.UnitTests.InfrastructureUnitTests;

[TestFixture]
public class UserRepositoryUnitTests
{
	private ILogger<UserRepository> _logger = null!;
	private AppDbContext _dbContext = null!;
	private UserManager<User> _userManager = null!;
	private RoleManager<IdentityUserRole> _roleManager = null!;
	
	[SetUp]
	public void SetUp()
	{
		_logger = Substitute.For<ILogger<UserRepository>>();
		_dbContext = AppDbContextMock.Get();
		_userManager = UserManagerMock.Get<User>();
		_roleManager = RoleManagerMock.Get<IdentityUserRole>();
	}
	
	[Test]
	[TestCase("3b0e2f40-3883-11ee-be56-0242ac120002")]
	[TestCase("668a7286-7556-4b3f-bcd1-1d687109c6b1")]
	public async Task FindById_WhenIdIsValid_ShouldReturnResultUser(string guid)
	{
		// Arrange
		var id = Guid.Parse(guid);
		var user = EntityMock.CreateUser(id);
		_dbContext.Users.FindAsync(id).Returns(user);
		var userRepository = new UserRepository(_logger, _dbContext, _userManager, _roleManager);
		
		// Act
		var result = await userRepository.FindByIdAsync(id);

		// Assert
		await _dbContext.Users.Received(1).FindAsync(id);
		Assert.That(result, Is.Not.Null);
		Assert.That(result.IsSuccessful, Is.True);
		Assert.That(result.Value, Is.EqualTo(user));
		Assert.That(result.Value.Id, Is.EqualTo(id));
	}
	
	[Test]
	[TestCase("668a7286-7556-4b3f-bcd1-1d687109c6b1")]
	[TestCase("e0ad8fc8-3891-11ee-be56-0242ac120002")]
	public async Task FindById_WhenIdIsInvalid_ShouldReturnError(string badGuid)
	{
		// Arrange
		var goodId = Guid.Parse("3b0e2f40-3883-11ee-be56-0242ac120002");
		var badId = Guid.Parse(badGuid);
		var goodUser = EntityMock.CreateUser(goodId);
		_dbContext.Users.FindAsync(goodId).Returns(goodUser);
		_dbContext.Users.FindAsync(Arg.Any<string>()).Returns(null as User);
		var userRepository = new UserRepository(_logger, _dbContext, _userManager, _roleManager);
		
		// Act
		var result = await userRepository.FindByIdAsync(badId);

		// Assert
		await _dbContext.Users.DidNotReceive().FindAsync(goodId);
		await _dbContext.Users.Received(1).FindAsync(badId);
		Assert.That(result, Is.Not.Null);
		Assert.That(result.IsSuccessful, Is.False);
	}
}