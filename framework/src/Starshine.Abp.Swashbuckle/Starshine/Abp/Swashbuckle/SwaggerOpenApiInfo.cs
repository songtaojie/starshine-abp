using Microsoft.OpenApi.Models;

namespace Starshine.Abp.Swashbuckle
{
    /// <summary>
    /// 规范化文档开放接口信息
    /// </summary>
    public sealed class SwaggerOpenApiInfo : OpenApiInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SwaggerOpenApiInfo()
        {
            Version = "1.0.0";
        }

        /// <summary>
        /// 分组私有字段
        /// </summary>
        private string? _group;

        /// <summary>
        /// 所属组
        /// </summary>
        public string? Group
        {
            get => _group;
            set
            {
                _group = value;
                Title ??= string.Join(' ', Penetrates.SplitCamelCase(_group));
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool? Visible { get; set; }

        /// <summary>
        /// 路由模板
        /// </summary>
        public string? RouteTemplate { get; internal set; }
    }
}