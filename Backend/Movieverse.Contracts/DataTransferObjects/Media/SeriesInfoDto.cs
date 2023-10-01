namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class SeriesInfoDto
{
	public IEnumerable<PostSeasonDto>? Seasons { get; set; }
	public DateTimeOffset? SeriesEnded { get; set; }
}