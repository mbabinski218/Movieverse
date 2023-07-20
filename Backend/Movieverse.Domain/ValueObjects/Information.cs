using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public class Information : ValueObject
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public short Age { get; set; }
}