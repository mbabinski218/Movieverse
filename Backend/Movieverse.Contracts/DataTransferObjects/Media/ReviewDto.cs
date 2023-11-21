namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class ReviewDto
{
	public Guid UserId { get; set; }
	public string UserName { get; set; } = null!;
	public string Text { get; set; } = null!;
	public DateTimeOffset Date { get; private set; }
}