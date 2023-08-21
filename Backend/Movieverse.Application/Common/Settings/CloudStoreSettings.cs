using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Settings;

public sealed class CloudStoreSettings : ISettings
{
	public string Key => "CloudStore";
	public AmazonS3 AmazonS3 { get; init; } = null!;
}

public sealed class AmazonS3
{
	public string Host { get; init; } = null!;
	public string AccessKey { get; init; } = null!;
	public string SecretKey { get; init; } = null!;
	public string BucketName { get; init; } = null!;
	public string ImagesFolder { get; init; } = null!;
} 