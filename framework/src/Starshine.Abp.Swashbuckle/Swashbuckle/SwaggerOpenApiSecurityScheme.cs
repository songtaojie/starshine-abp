using Microsoft.OpenApi.Models;

namespace Starshine.Abp.Swashbuckle
{
    /// <summary>
    /// 规范化稳定安全配置
    /// </summary>
    public sealed class SwaggerOpenApiSecurityScheme : OpenApiSecurityScheme
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SwaggerOpenApiSecurityScheme()
        {
        }

        /// <summary>
        /// 唯一Id
        /// </summary>
        public string? Id { get; set; }
    }
}