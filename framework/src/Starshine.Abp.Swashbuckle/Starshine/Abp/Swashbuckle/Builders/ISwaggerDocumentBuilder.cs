using IGeekFan.AspNetCore.Knife4jUI;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Swashbuckle.Builders
{
    /// <summary>
    /// Document构建器
    /// </summary>
    public interface ISwaggerDocumentBuilder
    {
        /// <summary>
        /// 构造SwaggerGen
        /// </summary>
        /// <param name="swaggerGenOptions"></param>
        void BuildSwaggerGen(SwaggerGenOptions swaggerGenOptions);

        /// <summary>
        /// 构造Swagger
        /// </summary>
        /// <param name="swaggerOptions"></param>
        /// <param name="configure"></param>
        void BuildSwagger(SwaggerOptions swaggerOptions, Action<SwaggerOptions>? configure = default);

        /// <summary>
        /// 构造SwaggerUI
        /// </summary>
        /// <param name="swaggerUIOptions"></param>
        /// <param name="configure"></param>
        void BuildSwaggerUI(SwaggerUIOptions swaggerUIOptions, Action<SwaggerUIOptions>? configure = default);

        /// <summary>
        /// 构造Knife4jSwaggerUI
        /// </summary>
        /// <param name="knife4UIOptions"></param>
        /// <param name="configure"></param>
        void BuildSwaggerKnife4jUI(Knife4UIOptions knife4UIOptions, Action<Knife4UIOptions>? configure = default);
    }
}
