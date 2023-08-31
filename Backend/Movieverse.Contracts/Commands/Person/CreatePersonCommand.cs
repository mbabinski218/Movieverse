using MediatR;
using Microsoft.AspNetCore.Http;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.Commands.Person;

public sealed record CreatePersonCommand(
	bool ForUser,
	Information? Information,
	LifeHistory LifeHistory,
	string? Biography,
	string? FunFacts,
	IEnumerable<IFormFile>? Pictures
	) : IRequest<Result>;