using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Starshine.Abp.AspNetCore.Middleware;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.RequestLocalization;

public class RequestLocalizationMiddleware : MiddlewareBase, ITransientDependency
{
    public const string HttpContextItemName = "__StarshineSetCultureCookie";

    private readonly IRequestLocalizationOptionsProvider _requestLocalizationOptionsProvider;
    private readonly ILoggerFactory _loggerFactory;

    public RequestLocalizationMiddleware(
        IRequestLocalizationOptionsProvider requestLocalizationOptionsProvider,
        ILoggerFactory loggerFactory)
    {
        _requestLocalizationOptionsProvider = requestLocalizationOptionsProvider;
        _loggerFactory = loggerFactory;
    }

    public async override Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var middleware = new Localization.RequestLocalizationMiddleware(
            next,
            new OptionsWrapper<RequestLocalizationOptions>(
                await _requestLocalizationOptionsProvider.GetLocalizationOptionsAsync()
            ),
            _loggerFactory
        );

        context.Response.OnStarting(() =>
        {
            if (context.Items[HttpContextItemName] == null)
            {
                var requestCultureFeature = context.Features.Get<IRequestCultureFeature>();
                if (requestCultureFeature?.Provider is QueryStringRequestCultureProvider)
                {
                    RequestCultureCookieHelper.SetCultureCookie(
                        context,
                        requestCultureFeature.RequestCulture
                    );
                }
            }

            return Task.CompletedTask;
        });

        await middleware.Invoke(context);
    }
}
