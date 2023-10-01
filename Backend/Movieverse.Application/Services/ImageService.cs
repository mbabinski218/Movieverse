using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Services;

public sealed class ImageService : IImageService
{
	private readonly ILogger<ImageService> _logger;
	private readonly IAmazonS3 _s3Client;
	private readonly CloudStoreSettings _cloudStoreSettings;

	public ImageService(ILogger<ImageService> logger, IAmazonS3 s3Client, IOptions<CloudStoreSettings> cloudStoreSettings)
	{
		_logger = logger;
		_s3Client = s3Client;
		_cloudStoreSettings = cloudStoreSettings.Value;
	}

	public async Task<Result<PutObjectResponse>> UploadImageAsync(ContentId id, IFormFile image, CancellationToken cancellationToken)
	{
		var key = GetImageKey(id);
		
		_logger.LogDebug("Uploading image with key {key} to S3.", key);

		try
		{
			var putObjectRequest = new PutObjectRequest
			{
				BucketName = _cloudStoreSettings.AmazonS3.BucketName,
				Key = key,
				ContentType = image.ContentType,
				InputStream = image.OpenReadStream(),
				Metadata =
				{
					["x-amz-meta-name"] = Path.GetFileName(image.FileName),
					["x-amz-meta-extension"] = Path.GetExtension(image.FileName),
					["x-amz-meta-size"] = image.Length.ToString(),
					["x-amz-meta-resized"] = false.ToString()
				}
			};

			return await _s3Client.PutObjectAsync(putObjectRequest, cancellationToken);
		}
		catch (AmazonS3Exception ex)
		{
			return Error.Invalid(ex.Message);
		}
	}

	public async Task<Result<DeleteObjectResponse>> DeleteImageAsync(ContentId id, CancellationToken cancellationToken)
	{
		var key = GetImageKey(id);
		
		_logger.LogDebug("Deleting image with key {key} from S3.", key);

		try
		{
			var deleteObjectRequest = new DeleteObjectRequest
			{
				BucketName = _cloudStoreSettings.AmazonS3.BucketName,
				Key = key
			};
			
			return await _s3Client.DeleteObjectAsync(deleteObjectRequest, cancellationToken);
		}
		catch (AmazonS3Exception ex) when (ex.Message is "The specified key does not exist.")
		{
			return Error.NotFound($"Image with key {key} not found.");
		}
		catch (AmazonS3Exception ex)
		{
			return Error.Invalid(ex.Message);
		}
	}

	public async Task<Result<GetObjectResponse>> GetImageAsync(ContentId id, CancellationToken cancellationToken)
	{
		var key = GetImageKey(id);
		
		_logger.LogDebug("Getting image with key {key} from S3.", key);

		try
		{
			var getObjectRequest = new GetObjectRequest
			{
				BucketName = _cloudStoreSettings.AmazonS3.BucketName,
				Key = key
			};
			
			return await _s3Client.GetObjectAsync(getObjectRequest, cancellationToken);
		}
		catch (AmazonS3Exception ex) when (ex.Message is "The specified key does not exist.")
		{
			return Error.NotFound($"Image with key {key} not found.");
		}
		catch (AmazonS3Exception ex)
		{
			return Error.Invalid(ex.Message);
		}
	}
	
	private string GetImageKey(ContentId id) => $"{_cloudStoreSettings.AmazonS3.ImagesFolder}/{id.Value.ToString()}";
}