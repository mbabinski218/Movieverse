using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Staff : Entity<ObjectId>
{
	public ObjectId PersonId { get; set; } = null!;
	public Role Role { get; set; }
}