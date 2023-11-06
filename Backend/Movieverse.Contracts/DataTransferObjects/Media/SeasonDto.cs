namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class SeasonDto
{
	public short SeasonNumber { get; set; }
	public List<EpisodeDto> Episodes { get; set; } = new();
}