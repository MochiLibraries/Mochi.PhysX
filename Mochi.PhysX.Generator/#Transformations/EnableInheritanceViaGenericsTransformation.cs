using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.CSharp.Trampolines;
using Biohazrd.Transformation;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Mochi.PhysX.Generator;

/// <remarks>Depends on <see cref="PhysXCreateMarkerInterfacesTransformation"/> and <see cref="CreateTrampolinesTransformation"/>.</remarks>
internal sealed class EnableInheritanceViaGenericsTransformation : TransformationBase
{
    private HashSet<string> SkippedTypes = new();

    protected override TranslatedLibrary PreTransformLibrary(TranslatedLibrary library)
    {
        SkippedTypes.Clear();
        return base.PreTransformLibrary(library);
    }

    protected override TranslatedLibrary PostTransformLibrary(TranslatedLibrary library)
    {
        File.WriteAllLines("physx.skippedtypes.txt", SkippedTypes.OrderBy(s => s));
        return base.PostTransformLibrary(library);
    }

    protected override TransformationResult TransformFunction(TransformationContext context, TranslatedFunction declaration)
    {
        // Special functions cannot be eimtted with generic adapters
        if (declaration.SpecialFunctionKind != SpecialFunctionKind.None)
        { return declaration; }

        // These methods would be overloaded purely by generic type constraints, which is not possible in .NET
        // In theory we could handle this situation by generating them as extension methods instead, but since it's just these two we just manually handle them
        if (declaration.Parameters.Length == 1 && context.ParentDeclaration?.Name == "PxControllerBehaviorCallback" && declaration.Name == "getBehaviorFlags")
        { return declaration; }

        // Build the inheritance via generic trampoline as needed
        if (!declaration.Metadata.TryGet(out TrampolineCollection trampolines))
        { return declaration; }

        Trampoline? MakeTrampoline(bool allowDefaultParameters, string nameSuffix, out bool hadSkippedDefaultParameter)
        {
            Trampoline targetTrampoline = trampolines.PrimaryTrampoline;
            TrampolineBuilder trampolineBuilder = new(targetTrampoline, useAsTemplate: true);
            hadSkippedDefaultParameter = false;

            foreach (TranslatedParameter parameter in declaration.Parameters)
            {
                // Determine the record this parameter points to
                TypeReference type = parameter.Type;

                // Don't bother with by-value parameters, generics would not have the correct semantics and we can't handle them
                if (type is TranslatedTypeReference)
                { continue; }

                while (true)
                {
                    if (type is ByRefTypeReference byRefType)
                    { type = byRefType.Inner; }
                    else if (type is PointerTypeReference pointerType)
                    { type = pointerType.Inner; }
                    else
                    { break; }
                }

                if (type is not TranslatedTypeReference translatedTypeReference)
                { continue; }

                if (translatedTypeReference.TryResolve(context.Library) is not TranslatedRecord targetRecord)
                { continue; }

                if (!targetRecord.Metadata.TryGet(out HasMarkerInterface marker))
                { continue; }

                if (!marker.WantsInheritanceViaGenerics)
                {
                    SkippedTypes.Add(targetRecord.Name);
                    continue;
                }

                // If we got this far, the parameter is a reference to a record which has a marker interface we can use
                Adapter? targetAdapter = trampolines.NativeFunction.Adapters.FirstOrDefault(a => a.CorrespondsTo(parameter));
                Adapter? replacedAdapter = targetTrampoline.Adapters.FirstOrDefault(a => a.CorrespondsTo(parameter));

                if (targetAdapter is null || replacedAdapter is null)
                {
                    Debug.Fail("Could not find target adapters.");
                    continue;
                }

                // Parameters which have a default value don't play nice with generic inheritance because the type can't be inferred.
                // As such we create two trampolines for these: One without the default parameter generified and one without
                if (!allowDefaultParameters && parameter.DefaultValue is not null)
                {
                    hadSkippedDefaultParameter = true;
                    continue;
                }

                trampolineBuilder.AdaptParameter(replacedAdapter, new InheritanceViaGenericAdapter(context.Library, targetAdapter, replacedAdapter));
            }

            // Build the trampoline
            if (!trampolineBuilder.HasAdapters)
            { return null; }
            else
            {
                trampolineBuilder.Description = $"{targetTrampoline.Description} + {nameSuffix}";
                return trampolineBuilder.Create();
            }
        }

        // Create the trampoline(s)
        Trampoline? mainTrampoline = MakeTrampoline(allowDefaultParameters: false, "Generic inheritance enhancements", out bool hadSkippedDefaultParameter);
        Trampoline? withDefaultsTrampoline = null;

        if (hadSkippedDefaultParameter)
        {
            // There's a lot of duplicated effort here, but there are only a small handful of APIs which actually qualify for this trampoline.
            // As such it's not worth complicating MakeTrampoline to try and make both trampolines at the same time.
            withDefaultsTrampoline = MakeTrampoline(allowDefaultParameters: true, "Generic inheritance enhancements w/ defaults", out _);
        }

        if (mainTrampoline is null && withDefaultsTrampoline is null)
        { return declaration; }

        // Add the trampoline(s) to the function
        trampolines = trampolines with
        {
            // We replace the default friendly trampoline with ours entirely since they only enable new possibilities without restricting anything
            PrimaryTrampoline = mainTrampoline is null ? trampolines.PrimaryTrampoline : mainTrampoline,
            SecondaryTrampolines = withDefaultsTrampoline is null ? trampolines.SecondaryTrampolines : trampolines.SecondaryTrampolines.Add(withDefaultsTrampoline)
        };

        return declaration with
        {
            Metadata = declaration.Metadata.Set(trampolines)
        };
    }
}
