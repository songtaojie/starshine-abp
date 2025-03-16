// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.AspNetCore.VirtualFileSystem;

public class StarshineFileExtensionContentTypeProvider : FileExtensionContentTypeProvider, ITransientDependency
{
    public StarshineFileExtensionContentTypeProvider(IOptions<AspNetCoreContentOptions> aspNetCoreContentOptions):base(aspNetCoreContentOptions.Value.ContentTypeMaps)
    {
    }
}
