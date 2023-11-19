using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Application.Authorization;
using Movieverse.Contracts.Commands.Person;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Contracts.Queries.Person;
using Movieverse.Domain.Common;

namespace Movieverse.API.Controllers;

public sealed class PersonController : ApiController
{
	public PersonController(IMediator mediator) : base(mediator)
	{
	}
	
	[PolicyAuthorize(Policies.atLeastPro)]
    [OutputCache(NoStore = true)]
    [HttpPost]
    public async Task<ActionResult<string>> Create([FromQuery] CreatePersonCommand command, CancellationToken cancellationToken) =>
    	 await mediator.Send(command, cancellationToken).Then(
    		Ok,
    		err => StatusCode(err.Code, err.Messages));
    
    [PolicyAuthorize(Policies.atLeastPro)]
    [OutputCache(NoStore = true)]
    [HttpPut("{Id:guid}")]
    public async Task<ActionResult> Update([FromForm] UpdatePersonCommand command, CancellationToken cancellationToken) =>
	    await mediator.Send(command, cancellationToken).Then(
		    Ok,
		    err => StatusCode(err.Code, err.Messages));
    
    [AllowAnonymous]
    [OutputCache]
    [HttpGet("{Id:guid}")]
    public async Task<ActionResult> Get([FromRoute] GetPersonQuery query, CancellationToken cancellationToken) =>
	    await mediator.Send(query, cancellationToken).Then(
		    Ok,
		    err => StatusCode(err.Code, err.Messages));
    
    [AllowAnonymous]
    [OutputCache(NoStore = true)]
    [HttpGet("search")]
    public async Task<ActionResult<IPaginatedList<SearchPersonDto>>> Search([FromQuery] SearchPersonsQuery query, CancellationToken cancellationToken) =>
	    await mediator.Send(query, cancellationToken).Then(
		    Ok,
		    err => StatusCode(err.Code, err.Messages));
    
    [AllowAnonymous]
    [OutputCache(NoStore = true)]
    [HttpGet("chart")]
    public async Task<ActionResult<IPaginatedList<SearchPersonDto>>> Search([FromQuery] PersonsChartQuery query, CancellationToken cancellationToken) =>
	    await mediator.Send(query, cancellationToken).Then(
		    Ok,
		    err => StatusCode(err.Code, err.Messages));
    
    [AllowAnonymous]
    [OutputCache]
    [HttpGet("{Id:guid}/media")]
    public async Task<ActionResult<IEnumerable<MediaSectionDto>>> GetMedia([FromRoute] GetMediaQuery query, CancellationToken cancellationToken) =>
	    await mediator.Send(query, cancellationToken).Then(
		    Ok,
		    err => StatusCode(err.Code, err.Messages));
}