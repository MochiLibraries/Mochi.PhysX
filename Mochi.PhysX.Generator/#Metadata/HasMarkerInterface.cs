using Biohazrd;

namespace Mochi.PhysX.Generator;

internal struct HasMarkerInterface : IDeclarationMetadataItem
{
    /// <summary>If <c>true</c>, <see cref="EnableInheritanceViaGenericsTransformation"/> will use this marker interface to enable inheritance via generics.</summary>
    public bool WantsInheritanceViaGenerics;
}
