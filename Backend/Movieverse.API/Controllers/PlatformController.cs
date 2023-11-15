using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Application.Authorization;
using Movieverse.Contracts.Commands.Platform;
using Movieverse.Contracts.Queries.Platform;

namespace Movieverse.API.Controllers;

public sealed class PlatformController : ApiController
{
	public PlatformController(IMediator mediator) : base(mediator)
	{
	}
	
	[PolicyAuthorize(Policies.administrator)]
	[OutputCache(NoStore = true)]
	[HttpPost]
	public async Task<ActionResult> Add([FromForm] AddPlatformCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.administrator)]
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
    
	[AllowAnonymous]
	[OutputCache]
	[HttpGet]
	public async Task<ActionResult> GetAll(CancellationToken cancellationToken) =>
		await mediator.Send(new GetPlatformsQuery(), cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}/media")]
	public async Task<ActionResult> GetAllMedia([FromQuery] GetAllMediaQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
}