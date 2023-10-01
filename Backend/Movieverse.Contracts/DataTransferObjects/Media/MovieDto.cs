namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class MovieDto : MediaDto
{
	public Guid? SequelId { get; set; }
	public string? SequelTitle { get; set; }
	public Guid? PrequelId { get; set; }
	public string? PrequelTitle { get; set; }
}