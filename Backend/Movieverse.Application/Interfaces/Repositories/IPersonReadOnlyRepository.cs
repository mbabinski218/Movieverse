using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IPersonReadOnlyRepository
{
	Task<Result> AddAsync(Person person, CancellationToken cancellationToken = default);
	Task<Result<Person>> FindAsync(PersonId id, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchPersonDto>>> SearchAsync(string? term, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchPersonDto>>> FindPersonsBornTodayAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<PersonInfoDto>>> GetPersonsAsync(IEnumerable<PersonId> ids, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<MediaId>>> GetMediaIdsAsync(PersonId id, CancellationToken cancellationToken = default);
}