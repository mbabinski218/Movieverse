using Microsoft.AspNetCore.Http;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class EpisodeDto
{
	public short? EpisodeNumber { get; set; }
	public string? Title { get; set; } = null!;
	public Details? Details { get; set; } = null!;
	public IEnumerable<IFormFile>? Images { get; set; }
	public IEnumerable<string>? Videos { get; set; }
}