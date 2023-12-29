using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class PersonReadOnlyRepository : IPersonReadOnlyRepository
{
	private readonly ILogger<PersonReadOnlyRepository> _logger;
	private readonly ReadOnlyContext _dbContext;
	
	public PersonReadOnlyRepository(ILogger<PersonReadOnlyRepository> logger, ReadOnlyContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	private static readonly Func<ReadOnlyContext, PersonId, Task<Person?>> findById = 
		EF.CompileAsyncQuery((ReadOnlyContext context, PersonId id) =>
			context.Persons.AsNoTracking().SingleOrDefault(m => m.Id == id));
	
	public async Task<Result<Person>> FindAsync(PersonId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting person with id {id}...", id.ToString());
		
		var person = await findById(_dbContext, id);
		
		return person is null ? Error.NotFound(PersonResources.PersonDoesNotExist) : person;
	}

	public async Task<Result<IPaginatedList<SearchPersonDto>>> SearchAsync(string? term, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Searching people with term: {Term}", term);
		
		var people = await _dbContext.Persons
			.AsNoTracking()
			.Where(p => term == null || (p.Information.FirstName + " " + p.Information.LastName).ToLower().StartsWith(term.ToLower()))
			.ProjectToType<SearchPersonDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return people;
	}

	public async Task<Result<IPaginatedList<SearchPersonDto>>> FindPersonsBornTodayAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting people born today...");
		
		var people = await _dbContext.Persons
			.AsNoTracking()
			.Where(p => p.LifeHistory.BirthDate != null && p.LifeHistory.BirthDate.Value.Date == DateTimeOffset.UtcNow.Date)
			.ProjectToType<SearchPersonDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return people;
	}

	public async Task<Result<IEnumerable<PersonInfoDto>>> GetPeopleAsync(IEnumerable<PersonId> ids, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting people...");

		var people = await _dbContext.Persons
			.AsNoTracking()
			.Where(p => ids.Contains(p.Id))
			.ProjectToType<PersonInfoDto>()
			.ToListAsync(cancellationToken);

		return people;
	}

	public async Task<Result<IEnumerable<MediaId>>> GetMediaIdsAsync(PersonId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting media ids for person with id {id}...", id.ToString());
		
		var mediaIds = await _dbContext.Persons
			.AsNoTracking()
			.Where(p => p.Id == id)
			.SelectMany(p => p.MediaIds)
			.ToListAsync(cancellationToken);

		return mediaIds;
	}

	public async Task<Result> AddAsync(Person person, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Database - Adding person with id {id}...", person.Id.ToString());
		
		await _dbContext.Persons.AddAsync(person, cancellationToken);
		return Result.Ok();
	}
}