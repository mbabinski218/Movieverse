namespace Movieverse.Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject>
{
	protected virtual IEnumerable<object?> GetEqualityComponents() => GetType()
			.GetProperties()
			.Select(x => x.GetValue(this));

	public override bool Equals(object? obj)
	{
		if (obj == null || obj.GetType() != GetType())
		{
			return false;
		}

		var valueObject = (ValueObject)obj;
		
		return GetEqualityComponents()
			.SequenceEqual(valueObject.GetEqualityComponents());
	}
	
	public bool Equals(ValueObject? other) => Equals((object?)other);

	public override int GetHashCode() => GetEqualityComponents()
			.Select(x => x?.GetHashCode() ?? 0)
			.Aggregate((x, y) => x ^ y);

	public static bool operator ==(ValueObject? left, ValueObject? right) => Equals(left, right);

	public static bool operator !=(ValueObject? left, ValueObject? right) => !Equals(left, right);
}