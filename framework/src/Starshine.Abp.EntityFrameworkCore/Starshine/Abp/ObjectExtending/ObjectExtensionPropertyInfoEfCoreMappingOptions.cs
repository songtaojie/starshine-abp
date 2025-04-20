using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Starshine.Abp.ObjectExtending;

public class ObjectExtensionPropertyInfoEfCoreMappingOptions
{
    [NotNull]
    public ObjectExtensionPropertyInfo ExtensionProperty { get; }

    [NotNull]
    public ObjectExtensionInfo ObjectExtension => ExtensionProperty.ObjectExtension;

    public Action<EntityTypeBuilder, PropertyBuilder>? EntityTypeAndPropertyBuildAction { get; set; }

    public ObjectExtensionPropertyInfoEfCoreMappingOptions(
        [NotNull] ObjectExtensionPropertyInfo extensionProperty)
    {
        ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));
    }

    public ObjectExtensionPropertyInfoEfCoreMappingOptions(
        [NotNull] ObjectExtensionPropertyInfo extensionProperty,
        Action<EntityTypeBuilder, PropertyBuilder>? entityTypeAndPropertyBuildAction)
    {
        ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));

        EntityTypeAndPropertyBuildAction = entityTypeAndPropertyBuildAction;
    }
}
