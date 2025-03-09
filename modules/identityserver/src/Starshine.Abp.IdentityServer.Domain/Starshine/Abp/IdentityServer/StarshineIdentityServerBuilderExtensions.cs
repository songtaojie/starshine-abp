using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using IdentityModel;
using Starshine.IdentityServer;
using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;
using Starshine.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.Security.Claims;
using IdentityUser = Starshine.Abp.Identity.IdentityUser;
using Starshine.Abp.Identity;
using System.Text.Json.Nodes;

namespace Starshine.Abp.IdentityServer;

/// <summary>
/// 将 Starshine IdentityServer 服务添加到指定的 <see cref="IIdentityServerBuilder"/>。
/// </summary>
public static class StarshineIdentityServerBuilderExtensions
{
    /// <summary>
    /// 将 Starshine IdentityServer 服务添加到指定的 <see cref="IIdentityServerBuilder"/>。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddStarshineIdentityServer(this IIdentityServerBuilder builder,StarshineIdentityServerBuilderOptions? options = null)
    {
        if (options == null)
        {
            options = new StarshineIdentityServerBuilderOptions();
        }

        //TODO:AspNet Identity 集成线。可以提取到扩展方法
        if (options.IntegrateToAspNetIdentity)
        {
            builder.AddAspNetIdentity<IdentityUser>();
            builder.AddProfileService<StarshineProfileService>();
            builder.AddResourceOwnerValidator<StarshineResourceOwnerPasswordValidator>();
            var userClaimsPrincipalFactoryService =  builder.Services.LastOrDefault(x => x.ServiceType == typeof(IUserClaimsPrincipalFactory<IdentityUser>));
            if(userClaimsPrincipalFactoryService!=null)
                builder.Services.Remove(userClaimsPrincipalFactoryService);
            builder.Services.AddTransient<IUserClaimsPrincipalFactory<IdentityUser>, StarshineUserClaimsFactory<IdentityUser>>();
            builder.Services.AddTransient<IObjectAccessor<IUserClaimsPrincipalFactory<IdentityUser>>, ObjectAccessor<StarshineUserClaimsPrincipalFactory>>();
        }

        builder.Services.Replace(ServiceDescriptor.Transient<IClaimsService, StarshineClaimsService>());

        if (options.UpdateAbpClaimTypes)
        {
            AbpClaimTypes.UserId = JwtClaimTypes.Subject;
            AbpClaimTypes.UserName = JwtClaimTypes.Name;
            AbpClaimTypes.Role = JwtClaimTypes.Role;
            AbpClaimTypes.Email = JwtClaimTypes.Email;
        }

        if (options.UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.UserId] = AbpClaimTypes.UserId;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.UserName] = AbpClaimTypes.UserName;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.Role] = AbpClaimTypes.Role;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.Email] = AbpClaimTypes.Email;
        }

        return builder;
    }

    //TODO: 使用最新的 Identity 服务器代码来优化性能。
    // https://github.com/IdentityServer/Starshine.IdentityServer/blob/main/src/Starshine.IdentityServer/src/Configuration/DependencyInjection/BuilderExtensions/Crypto.cs
    private static IIdentityServerBuilder AddStarshineDeveloperSigningCredential(
        this IIdentityServerBuilder builder,
        bool persistKey = true,
        string? filename = null,
        IdentityServerConstants.RsaSigningAlgorithm signingAlgorithm = IdentityServerConstants.RsaSigningAlgorithm.RS256)
    {
        if (filename == null)
        {
            filename = Path.Combine(Directory.GetCurrentDirectory(), "tempkey.rsa");
        }

        if (File.Exists(filename))
        {
            var keyFile = File.ReadAllText(filename);

            var json = JsonNode.Parse(keyFile);
            var keyId = json!["KeyId"]!.ToString();
            JsonNode jsonParameters = json["Parameters"]!;
            RSAParameters rsaParameters;
            rsaParameters.D = Convert.FromBase64String(jsonParameters["D"]!.GetValue<string>());
            rsaParameters.DP = Convert.FromBase64String(jsonParameters["DP"]!.GetValue<string>());
            rsaParameters.DQ = Convert.FromBase64String(jsonParameters["DQ"]!.GetValue<string>());
            rsaParameters.Exponent = Convert.FromBase64String(jsonParameters["Exponent"]!.GetValue<string>());
            rsaParameters.InverseQ = Convert.FromBase64String(jsonParameters["InverseQ"]!.GetValue<string>());
            rsaParameters.Modulus = Convert.FromBase64String(jsonParameters["Modulus"]!.GetValue<string>());
            rsaParameters.P = Convert.FromBase64String(jsonParameters["P"]!.GetValue<string>());
            rsaParameters.Q = Convert.FromBase64String(jsonParameters["Q"]!.GetValue<string>());

            return builder.AddSigningCredential(CryptoHelper.CreateRsaSecurityKey(rsaParameters, keyId), signingAlgorithm);
        }
        else
        {
            var key = CryptoHelper.CreateRsaSecurityKey();

            RSAParameters parameters;

            if (key.Rsa != null)
            {
                parameters = key.Rsa.ExportParameters(includePrivateParameters: true);
            }
            else
            {
                parameters = key.Parameters;
            }

            var jObject = new JsonObject
                {
                    {
                        "KeyId", key.KeyId
                    },
                    {
                        "Parameters", new JsonObject
                        {
                            {"D", Convert.ToBase64String(parameters.D!)},
                            {"DP", Convert.ToBase64String(parameters.DP!)},
                            {"DQ", Convert.ToBase64String(parameters.DQ!)},
                            {"Exponent", Convert.ToBase64String(parameters.Exponent!)},
                            {"InverseQ", Convert.ToBase64String(parameters.InverseQ!)},
                            {"Modulus", Convert.ToBase64String(parameters.Modulus!)},
                            {"P", Convert.ToBase64String(parameters.P!)},
                            {"Q", Convert.ToBase64String(parameters.Q!)}
                        }
                    }
                };

            if (persistKey)
            {
                File.WriteAllText(filename, jObject.ToString());
            }
            return builder.AddSigningCredential(key, signingAlgorithm);
        }
    }
}
