namespace Movieverse.Application.Metrics;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MetricsAttribute : Attribute
{
	public string Name { get; }
	public AdditionalAttributeType AdditionalAttributeType { get; }
	public MetricsType MetricsType { get; }

	public MetricsAttribute(string name, AdditionalAttributeType additionalAttributeType = AdditionalAttributeType.None, 
		MetricsType metricsType = MetricsType.Counter)
	{
		Name = name;
		AdditionalAttributeType = additionalAttributeType;
		MetricsType = metricsType;
	}
}