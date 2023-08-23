namespace Movieverse.Application.Authorization;

public static class Policies
{
	public const string atLeastUser = "AtLeastUser";
	public const string atLeastPro = "AtLeastPro";
	public const string critic = "Critic";
	public const string administrator = "Administrator";
}