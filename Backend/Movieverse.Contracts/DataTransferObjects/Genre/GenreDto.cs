namespace Movieverse.Contracts.DataTransferObjects.Genre;

public sealed class GenreDto
{
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public int MediaCount { get; set; }
}