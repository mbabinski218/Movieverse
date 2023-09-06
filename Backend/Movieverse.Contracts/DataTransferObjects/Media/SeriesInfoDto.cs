namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class SeriesInfoDto
{
	public IEnumerable<PostSeasonDto>? Seasons { get; set; }
}