using NetEscapades.EnumGenerators;

namespace Movieverse.Domain.Common.Types;

[EnumExtensions]
public enum GrantType
{
	Password,
	RefreshToken,
	Google,
	Facebook,
	Amazon
}