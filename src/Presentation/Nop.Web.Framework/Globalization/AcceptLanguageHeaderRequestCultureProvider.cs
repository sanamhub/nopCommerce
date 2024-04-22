using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Nop.Core.Infrastructure;
using Nop.Services.Localization;

namespace Nop.Web.Framework.Globalization;
public class NopAcceptLanguageHeaderRequestCultureProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var acceptLanguageHeader = httpContext.Request.GetTypedHeaders().AcceptLanguage;

        if (acceptLanguageHeader == null || acceptLanguageHeader.Count == 0)
            return NullProviderCultureResult;

        var languageHeader = acceptLanguageHeader.FirstOrDefault();

        var languageService = EngineContext.Current.Resolve<ILanguageService>();
        var language = languageService
            .GetAllLanguages()
            .FirstOrDefault(urlLanguage => 
                new CultureInfo(urlLanguage.LanguageCulture).TwoLetterISOLanguageName.Equals(languageHeader.ToString(), StringComparison.InvariantCultureIgnoreCase));

        if (language == null)
            return NullProviderCultureResult;

        return Task.FromResult(new ProviderCultureResult(language.LanguageCulture));

    }
}
