using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.Transformation;
using ClangSharp;
using ClangSharp.Interop;

namespace Mochi.PhysX.Generator;

internal sealed class PhysXCSharpTypeReductionTransformation : CSharpTypeReductionTransformation
{
    protected override TypeTransformationResult TransformClangTypeReference(TypeTransformationContext context, ClangTypeReference type)
    {
        if (type.ClangType is TemplateSpecializationType templateSpecializationType
            && context.Library.FindClangCursor(templateSpecializationType.Handle.Declaration) is ClassTemplateSpecializationDecl templateSpecialization
            && templateSpecialization.Spelling == "PxFixedSizeLookupTable"
            && templateSpecialization.TemplateArgs.Count == 1
            && templateSpecialization.TemplateArgs[0] is { Kind: CXTemplateArgumentKind.CXTemplateArgumentKind_Integral, AsIntegral: 8 }
            )
        {
            return new ExternallyDefinedTypeReference("Mochi.PhysX", "PxFixedSizeLookupTable8");
        }

        return base.TransformClangTypeReference(context, type);
    }
}
