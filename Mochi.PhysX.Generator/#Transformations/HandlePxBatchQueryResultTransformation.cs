using Biohazrd;
using Biohazrd.Transformation;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Mochi.PhysX.Generator;

// Because PxBatchQueryResult<HitType> and related typedefs are deprecated, we just special-case them rather than trying to let Biohazrd's template specialization handle them
// Since they're deprecated we assume they're unlikely to change, so this is not the most robust transformation for handling this sort of situation
internal sealed class HandlePxBatchQueryResultTransformation : TransformationBase
{
    private TranslatedFile? PxBatchQueryDescFile = null;

    protected override TranslatedLibrary PreTransformLibrary(TranslatedLibrary library)
    {
        Debug.Assert(PxBatchQueryDescFile is null);
        PxBatchQueryDescFile = library.Files.FirstOrDefault(f => f.WasInScope && !f.WasNotUsed && Path.GetFileName(f.FilePath) == "PxBatchQueryDesc.h");
        return library;
    }

    protected override TransformationResult TransformTypedef(TransformationContext context, TranslatedTypedef declaration)
    {
        if (declaration.File != PxBatchQueryDescFile)
        { return declaration; }

        string? hitType = declaration.Name switch
        {
            "PxRaycastQueryResult" => "PxRaycastHit",
            "PxSweepQueryResult" => "PxSweepHit",
            "PxOverlapQueryResult" => "PxOverlapHit",
            _ => null
        };

        if (hitType is null)
        { return declaration; }

        Debug.Assert(declaration.Namespace == "physx");
        Debug.Assert(declaration.UnderlyingType is TranslatedTypeReference);
        Debug.Assert(((TranslatedTypeReference)declaration.UnderlyingType).TryResolve(context.Library) is TranslatedUnsupportedDeclaration or null);

        return declaration with
        {
            UnderlyingType = new ExternallyDefinedTypeReference("Mochi.PhysX", $"PxBatchQueryResult<{hitType}>")
        };
    }
}
