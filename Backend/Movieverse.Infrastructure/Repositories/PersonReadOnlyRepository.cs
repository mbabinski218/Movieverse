using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Domain.AggregateRoots;
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
		_logger.LogDebug("Getting person with id {id}...", id.ToString());
		
		var person = await _dbContext.Persons
			.FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
			;
		
		return person is null ? Error.NotFound(PersonResources.PersonDoesNotExist) : person;
	}
	
	public async Task<Result> AddAsync(Person person, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Adding person with id {id}...", person.Id.ToString());
		
		await _dbContext.Persons.AddAsync(person, cancellationToken);
		return Result.Ok();
	}
}