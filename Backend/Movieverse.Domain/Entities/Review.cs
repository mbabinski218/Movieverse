using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Review : Entity<ObjectId>
{
	public Media Media { get; set; } = null!;
	public ObjectId MediaId { get; set; } = null!;
	public ObjectId UserId { get; set; } = null!;
	public string Username { get; set; } = null!;
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public short Rating { get; set; } //update po evencie RatingChanged, przy tworzeniu ustawienie na wartość w userze
	public DateTimeOffset Date { get; set; }
	public bool ByCritic { get; set; }
	public bool Spoiler { get; set; }
	public bool Deleted { get; set; }
	public bool Banned { get; set; }
}