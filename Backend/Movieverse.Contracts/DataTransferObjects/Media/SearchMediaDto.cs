namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class SearchMediaDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public ushort Year { get; set; }
	public string? Poster { get; set; }
	public string? Description { get; set; }
}