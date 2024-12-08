using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using FreeRedis;
using Microsoft.Extensions.Options;

namespace Starshine.Abp.Caching.FreeRedis
{
    /// <summary>
    /// FreeRedis模块
    /// </summary>
    [DependsOn(typeof(AbpCachingModule))]
    public class StarshineAbpCachingFreeRedisModule: AbpModule
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var redisEnabled = configuration["CacheSettings:Redis:IsEnabled"];
            if (redisEnabled.IsNullOrEmpty() || bool.Parse(redisEnabled))
            {
                context.Services.AddStarshineOptions<CacheSettingsOptions>();
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
