using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.Contracts.Queries.Content;

namespace Movieverse.API.Controllers;

public sealed class ContentController : ApiController
{
	public ContentController(IMediator mediator) : base(mediator)
	{
	}
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}")]
	public async Task<ActionResult> GetContent([FromRoute] GetContentQuery query, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(query, cancellationToken);

		if (!result.IsSuccessful)
		{
			return StatusCode(result.Error.Code, result.Error.Messages);
		}
		
		return result.Value.ContentType == "video" ? 
			Content(result.Value.Path!, "video") : 
			File(result.Value.File!, result.Value.ContentType);
	}
}