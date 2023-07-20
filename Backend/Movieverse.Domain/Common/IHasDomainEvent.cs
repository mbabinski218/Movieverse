namespace Movieverse.Domain.Common;

public interface IHasDomainEvent
{
	IReadOnlyList<IDomainEvent> DomainEvents { get; }
	void ClearDomainEvents();
	void AddDomainEvent(IDomainEvent domainEvent);
}