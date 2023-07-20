namespace Movieverse.Domain.Common.Types;

public enum GrantType
{
	Password,
	RefreshToken,
	Google,
	Facebook,
	Amazon
}

public static class GrantTypeHelper
{
	public static GrantType ToGrantType(this string str) => Enum.Parse<GrantType>(str);
	
	public static IEnumerable<string> Supported => Enum.GetNames<GrantType>();
}
