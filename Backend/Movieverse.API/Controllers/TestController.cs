using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Contracts.Queries;

namespace Movieverse.API.Controllers;

public sealed class TestController : ApiController
{
	public TestController(IMediator mediator) : base(mediator)
	{
	}
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("query")]
	public async Task<IActionResult> Query(CancellationToken cancellationToken) => 
		await mediator.Send(new TestQuery(), cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
}