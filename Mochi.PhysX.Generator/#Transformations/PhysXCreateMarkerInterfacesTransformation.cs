using Biohazrd;
using Biohazrd.Transformation;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Mochi.PhysX.Generator;

internal sealed class PhysXCreateMarkerInterfacesTransformation : TransformationBase
{
    private readonly ConcurrentBag<TranslatedRecord> AllCandidateRecords = new();
    private readonly HashSet<TranslatedRecord> RecordsWithChildren = new(ReferenceEqualityComparer.Instance);

    protected override TranslatedLibrary PreTransformLibrary(TranslatedLibrary library)
    {
        Debug.Assert(AllCandidateRecords.Count == 0);
        AllCandidateRecords.Clear();

        Debug.Assert(RecordsWithChildren.Count == 0);
        RecordsWithChildren.Clear();

        foreach (TranslatedDeclaration declaration in library.EnumerateRecursively())
        {
            if (declaration is not TranslatedRecord record)
            { continue; }

            if (record.NonVirtualBaseField?.Type is not TranslatedTypeReference baseTypeReference)
            { continue; }

            if (baseTypeReference.TryResolve(library) is not TranslatedRecord baseRecord)
            { continue; }

            RecordsWithChildren.Add(baseRecord);
        }

        return library;
    }

    protected override TransformationResult TransformRecord(TransformationContext context, TranslatedRecord declaration)
    {
        // Skip nested records
        // (PhysX doesn't currently have any that would be candidates for marker interfaces so we don't bother to handle them properly.)
        if (context.ParentDeclaration is TranslatedRecord)
        { return declaration; }

        bool wantsInheritanceViaGenerics;

        // Determine whether a this type should participate in inheritance-via-generics
        // By default, only types which have children are candidates for inheritance-via-generics
        // Also skip any records which do not have a family (either any child types or a base type)
        if (RecordsWithChildren.Contains(declaration))
        { wantsInheritanceViaGenerics = true; }
        else if (declaration.NonVirtualBaseField?.Type is TranslatedTypeReference baseReference && baseReference.TryResolve(context.Library) is TranslatedRecord baseRecord)
        { wantsInheritanceViaGenerics = false; }
        else
        { return declaration; }

        // PxTask has no children but it's expected to be extended by PhysX consumers so we expose it as a generic in case people want to extend it that way.
        if (!wantsInheritanceViaGenerics && declaration.Name == "PxTask")
        { wantsInheritanceViaGenerics = true; }

        // Note that this type will get a marker interface
        declaration = declaration with
        {
            Metadata = declaration.Metadata.Add(new HasMarkerInterface() { WantsInheritanceViaGenerics = wantsInheritanceViaGenerics })
        };
        AllCandidateRecords.Add(declaration);
        return declaration;
    }

    protected override TranslatedLibrary PostTransformLibrary(TranslatedLibrary library)
    {
        List<PhysXMarkerInterfacesDeclaration> markerInterfaceGroups = new(1);

        ImmutableArray<TypeReference>.Builder typesBuilder = ImmutableArray.CreateBuilder<TypeReference>(AllCandidateRecords.Count);
        foreach (IGrouping<string?, TranslatedRecord> group in AllCandidateRecords.GroupBy(r => r.Namespace))
        {
            typesBuilder.Clear();

            foreach (TranslatedRecord record in group.OrderBy(r => r.Name))
            { typesBuilder.Add(TranslatedTypeReference.Create(record)); }

            PhysXMarkerInterfacesDeclaration markerInterfaceGroup = new()
            {
                TargetTypes = typesBuilder.ToImmutable(),
                Namespace = group.Key
            };
            markerInterfaceGroups.Add(markerInterfaceGroup);
        }

        AllCandidateRecords.Clear();
        RecordsWithChildren.Clear();

        return library with
        {
            Declarations = library.Declarations.AddRange(markerInterfaceGroups)
        };
    }
}
