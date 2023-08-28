using Microsoft.AspNetCore.Localization;

namespace Movieverse.API.Common;

public sealed class AppRequestCultureProvider : IRequestCultureProvider
{
	private readonly string _culture;

	public AppRequestCultureProvider(string culture)
	{
		_culture = culture;
	}

	public Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
	{
		var culture = httpContext.Request.GetTypedHeaders().AcceptLanguage.FirstOrDefault()?.Value.Value ?? _culture;

		return Task.FromResult(new ProviderCultureResult(culture, culture))!;
	}
}