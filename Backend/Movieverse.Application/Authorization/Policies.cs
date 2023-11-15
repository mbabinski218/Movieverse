namespace Movieverse.Application.Authorization;

public static class Policies
{
	public const string atLeastUser = "AtLeastUser";
	public const string atLeastPro = "AtLeastPro";
	public const string administrator = "Administrator";
	public const string personalData = "PersonalData";
}