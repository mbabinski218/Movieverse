using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class PersonRepository : IPersonRepository
{
	private readonly ILogger<PersonRepository> _logger;
	private readonly Context _dbContext;
	public PersonRepository(ILogger<PersonRepository> logger, Context dbContext)
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
	
	public async Task<Result> AddAsync(Person person, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Database - Adding person with id {id}...", person.Id.ToString());
		
		await _dbContext.Persons.AddAsync(person, cancellationToken);
		return Result.Ok();
	}
	
	public Task<Result> UpdateAsync(Person person, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Updating person with id {id}...", person.Id.ToString());
		
		_dbContext.Persons.Update(person);
		return Task.FromResult(Result.Ok());
	}
}