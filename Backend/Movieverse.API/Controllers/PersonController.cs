using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Application.Authorization;
using Movieverse.Contracts.Commands.Person;
using Movieverse.Contracts.Queries.Person;

namespace Movieverse.API.Controllers;

public sealed class PersonController : ApiController
{
	public PersonController(IMediator mediator) : base(mediator)
	{
	}
	
	[PolicyAuthorize(Policies.atLeastUser)]
    [OutputCache(NoStore = true)]
    [HttpPost]
    public async Task<ActionResult> Create([FromForm] CreatePersonCommand command, CancellationToken cancellationToken) =>
    	 await mediator.Send(command, cancellationToken).Then(
    		Ok,
    		err => StatusCode(err.Code, err.Messages));
    
    [AllowAnonymous]
    [OutputCache]
    [HttpGet("{Id:guid}")]
    public async Task<ActionResult> Get([FromRoute] GetPersonQuery command, CancellationToken cancellationToken) =>
	    await mediator.Send(command, cancellationToken).Then(
		    Ok,
		    err => StatusCode(err.Code, err.Messages));
    
    [AllowAnonymous]
    [OutputCache(NoStore = true)]
    [HttpGet("search")]
    public async Task<ActionResult> Search([FromQuery] SearchPersonsQuery query, CancellationToken cancellationToken) =>
	    await mediator.Send(query, cancellationToken).Then(
		    Ok,
		    err => StatusCode(err.Code, err.Messages));
}