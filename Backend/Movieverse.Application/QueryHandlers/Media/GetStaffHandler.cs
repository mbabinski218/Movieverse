using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetStaffHandler : IRequestHandler<GetStaffQuery, Result<IEnumerable<StaffDto>>>
{
	private readonly ILogger<GetStaffHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;
	private readonly IPersonReadOnlyRepository _personRepository;

	public GetStaffHandler(ILogger<GetStaffHandler> logger, IMediaReadOnlyRepository mediaRepository, IPersonReadOnlyRepository personRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_personRepository = personRepository;
	}

	public async Task<Result<IEnumerable<StaffDto>>> Handle(GetStaffQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving staff info for media with id {Id}", request.Id);
		
		var staff = await _mediaRepository.GetStaffAsync(request.Id, cancellationToken);
		if (staff.IsUnsuccessful)
		{
			return staff.Error;
		}

		var persons = await _personRepository.GetPersonsAsync(staff.Value.Select(x => x.PersonId), cancellationToken);
		if (persons.IsUnsuccessful)
		{
			return persons.Error;
		}

		var staffDto = (
			from person in persons.Value
			let temp = staff.Value.FirstOrDefault(x => x.PersonId == person.Id)
			where temp is not null
			select new StaffDto
			{
				PersonId = person.Id,
				FirstName = person.FirstName,
				LastName = person.LastName,
				PictureId = person.PictureId,
				Role = temp.Role.ToStringFast()
			}
		).ToList();
		
		return staffDto;
	}
}