﻿using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces;

public interface IPersonRepository
{
	Task<Result> AddAsync(Person person, CancellationToken cancellationToken = default);
	Task<Result<Person>> FindAsync(PersonId id, CancellationToken cancellationToken = default);
}