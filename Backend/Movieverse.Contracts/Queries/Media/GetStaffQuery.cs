using MediatR;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public sealed record GetStaffQuery(
	Guid Id
	) : IRequest<Result<IEnumerable<StaffDto>>>;