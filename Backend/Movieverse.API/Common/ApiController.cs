using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Movieverse.API.Common;

[ApiController]
[Route("/api/[controller]")]
public class ApiController : ControllerBase
{
	protected readonly IMediator mediator;

	protected ApiController(IMediator mediator)
	{
		this.mediator = mediator;
	}
}