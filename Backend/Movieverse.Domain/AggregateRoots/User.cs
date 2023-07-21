using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.AggregateRoots;

public class User : IdentityEntity<ObjectId>
{
	public Information Information { get; set; } = null!;
	public ObjectId AvatarId { get; set; } = null!;
	public virtual List<MediaInfo> Data { get; set; } = new();
	public ObjectId? PersonId { get; set; }
}