using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.Commands.Person;

public sealed record UpdatePersonCommand(
	[FromRoute] Guid Id,
	Information? Information,
	LifeHistory? LifeHistory,
	string? Biography,
	string? FunFacts,
	bool? ChangePicture,
	IFormFile? Picture,
	IEnumerable<IFormFile>? Pictures,
	IEnumerable<Guid>? PicturesToRemove
	) : IRequest<Result>;