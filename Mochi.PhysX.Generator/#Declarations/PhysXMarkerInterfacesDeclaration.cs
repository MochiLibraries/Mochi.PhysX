using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.CSharp.Infrastructure;
using Biohazrd.Transformation;
using Biohazrd.Transformation.Infrastructure;
using System.Collections.Immutable;
using static Biohazrd.CSharp.CSharpCodeWriter;

namespace Mochi.PhysX.Generator;

internal sealed record PhysXMarkerInterfacesDeclaration : TranslatedDeclaration, ICustomTranslatedDeclaration, ICustomCSharpTranslatedDeclaration
{
    internal const string InfrastructureNamespaceName = "Infrastructure";

    public ImmutableArray<TypeReference> TargetTypes { get; init; }

    public PhysXMarkerInterfacesDeclaration()
        : base(TranslatedFile.Synthesized)
        => Name = "MarkerInterfaces";

    TransformationResult ICustomTranslatedDeclaration.TransformChildren(ITransformation transformation, TransformationContext context)
        => this;

    TransformationResult ICustomTranslatedDeclaration.TransformTypeChildren(ITypeTransformation transformation, TransformationContext context)
    {
        DiagnosticAccumulator diagnostics = new();
        ImmutableArray<TypeReference>.Builder? typesBuilder = null;

        for (int i = 0; i < TargetTypes.Length; i++)
        {
            TypeReference targetType = TargetTypes[i];
            SingleTypeTransformHelper newType = new(targetType, ref diagnostics);
            newType.SetValue(transformation.TransformTypeRecursively(context, targetType));

            if (newType.WasChanged)
            {
                if (typesBuilder is null)
                { typesBuilder = TargetTypes.ToBuilder(); }

                typesBuilder[i] = newType.NewValue;
            }
        }

        if (typesBuilder is not null || diagnostics.HasDiagnostics)
        {
            return this with
            {
                Diagnostics = Diagnostics.AddRange(diagnostics.MoveToImmutable()),
                TargetTypes = typesBuilder is null ? TargetTypes : typesBuilder.MoveToImmutable()
            };
        }
        else
        { return this; }
    }

    void ICustomCSharpTranslatedDeclaration.GenerateOutput(ICSharpOutputGenerator outputGenerator, VisitorContext context, CSharpCodeWriter writer)
    {
        //-----------------------------------------------------------------------------------------------------------------------------
        // Emit interface markers
        //-----------------------------------------------------------------------------------------------------------------------------
        writer.WriteLine($"namespace {InfrastructureNamespaceName}");
        using (writer.Block())
        {
            foreach (TypeReference targetType in TargetTypes)
            {
                if (targetType is not TranslatedTypeReference translatedType)
                {
                    writer.WriteLine($"// Skipping invalid target type: '{targetType}'");
                    continue;
                }

                if (translatedType.TryResolve(context.Library) is not TranslatedDeclaration targetDeclaration)
                {
                    writer.WriteLine($"// Skipping unresolvable target type: '{targetType}'");
                    continue;
                }

                if (targetDeclaration is not TranslatedRecord targetRecord)
                {
                    writer.WriteLine($"// Skipping non-record target: '{targetDeclaration}'");
                    continue;
                }

                writer.Write($"public partial interface {SanitizeIdentifier($"I{targetRecord.Name}")}");

                if (targetRecord.NonVirtualBaseField?.Type is TranslatedTypeReference baseReference)
                {
                    if (baseReference.TryResolve(context.Library) is not TranslatedDeclaration baseDeclaration)
                    { writer.Write($"/* : unresolvable base '{baseReference}' */"); }
                    else
                    {
                        writer.Using($"{baseDeclaration.Namespace}.{InfrastructureNamespaceName}");
                        writer.Write($" : {SanitizeIdentifier($"I{baseDeclaration.Name}")}");
                    }
                }

                writer.WriteLine(" { }");
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        // Emit interface "implementations"
        //-----------------------------------------------------------------------------------------------------------------------------
        writer.Using($"{Namespace}.Infrastructure"); // For accessing our own marker interfaces
        writer.EnsureSeparation();

        foreach (TypeReference targetType in TargetTypes)
        {
            if (targetType is not TranslatedTypeReference translatedType || translatedType.TryResolve(context.Library) is not TranslatedRecord targetRecord)
            { continue; }

            string sanitizedName = SanitizeIdentifier(targetRecord.Name);
            writer.WriteLine($"partial struct {sanitizedName} : I{sanitizedName} {{ }}");
        }
    }
}
