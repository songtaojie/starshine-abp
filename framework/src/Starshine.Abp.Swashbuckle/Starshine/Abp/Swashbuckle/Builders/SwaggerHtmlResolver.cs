// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Swashbuckle.Builders;
public class SwaggerHtmlResolver : ISwaggerHtmlResolver, ISingletonDependency
{
    public virtual Stream Resolver()
    {
        var scriptBundleScript = "<script src=\"%(ScriptBundlePath)\" charset=\"utf-8\"></script>";
        var abpSwaggerScript = "<script src=\"ui/abp.swagger.js\" charset=\"utf-8\"></script>";
        var stream = typeof(SwaggerUIOptions).GetTypeInfo().Assembly
            .GetManifestResourceStream("Swashbuckle.AspNetCore.SwaggerUI.index.html");

        var html = new StreamReader(stream!)
            .ReadToEnd()
            .Replace(scriptBundleScript, $"{scriptBundleScript}\n{abpSwaggerScript}");

        return new MemoryStream(Encoding.UTF8.GetBytes(html));
    }
}
