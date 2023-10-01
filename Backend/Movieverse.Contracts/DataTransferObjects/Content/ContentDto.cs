namespace Movieverse.Contracts.DataTransferObjects.Content;

public sealed class ContentDto
{
	public Stream? File { get; init; }
	public string? Path { get; init; }
	public string ContentType { get; init; } = null!;
}