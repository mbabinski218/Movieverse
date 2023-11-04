namespace Movieverse.Contracts.DataTransferObjects.Content;

public sealed class ContentInfoDto
{
	public string Path { get; init; } = null!;
	public string ContentType { get; init; } = null!;
}