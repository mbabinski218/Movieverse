﻿using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class GenreReadOnlyRepository : IGenreReadOnlyRepository
{
	private readonly ILogger<GenreReadOnlyRepository> _logger;
	private readonly Context _dbContext;
	private readonly IMapper _mapper;

	public GenreReadOnlyRepository(ILogger<GenreReadOnlyRepository> logger, Context dbContext, IMapper mapper)
	{
		_logger = logger;
		_dbContext = dbContext;
		_mapper = mapper;
	}
	
	public async Task<Result<GenreDto>> FindAsync(GenreId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Finding genre with id {Id} from database", id);
		
		var genre = await _dbContext.Genres
			.FirstOrDefaultAsync(g => g.Id == id, cancellationToken)
			.ConfigureAwait(false);
		
		return genre is null ? Error.NotFound(GenreResources.GenreNotFound) : _mapper.Map<GenreDto>(genre);
	}
	
	public async Task<Result<IPaginatedList<GenreDto>>> GetAllAsync(short? pageNumber = null, short? pageSize = null, 
		CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting all genres from database");
		
		return await _dbContext.Genres
			.ProjectToType<GenreDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken)
			.ConfigureAwait(false);
	}
}