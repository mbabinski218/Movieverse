namespace Movieverse.Consumer.Common.Settings;

public sealed class AmazonSQSSettings
{
	public const string key = "AmazonSQS";
	public string Host { get; init; } = null!;
	public string AccessKey { get; init; } = null!;
	public string SecretKey { get; init; } = null!;
	public string QueueName { get; init; } = null!;
}