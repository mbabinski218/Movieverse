using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Application.Commands.UserCommands.ConfirmEmail;
using Movieverse.Application.Commands.UserCommands.Register;
using Movieverse.Application.Commands.UserCommands.ResendEmailConfirmation;

namespace Movieverse.API.Controllers;

public sealed class UserController : ApiController
{
	public UserController(IMediator mediator) : base(mediator)
	{
	}
	
	[AllowAnonymous]
	[HttpPost("register")]
	public async Task<ActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken) => 
		await mediator.Send(command, cancellationToken).Then(
		Ok,
		err => StatusCode(err.Code, err.Messages));

	[AllowAnonymous]
	[HttpPost("resend-email-confirmation")]
	public async Task<ActionResult> ResendEmailConfirmation([FromQuery] ResendEmailConfirmationCommand command, CancellationToken cancellationToken) => 
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[HttpPost("confirm-email")]
	public async Task<ActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command, CancellationToken cancellationToken) => 
		await mediator.Send(command, cancellationToken).Then(
		Ok,
		err => StatusCode(err.Code, err.Messages));
	
} 