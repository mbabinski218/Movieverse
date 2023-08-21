using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IImageService
{
	Task<Result<PutObjectResponse>> UploadImageAsync(AggregateRootId id, IFormFile image, CancellationToken cancellationToken);
	Task<Result<DeleteObjectResponse>> DeleteImageAsync(AggregateRootId id, CancellationToken cancellationToken);
	Task<Result<GetObjectResponse>> GetImageAsync(AggregateRootId id, CancellationToken cancellationToken);
}