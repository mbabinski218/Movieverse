using NetEscapades.EnumGenerators;

namespace Movieverse.Domain.Common.Types;

[EnumExtensions]
public enum UserRole
{
	Administrator,
	Critic,
	Pro,
	User
}
