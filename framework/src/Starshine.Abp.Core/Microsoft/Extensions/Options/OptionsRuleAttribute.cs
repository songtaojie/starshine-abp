using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// 选项配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OptionsRuleAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OptionsRuleAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="postConfigureAll">启动所有实例进行后期配置</param>
        public OptionsRuleAttribute(bool postConfigureAll)
        {
            PostConfigureAll = postConfigureAll;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jsonKey">appsetting.json 对应键</param>
        /// <param name="postConfigureAll">启动所有实例进行后期配置</param>
        public OptionsRuleAttribute(string jsonKey, bool postConfigureAll)
        {
            JsonKey = jsonKey;
            PostConfigureAll = postConfigureAll;
        }


        /// <summary>
        /// 对应配置文件中的Key
        /// </summary>
        public string? JsonKey { get; set; }

        /// <summary>
        /// 对所有配置实例进行后期配置
        /// </summary>
        public bool PostConfigureAll { get; set; }
    }
}
