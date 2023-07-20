namespace Movieverse.Domain.Common.Types;

public enum UserRole
{
	Administrator,
	Moderator,
	Critic,
	Pro,
	User
}

public static class UserRoleHelper
{
	public static UserRole ToUserRole(this string str) => Enum.Parse<UserRole>(str);
	
	public static IEnumerable<string> Supported => Enum.GetNames<UserRole>();
}
