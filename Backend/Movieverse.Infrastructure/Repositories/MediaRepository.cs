using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Infrastructure.Repositories;

public sealed class MediaRepository : IMediaRepository
{
	public Task<Result> UpdateStatisticsAsync()
	{
		throw new NotImplementedException();
	}
}