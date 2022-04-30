using Biohazrd;
using Biohazrd.Transformation;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Mochi.PhysX.Generator;

//TODO:
// * This transformation doesn't handle lifting overloads correctly (If A derives from B and A has a method Method(int) and B has a method Method(char*), Method(int) will not be lifted.)
// * Ideally we shouldn't emit duplicate P/Invokes for non-virtual lifted methods
// * Virtual destructors cannot be lifted
internal sealed class LiftBaseMembersTransformation : TransformationBase
{
    private TranslatedRecord? TryGetSimpleBaseType(TranslatedLibrary library, TranslatedRecord declaration)
    {
        if (declaration.NonVirtualBaseField is null || declaration.NonVirtualBaseField.Offset != 0)
        { return null; }

        if (declaration.NonVirtualBaseField.Type is not TranslatedTypeReference baseType)
        { return null; }

        if (baseType.TryResolve(library) is not TranslatedRecord baseRecord)
        { return null; }

        return baseRecord;
    }

    protected override TransformationResult TransformRecord(TransformationContext context, TranslatedRecord declaration)
    {
        TranslatedRecord? baseType = TryGetSimpleBaseType(context.Library, declaration);

        if (baseType is null)
        { return declaration; }

        TranslatedRecord immediateBaseType = baseType;

        HashSet<string> takenNames = new(declaration.Members.Select(m => m.Name));

        bool FilterAndLog(TranslatedDeclaration parentMember)
        {
            //TODO: Virtual destructors don't lift correctly because their VTable entries are attributed to a non-existent destructor on the derived type
            if (parentMember is TranslatedFunction { SpecialFunctionKind: SpecialFunctionKind.Destructor, IsVirtual: true })
            { return false; }

            // Otherwise just do a name check
            return takenNames.Add(parentMember.Name);
        }

        ImmutableList<TranslatedDeclaration>.Builder newMembers = ImmutableList.CreateBuilder<TranslatedDeclaration>();
        newMembers.AddRange(baseType.Members.Where(FilterAndLog));

        // Inherit base types recursively
        while (true)
        {
            baseType = TryGetSimpleBaseType(context.Library, baseType);

            if (baseType is null)
            { break; }

            //TODO: In theory it might be better here to create unique clones and fixup any vtable references to these members.
            newMembers.InsertRange(0, baseType.Members.Where(FilterAndLog));
        }

        // Add our members to the end
        newMembers.AddRange(declaration.Members);

        return declaration with
        {
            // Let's keep the base field for now even though it should be basically useless after this transformation
            // If we decide we do want to get rid of it, we need to make sure this transformation runs after PhysXCreateMarkerInterfacesTransformation
            //NonVirtualBaseField = null,
            Members = newMembers.ToImmutable(),
        };
    }
}
