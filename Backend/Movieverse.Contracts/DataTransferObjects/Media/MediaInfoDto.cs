namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class MediaInfoDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public short? Certificate { get; set; }
	public short? Runtime { get; set; }
	public Guid? PosterId { get; set; }
	public string? Storyline { get; set; }
	public short Rating { get; set; }
	public string Type { get; set; } = null!;
	public short StartYear { get; set; }
	public short? EndYear { get; set; }
}