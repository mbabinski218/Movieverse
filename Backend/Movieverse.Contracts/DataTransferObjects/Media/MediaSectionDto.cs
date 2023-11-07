namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class MediaSectionDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public decimal Rating { get; set; }
	public Guid? PosterId { get; set; }
    public ushort? Year { get; set; }
}