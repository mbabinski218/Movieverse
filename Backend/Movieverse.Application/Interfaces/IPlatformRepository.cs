﻿using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IPlatformRepository
{
	Task<Result> AddAsync(Platform platform, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(Platform platform, CancellationToken cancellationToken = default);
	Task<Result<Platform>> FindAsync(AggregateRootId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<Platform>>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<Result<List<AggregateRootId>>> GetAllMediaIdsAsync(AggregateRootId id, CancellationToken cancellationToken = default);
}