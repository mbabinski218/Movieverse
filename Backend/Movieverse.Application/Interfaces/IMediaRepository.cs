using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.Interfaces;

public interface IMediaRepository
{
	public Task<Result> UpdateStatisticsAsync();
}