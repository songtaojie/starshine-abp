// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Abp.Swashbuckle.Attributes;
using Starshine.Abp.Swashbuckle.Filters;
using Starshine.Abp.Swashbuckle.Internal;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using DocExpansion = Swashbuckle.AspNetCore.SwaggerUI.DocExpansion;
using Swashbuckle.AspNetCore.Filters;
using Volo.Abp.Reflection;

namespace Starshine.Abp.Swashbuckle;
internal class SwaggerDocumentBuilder: ISwaggerDocumentBuilder
{
    /// <summary>
    /// 所有分组默认的组名 Key
    /// </summary>
    private const string AllGroupsKey = "All Groups";

    /// <summary>
    /// swagger配置
    /// </summary>
    private readonly SwaggerSettingsOptions _swaggerSettings;

    /// <summary>
    /// 文档分组列表
    /// </summary>
    private static IEnumerable<string>? _documentGroups;
    /// <summary>
    /// 带排序的分组名
    /// </summary>
    private static Regex? _groupOrderRegex;

    /// <summary>
    /// 分组信息
    /// </summary>
    private static IEnumerable<GroupExtraInfo>? _groupExtraInfos;

    /// <summary>
    /// <see cref="GetActionTag(ApiDescription)"/> 缓存集合
    /// </summary>
    private static ConcurrentDictionary<ControllerActionDescriptor, string>? _getControllerTagCached;

    /// <summary>
    /// <see cref="GetActionTag(ApiDescription)"/> 缓存集合
    /// </summary>
    private static ConcurrentDictionary<ApiDescription, string>? _getActionTagCached;

    /// <summary>
    /// 获取控制器组缓存集合
    /// </summary>
    private static ConcurrentDictionary<Type, IEnumerable<GroupExtraInfo>>? _getControllerGroupsCached;

    /// <summary>
    /// 获取分组信息缓存集合
    /// </summary>
    private static ConcurrentDictionary<string, SwaggerOpenApiInfo>? _getGroupOpenApiInfoCached;

    /// <summary>
    /// <see cref="GetActionGroups(MethodInfo,SwaggerSettingsOptions)"/> 缓存集合
    /// </summary>
    private static ConcurrentDictionary<MethodInfo, IEnumerable<GroupExtraInfo>>? _getActionGroupsCached;

    private readonly ITypeFinder _typeFinder;
    private readonly IAssemblyFinder _assemblyFinder;
    public SwaggerDocumentBuilder(IAssemblyFinder assemblyFinder,ITypeFinder typeFinder,IOptions<SwaggerSettingsOptions> options)
    {
        _assemblyFinder = assemblyFinder;
        _typeFinder = typeFinder;
        _swaggerSettings = options.Value;

        // 初始化常量
        _groupOrderRegex ??= new Regex(@"@(?<order>[0-9]+$)");
        _getControllerTagCached ??= new ConcurrentDictionary<ControllerActionDescriptor, string>();
        _getActionTagCached ??= new ConcurrentDictionary<ApiDescription, string>();
        _getControllerGroupsCached ??= new ConcurrentDictionary<Type, IEnumerable<GroupExtraInfo>>();
        _getGroupOpenApiInfoCached ??= new ConcurrentDictionary<string, SwaggerOpenApiInfo>();
        _getActionGroupsCached ??= new ConcurrentDictionary<MethodInfo, IEnumerable<GroupExtraInfo>>();
        // 默认分组，支持多个逗号分割
        _groupExtraInfos ??= new List<GroupExtraInfo> { ResolveGroupExtraInfo(_swaggerSettings.DefaultGroupName!, _swaggerSettings) };
        _documentGroups ??= ReadGroups(_swaggerSettings);
    }

    /// <summary>
    /// Swagger 生成器构建
    /// </summary>
    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    public void BuildSwaggerGen(SwaggerGenOptions swaggerGenOptions)
    {
        //// 创建分组文档
        CreateSwaggerDocs(swaggerGenOptions, _swaggerSettings);

        //// 加载分组控制器和动作方法列表
        LoadGroupControllerWithActions(swaggerGenOptions, _swaggerSettings);

        // 配置 Swagger OperationIds
        ConfigureOperationIds(swaggerGenOptions);

        // 配置 Swagger SchemaId
        ConfigureSchemaId(swaggerGenOptions);

        // 配置标签
        ConfigureTagsAction(swaggerGenOptions);

        // 配置 Action 排序
        ConfigureActionSequence(swaggerGenOptions);

        // 加载注释描述文件
        LoadXmlComments(swaggerGenOptions, _swaggerSettings);

        // 配置授权
        ConfigureSecurities(swaggerGenOptions, _swaggerSettings);

        //使得Swagger能够正确地显示Enum的对应关系
        if (_swaggerSettings.EnableEnumSchemaFilter == true) swaggerGenOptions.SchemaFilter<EnumSchemaFilter>(_assemblyFinder.Assemblies);

        //// 支持控制器排序操作
        if (_swaggerSettings.EnableTagsOrderDocumentFilter == true) swaggerGenOptions.DocumentFilter<TagsOrderDocumentFilter>();

        // 添加 Action 操作过滤器
        swaggerGenOptions.OperationFilter<ApiActionFilter>();

        // 自定义配置
        _swaggerSettings.SwaggerGenOptionsAction?.Invoke(swaggerGenOptions);
    }

    /// <summary>
    /// 构建Swagger全局配置
    /// </summary>
    /// <param name="swaggerOptions">Swagger 全局配置</param>
    /// <param name="configure"></param>
    public void BuildSwagger(SwaggerOptions swaggerOptions, Action<SwaggerOptions>? configure = default)
    {
        // 生成V2版本
        swaggerOptions.SerializeAsV2 = _swaggerSettings.FormatAsV2 == true;

        // 判断是否启用 Server
        if (_swaggerSettings.HideServers != true)
        {
            // 启动服务器 Servers
            swaggerOptions.PreSerializeFilters.Add((swagger, request) =>
            {
                // 默认 Server
                var servers = new List<OpenApiServer> {
                        new OpenApiServer { Url = $"{request.Scheme}://{request.Host.Value}{_swaggerSettings.VirtualPath}",Description="Default" }
                    };
                if(_swaggerSettings.Servers != null)
                    servers.AddRange(_swaggerSettings.Servers);

                swagger.Servers = servers;
            });
        }

        // 配置路由模板
        swaggerOptions.RouteTemplate = _swaggerSettings.RouteTemplate;

        // 自定义配置
        configure?.Invoke(swaggerOptions);
    }

    /// <summary>
    /// Swagger UI 构建
    /// </summary>
    /// <param name="swaggerUIOptions"></param>
    /// <param name="configure"></param>
    public void BuildSwaggerUI(SwaggerUIOptions swaggerUIOptions, Action<SwaggerUIOptions>? configure = default)
    {
        // 配置分组终点路由
        CreateGroupEndpoint(swaggerUIOptions, _swaggerSettings);

        // 配置文档标题
        swaggerUIOptions.DocumentTitle = _swaggerSettings.DocumentTitle;

        // 配置UI地址
        swaggerUIOptions.RoutePrefix = _swaggerSettings.RoutePrefix ?? "swagger";

        // 文档展开设置
        swaggerUIOptions.DocExpansion(_swaggerSettings.DocExpansion ?? DocExpansion.None);

        // 自定义首页
        CustomizeIndex(swaggerUIOptions, _swaggerSettings);

        AddDefaultInterceptor(swaggerUIOptions);

        // 自定义配置
        configure?.Invoke(swaggerUIOptions);
    }

    /// <summary>
    /// Swagger UI 构建
    /// </summary>
    /// <param name="knife4UIOptions"></param>
    /// <param name="configure"></param>
    public void BuildSwaggerKnife4jUI(Knife4UIOptions knife4UIOptions, Action<Knife4UIOptions>? configure = default)
    {
        // 配置分组终点路由
        CreateGroupEndpoint(knife4UIOptions, _swaggerSettings);

        // 配置文档标题
        knife4UIOptions.DocumentTitle = _swaggerSettings.DocumentTitle;

        //// 配置UI地址
        knife4UIOptions.RoutePrefix = _swaggerSettings.RoutePrefix ?? "swagger";

        // 文档展开设置
        knife4UIOptions.DocExpansion(GetKnife4jUIDocExpansion(_swaggerSettings.DocExpansion));

        // 自定义配置
        configure?.Invoke(knife4UIOptions);
    }
    
    private static IGeekFan.AspNetCore.Knife4jUI.DocExpansion GetKnife4jUIDocExpansion(DocExpansion? docExpansion)
    {
        return docExpansion switch
        {
            DocExpansion.List => IGeekFan.AspNetCore.Knife4jUI.DocExpansion.List,
            DocExpansion.None => IGeekFan.AspNetCore.Knife4jUI.DocExpansion.None,
            DocExpansion.Full => IGeekFan.AspNetCore.Knife4jUI.DocExpansion.Full,
            _ => IGeekFan.AspNetCore.Knife4jUI.DocExpansion.List
        };
    }

    #region 静态私有函数

    /// <summary>
    /// 创建分组文档
    /// </summary>
    /// <param name="swaggerGenOptions">Swagger生成器对象</param>
    /// <param name="swaggerSettings"></param>
    private static void CreateSwaggerDocs(SwaggerGenOptions swaggerGenOptions, SwaggerSettingsOptions swaggerSettings)
    {
        if (_documentGroups == null) return;
        foreach (var group in _documentGroups)
        {
            var groupOpenApiInfo = GetGroupOpenApiInfo(group, swaggerSettings) as OpenApiInfo;
            swaggerGenOptions.SwaggerDoc(group, groupOpenApiInfo);
        }
    }

    /// <summary>
    /// 加载分组控制器和动作方法列表
    /// </summary>
    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    /// <param name="swaggerSettings"></param>
    private static void LoadGroupControllerWithActions(SwaggerGenOptions swaggerGenOptions, SwaggerSettingsOptions swaggerSettings)
    {
        swaggerGenOptions.DocInclusionPredicate((currentGroup, apiDescription) => CheckApiDescriptionInCurrentGroup(currentGroup, apiDescription, swaggerSettings));
    }

    /// <summary>
    /// 检查方法是否在分组中
    /// </summary>
    /// <param name="currentGroup"></param>
    /// <param name="apiDescription"></param>
    /// <param name="swaggerSettings"></param>
    /// <returns></returns>
    private static bool CheckApiDescriptionInCurrentGroup(string currentGroup, ApiDescription apiDescription, SwaggerSettingsOptions swaggerSettings)
    {
        if (!apiDescription.TryGetMethodInfo(out var method)) return false;

        // 处理 Mvc 和 WebAPI 混合项目路由问题
        if (typeof(Controller).IsAssignableFrom(method.DeclaringType) && apiDescription.ActionDescriptor.ActionConstraints == null)
        {
            return false;
        }

        // 处理贴有 [ApiExplorerSettings(IgnoreApi = true)] 或者 [ApiDescriptionSettings(false)] 特性的接口
        var apiExplorerSettings = method.GetCustomAttribute<ApiExplorerSettingsAttribute>(true);
        if (apiExplorerSettings?.IgnoreApi == true) return false;

        if (currentGroup == AllGroupsKey)
        {
            return true;
        }

        return GetActionGroups(method, swaggerSettings).Any(u => u.Group == currentGroup);
    }

    /// <summary>
    ///  配置标签
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    private static void ConfigureTagsAction(SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.TagActionsBy(apiDescription =>
        {
            return new[] { GetActionTag(apiDescription) };
        });
    }

    /// <summary>
    ///  配置 Action 排序
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    private static void ConfigureActionSequence(SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.OrderActionsBy(apiDesc =>
        {
            var apiDescriptionSettings = apiDesc.CustomAttributes()
                                   .FirstOrDefault(u => u.GetType() == typeof(ApiDescriptionSettingsAttribute))
                                   as ApiDescriptionSettingsAttribute ?? new ApiDescriptionSettingsAttribute();

            return (int.MaxValue - apiDescriptionSettings.Order).ToString()
                            .PadLeft(int.MaxValue.ToString().Length, '0');
        });
    }

    /// <summary>
    /// 配置 Swagger OperationIds
    /// </summary>
    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    private static void ConfigureOperationIds(SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.CustomOperationIds(apiDescription =>
        {
            var isMethod = apiDescription.TryGetMethodInfo(out var method);

            // 判断是否自定义了 [OperationId] 特性
            if (isMethod && method.IsDefined(typeof(OperationIdAttribute), false))
            {
                return method.GetCustomAttribute<OperationIdAttribute>(false)!.OperationId;
            }

            var operationId = apiDescription.RelativePath!.Replace("/", "-")
                                       .Replace("{", "-")
                                       .Replace("}", "-") + "-" + apiDescription.HttpMethod!.ToLower().ToUpperCamelCase();

            return operationId.Replace("--", "-");
        });

    }

    /// <summary>
    /// 配置 Swagger SchemaId
    /// </summary>
    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    private static void ConfigureSchemaId(SwaggerGenOptions swaggerGenOptions)
    {
        // 本地函数
        static string DefaultSchemaIdSelector(Type modelType)
        {
            var modelName = modelType.FullName!;

            // 处理泛型类型问题
            if (modelType.IsConstructedGenericType)
            {
                var prefix = modelType.GetGenericArguments()
                    .Select(genericArg => DefaultSchemaIdSelector(genericArg))
                    .Aggregate((previous, current) => previous + current);

                // 通过 _ 拼接多个泛型
                modelName = modelName.Split('`').First() + "_" + prefix;
            }

            // 判断是否自定义了 [SchemaId] 特性，解决模块化多个程序集命名冲突
            var isCustomize = modelType.IsDefined(typeof(SchemaIdAttribute));
            if (isCustomize)
            {
                var schemaIdAttribute = modelType.GetCustomAttribute<SchemaIdAttribute>()!;
                if (!schemaIdAttribute.Replace) return schemaIdAttribute.SchemaId + modelName;
                else return schemaIdAttribute.SchemaId;
            }

            return modelName;
        }

        // 调用本地函数
        swaggerGenOptions.CustomSchemaIds(DefaultSchemaIdSelector);
    }

    /// <summary>
    /// 加载注释描述文件
    /// </summary>
    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    /// <param name="swaggerSettings"></param>
    private void LoadXmlComments(SwaggerGenOptions swaggerGenOptions, SwaggerSettingsOptions swaggerSettings)
    {
        var xmlComments = swaggerSettings.XmlComments;
        if (xmlComments == null || !xmlComments.Any())
        {
            var excludeAssemblyNames = new List<string>() { nameof(System),nameof(Microsoft),nameof(Swashbuckle),nameof(Volo) };
            xmlComments = _assemblyFinder.Assemblies.Where(u => !excludeAssemblyNames.Any(r=> u.FullName?.Contains(r) ?? false)).Select(t => t.GetName().Name!).ToArray();
            if(!xmlComments.Any()) return;
        }
        var members = new Dictionary<string, XElement>();

        // 显式继承的注释
        var regex = new Regex(@"[A-Z]:[a-zA-Z_@\.]+");
        // 隐式继承的注释
        var regex2 = new Regex(@"[A-Z]:[a-zA-Z_@\.]+\.");

        // 支持注释完整特性，包括 inheritdoc 注释语法
        foreach (var xmlComment in xmlComments)
        {
            var assemblyXmlName = xmlComment.EndsWith(".xml") ? xmlComment : $"{xmlComment}.xml";
            var assemblyXmlPath = Path.Combine(AppContext.BaseDirectory, assemblyXmlName);

            if (File.Exists(assemblyXmlPath))
            {
                var xmlDoc = XDocument.Load(assemblyXmlPath);

                // 查找所有 member[name] 节点，且不包含 <inheritdoc /> 节点的注释
                var memberNotInheritdocElementList = xmlDoc.XPathSelectElements("/doc/members/member[@name and not(inheritdoc)]");

                foreach (var memberElement in memberNotInheritdocElementList)
                {
                    if (memberElement == null) continue;
                    var memberName = memberElement.Attribute("name")?.Value;
                    if (string.IsNullOrEmpty(memberName)) continue;
                    members.Add(memberName, memberElement);
                }

                // 查找所有 member[name] 含有 <inheritdoc /> 节点的注释
                var memberElementList = xmlDoc.XPathSelectElements("/doc/members/member[inheritdoc]");
                foreach (var memberElement in memberElementList)
                {
                    var inheritdocElement = memberElement.Element("inheritdoc");
                    if (inheritdocElement == null) continue;
                    var cref = inheritdocElement.Attribute("cref");
                    var value = cref?.Value;

                    // 处理不带 cref 的 inheritdoc 注释
                    if (value == null)
                    {
                        var memberName = inheritdocElement.Parent?.Attribute("name")?.Value;
                        if(string.IsNullOrWhiteSpace(memberName)) continue;
                        // 处理隐式实现接口的注释
                        // 注释格式：M:Furion.Application.TestInheritdoc.Furion#Application#ITestInheritdoc#Abc(System.String)
                        // 匹配格式：[A-Z]:[a-zA-Z_@\.]+\.
                        // 处理逻辑：直接替换匹配为空，然后讲 # 替换为 . 查找即可
                        if (memberName.Contains('#'))
                        {
                            value = $"{memberName[..2]}{regex2.Replace(memberName, "").Replace('#', '.')}";
                        }
                        // 处理带参数的注释
                        // 注释格式：M:Furion.Application.TestInheritdoc.WithParams(System.String)
                        // 匹配格式：[A-Z]:[a-zA-Z_@\.]+
                        // 处理逻辑：匹配出不带参数的部分，然后获取类型命名空间，最后调用 GenerateInheritdocCref 进行生成
                        else if (memberName.Contains('('))
                        {
                            var noParamsClassName = regex.Match(memberName).Value;
                            var className = noParamsClassName[noParamsClassName.IndexOf(":")..noParamsClassName.LastIndexOf(".")];
                            value = GenerateInheritdocCref(xmlDoc, memberName, className);
                        }
                        // 处理不带参数的注释
                        // 注释格式：M:Furion.Application.TestInheritdoc.WithParams
                        // 匹配格式：无
                        // 处理逻辑：获取类型命名空间，最后调用 GenerateInheritdocCref 进行生成
                        else
                        {
                            var className = memberName[memberName.IndexOf(":")..memberName.LastIndexOf(".")];
                            value = GenerateInheritdocCref(xmlDoc, memberName, className);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(value)) continue;

                    // 处理带 cref 的 inheritdoc 注释
                    if (members.TryGetValue(value, out var realDocMember))
                    {
                        memberElement.SetAttributeValue("_ref_", value);
                        inheritdocElement.Parent?.ReplaceNodes(realDocMember.Nodes());
                    }
                }

                swaggerGenOptions.IncludeXmlComments(() => new XPathDocument(xmlDoc.CreateReader()), true);
            }
        }
    }

    /// <summary>
    /// 生成 Inheritdoc cref 属性
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="memberName"></param>
    /// <param name="className"></param>
    /// <returns></returns>
    private static string? GenerateInheritdocCref(XDocument xmlDoc, string memberName, string className)
    {
        var classElement = xmlDoc.XPathSelectElements($"/doc/members/member[@name='{"T" + className}' and @_ref_]").FirstOrDefault();
        if (classElement == null) return default;

        var _ref_value = classElement.Attribute("_ref_")?.Value;
        if (_ref_value == null) return default;

        var classCrefValue = _ref_value[_ref_value.IndexOf(":")..];
        return memberName.Replace(className, classCrefValue);
    }

    /// <summary>
    /// 配置授权
    /// </summary>
    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    /// <param name="swaggerSettings"></param>
    private static void ConfigureSecurities(SwaggerGenOptions swaggerGenOptions, SwaggerSettingsOptions swaggerSettings)
    {
        // 判断是否启用了授权
        if (swaggerSettings.EnableAuthorized != true || swaggerSettings.SecurityDefinitions == null || swaggerSettings.SecurityDefinitions.Length == 0) return;

        //开启加权小锁
        swaggerGenOptions.OperationFilter<AddResponseHeadersFilter>();
        swaggerGenOptions.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

        // 在header中添加token，传递到后台
        swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
        // 在header中添加token，传递到后台
        swaggerGenOptions.OperationFilter<OpenApiSecurityFilter>();
        ////生成安全定义
        foreach (var securityDefinition in swaggerSettings.SecurityDefinitions)
        {
            // Id 必须定义
            if (string.IsNullOrWhiteSpace(securityDefinition.Id)) continue;
            swaggerGenOptions.AddSecurityDefinition(securityDefinition.Id, securityDefinition);
        }
    }

    /// <summary>
    /// 配置分组终点路由
    /// </summary>
    /// <param name="swaggerUIOptions"></param>
    /// <param name="swaggerSettings"></param>
    private static void CreateGroupEndpoint(SwaggerUIOptions swaggerUIOptions, SwaggerSettingsOptions swaggerSettings)
    {
        if (_documentGroups == null) return;
        foreach (var group in _documentGroups)
        {
            var groupOpenApiInfo = GetGroupOpenApiInfo(group, swaggerSettings);

            // 替换路由模板
            //var routeTemplate = _swaggerSettings.RouteTemplate.Replace("{documentName}", Uri.EscapeDataString(group));
            swaggerUIOptions.SwaggerEndpoint(groupOpenApiInfo.RouteTemplate, groupOpenApiInfo?.Title ?? group);
        }
    }

    /// <summary>
    /// 配置分组终点路由
    /// </summary>
    /// <param name="knife4UIOptions"></param>
    /// <param name="swaggerSettings"></param>
    private static void CreateGroupEndpoint(Knife4UIOptions knife4UIOptions, SwaggerSettingsOptions swaggerSettings)
    {
        if (_documentGroups == null) return;
        foreach (var group in _documentGroups)
        {
            var groupOpenApiInfo = GetGroupOpenApiInfo(group, swaggerSettings);

            // 替换路由模板
            var routeTemplate = swaggerSettings.RouteTemplate!.Replace("{documentName}", Uri.EscapeDataString(group));
            knife4UIOptions.SwaggerEndpoint($"{swaggerSettings.VirtualPath}/{routeTemplate}", groupOpenApiInfo?.Title ?? group);
        }
    }

    /// <summary>
    /// 自定义 Swagger 首页
    /// </summary>
    /// <param name="swaggerUIOptions"></param>
    /// <param name="swaggerSettings"></param>
    private static void CustomizeIndex(SwaggerUIOptions swaggerUIOptions, SwaggerSettingsOptions swaggerSettings)
    {
        var thisType = typeof(SwaggerDocumentBuilder);
        var thisAssembly = thisType.Assembly;

        // 判断是否启用 MiniProfile
        var customIndex = $"wwwroot.swagger.ui.{(swaggerSettings.EnabledMiniProfiler != true ? "index" : "index-mini-profiler")}.html";
        swaggerUIOptions.IndexStream = () =>
        {
            StringBuilder htmlBuilder;
            // 自定义首页模板参数
            var indexArguments = new Dictionary<string, string>
                {
                    {"%(VirtualPath)", swaggerSettings.VirtualPath! }    // 解决二级虚拟目录 MiniProfiler 丢失问题
                };

            // 读取文件内容
            using (var stream = thisAssembly.GetManifestResourceStream(customIndex))
            {
                if(stream == null)
                    throw new Exception($"{customIndex} 文件不存在");
                using var reader = new StreamReader(stream!);
                htmlBuilder = new StringBuilder(reader.ReadToEnd());
            }

            // 替换模板参数
            foreach (var (template, value) in indexArguments)
            {
                htmlBuilder.Replace(template, value);
            }

            // 返回新的内存流
            var byteArray = Encoding.UTF8.GetBytes(htmlBuilder.ToString());
            return new MemoryStream(byteArray);
        };
    }

    /// <summary>
    /// 添加默认请求/响应拦截器
    /// </summary>
    /// <param name="swaggerUIOptions"></param>
    private static void AddDefaultInterceptor(SwaggerUIOptions swaggerUIOptions)
    {
        // 配置多语言和自动登录token
        swaggerUIOptions.UseRequestInterceptor("function(request) { return defaultRequestInterceptor(request); }");
        swaggerUIOptions.UseResponseInterceptor("function(response) { return defaultResponseInterceptor(response); }");
    }



    /// <summary>
    /// 读取所有分组信息
    /// </summary>
    /// <returns></returns>
    private IEnumerable<string> ReadGroups(SwaggerSettingsOptions swaggerSettings)
    {
        // 获取所有的控制器和动作方法
        var controllers = _typeFinder.Types.Where(Penetrates.IsApiController);
        if (!controllers.Any())
        {
            var defaultGroups = new List<string>
                {
                    swaggerSettings.DefaultGroupName!
                };
            // 启用总分组功能
            if (swaggerSettings.EnableAllGroups == true)
            {
                defaultGroups.Add(AllGroupsKey);
            }
            return defaultGroups;
        }

        var actions = controllers.SelectMany(c => c.GetMethods().Where(u => IsApiAction(u, c)));

        // 合并所有分组
        var groupOrders = controllers.SelectMany(u => GetControllerGroups(u, swaggerSettings))
            .Union(
                actions.SelectMany(u => GetActionGroups(u, swaggerSettings))
            )
            .Where(u => u != null && u.Visible)
            // 分组后取最大排序
            .GroupBy(u => u.Group)
            .Select(u => new GroupExtraInfo
            {
                Group = u.Key,
                Order = u.Max(x => x.Order),
                Visible = true
            });

        // 分组排序
        var groups = groupOrders
            .OrderByDescending(u => u.Order)
            .ThenBy(u => u.Group)
            .Select(u => u.Group!);
        // 启用总分组功能
        if (swaggerSettings.EnableAllGroups == true)
        {
            groups = groups.Concat(new[] { AllGroupsKey });
        }

        return groups;
    }

    /// <summary>
    /// 获取控制器标签
    /// </summary>
    /// <param name="controllerActionDescriptor">控制器接口描述器</param>
    /// <returns></returns>
    private static string GetControllerTag(ControllerActionDescriptor controllerActionDescriptor)
    {
        return _getControllerTagCached!.GetOrAdd(controllerActionDescriptor, Function);

        // 本地函数
        static string Function(ControllerActionDescriptor controllerActionDescriptor)
        {
            var type = controllerActionDescriptor.ControllerTypeInfo;
            // 如果动作方法没有定义 [ApiDescriptionSettings] 特性，则返回所在控制器名
            if (!type.IsDefined(typeof(ApiDescriptionSettingsAttribute), true)) return controllerActionDescriptor.ControllerName;

            // 读取标签
            var apiDescriptionSettings = type.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true)!;
            return string.IsNullOrWhiteSpace(apiDescriptionSettings.Tag) ? controllerActionDescriptor.ControllerName : apiDescriptionSettings.Tag;
        }
    }

    /// <summary>
    /// 是否是动作方法
    /// </summary>
    /// <param name="method">方法</param>
    /// <param name="ReflectedType">声明类型</param>
    /// <returns></returns>
    private static bool IsApiAction(MethodInfo method, Type ReflectedType)
    {
        // 不是非公开、抽象、静态、泛型方法
        if (!method.IsPublic || method.IsAbstract || method.IsStatic || method.IsGenericMethod) return false;

        // 如果所在类型不是控制器，则该行为也被忽略
        if (method.ReflectedType != ReflectedType || method.DeclaringType == typeof(object)) return false;

        // 不是能被导出忽略的接方法
        if (method.IsDefined(typeof(ApiExplorerSettingsAttribute), true) && method.GetCustomAttribute<ApiExplorerSettingsAttribute>(true)!.IgnoreApi) return false;

        return true;
    }

    /// <summary>
    /// 获取动作方法标签
    /// </summary>
    /// <param name="apiDescription">接口描述器</param>
    /// <returns></returns>
    private static string GetActionTag(ApiDescription apiDescription)
    {
        return _getActionTagCached!.GetOrAdd(apiDescription, Function);

        // 本地函数
        static string Function(ApiDescription apiDescription)
        {
            if (!apiDescription.TryGetMethodInfo(out var method)) return Assembly.GetEntryAssembly()!.GetName().Name!;

            // 获取控制器描述器
            var controllerActionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            if(controllerActionDescriptor == null) return Assembly.GetEntryAssembly()!.GetName().Name!;
            // 如果动作方法没有定义 [ApiDescriptionSettings] 特性，则返回所在控制器名
            if (!method.IsDefined(typeof(ApiDescriptionSettingsAttribute), true)) return GetControllerTag(controllerActionDescriptor);

            // 读取标签
            var apiDescriptionSettings = method.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true)!;
            return string.IsNullOrWhiteSpace(apiDescriptionSettings.Tag) ? GetControllerTag(controllerActionDescriptor) : apiDescriptionSettings.Tag;

        }
    }

    /// <summary>
    /// 获取控制器分组列表
    /// </summary>
    /// <param name="type"></param>
    /// <param name="swaggerSettings"></param>
    /// <returns></returns>
    internal static IEnumerable<GroupExtraInfo> GetControllerGroups(Type type, SwaggerSettingsOptions swaggerSettings)
    {
        return _getControllerGroupsCached!.GetOrAdd(type, Function(type, swaggerSettings));

        // 本地函数
        static IEnumerable<GroupExtraInfo> Function(Type type, SwaggerSettingsOptions swaggerSettings)
        {
            // 如果控制器没有定义 [ApiDescriptionSettings] 特性，则返回默认分组
            if (!type.IsDefined(typeof(ApiDescriptionSettingsAttribute), true)) return _groupExtraInfos!;

            // 读取分组
            var apiDescriptionSettings = type.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true)!;
            if (apiDescriptionSettings.Groups == null || apiDescriptionSettings.Groups.Length == 0) return _groupExtraInfos!;

            // 处理分组额外信息
            var groupExtras = new List<GroupExtraInfo>();
            foreach (var group in apiDescriptionSettings.Groups)
            {
                groupExtras.Add(ResolveGroupExtraInfo(group, swaggerSettings));
            }

            return groupExtras;
        }
    }

    /// <summary>
    /// 获取动作方法分组列表
    /// </summary>
    /// <param name="method">方法</param>
    /// <param name="swaggerSettings"></param>
    /// <returns></returns>
    private static IEnumerable<GroupExtraInfo> GetActionGroups(MethodInfo method, SwaggerSettingsOptions swaggerSettings)
    {
        return _getActionGroupsCached!.GetOrAdd(method, Function(method, swaggerSettings));

        // 本地函数
        static IEnumerable<GroupExtraInfo> Function(MethodInfo method, SwaggerSettingsOptions swaggerSettings)
        {
            // 如果动作方法没有定义 [ApiDescriptionSettings] 特性，则返回所在控制器分组
            if (!method.IsDefined(typeof(ApiDescriptionSettingsAttribute), true)) return GetControllerGroups(method.ReflectedType!, swaggerSettings);

            // 读取分组
            var apiDescriptionSettings = method.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true)!;
            if (apiDescriptionSettings.Groups == null || apiDescriptionSettings.Groups.Length == 0) return GetControllerGroups(method.ReflectedType!, swaggerSettings);

            // 处理排序
            var groupExtras = new List<GroupExtraInfo>();
            foreach (var group in apiDescriptionSettings.Groups)
            {
                groupExtras.Add(ResolveGroupExtraInfo(group, swaggerSettings));
            }

            return groupExtras;
        }
    }

    /// <summary>
    /// 解析分组附加信息
    /// </summary>
    /// <param name="group">分组名</param>
    /// <param name="swaggerSettings"></param>
    /// <returns></returns>
    private static GroupExtraInfo ResolveGroupExtraInfo(string group, SwaggerSettingsOptions swaggerSettings)
    {
        string realGroup;
        var order = 0;

        if (!_groupOrderRegex!.IsMatch(group)) realGroup = group;
        else
        {
            realGroup = _groupOrderRegex.Replace(group, "");
            order = int.Parse(_groupOrderRegex.Match(group).Groups["order"].Value);
        }

        var groupOpenApiInfo = GetGroupOpenApiInfo(realGroup, swaggerSettings);
        return new GroupExtraInfo
        {
            Group = realGroup,
            Order = groupOpenApiInfo.Order ?? order,
            Visible = groupOpenApiInfo.Visible ?? true
        };
    }

    /// <summary>
    /// 获取分组配置信息
    /// </summary>
    /// <param name="group"></param>
    /// <param name="swaggerSettings"></param>
    /// <returns></returns>
    private static SwaggerOpenApiInfo GetGroupOpenApiInfo(string group, SwaggerSettingsOptions swaggerSettings)
    {
        return _getGroupOpenApiInfoCached!.GetOrAdd(group, Function(group, swaggerSettings));

        // 本地函数
        static SwaggerOpenApiInfo Function(string group, SwaggerSettingsOptions swaggerSettings)
        {
            // 替换路由模板
            var routeTemplate = swaggerSettings.RouteTemplate!.Replace("{documentName}", Uri.EscapeDataString(group));
            if (!string.IsNullOrWhiteSpace(swaggerSettings.ServerDir))
            {
                routeTemplate = swaggerSettings.ServerDir + "/" + routeTemplate;
            }

            // 处理虚拟目录问题
            var template = $"{swaggerSettings.VirtualPath}/{routeTemplate}";

            var groupInfo = swaggerSettings.GroupOpenApiInfos!.FirstOrDefault(u => u.Group == group);
            if (groupInfo != null)
            {
                groupInfo.RouteTemplate = template;
                groupInfo.Title ??= group;
            }
            else
            {
                groupInfo = new SwaggerOpenApiInfo { Group = group, RouteTemplate = template };
            }
            return groupInfo;
        }
    }
    #endregion
}
