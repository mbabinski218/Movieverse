using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Movieverse.UnitTests.InfrastructureUnitTests.Mocks;

public static class UserManagerMock
{
	public static UserManager<TUser> Get<TUser>()
		where TUser : class
	{
		var store = Substitute.For<IUserStore<TUser>>();
		var passwordHasher = Substitute.For<IPasswordHasher<TUser>>();
		var userValidators = new List<IUserValidator<TUser>>
		{
			new UserValidator<TUser>()
		};
		var passwordValidators = new List<IPasswordValidator<TUser>>
		{
			new PasswordValidator<TUser>()
		};
		userValidators.Add(new UserValidator<TUser>());
		passwordValidators.Add(new PasswordValidator<TUser>());
		
		return Substitute.For<UserManager<TUser>>(
			store, null, passwordHasher, userValidators, passwordValidators, null, null, null, null);
	}
}