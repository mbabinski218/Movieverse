using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movieverse.API.Common;
using Movieverse.Application.Commands.UserCommands.Register;

namespace Movieverse.API.Controllers;

public sealed class UserController : ApiController
{
	public UserController(IMediator mediator) : base(mediator)
	{
	}
	
	[HttpPost("register")]
	public async Task<ActionResult> Register([FromBody] RegisterUserCommand command) => 
		await mediator.Send(command).Then(
		Ok,
		err => StatusCode(err.Code, err.Messages));

} 