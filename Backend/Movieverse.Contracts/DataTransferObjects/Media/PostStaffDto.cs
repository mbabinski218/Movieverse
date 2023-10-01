using Movieverse.Domain.Common.Types;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class PostStaffDto
{
	public Guid PersonId { get; set; }
	public Role Role { get; set; }
}