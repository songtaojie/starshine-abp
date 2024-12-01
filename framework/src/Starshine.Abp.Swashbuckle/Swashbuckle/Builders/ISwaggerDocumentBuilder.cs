using IGeekFan.AspNetCore.Knife4jUI;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Swashbuckle
{
    public interface ISwaggerDocumentBuilder
    {
        void BuildSwaggerGen(SwaggerGenOptions swaggerGenOptions);

        void BuildSwagger(SwaggerOptions swaggerOptions, Action<SwaggerOptions>? configure = default);

        void BuildSwaggerUI(SwaggerUIOptions swaggerUIOptions, Action<SwaggerUIOptions>? configure = default);

        void BuildSwaggerKnife4jUI(Knife4UIOptions knife4UIOptions, Action<Knife4UIOptions>? configure = default);
    }
}
