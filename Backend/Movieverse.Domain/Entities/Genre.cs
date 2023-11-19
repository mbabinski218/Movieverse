using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.Entities;

public class Genre : Entity<int>
{
	// Map to table
	private readonly List<Media> _media = new();
	
	public string Name { get; set; } = null!;
	public IReadOnlyList<Media> Media => _media.AsReadOnly();
	
	// Constructors
	private Genre(string name)
	{
		Name = name;
	}

	// Methods
	public static Genre Create(string name) => new(name);
	
	public void AddMedia(Media media)
	{
		_media.Add(media);
	}
	
	public void RemoveMedia(Media media)
	{
		_media.Remove(media);
	}

	// EF Core
	private Genre()
	{
		
	}
}