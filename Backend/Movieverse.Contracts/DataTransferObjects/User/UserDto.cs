using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.DataTransferObjects.User;

public sealed class UserDto
{
	public string UserName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string PhoneNumber { get; set; } = null!;
	public Information Information { get; set; } = null!;
	public Guid? AvatarPath { get; set; } = null!;
	public bool EmailConfirmed { get; set; }
	public Guid? PersonId { get; set; }
}