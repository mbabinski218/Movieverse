using Movieverse.Domain.Common.Types;

namespace Movieverse.Contracts.DataTransferObjects.Media;

public sealed class StaffDto
{
	public Guid PersonId { get; set; }
	public Role Role { get; set; }
}