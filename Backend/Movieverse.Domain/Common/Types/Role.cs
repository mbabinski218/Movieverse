namespace Movieverse.Domain.Common.Types;

public enum Role
{
	Director,
	Writer,
	Actor,
	Creator,
	Producer,
	Composer,
	Cinematographer,
	Editor,
	ArtDirector,
	CostumeDesigner,
	MakeupArtist,
	SoundDesigner,
	Other
}

public static class RoleHelper
{
	public static Role ToRole(this string str) => Enum.Parse<Role>(str);
	
	public static IEnumerable<string> Supported => Enum.GetNames<Role>();
}