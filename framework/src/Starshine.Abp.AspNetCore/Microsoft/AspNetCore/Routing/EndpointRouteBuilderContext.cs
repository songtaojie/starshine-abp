// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

namespace Microsoft.AspNetCore.Routing;

public class EndpointRouteBuilderContext
{
    public IEndpointRouteBuilder Endpoints { get; }

    public IServiceProvider ScopeServiceProvider { get; }

    public EndpointRouteBuilderContext(
        IEndpointRouteBuilder endpoints,
        IServiceProvider scopeServiceProvider)
    {
        Endpoints = endpoints;
        ScopeServiceProvider = scopeServiceProvider;
    }
}