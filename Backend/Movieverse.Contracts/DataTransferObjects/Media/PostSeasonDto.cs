namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class PostSeasonDto
{
	public short SeasonNumber { get; set; }
	public IEnumerable<PostEpisodeDto>? Episodes { get; set; }
}