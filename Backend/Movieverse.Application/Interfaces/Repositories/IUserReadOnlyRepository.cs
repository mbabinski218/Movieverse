﻿using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Payments.PayPal.Responses;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IUserReadOnlyRepository
{
	Task<Result<UserDto>> FindAsync(Guid id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<WatchlistStatusDto>>> GetWatchlistStatusesAsync(Guid userId, IEnumerable<MediaId> mediaIds, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<MediaId>>> GetWatchlistAsync(Guid userId, CancellationToken cancellationToken = default);
	Task<Result<SubscriptionResponse>> GetSubscriptionAsync(Guid userId, CancellationToken cancellationToken = default);
	Task<bool> IsFreeTrialAvailableAsync(Guid userId, CancellationToken cancellationToken = default);
	Task<Result<MediaInfoDto>> GetMediaInfoAsync(Guid userId, MediaId mediaId, CancellationToken cancellationToken = default);
}