using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Movieverse.UnitTests.InfrastructureUnitTests.Mocks;

public static class RoleManagerMock
{
	public static RoleManager<TRole> Get<TRole>()
		where TRole : class
	{
		var store = Substitute.For<IRoleStore<TRole>>();
		
		var roleValidators = new List<IRoleValidator<TRole>>
		{
			new RoleValidator<TRole>()
		};

		return Substitute.For<RoleManager<TRole>>(
			store, roleValidators, null, null, null);
	}
}