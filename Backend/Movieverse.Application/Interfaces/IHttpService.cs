using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IHttpService
{
	Uri? Uri { get; }
	string? AccessToken { get; }
	AggregateRootId? UserId { get; }
}