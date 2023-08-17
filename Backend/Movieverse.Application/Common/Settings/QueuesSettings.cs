using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Settings;

public sealed class QueuesSettings : ISettings
{
	public string Key => "Queues";
	public AmazonSQS AmazonSQS { get; init; } = null!;
}

public sealed class AmazonSQS
{
	public string Host { get; init; } = null!;
	public string AccessKey { get; init; } = null!;
	public string SecretKey { get; init; } = null!;
	public string TopicName { get; init; } = null!;
}
