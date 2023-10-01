namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class MediaShortInfoDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public short? Certificate { get; set; }
	public Guid? PosterId { get; set; }
	public short Rating { get; set; }
	public short StartYear { get; set; }
	public short? EndYear { get; set; }
}