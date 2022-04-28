using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.CSharp.Trampolines;
using Biohazrd.Transformation;
using System;

namespace Mochi.PhysX.Generator;

internal sealed class InheritanceViaGenericAdapter : Adapter, IAdapterWithGenericParameter
{
    private readonly string ConstraintInterfaceNamespace;
    private readonly string ConstraintInterfaceTypeName;
    private readonly string GenericParameterName;

    private TypeReference OutputType { get; }
    private string TemporaryName { get; }
    private TypeReference TemporaryType { get; }
    private ByRefKind? ByRefKind { get; }

    public InheritanceViaGenericAdapter(TranslatedLibrary library, Adapter targetAdapter, Adapter replacedAdapter)
        : base(targetAdapter)
    {
        // Save and validate our output type
        // (We mostly only validate the replaced adapter since we assume it's well-formed to adapt to this output.)
        OutputType = targetAdapter.InputType;

        if (OutputType is not PointerTypeReference)
        { throw new ArgumentException("The target adapter must take a pointer.", nameof(targetAdapter)); }

        // Determine the actual record type
        TypeReference recordTypeReference = replacedAdapter.InputType;

        if (recordTypeReference is ByRefTypeReference byRefType)
        {
            ByRefKind = byRefType.Kind;
            recordTypeReference = byRefType.Inner;
        }

        int pointerArity = 0;
        while (recordTypeReference is PointerTypeReference pointerType)
        {
            pointerArity++;
            recordTypeReference = pointerType.Inner;
        }

        if (ByRefKind is null && pointerArity == 0)
        { throw new ArgumentException("The replaced adapter must be passed by reference.", nameof(replacedAdapter)); }

        if (recordTypeReference is not TranslatedTypeReference translatedType)
        { throw new ArgumentException("The replaced adapter does not adapt a translated type reference or uses an unrecognized method to do so.", nameof(replacedAdapter)); }

        if (translatedType.TryResolve(library) is not TranslatedRecord targetRecord)
        { throw new ArgumentException("The replaced adapter does not adapt a record type reference.", nameof(replacedAdapter)); }

        // Determine the constraint info
        ConstraintInterfaceNamespace = $"{targetRecord.Namespace}.{PhysXMarkerInterfacesDeclaration.InfrastructureNamespaceName}";
        ConstraintInterfaceTypeName = $"I{targetRecord.Name}";

        // Name the generic parameter based on the parameter name
        GenericParameterName = $"T{Char.ToUpperInvariant(replacedAdapter.Name[0])}{replacedAdapter.Name.AsSpan().Slice(1)}";

        // Name our temporary (only used when we're byref)
        TemporaryName = $"__{Name}P";

        // Create the input type
        InputType = new ExternallyDefinedTypeReference(GenericParameterName);

        for (int i = 0; i < pointerArity; i++)
        { InputType = new PointerTypeReference(InputType); }

        TemporaryType = InputType;

        if (ByRefKind is ByRefKind byRefKind)
        {
            InputType = new ByRefTypeReference(byRefKind, InputType);
            TemporaryType = new PointerTypeReference(TemporaryType);
        }
    }

    public void WriteGenericParameter(TrampolineContext context, CSharpCodeWriter writer)
        => writer.WriteIdentifier(GenericParameterName);

    public void WriteGenericConstraint(TrampolineContext context, CSharpCodeWriter writer)
    {
        writer.Write("where ");
        writer.WriteIdentifier(GenericParameterName);
        writer.Write(" : unmanaged, ");
        writer.Using(ConstraintInterfaceNamespace);
        writer.WriteIdentifier(ConstraintInterfaceTypeName);
        writer.WriteLine();
    }

    public override void WritePrologue(TrampolineContext context, CSharpCodeWriter writer)
    { }

    public override bool WriteBlockBeforeCall(TrampolineContext context, CSharpCodeWriter writer)
    {
        if (ByRefKind is not ByRefKind byRefKind)
        { return false; }

        writer.Write("fixed (");
        context.WriteType(TemporaryType);
        writer.Write(' ');
        writer.WriteIdentifier(TemporaryName);
        writer.Write(" = ");
        writer.Write('&');
        writer.WriteIdentifier(Name);
        writer.WriteLine(')');
        return true;
    }

    public override void WriteOutputArgument(TrampolineContext context, CSharpCodeWriter writer)
    {
        writer.Write('(');
        context.WriteType(OutputType);
        writer.Write(')');

        if (ByRefKind is not null)
        { writer.WriteIdentifier(TemporaryName); }
        else
        { writer.WriteIdentifier(Name); }
    }
}
