namespace Movieverse.Application.Common.Settings;

public sealed class QueuesSettings
{
	public const string key = "Queues";
	public AmazonSQS AmazonSQS { get; init; } = null!;
}

public sealed class AmazonSQS
{
	public const string key = "AmazonSQS";
	public string Host { get; init; } = null!;
	public string AccessKey { get; init; } = null!;
	public string SecretKey { get; init; } = null!;
	public string TopicArn { get; init; } = null!;
	public string TopicName { get; init; } = null!;
}
