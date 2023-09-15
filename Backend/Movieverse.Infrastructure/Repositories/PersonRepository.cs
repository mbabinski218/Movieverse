using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Resources;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class PersonRepository : IPersonRepository
{
	private readonly ILogger<PersonRepository> _logger;
	private readonly AppDbContext _dbContext;
	public PersonRepository(ILogger<PersonRepository> logger, AppDbContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}
	
	public async Task<Result> AddAsync(Person person, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Adding person with id {id}...", person.Id.ToString());
		
		await _dbContext.Persons.AddAsync(person, cancellationToken).ConfigureAwait(false);
		return Result.Ok();
	}

	public async Task<Result<Person>> FindAsync(PersonId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting person with id {id}...", id.ToString());
		
		var person = await _dbContext.Persons.FindAsync(new object?[] { id.Value }, cancellationToken).ConfigureAwait(false);
		return person is null ? Error.NotFound(PersonResources.PersonDoesNotExist) : person;
	}
}