﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects.Id;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
	private readonly IAppDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityUserRole> _roleManager;

	public UserRepository(IAppDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityUserRole> roleManager)
	{
		_dbContext = dbContext;
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task<Result<User>> FindByIdAsync(AggregateRootId id)
	{
		var user = await _dbContext.Users.FindAsync(id.Value);
		return user is null ? Error.NotFound($"User with id: {id.Value} not found") : user;
	}

	public async Task<Result<User>> FindByEmailAsync(string email)
	{
		var user = await _userManager.FindByEmailAsync(email);
		return user is null ? Error.NotFound($"User with email: {email} not found") : user;
	}

	public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user)
	{
		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		return token;
	}
	
	public async Task<Result<string>> RegisterAsync(User user, string password)
	{
		var result = await _userManager.CreateAsync(user, password);
		
		if(!result.Succeeded) GenerateError(result);
		
		result = await _userManager.AddToRoleAsync(user, UserRole.User.ToString());
        
		if(!result.Succeeded) GenerateError(result);

		var claims = new List<Claim>
		{
			new(ClaimNames.id, user.Id.ToString()),
			new(ClaimNames.email, user.Email!),
			new(ClaimNames.displayName, user.Information.FirstName ?? user.UserName!),
			new(ClaimNames.age, user.Information.Age.ToString()),
			new(ClaimNames.role, UserRole.User.ToString())
		};
		result = await _userManager.AddClaimsAsync(user, claims);
		
		return result.Succeeded 
			? await GenerateEmailConfirmationTokenAsync(user) 
			: GenerateError(result);
	}

	public async Task<Result> ConfirmEmailAsync(User user, string token)
	{
		var result = await _userManager.ConfirmEmailAsync(user, token);
		return result.Succeeded ? Result.Ok() : GenerateError(result);
	}

	private static Error GenerateError(IdentityResult? result)
	{
		var errors = result?.Errors.Select(e => e.Description).ToList() ?? new List<string>();
		return Error.Invalid(errors);
	}
}