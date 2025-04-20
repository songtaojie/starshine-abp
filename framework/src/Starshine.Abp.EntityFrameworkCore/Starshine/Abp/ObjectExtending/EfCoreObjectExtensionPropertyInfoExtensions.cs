using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Starshine.Abp.ObjectExtending;

public static class EfCoreObjectExtensionPropertyInfoExtensions
{
    public const string EfCorePropertyConfigurationName = "EfCoreMapping";

    [NotNull]
    public static ObjectExtensionPropertyInfo MapEfCore(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        propertyExtension.Configuration[EfCorePropertyConfigurationName] =
            new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                propertyExtension
            );

        return propertyExtension;
    }

   
    [NotNull]
    public static ObjectExtensionPropertyInfo MapEfCore(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
        Action<EntityTypeBuilder, PropertyBuilder>? entityTypeAndPropertyBuildAction)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        propertyExtension.Configuration[EfCorePropertyConfigurationName] =
            new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                propertyExtension,
                entityTypeAndPropertyBuildAction
            );

        return propertyExtension;
    }

    public static ObjectExtensionPropertyInfoEfCoreMappingOptions? GetEfCoreMappingOrNull(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        return propertyExtension
                .Configuration
                .GetOrDefault(EfCorePropertyConfigurationName)
            as ObjectExtensionPropertyInfoEfCoreMappingOptions;
    }

    public static bool IsMappedToFieldForEfCore(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        return propertyExtension
            .Configuration
            .ContainsKey(EfCorePropertyConfigurationName);
    }
}
