using Starshine.Abp.AspNetCore.Serilog;

namespace Microsoft.AspNetCore.Builder;

public static class AspNetCoreSerilogApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSerilogEnrichers(this IApplicationBuilder app)
    {
        return app
            .UseMiddleware<AspNetCoreSerilogMiddleware>();
    }
}
