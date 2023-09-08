using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IHttpService
{
	Uri? Uri { get; }
	AggregateRootId? IdHeader { get; }
	string? AccessToken { get; }
	AggregateRootId? UserId { get; }
	UserRole? Role { get; }
}