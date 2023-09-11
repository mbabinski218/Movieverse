namespace Movieverse.Application.Metrics;

public enum MetricsType : byte
{
	Counter,
	Histogram
}

public enum AdditionalAttributeType : byte
{
	None,
	Id,
	Query,
	Path
}

public sealed class Metrics
{
	public const string mediaCounter = "MediaCounter";

	public string Name { get; set; } = null!;
	public AdditionalAttributeType AdditionalAttributeType { get; set; }
	public MetricsType MetricsType { get; set; }

	public Metrics()
	{
		
	}
	
	public Metrics(string name, AdditionalAttributeType additionalAttributeType, MetricsType metricsType)
	{
		Name = name;
		AdditionalAttributeType = additionalAttributeType;
		MetricsType = metricsType;
	}
}

public static class MetricsExtensions
{
	public static Metrics BuildMetrics(this MetricsAttribute metricsAttribute) => 
		new(metricsAttribute.Name, metricsAttribute.AdditionalAttributeType, metricsAttribute.MetricsType);
}