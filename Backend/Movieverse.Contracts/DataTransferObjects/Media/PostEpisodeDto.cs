using Microsoft.AspNetCore.Http;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class PostEpisodeDto
{
	public short EpisodeNumber { get; set; }
	public string? Title { get; set; } = null!;
	public Details? Details { get; set; } = null!;
}