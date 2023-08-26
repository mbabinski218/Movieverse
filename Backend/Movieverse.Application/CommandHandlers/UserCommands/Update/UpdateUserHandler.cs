using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.Commands.User;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.CommandHandlers.UserCommands.Update;

public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IOutputCacheStore _outputCacheStore;

	public UpdateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IOutputCacheStore outputCacheStore)
	{
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_outputCacheStore = outputCacheStore;
	}

	public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		var findResult = await _userRepository.FindByIdAsync(request.Id, cancellationToken);

		if (!findResult.IsSuccessful)
		{
			return findResult.Error;
		}
		
		var user = findResult.Value;
		
		if (request.UserName is not null) user.UserName = request.UserName;
		if (request.Email is not null) user.Email = request.Email;
		if (request.Information is not null)
		{
			if (request.Information.FirstName is not null) user.Information.FirstName = request.Information.FirstName;
			if (request.Information.LastName is not null) user.Information.LastName = request.Information.LastName;
			if (request.Information.Age is not null) user.Information.Age = request.Information.Age.Value;
		}
		
		if (request.Avatar is not null)
		{
			if (user.AvatarId is null)
			{
				var avatarId = AggregateRootId.Create();
				user.AddDomainEvent(new ImageChanged(avatarId, request.Avatar));
				user.AvatarId = avatarId;
			}
			else
			{
				user.AddDomainEvent(new ImageChanged(user.AvatarId, request.Avatar));
			}
		}
		
		var updateResult = await _userRepository.UpdateAsync(user, cancellationToken);

		if (!updateResult.IsSuccessful)
		{
			return updateResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
		{
			return Error.Invalid("User update failed.");
		}
		
		await _outputCacheStore.EvictByTagAsync(request.Id.ToString(), cancellationToken);
		
        return _mapper.Map<UserDto>(user);
	}
}