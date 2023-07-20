using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.ValueObjects;

public class ObjectId : ValueObject, IEquatable<ObjectId>
{
	public Guid Value { get; } = Guid.NewGuid();

	public bool Equals(ObjectId? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return base.Equals(other) && Value.Equals(other.Value);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		return obj.GetType() == GetType() && Equals((ObjectId)obj);
	}

	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);
}