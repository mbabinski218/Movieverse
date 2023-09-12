using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Metrics;

public interface IMetricsService
{
	void CounterLogic(string name);
	void ResetCounter(string name, string additionalName);
	void HistogramLogic(string name);
	Dictionary<AggregateRootId, long> GetCounters(string name);
}