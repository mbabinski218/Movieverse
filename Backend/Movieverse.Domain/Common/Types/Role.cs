using NetEscapades.EnumGenerators;

namespace Movieverse.Domain.Common.Types;

[EnumExtensions]
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

public static partial class RoleExtensions
{
	public static Role ParseOrDefault(string value)
	{
		var success = TryParse(value, out var role);
		return success ? role : Role.Other;
	}
}