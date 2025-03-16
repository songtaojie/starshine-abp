using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Volo.Abp.Options;

namespace Microsoft.AspNetCore.RequestLocalization;

public class RequestLocalizationOptionsManager : AbpDynamicOptionsManager<RequestLocalizationOptions>
{
    private RequestLocalizationOptions? _options;

    private readonly IRequestLocalizationOptionsProvider _abpRequestLocalizationOptionsProvider;

    public RequestLocalizationOptionsManager(
        IOptionsFactory<RequestLocalizationOptions> factory,
        IRequestLocalizationOptionsProvider abpRequestLocalizationOptionsProvider)
        : base(factory)
    {
        _abpRequestLocalizationOptionsProvider = abpRequestLocalizationOptionsProvider;
    }

    public override RequestLocalizationOptions Get(string? name)
    {
        return _options ?? base.Get(name);
    }

    protected override async Task OverrideOptionsAsync(string name, RequestLocalizationOptions options)
    {
        _options = await _abpRequestLocalizationOptionsProvider.GetLocalizationOptionsAsync();
    }
}
