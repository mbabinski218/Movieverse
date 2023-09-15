using Movieverse.Domain.ValueObjects.Ids;

namespace Movieverse.Application.Common.Extensions;

public static class AggregateRootExtensions
{
	public static Guid? GetValue(this AggregateRootId<Guid>? aggregateRootId) => aggregateRootId?.Value;
}