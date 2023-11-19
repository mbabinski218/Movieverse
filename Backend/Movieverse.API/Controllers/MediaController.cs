using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.API.Common;
using Movieverse.API.Common.Extensions;
using Movieverse.Application.Authorization;
using Movieverse.Application.Caching;
using Movieverse.Application.Metrics;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Contracts.DataTransferObjects.Content;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Contracts.Queries.Media;

namespace Movieverse.API.Controllers;

public sealed class MediaController : ApiController
{
	public MediaController(IMediator mediator) : base(mediator)
	{
	}
	
	[PolicyAuthorize(Policies.administrator)]
	[OutputCache(NoStore = true)]
	[HttpPost]
	public async Task<ActionResult> Add([FromBody] AddMediaCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[PolicyAuthorize(Policies.administrator)]
	[OutputCache(NoStore = true)]
	[HttpPut("{Id:guid}")]
	public async Task<ActionResult> Update([FromForm] UpdateMediaCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[Metrics(Meter.mediaCounter, AdditionalAttributeType.Id)]
	[OutputCache]
	[HttpGet("{Id:guid}")]
	public async Task<ActionResult> Get([FromRoute] GetMediaQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(PolicyName = CachePolicies.byQuery)]
	[HttpGet("latest")]
	public async Task<ActionResult> GetLatest([FromQuery] GetLatestMediaQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpGet("search")]
	public async Task<ActionResult> Search([FromQuery] SearchMediaQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpGet("searchWithFilters")]
	public async Task<ActionResult> SearchWithFilters([FromQuery] SearchMediaWithFiltersQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpGet("chart")]
	public async Task<ActionResult> Chart([FromQuery] MediaChartQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("genres")]
	public async Task<ActionResult> GetAll([FromQuery] GetAllGenresQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}/content")]
	public async Task<ActionResult<IEnumerable<ContentInfoDto>>> GetContent([FromRoute] GetContentPathQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}/platform")]
	public async Task<ActionResult<IEnumerable<PlatformInfoDto>>> GetPlatform([FromRoute] GetPlatformQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}/genre")]
	public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenre([FromRoute] GetGenreQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}/staff")]
	public async Task<ActionResult<IEnumerable<GenreInfoDto>>> GetStaff([FromRoute] GetStaffQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpGet("{Id:guid}/statistics")]
	public async Task<ActionResult<StatisticsDto>> GetStatistics([FromRoute] GetStatisticsQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache]
	[HttpGet("{Id:guid}/seasons")]
	public async Task<ActionResult<SeasonInfoDto>> GetSeasons([FromRoute] GetSeasonsQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpPost("{Id:guid}/review")]
	public async Task<ActionResult<ReviewDto>> AddReview([FromQuery] AddReviewCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
	
	[AllowAnonymous]
	[OutputCache(NoStore = true)]
	[HttpGet("{Id:guid}/review")]
	public async Task<ActionResult> AddReview([FromRoute] GetReviewQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			Ok,
			err => StatusCode(err.Code, err.Messages));
}