using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Common.Extensions;

public static class AggregateRootExtensions
{
	public static Guid? GetValue(this AggregateRootId? aggregateRootId) => aggregateRootId?.Value;
}