using Movieverse.Application.Common.Result;
using Movieverse.Application.Interfaces;

namespace Movieverse.Infrastructure.Repositories;

public sealed class MediaRepository : IMediaRepository
{
	public Task<Result> UpdateStatistics()
	{
		throw new NotImplementedException();
	}
}