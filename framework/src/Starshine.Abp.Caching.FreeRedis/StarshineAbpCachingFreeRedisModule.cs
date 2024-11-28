using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using FreeRedis;
using Microsoft.Extensions.Options;

namespace Starshine.Abp.Caching.FreeRedis
{
    [DependsOn(typeof(AbpCachingModule))]
    public class StarshineAbpCachingFreeRedisModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStarshineOptions<CacheSettingsOptions>();
            var configuration = context.Services.GetConfiguration();
            var redisEnabled = configuration["CacheSettings:Redis:IsEnabled"];
            if (redisEnabled.IsNullOrEmpty() || bool.Parse(redisEnabled))
            {
                context.Services.AddSingleton<IRedisClient>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<CacheSettingsOptions>>().Value;
                    var redisOptions = options.Redis;
                    if (redisOptions == null || string.IsNullOrEmpty(redisOptions.ConnectionString))
                        throw new ArgumentNullException(nameof(RedisCacheSettingsOptions.ConnectionString));
                    if (redisOptions.SlaveConnectionStrings == null || !redisOptions.SlaveConnectionStrings.Any())
                    {
                        return new RedisClient(redisOptions.ConnectionString);
                    }
                    else
                    {
                        var slaveConnectionStrings = redisOptions.SlaveConnectionStrings.Select(r => ConnectionStringBuilder.Parse(r)).ToArray();
                        return new RedisClient(redisOptions.ConnectionString, slaveConnectionStrings);
                    }
                });
                context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache>(sp =>
                {
                    var redisClient = sp.GetService<IRedisClient>();
                    return new DistributedCache(redisClient as RedisClient);
                }));
            }
        }
    }
}
