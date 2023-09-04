namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class SeasonDto
{
	public short? SeasonNumber { get; set; }
	public IEnumerable<EpisodeDto>? Episodes { get; set; }
}