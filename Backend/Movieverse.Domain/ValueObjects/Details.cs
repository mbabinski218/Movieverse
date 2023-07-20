using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public class Details : ValueObject
{
	public int? Runtime { get; set; }
	public short? Certificate { get; set; }
	public string? Storyline { get; set; }
	public string? Tagline { get; set; }
	public DateTimeOffset? ReleaseDate { get; set; }
	public string? CountryOfOrigin { get; set; }
	public string? Language { get; set; }
	public string? FilmingLocations { get; set; }
}