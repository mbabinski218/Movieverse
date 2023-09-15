using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.Interfaces;

public interface IHttpService
{
	Uri? Uri { get; }
	Guid? IdHeader { get; }
	string? AccessToken { get; }
	Guid? UserId { get; }
	UserRole? Role { get; }
}