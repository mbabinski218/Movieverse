using Movieverse.Application.Common.Result;

namespace Movieverse.Application.Interfaces;

public interface IMediaRepository
{
	public Task<Result> UpdateStatistics();
}