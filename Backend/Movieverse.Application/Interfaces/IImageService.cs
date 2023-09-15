using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces;

public interface IImageService
{
	Task<Result<PutObjectResponse>> UploadImageAsync(ContentId id, IFormFile image, CancellationToken cancellationToken);
	Task<Result<DeleteObjectResponse>> DeleteImageAsync(ContentId id, CancellationToken cancellationToken);
	Task<Result<GetObjectResponse>> GetImageAsync(ContentId id, CancellationToken cancellationToken);
}