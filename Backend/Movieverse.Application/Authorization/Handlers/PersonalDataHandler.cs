﻿using Microsoft.AspNetCore.Authorization;
using Movieverse.Application.Authorization.Requirements;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.Authorization.Handlers;

public sealed class PersonalDataHandler : AuthorizationHandler<PersonalDataRequirement>
{
	private readonly IHttpService _httpService;

	public PersonalDataHandler(IHttpService httpService)
	{
		_httpService = httpService;
	}
	
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PersonalDataRequirement requirement)
	{
		if (_httpService.Role == UserRole.Administrator || _httpService.IdHeader == _httpService.UserId)
		{
			context.Succeed(requirement);
			return Task.CompletedTask;
		}
		
		return Task.CompletedTask;
	}
}