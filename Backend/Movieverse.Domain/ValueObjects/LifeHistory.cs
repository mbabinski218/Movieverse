using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class LifeHistory : ValueObject
{
	public string? BirthPlace { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
    public string? DeathPlace { get; set; }
    public DateTimeOffset? DeathDate { get; set; }
    public DateTimeOffset? CareerStart { get; set; }
    public DateTimeOffset? CareerEnd { get; set; }
}