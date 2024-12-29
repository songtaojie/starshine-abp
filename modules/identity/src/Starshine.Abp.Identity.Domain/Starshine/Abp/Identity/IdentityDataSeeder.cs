using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份数据播种者
/// </summary>
public class IdentityDataSeeder : ITransientDependency, IIdentityDataSeeder
{
    /// <summary>
    /// Guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }
    /// <summary>
    /// 角色存储库
    /// </summary>
    protected IIdentityRoleRepository RoleRepository { get; }
    /// <summary>
    /// 用户存储库
    /// </summary>
    protected IIdentityUserRepository UserRepository { get; }
    /// <summary>
    /// 查找规范器
    /// </summary>
    protected ILookupNormalizer LookupNormalizer { get; }
    /// <summary>
    /// 用户管理器
    /// </summary>
    protected IdentityUserManager UserManager { get; }
    /// <summary>
    /// 角色管理器
    /// </summary>
    protected IdentityRoleManager RoleManager { get; }
    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }
    /// <summary>
    /// 配置
    /// </summary>
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="roleRepository"></param>
    /// <param name="userRepository"></param>
    /// <param name="lookupNormalizer"></param>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="currentTenant"></param>
    /// <param name="identityOptions"></param>
    public IdentityDataSeeder(
        IGuidGenerator guidGenerator,
        IIdentityRoleRepository roleRepository,
        IIdentityUserRepository userRepository,
        ILookupNormalizer lookupNormalizer,
        IdentityUserManager userManager,
        IdentityRoleManager roleManager,
        ICurrentTenant currentTenant,
        IOptions<IdentityOptions> identityOptions)
    {
        GuidGenerator = guidGenerator;
        RoleRepository = roleRepository;
        UserRepository = userRepository;
        LookupNormalizer = lookupNormalizer;
        UserManager = userManager;
        RoleManager = roleManager;
        CurrentTenant = currentTenant;
        IdentityOptions = identityOptions;
    }

    /// <summary>
    /// 设置种子数据
    /// </summary>
    /// <param name="adminEmail"></param>
    /// <param name="adminPassword"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task<IdentityDataSeedResult> SeedAsync(
        string adminEmail,
        string adminPassword,
        Guid? tenantId = null)
    {
        Check.NotNullOrWhiteSpace(adminEmail, nameof(adminEmail));
        Check.NotNullOrWhiteSpace(adminPassword, nameof(adminPassword));

        using (CurrentTenant.Change(tenantId))
        {
            await IdentityOptions.SetAsync();

            var result = new IdentityDataSeedResult();
            //"admin" user
            const string adminUserName = "admin";
            var adminUser = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName(adminUserName));

            if (adminUser != null)
            {
                return result;
            }

            adminUser = new IdentityUser( GuidGenerator.Create(), adminUserName,adminEmail, tenantId)
            {
                Name = adminUserName
            };

            (await UserManager.CreateAsync(adminUser, adminPassword, validatePassword: false)).CheckErrors();
            result.CreatedAdminUser = true;

            //"admin" role
            const string adminRoleName = "admin";
            var adminRole = await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName(adminRoleName));
            if (adminRole == null)
            {
                adminRole = new IdentityRole(GuidGenerator.Create(), adminRoleName,tenantId)
                {
                    IsStatic = true,
                    IsPublic = true
                };

                (await RoleManager.CreateAsync(adminRole)).CheckErrors();
                result.CreatedAdminRole = true;
            }

            (await UserManager.AddToRoleAsync(adminUser, adminRoleName)).CheckErrors();

            return result;
        }
    }
}
