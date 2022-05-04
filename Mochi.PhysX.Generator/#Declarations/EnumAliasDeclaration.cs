using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.CSharp.Infrastructure;
using Biohazrd.Transformation;
using Biohazrd.Transformation.Infrastructure;

namespace Mochi.PhysX.Generator;

internal sealed record EnumAliasDeclaration : TranslatedDeclaration, ICustomTranslatedDeclaration, ICustomCSharpTranslatedDeclaration
{
    public DeclarationReference EnumConstantReference { get; init; }

    public EnumAliasDeclaration(TranslatedEnumConstant constant)
        : base(constant.File)
    {
        Accessibility = constant.Accessibility;
        Name = constant.Name;
        EnumConstantReference = new DeclarationReference(constant);
    }

    TransformationResult ICustomTranslatedDeclaration.TransformChildren(ITransformation transformation, TransformationContext context)
        => this;

    TransformationResult ICustomTranslatedDeclaration.TransformTypeChildren(ITypeTransformation transformation, TransformationContext context)
        => this;

    void ICustomCSharpTranslatedDeclaration.GenerateOutput(ICSharpOutputGenerator outputGenerator, VisitorContext context, CSharpCodeWriter writer)
    {
        void Failure(string message)
        {
            writer.WriteLine($"// {message}");
            outputGenerator.AddDiagnostic(Severity.Warning, message);
        }

        if (EnumConstantReference.TryResolve(context.Library, out VisitorContext resolvedContext) is not TranslatedDeclaration resolved)
        {
            Failure($"Failed to resolve `{EnumConstantReference}` for enum alias {Name}");
            return;
        }

        if (resolved is not TranslatedEnumConstant enumConstant)
        {
            Failure($"Enum alias {Name} resolved to `{resolved}`, which is not an enum constant.");
            return;
        }

        if (resolvedContext.ParentDeclaration is not TranslatedEnum parentEnum)
        {
            Failure($"Enum alias {Name} resolved to `{resolved}`, a child of {resolvedContext.ParentDeclaration?.ToString() ?? "<null>"} which is not an enum.");
            return;
        }

        PreResolvedTypeReference enumType = new(resolvedContext.MakePrevious(), parentEnum);
        string enumTypeString = outputGenerator.GetTypeAsString(context, this, enumType);

        writer.EnsureSeparation();
        writer.Write($"{Accessibility.ToCSharpKeyword()} const {enumTypeString} ");
        writer.WriteIdentifier(Name);
        writer.Write($" = {enumTypeString}.");
        writer.WriteIdentifier(enumConstant.Name);
        writer.WriteLine(';');
    }
}
