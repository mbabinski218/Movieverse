namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class MediaDemoDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public short Rating { get; set; }
	public Guid? PosterId { get; set; }
	public Guid? TrailerId { get; set; }
}