using NetEscapades.EnumGenerators;

namespace Movieverse.Domain.Common.Types;

[EnumExtensions]
public enum UserRole
{
	SystemAdministrator,
	Administrator,
	Pro,
	User
}
