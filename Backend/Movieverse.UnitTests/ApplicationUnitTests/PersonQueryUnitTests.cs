using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Common.Extensions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.QueryHandlers.Person;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Contracts.Queries.Person;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using NSubstitute;
using NUnit.Framework;

namespace Movieverse.UnitTests.ApplicationUnitTests;

public class PersonQueryUnitTests
{
	private IPersonReadOnlyRepository _personRepository = null!;
	private IMapper _mapper = null!;

	[SetUp]
	public void Setup()
	{
		_personRepository = Substitute.For<IPersonReadOnlyRepository>();
		_mapper = new Mapper(GetConfig());
	}
	
	[Test]
	public async Task Handle_WhenPersonExists_ReturnsPersonDto()
	{
		// Arrange
		var personId = PersonId.Create();
		var person = Person.Create(personId);

		var loggerMock = Substitute.For<ILogger<GetPersonHandler>>();

		_personRepository.FindAsync(personId, Arg.Any<CancellationToken>())
			.Returns(Result<Person>.Ok(person));

		var handler = new GetPersonHandler(loggerMock, _personRepository, _mapper);
		var query = new GetPersonQuery(personId);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
	}
	
	[Test]
	public async Task Handle_WhenPersonNotFound_ReturnsNotFoundError()
	{
		// Arrange
		var personId = PersonId.Create();

		var loggerMock = Substitute.For<ILogger<GetPersonHandler>>();

		_personRepository.FindAsync(personId, Arg.Any<CancellationToken>())
			.Returns(Result<Person>.Fail(Error.NotFound()));

		var handler = new GetPersonHandler(loggerMock, _personRepository, _mapper);
		var query = new GetPersonQuery(personId);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.That(result.Error.Code, Is.EqualTo((ushort)StatusCode.NotFound));
	}
	
	[Test]
	public async Task Handle_BornTodayCategory_ReturnsPaginatedListOfPersons()
	{
		// Arrange
		const string category = "bornToday";
		const int pageNumber = 2;
		const int pageSize = 3;

		var people = new List<SearchPersonDto>
		{
			new() { Id = PersonId.Create(), FullName = "Person 1" },
			new() { Id = PersonId.Create(), FullName = "Person 2" },
			new() { Id = PersonId.Create(), FullName = "Person 3" }
		};
		
		var expected = new PaginatedList<SearchPersonDto>(people, pageNumber, pageSize);

		var loggerMock = Substitute.For<ILogger<PersonsChartHandler>>();
		
		_personRepository.FindPersonsBornTodayAsync(pageNumber, pageSize, Arg.Any<CancellationToken>())
			.Returns(Result<IPaginatedList<SearchPersonDto>>.Ok(expected));

		var handler = new PersonsChartHandler(loggerMock, _personRepository);
		var query = new PersonsChartQuery(category, pageNumber, pageSize);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsSuccessful);
		Assert.AreEqual(expected, result.Value);
	}
	
	[Test]
	public async Task Handle_InvalidCategory_ReturnsError()
	{
		// Arrange
		const string category = "invalidCategory";
		
		var loggerMock = Substitute.For<ILogger<PersonsChartHandler>>();

		var handler = new PersonsChartHandler(loggerMock, _personRepository);
		var query = new PersonsChartQuery(category, null, null);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.AreEqual(new[] { PersonResources.InvalidChartCategory }, result.Error.Messages);
		Assert.That(result.Error.Code, Is.EqualTo((ushort)StatusCode.Invalid));
	}
	
	[Test]
	public async Task Handle_BornTodayCategory_ReturnsError()
	{
		// Arrange
		const string category = "bornToday";
		const int pageNumber = 2;
		const int pageSize = 3;

		var loggerMock = Substitute.For<ILogger<PersonsChartHandler>>();
		
		_personRepository.FindPersonsBornTodayAsync(pageNumber, pageSize, Arg.Any<CancellationToken>())
			.Returns(Result<IPaginatedList<SearchPersonDto>>.Fail(Error.InternalError()));

		var handler = new PersonsChartHandler(loggerMock, _personRepository);
		var query = new PersonsChartQuery(category, pageNumber, pageSize);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsUnsuccessful);
		Assert.That(result.Error.Code, Is.EqualTo((ushort)StatusCode.InternalError));
	}
	
	// Helper methods
	private static TypeAdapterConfig GetConfig()
	{
		var config = new TypeAdapterConfig();
		
		config.NewConfig<Person, SearchPersonDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue())
			.Map(dest => dest.FullName, src => src.Information.FirstName + " " + src.Information.LastName)
			.Map(dest => dest.Age, src => src.Information.Age)
			.Map(dest => dest.Picture, src => src.PictureId.GetValue())
			.Map(dest => dest.Biography, src => src.Biography);
		
		return config;
	}
}