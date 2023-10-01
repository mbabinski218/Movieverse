using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Movieverse.ImageResizer;

public class Function
{
	private const string name = "x-amz-meta-name";
	private const string extension = "x-amz-meta-extension";
	private const string size = "x-amz-meta-size";
	private const string resized = "x-amz-meta-resized";
	
	private IAmazonS3 S3Client { get; } = new AmazonS3Client();
	
	// Lambda function handler
	public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
	{
		var eventRecords = evnt.Records ?? new List<S3Event.S3EventNotificationRecord>();
        
		foreach (var s3Event in eventRecords.Select(record => record.S3).Where(s3Event => s3Event != null))
		{
			try
			{
				context.Logger.LogInformation($"Processing object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}.");
				
				var objectMetadata = await S3Client.GetObjectMetadataAsync(s3Event.Bucket.Name, s3Event.Object.Key);

				if (objectMetadata.Metadata[resized] == true.ToString())
				{
					continue;
				}
				
				context.Logger.LogInformation($"Resizing image {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}.");
				
				await using var inputStream = await S3Client.GetObjectStreamAsync(s3Event.Bucket.Name, s3Event.Object.Key, null);
				
				using var image = await Image.LoadAsync(inputStream);
				image.Mutate(i => i.Resize(new ResizeOptions
				{
					Size = new Size(300, 300),
					Mode = ResizeMode.Max,
					Sampler = LanczosResampler.Lanczos3
				}));
				
				using var outputStream = new MemoryStream();
				await image.SaveAsync(outputStream, image.DetectEncoder(objectMetadata.Metadata[extension]));
				
				context.Logger.LogInformation($"Saving resized image {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}.");
				
				var putObjectRequest = new PutObjectRequest
				{
					BucketName = s3Event.Bucket.Name,
					Key = s3Event.Object.Key,
					ContentType = objectMetadata.Headers.ContentType,
					InputStream = outputStream,
					Metadata =
					{
						[name] = objectMetadata.Metadata[name],
						[extension] = objectMetadata.Metadata[extension],
						[size] = objectMetadata.Metadata[size],
						[resized] = true.ToString()
					}
				};
				
				await S3Client.PutObjectAsync(putObjectRequest);
			}
			catch (Exception ex)
			{
				context.Logger.LogError($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}.");
				context.Logger.LogError(ex.Message);
				context.Logger.LogError(ex.StackTrace);
				throw;
			}
		}
	}
}