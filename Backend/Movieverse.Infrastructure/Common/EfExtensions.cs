using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Infrastructure.Common;

public static class EfExtensions
{
	public static readonly ValueConverter<AggregateRootId, Guid>? aggregateRootIdConverter = new
	(
		x => x.Value,
		x => AggregateRootId.Create(x)
	);
	
	public static readonly ValueConverter<AggregateRootId?, Guid?>? nullableAggregateRootIdConverter = new
	(
		x => x == null ? null : x.Value,
		x => x == null ? null : AggregateRootId.Create(x.Value)
	);
}