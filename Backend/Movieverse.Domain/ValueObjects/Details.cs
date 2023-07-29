using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class Details : ValueObject
{
	public ushort? Runtime { get; set; }
	public ushort? Certificate { get; set; }
	public string? Storyline { get; set; }
	public string? Tagline { get; set; }
	public DateTimeOffset? ReleaseDate { get; set; }
	public string? CountryOfOrigin { get; set; }
	public string? Language { get; set; }
	public string? FilmingLocations { get; set; }

	private Details()
	{
		
	}
}