using NetEscapades.EnumGenerators;

namespace Movieverse.Domain.Common.Types;

[EnumExtensions]
public enum UserRole
{
	Administrator,
	Moderator,
	Critic,
	Pro,
	User
}
