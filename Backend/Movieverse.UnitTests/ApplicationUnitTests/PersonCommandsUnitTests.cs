using Microsoft.Extensions.Logging;
using Movieverse.Application.CommandHandlers.PersonCommands.Create;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Person;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects;
using NSubstitute;
using NUnit.Framework;

namespace Movieverse.UnitTests.ApplicationUnitTests;

public class PersonCommandsUnitTests
{
	private ILogger<CreatePersonCommand> _logger = null!;
	private IPersonRepository _personRepository = null!;
	private IUserReadOnlyRepository _userRepository = null!;
	private IHttpService _httpService = null!;
	private IUnitOfWork _unitOfWork = null!;
	private CreatePersonHandler _handler = null!;
	
	[SetUp]
	public void Setup()
	{
		_logger = Substitute.For<ILogger<CreatePersonCommand>>();
		_personRepository = Substitute.For<IPersonRepository>();
		_userRepository = Substitute.For<IUserReadOnlyRepository>();
		_httpService = Substitute.For<IHttpService>();
		_unitOfWork = Substitute.For<IUnitOfWork>();
		
		_handler = new CreatePersonHandler(_logger, _personRepository, _userRepository, _httpService, _unitOfWork);
	}
	
	[Test]
	public async Task Handle_WhenCreatingPerson_SuccessfullyCreatesPerson()
	{
		// Arrange
		var userId = Guid.NewGuid();

		_httpService.UserId.Returns(userId);

		var userDto = new UserDto
		{
			UserName = "TestUser",
			Email = "test@test.com",
			Information = new Information
			{
				Age = 20
			}
		};
		_userRepository.FindAsync(userId, Arg.Any<CancellationToken>()).Returns(Result<UserDto>.Ok(userDto));
		_personRepository.AddAsync(Arg.Any<Person>(), Arg.Any<CancellationToken>()).Returns(Result.Ok());
		_unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(true);

		var request = new CreatePersonCommand(true);

		// Act
		var result = await _handler.Handle(request, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.That(result.Value, Is.Not.Null);
		Assert.IsTrue(Guid.TryParse(result.Value, out _));
	}
	
	[Test]
	public async Task Handle_WhenUserNotLoggedIn_ReturnsUnauthorizedError()
	{
		// Arrange
		_httpService.UserId.Returns((Guid?)null);

		var request = new CreatePersonCommand(true);

		// Act
		var result = await _handler.Handle(request, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual((ushort)StatusCode.Unauthorized, result.Error.Code);
		Assert.AreEqual(new[] { UserResources.YouAreNotLoggedIn }, result.Error.Messages);
	}
	
	[Test]
	public async Task Handle_WhenUserAlreadyHasPersonId_ReturnsInvalidError()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var personId = Guid.NewGuid();

		_httpService.UserId.Returns(userId);

		var userDto = new UserDto
		{
			UserName = "TestUser",
			Email = "test@test.com",
			Information = new Information
			{
				Age = 20
			},
			PersonId = personId
		};
		_userRepository.FindAsync(userId, Arg.Any<CancellationToken>()).Returns(Result<UserDto>.Ok(userDto));

		var request = new CreatePersonCommand(true);

		// Act
		var result = await _handler.Handle(request, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual((ushort)StatusCode.Invalid, result.Error.Code);
	}
	
	[Test]
	public async Task Handle_WhenPersonRepositoryFails_ReturnsError()
	{
		// Arrange
		_personRepository.AddAsync(Arg.Any<Person>(), Arg.Any<CancellationToken>())
			.Returns(Result.Fail(Error.InternalError()));

		var request = new CreatePersonCommand(false);

		// Act
		var result = await _handler.Handle(request, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual((ushort)StatusCode.InternalError, result.Error.Code);
	}
	
	[Test]
	public async Task Handle_WhenSaveChangesFails_ReturnsError()
	{
		// Arrange
		_personRepository.AddAsync(Arg.Any<Person>(), Arg.Any<CancellationToken>()).Returns(Result.Ok());
		_unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(false);
		
		var request = new CreatePersonCommand(false);

		// Act
		var result = await _handler.Handle(request, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual((ushort)StatusCode.Invalid, result.Error.Code);
		Assert.AreEqual(new[] { PersonResources.CouldNotCreatePerson }, result.Error.Messages);
	}
}