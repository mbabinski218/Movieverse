using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public sealed class Information : ValueObject
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public short Age { get; set; }
}