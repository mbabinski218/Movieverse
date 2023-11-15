using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record ChangePasswordCommand(
	string CurrentPassword,
	string NewPassword,
	string ConfirmNewPassword
	) : IRequest<Result>;
