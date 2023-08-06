using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class Information : ValueObject
{
	public string? FirstName { get; set; } = null!;
	public string? LastName { get; set; } = null!;
	public ushort Age { get; set; }
}