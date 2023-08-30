using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Contracts.Commands.User;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Queries.User;

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
	
		[AllowAnonymous]
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
	
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}")]
	public async Task<ActionResult<UserDto>> Get([FromRoute] GetUserByIdQuery query, CancellationToken cancellationToken) => 
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpPut("{Id:guid}")]
	public async Task<ActionResult<UserDto>> Update([FromForm] UpdateCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
} 