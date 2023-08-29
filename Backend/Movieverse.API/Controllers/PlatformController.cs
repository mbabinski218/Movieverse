using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Contracts.Commands.Platform;
using Movieverse.Contracts.Queries;

namespace Movieverse.API.Controllers;

public sealed class PlatformController : ApiController
{
	public PlatformController(IMediator mediator) : base(mediator)
	{
	}
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpPost]
	public async Task<ActionResult> Add([FromForm] AddPlatformCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpPut("{Id:guid}")]
	public async Task<ActionResult> Update([FromForm] UpdatePlatformCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}")]
	public async Task<ActionResult> Get([FromRoute] GetPlatformQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
}