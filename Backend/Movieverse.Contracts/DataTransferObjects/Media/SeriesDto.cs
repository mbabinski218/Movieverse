namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class SeriesDto : MediaDto
{
	public short SeasonCount { get; set; }
	public short EpisodeCount { get; set; }
	public DateTimeOffset? SeriesEnded { get; set; }
}