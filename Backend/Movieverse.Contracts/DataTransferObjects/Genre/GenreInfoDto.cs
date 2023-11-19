namespace Movieverse.Contracts.DataTransferObjects.Genre;

public sealed class GenreInfoDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
}