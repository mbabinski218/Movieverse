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