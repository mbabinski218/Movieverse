namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class SeriesInfoDto
{
	public IEnumerable<SeasonDto>? Seasons { get; set; }
}