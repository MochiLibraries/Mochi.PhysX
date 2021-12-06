using Biohazrd;
using Biohazrd.Transformation;

namespace Mochi.PhysX.Generator
{
    public sealed class RemoveBadPhysXDeclarationsTransformation : TransformationBase
    {
        protected override TransformationResult TransformUnsupportedDeclaration(TransformationContext context, TranslatedUnsupportedDeclaration declaration)
            // Remove all of the PxTypeInfo<T> helpers.
            // They aren't especially useful from C#, if we determine we need eFastTypeId we'd likely want to implement it some other way.
            => declaration.Name == "PxTypeInfo" ? null : declaration;

        protected override TransformationResult TransformFunction(TransformationContext context, TranslatedFunction declaration)
        {
            // PxRepXInstantiationArg::operator= is never actually defined in PhysX and as such cannot be called.
            if (context.ParentDeclaration?.Name == "PxRepXInstantiationArgs" && declaration.SpecialFunctionKind == SpecialFunctionKind.OperatorOverload)
            { return null; }

            return declaration;
        }
    }
}
