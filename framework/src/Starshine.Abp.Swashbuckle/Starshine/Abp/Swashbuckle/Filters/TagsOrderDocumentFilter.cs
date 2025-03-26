// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Starshine.Abp.Swashbuckle.Filters
{
    /// <summary>
    /// 标签文档排序拦截器
    /// </summary>
    public class TagsOrderDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 配置拦截
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(u => u.Name).ToArray();
        }
    }
}