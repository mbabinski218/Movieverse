using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Domain.ValueObjects;

public class Staff : ValueObject
{
	public ObjectId MediaId { get; set; } = null!;
	public Role Role { get; set; }
}