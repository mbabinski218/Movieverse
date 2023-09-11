using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Movieverse.Application.Metrics;

public sealed class MetricsService : IMetricsService
{
	private static readonly ConcurrentDictionary<string, long> counters = new();
	
	public void CounterLogic(string name)
	{ 
		counters.AddOrUpdate(name, key => 1, (key, actualValue) => actualValue + 1);
	}

	public void ResetCounter(string name, string? additionalAttribute = null)
	{
		var fullName = $"{name} {additionalAttribute}";

		foreach (var key in counters.Where(x => x.Key.StartsWith(fullName)).Select(x => x.Key))
		{
			counters.TryRemove(key, out _);
		}
	}

	public void HistogramLogic(string name)
	{
		throw new NotImplementedException();
	}

	public ImmutableDictionary<string, long> GetCounters(string? name = null)
		=> name is null 
			? counters.ToImmutableDictionary() 
			: counters.Where(x => x.Key.StartsWith(name)).ToImmutableDictionary();
}