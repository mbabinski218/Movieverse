using MediatR;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Content;
using Movieverse.Contracts.Queries.Content;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Content;

public sealed class GetContentHandler : IRequestHandler<GetContentQuery, Result<ContentDto>>
{
	private readonly IImageService _imageService;
	private readonly IContentReadOnlyRepository _contentRepository;

	public GetContentHandler(IImageService imageService, IContentReadOnlyRepository contentRepository)
	{
		_imageService = imageService;
		_contentRepository = contentRepository;
	}

	public async Task<Result<ContentDto>> Handle(GetContentQuery request, CancellationToken cancellationToken)
	{
		var contentTypeResult = await _contentRepository.GetContentTypeAsync(request.Id, cancellationToken);
		if (!contentTypeResult.IsSuccessful)
		{
			return contentTypeResult.Error;
		}
		
		if (contentTypeResult.Value == "video")
		{
			var pathResult = await _contentRepository.GetPathAsync(request.Id, cancellationToken);
			
			return !pathResult.IsSuccessful ? 
				pathResult.Error :
				new ContentDto
				{
					Path = pathResult.Value,
					ContentType = "video"
				};
		}
		
		var imageResult = await _imageService.GetImageAsync(request.Id, cancellationToken);

		return !imageResult.IsSuccessful ? 
			imageResult.Error :
			new ContentDto
			{
				File = imageResult.Value.ResponseStream,
				ContentType = imageResult.Value.Headers.ContentType
			};
	}
}