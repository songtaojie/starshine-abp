using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Swashbuckle.Filters
{
    /// <summary>
    /// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
    /// </summary>
    /// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
    /// Once they are fixed and published, this class can be removed.</remarks>
    internal class OpenApiSecurityFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // NOTE: This adds the "Padlock" icon to the endpoint in swagger, 
            //       we can also pass through the names of the policies in the string[]
            //       which will indicate which permission you require.
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Authorization"
                            },
                            Description="JWT Authorization header using the Bearer scheme.Enter Bearer {Token} in box below (note space between two)",
                            BearerFormat="JWT",
                            Scheme="Bearer",
                         },
                         new string[] { "global" }
                    }
                }
            };
        }
    }
}
