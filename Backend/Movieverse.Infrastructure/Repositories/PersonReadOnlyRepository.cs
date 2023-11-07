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

	public async Task<Result<Person>> FindAsync(PersonId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting person with id {id}...", id.ToString());
		
		var person = await _dbContext.Persons
			.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
		
		return person is null ? Error.NotFound(PersonResources.PersonDoesNotExist) : person;
	}

	public async Task<Result<IPaginatedList<SearchPersonDto>>> SearchAsync(string? term, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Searching persons with term: {Term}", term);
		
		var persons = await _dbContext.Persons
			.AsNoTracking()
			.Where(p => term == null || (p.Information.FirstName + " " + p.Information.LastName).ToLower().StartsWith(term.ToLower()))
			.ProjectToType<SearchPersonDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return persons;
	}

	public async Task<Result<IPaginatedList<SearchPersonDto>>> FindPersonsBornTodayAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting persons born today...");
		
		var persons = await _dbContext.Persons
			.AsNoTracking()
			.Where(p => p.LifeHistory.BirthDate != null && p.LifeHistory.BirthDate.Value.Date == DateTimeOffset.UtcNow.Date)
			.ProjectToType<SearchPersonDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return persons;
	}

	public async Task<Result<IEnumerable<PersonInfoDto>>> GetPersonsAsync(IEnumerable<PersonId> ids, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting persons...");

		var persons = await _dbContext.Persons
			.AsNoTracking()
			.Where(p => ids.Contains(p.Id))
			.ProjectToType<PersonInfoDto>()
			.ToListAsync(cancellationToken);

		return persons;
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