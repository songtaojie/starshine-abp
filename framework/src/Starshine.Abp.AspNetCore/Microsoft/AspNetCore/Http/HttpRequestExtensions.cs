// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Net.Http.Headers;
using Volo.Abp;

namespace Microsoft.AspNetCore.Http;

public static class HttpRequestExtensions
{
    public static bool IsAjax(this HttpRequest request)
    {
        Check.NotNull(request, nameof(request));

        return string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
               string.Equals(request.Headers[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal);
    }

    public static bool CanAccept(this HttpRequest request, string contentType)
    {
        Check.NotNull(request, nameof(request));
        Check.NotNull(contentType, nameof(contentType));

        return request.Headers[HeaderNames.Accept].ToString().Contains(contentType, StringComparison.OrdinalIgnoreCase);
    }
}