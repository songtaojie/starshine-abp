using Starshine.Abp.Swashbuckle.Internal;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Starshine.Abp.Swashbuckle.Filters
{
    /// <summary>
    /// 修正 规范化文档 Enum 提示
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        private readonly IReadOnlyList<Assembly> _assemblies;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        public EnumSchemaFilter(IReadOnlyList<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }
        /// <summary>
        /// 实现过滤器方法
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            Type type = context.Type;

            // 排除其他程序集的枚举
            if (type.IsEnum && _assemblies?.Contains(type.Assembly) == true)
            {
                model.Enum.Clear();
                var stringBuilder = new StringBuilder();
                stringBuilder.Append($"{model.Description}<br />");

                var enumValues = Enum.GetValues(type);
                // 获取枚举实际值类型
                var enumValueType = type.GetField("value__")?.FieldType;
                if (enumValueType == null) return;
                foreach (var value in enumValues)
                {
                    var numValue = value.ChangeType(enumValueType);

                    // 获取枚举成员特性
                    var numName = Enum.GetName(type, value);
                    if (string.IsNullOrEmpty(numName)) continue;
                    var fieldinfo = type.GetField(numName);
                    if(fieldinfo == null) continue;
                    var descriptionAttribute = fieldinfo.GetCustomAttribute<DescriptionAttribute>(true);
                    model.Enum.Add(OpenApiAnyFactory.CreateFromJson($"{numValue}"));

                    stringBuilder.Append($"&nbsp;{descriptionAttribute?.Description} {value} = {numValue}<br />");

                    //// 获取枚举成员特性
                    //var fieldinfo = type.GetField(Enum.GetName(type, value));
                    //var descriptionAttribute = fieldinfo.GetCustomAttribute<DescriptionAttribute>(true);
                    //model.Enum.Add(OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(value)));

                    //stringBuilder.Append($"&nbsp;{descriptionAttribute?.Description} {value} = {value}<br />");
                }
                model.Description = stringBuilder.ToString();
            }
        }
    }
}