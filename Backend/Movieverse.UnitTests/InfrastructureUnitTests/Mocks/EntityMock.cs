using Movieverse.Domain.AggregateRoots;
using NSubstitute;

namespace Movieverse.UnitTests.InfrastructureUnitTests.Mocks;

public static class EntityMock
{
	private const string email = "test@test.com";
	private const string userName = "Test";
	private const string firstName = "Test";
	private const string lastName = "Test";
	private const short age = 99;
	
	public static User CreateUser(Guid id)
	{
		var user = Substitute.For<User>(id ,email, userName, firstName, lastName, age);
		user.Id.Returns(id);
		return user;
	}
}