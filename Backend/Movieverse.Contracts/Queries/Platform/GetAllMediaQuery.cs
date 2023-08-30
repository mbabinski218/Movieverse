using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Platform;

public sealed record GetAllMediaQuery(
	[FromRoute] Guid Id,
	string Type,
	short? PageNumber,
	short? PageSize
	) : IRequest<Result<IPaginatedList<MediaInfoDto>>>;