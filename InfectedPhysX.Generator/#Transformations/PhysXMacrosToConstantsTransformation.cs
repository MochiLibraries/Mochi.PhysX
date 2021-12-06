using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.Expressions;
using Biohazrd.Transformation;
using System.Collections.Immutable;
using System.Linq;

namespace InfectedPhysX.Generator
{
    public sealed class PhysXMacrosToConstantsTransformation : CSharpTransformationBase
    {
        private readonly TranslatedLibraryConstantEvaluator ConstantEvaluator;
        private const string TargetClassName = "Globals";
        private ImmutableList<TranslatedDeclaration>? Constants = null;

        public PhysXMacrosToConstantsTransformation(TranslatedLibraryConstantEvaluator constantEvaluator)
            => ConstantEvaluator = constantEvaluator;

        private bool MacroFilter(TranslatedMacro macro)
        {
            if (!macro.HasValue || macro.IsFunctionLike)
            { return false; }

            switch (macro.Name)
            {
                case "INVALID_OBSTACLE_HANDLE":
                case "LOCAL_CONTACTS_SIZE":
                case "PXC_CONTACT_NO_FACE_INDEX":
                case "PX_BINARY_SERIAL_VERSION":
                case "PX_MAX_BOUNDS_EXTENTS":
                case "PX_MAX_EXTENDED":
                case "PX_MAX_NB_WHEELS":
                case "PX_MAX_SWEEP_DISTANCE":
                case "PX_MESH_SCALE_MAX":
                case "PX_MESH_SCALE_MIN":
                case "PX_MIN_HEIGHTFIELD_XZ_SCALE":
                case "PX_MIN_HEIGHTFIELD_Y_SCALE":
                case "PX_PHYSICS_VERSION":
                case "PX_PHYSICS_VERSION_BUGFIX":
                case "PX_PHYSICS_VERSION_MAJOR":
                case "PX_PHYSICS_VERSION_MINOR":
                case "PX_SERIAL_ALIGN":
                case "PX_SERIAL_FILE_ALIGN":
                case "PX_SERIAL_OBJECT_ID_INVALID":
                case "PX_SERIAL_REF_KIND_MATERIAL_IDX":
                case "PX_SERIAL_REF_KIND_PTR_TYPE_BIT":
                case "PX_SERIAL_REF_KIND_PXBASE":
                    return true;
                default:
                    return false;
            }
        }

        protected override TranslatedLibrary PreTransformLibrary(TranslatedLibrary library)
        {
            ImmutableList<TranslatedDeclaration>.Builder constants = ImmutableList.CreateBuilder<TranslatedDeclaration>();
            ImmutableArray<TranslationDiagnostic>.Builder diagnostics = library.ParsingDiagnostics.ToBuilder();

            // This is required to evaluate PX_MIN_HEIGHTFIELD_Y_SCALE because it references physx::PxReal without qualifying it
            const string extraCode = "using namespace physx;";

            foreach (ConstantEvaluationResult result in ConstantEvaluator.EvaluateBatch(extraCode, library.Macros.Where(MacroFilter)))
            {
                if (result.Value is null)
                {
                    diagnostics.AddRange(result.Diagnostics);
                    continue;
                }

                constants.Add(new TranslatedConstant(result.Expression, result));
            }

            Constants = constants.ToImmutable();
            return library with { ParsingDiagnostics = diagnostics.MoveToImmutableSafe() };
        }

        protected override TransformationResult TransformSynthesizedLooseDeclarationsType(TransformationContext context, SynthesizedLooseDeclarationsTypeDeclaration declaration)
        {
            if (Constants is not null && declaration.Name == TargetClassName)
            {
                declaration = declaration with
                {
                    Members = declaration.Members.AddRange(Constants)
                };
                Constants = null;
            }

            return declaration;
        }

        protected override TranslatedLibrary PostTransformLibrary(TranslatedLibrary library)
        {
            if (Constants is not null)
            {
                Constants = null;
                return library with
                {
                    ParsingDiagnostics = library.ParsingDiagnostics.Add(Severity.Warning, $"{nameof(PhysXMacrosToConstantsTransformation)} failed to find type named '{TargetClassName}' to hold constants.")
                };
            }

            return library;
        }
    }
}
