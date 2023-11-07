using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Application.Authorization;
using Movieverse.Contracts.Commands.User;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Queries.User;
using Movieverse.Domain.Common;

namespace Movieverse.API.Controllers;

public sealed class UserController : ApiController
{
	public UserController(IMediator mediator) : base(mediator)
	{
	}
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpPost("register")]
	public async Task<ActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken) =>
		 await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpPost("login")]
	public async Task<ActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.atLeastUser)]
    [OutputCache(NoStore = true)]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken) =>
    	await mediator.Send(new LogoutCommand(), cancellationToken).Then(
    		Ok,
    		err => StatusCode(err.Code, err.Messages));

	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpPost("resend-email-confirmation")]
	public async Task<ActionResult> ResendEmailConfirmation([FromQuery] ResendEmailConfirmationCommand command, CancellationToken cancellationToken) => 
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpPost("confirm-email")]
	public async Task<ActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command, CancellationToken cancellationToken) =>
		 await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	
	[PolicyAuthorize(Policies.personalData)]
	// [OutputCache(PolicyName = CachePolicies.byUserId)]
	[OutputCache(NoStore = true)]
	[HttpGet]
	public async Task<ActionResult<UserDto>> Get(CancellationToken cancellationToken) => 
		await mediator.Send(new GetUserByIdQuery(), cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.atLeastUser)]
	[PolicyAuthorize(Policies.personalData)]
	[OutputCache(NoStore = true)]
	[HttpPut]
	public async Task<ActionResult<UserDto>> Update([FromForm] UpdateCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.administrator)]
	[OutputCache(NoStore = true)]
	[HttpPut("roles")]
	public async Task<ActionResult> UpdateRoles([FromBody] UpdateRolesCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.atLeastUser)]
	[OutputCache(NoStore = true)]
	[HttpPut("watchlist/{MediaId:guid}")]
	public async Task<ActionResult> UpdateWatchlist([FromRoute] UpdateWatchlistCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.atLeastUser)]
	[OutputCache(NoStore = true)]
	[HttpPost("watchlist")]
	public async Task<ActionResult<IEnumerable<WatchlistStatusDto>>> GetWatchlistStatuses([FromBody] GetWatchlistStatusesQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.personalData)]
	// [OutputCache(PolicyName = CachePolicies.byUserId)]
	[OutputCache(NoStore = true)]
	[HttpGet("watchlist")]
	public async Task<ActionResult<IPaginatedList<SearchMediaDto>>> GetWatchlist([FromQuery] GetWatchlistQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.personalData)]
	[OutputCache(NoStore = true)]
	[HttpPut("rating/{MediaId:guid}/{Rating:int}")]
	public async Task<ActionResult> UpdateRating([FromRoute] UpdateRatingCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.personalData)]
	[OutputCache(NoStore = true)]
	[HttpGet("{MediaId:guid}")]
	public async Task<ActionResult> GetMediaInfo([FromRoute] GetMediaInfoQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
} 