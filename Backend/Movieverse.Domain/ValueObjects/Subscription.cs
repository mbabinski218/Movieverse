using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class Subscription : ValueObject
{
	public bool FreeTrial { get; set; }
	public string? Id { get; set; }
	public DateTimeOffset? Since { get; set; }
}