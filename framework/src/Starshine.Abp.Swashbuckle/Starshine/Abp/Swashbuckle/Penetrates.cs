using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace Starshine.Abp.Swashbuckle
{
    /// <summary>
    /// 常量、公共方法配置类
    /// </summary>
    internal static class Penetrates
    {
        /// <summary>
        /// 分组分隔符
        /// </summary>
        internal const string GroupSeparator = "@";

        /// <summary>
        /// 控制器排序集合
        /// </summary>
        internal static ConcurrentDictionary<string, (string, int, Type)> ControllerOrderCollection { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        static Penetrates()
        {
            ControllerOrderCollection = new ConcurrentDictionary<string, (string, int, Type)>();

            IsApiControllerCached = new ConcurrentDictionary<Type, bool>();
        }

        /// <summary>
        /// <see cref="IsApiController(Type)"/> 缓存集合
        /// </summary>
        private static readonly ConcurrentDictionary<Type, bool> IsApiControllerCached;

        /// <summary>
        /// 是否是Api控制器
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        internal static bool IsApiController(Type type)
        {
            return IsApiControllerCached.GetOrAdd(type, Function);

            // 本地静态方法
            static bool Function(Type type)
            {
                // 排除 OData 控制器
                if (type.Assembly.GetName().Name!.StartsWith("Microsoft.AspNetCore.OData")) return false;

                // 不能是非公开、基元类型、值类型、抽象类、接口、泛型类
                if (!type.IsPublic || type.IsPrimitive || type.IsValueType || type.IsAbstract || type.IsInterface || type.IsGenericType) return false;

                // 继承 ControllerBase 
                if (!typeof(Controller).IsAssignableFrom(type) && typeof(ControllerBase).IsAssignableFrom(type))
                {
                    // 不是能被导出忽略的接口
                    if (type.IsDefined(typeof(ApiExplorerSettingsAttribute), true) && type.GetCustomAttribute<ApiExplorerSettingsAttribute>(true)!.IgnoreApi) return false;

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 清除字符串前后缀
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="pos">0：前后缀，1：后缀，-1：前缀</param>
        /// <param name="affixes">前后缀集合</param>
        /// <returns></returns>
        internal static string ClearStringAffixes(string str, int pos = 0, params string[] affixes)
        {
            // 空字符串直接返回
            if (string.IsNullOrWhiteSpace(str)) return str;

            // 空前后缀集合直接返回
            if (affixes == null || affixes.Length == 0) return str;

            var startCleared = false;
            var endCleared = false;

            string? tempStr = null;
            foreach (var affix in affixes)
            {
                if (string.IsNullOrWhiteSpace(affix)) continue;

                if (pos != 1 && !startCleared && str.StartsWith(affix, StringComparison.OrdinalIgnoreCase))
                {
                    tempStr = str[affix.Length..];
                    startCleared = true;
                }
                if (pos != -1 && !endCleared && str.EndsWith(affix, StringComparison.OrdinalIgnoreCase))
                {
                    var _tempStr = !string.IsNullOrWhiteSpace(tempStr) ? tempStr : str;
                    tempStr = _tempStr.Substring(0, _tempStr.Length - affix.Length);
                    endCleared = true;
                }
                if (startCleared && endCleared) break;
            }

            return !string.IsNullOrWhiteSpace(tempStr) ? tempStr : str;
        }

        /// <summary>
        /// 切割骆驼命名式字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string[] SplitCamelCase(string? str)
        {
            if (string.IsNullOrWhiteSpace(str)) return new string[] { };
            if (str.Length == 1) return new string[] { str };

            return Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})")
                .Where(u => u.Length > 0)
                .ToArray();
        }

        /// <summary>
        /// 获取骆驼命名第一个单词
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>首单词</returns>
        internal static string GetCamelCaseFirstWord(string str)
        {
            return SplitCamelCase(str).First();
        }
    }
}
