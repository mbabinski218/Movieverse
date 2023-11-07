namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class SeasonInfoDto
{
	public string Title { get; set; } = null!;
	public List<SeasonDto> Seasons { get; set; } = new();
}