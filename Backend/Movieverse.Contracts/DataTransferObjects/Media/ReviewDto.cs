namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class ReviewDto
{
	public Guid UserId { get; set; }
	public string UserName { get; set; } = null!;
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public short Rating { get; set; }
	public DateTimeOffset Date { get; private set; }
	public bool ByCritic { get; set; }
	public bool Spoiler { get; set; }
	public bool Modified { get; set; }
	public bool Deleted { get; set; }
	public bool Banned { get; set; }
}