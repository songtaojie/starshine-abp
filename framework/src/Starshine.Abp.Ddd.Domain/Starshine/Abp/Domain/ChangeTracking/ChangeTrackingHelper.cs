using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Starshine.Abp.Domain.Repositories;
using Volo.Abp;

namespace Starshine.Abp.Domain.ChangeTracking;

public static class ChangeTrackingHelper
{
    public static bool IsEntityChangeTrackingType(TypeInfo implementationType)
    {
        return HasEntityChangeTrackingAttribute(implementationType) || AnyMethodHasEntityChangeTrackingAttribute(implementationType);
    }

    public static bool IsEntityChangeTrackingMethod([NotNull] MethodInfo methodInfo, out EntityChangeTrackingAttribute? entityChangeTrackingAttribute)
    {
        Check.NotNull(methodInfo, nameof(methodInfo));

        //Method declaration
        var attrs = methodInfo.GetCustomAttributes(true).OfType<EntityChangeTrackingAttribute>().ToArray();
        if (attrs.Length != 0)
        {
            entityChangeTrackingAttribute = attrs.First();
            return true;
        }

        if (methodInfo.DeclaringType != null)
        {
            //Class declaration
            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<EntityChangeTrackingAttribute>().ToArray();
            if (attrs.Length != 0)
            {
                entityChangeTrackingAttribute = attrs.First();
                return true;
            }
        }

        entityChangeTrackingAttribute = null;
        return false;
    }

    private static bool AnyMethodHasEntityChangeTrackingAttribute(TypeInfo implementationType)
    {
        return implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasEntityChangeTrackingAttribute);
    }

    private static bool HasEntityChangeTrackingAttribute(MemberInfo memberInfo)
    {
        return memberInfo.IsDefined(typeof(EntityChangeTrackingAttribute), true);
    }
}
