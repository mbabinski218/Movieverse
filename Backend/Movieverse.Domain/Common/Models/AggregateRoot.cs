namespace Movieverse.Domain.Common.Models;

public abstract class AggregateRoot<TKey> : Entity<TKey>
	where TKey : ValueObject
{
	
}