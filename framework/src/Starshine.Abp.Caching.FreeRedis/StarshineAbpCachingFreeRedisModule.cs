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

namespace Starshine.Abp.Caching.FreeRedis
{
    [DependsOn(typeof(AbpCachingModule))]
    public class StarshineAbpCachingFreeRedisModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var redisEnabled = configuration["Redis:IsEnabled"];
            if (redisEnabled.IsNullOrEmpty() || bool.Parse(redisEnabled))
            {
                var redisConfiguration = configuration["Redis:Configuration"];
                RedisClient redisClient = new RedisClient(redisConfiguration);

                context.Services.AddSingleton<IRedisClient>(redisClient);
                context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache>(new
                     DistributedCache(redisClient)));
            }
        }
    }
}
