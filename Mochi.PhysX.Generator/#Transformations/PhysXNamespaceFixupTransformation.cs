using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.Transformation;
using System;
using System.Diagnostics;

namespace Mochi.PhysX.Generator
{
    public sealed class PhysXNamespaceFixupTransformation : TransformationBase
    {
        protected override TransformationResult TransformDeclaration(TransformationContext context, TranslatedDeclaration declaration)
        {
            const string badCasing = "physx";
            const string mochiNamespace = "Mochi.PhysX";

            string? newNamespace;

            // Put Biohazrd infrastructure types in their own namespace
            if (declaration is ConstantArrayTypeDeclaration or NativeBooleanDeclaration or NativeCharDeclaration)
            {
                Debug.Assert(declaration.Namespace is null);
                newNamespace = $"{mochiNamespace}.Infrastructure";
            }
            else
            {
                newNamespace = declaration.Namespace switch
                {
                    // PhysX has a lot of global functions in the global namespace for some reason, move them into the PhysX namespace
                    null => mochiNamespace,
                    // This is a weird internal detail of how PhysX structures things, a `using namespace` is added for it so you don't normally see it when using it.
                    "physx.general_PxIOStream2" => mochiNamespace,

                    "physx.intrinsics" => $"{mochiNamespace}.Intrinsics",
                    "physx.pvdsdk" => $"{mochiNamespace}.PvdSdk",
                    "physx.immediate" => $"{mochiNamespace}.Immediate",
                    _ => declaration.Namespace.StartsWith(badCasing, StringComparison.Ordinal) ? $"{mochiNamespace}{declaration.Namespace.Substring(badCasing.Length)}" : declaration.Namespace
                };
            }

            return declaration with { Namespace = newNamespace };
        }
    }
}
