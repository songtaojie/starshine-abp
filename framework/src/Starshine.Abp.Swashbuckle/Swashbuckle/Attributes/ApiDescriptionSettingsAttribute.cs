using Starshine.Abp.Swashbuckle.Internal;
using System;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 接口描述设置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ApiDescriptionSettingsAttribute : ApiExplorerSettingsAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiDescriptionSettingsAttribute() : base()
        {
            Order = 0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enabled">是否启用</param>
        public ApiDescriptionSettingsAttribute(bool enabled) : base()
        {
            IgnoreApi = !enabled;
            Order = 0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="groups">分组列表</param>
        public ApiDescriptionSettingsAttribute(params string[] groups) : base()
        {
            GroupName = string.Join(Penetrates.GroupSeparator, groups);
            Groups = groups;
            Order = 0;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string[]? Groups { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 额外描述，支持 HTML
        /// </summary>
        public string? Description { get; set; }
    }
}
