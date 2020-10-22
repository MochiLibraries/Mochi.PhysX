using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.OutputGeneration;
using Biohazrd.Transformation.Common;
using InfectedPhysX.Generator;
using System;
using System.Collections.Immutable;
using System.IO;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage:");
    Console.Error.WriteLine("    InfectedPhysX.Generator <path-to-physx-source> <path-to-output>");
    return;
}

string physXSourceDirectoryPath = Path.GetFullPath(args[0]);
string outputDirectoryPath = Path.GetFullPath(args[1]);

if (!Directory.Exists(physXSourceDirectoryPath))
{
    Console.Error.WriteLine($"PhysX source directory '{physXSourceDirectoryPath}' not found.");
    return;
}

string[] includeDirectories =
{
    Path.Combine(physXSourceDirectoryPath, "physx", "include"),
    Path.Combine(physXSourceDirectoryPath, "pxshared", "include")
};

foreach (string includeDirectory in includeDirectories)
{
    if (!Directory.Exists(includeDirectory))
    {
        Console.Error.WriteLine($"PhysX source directory '{includeDirectory}' not found.");
        return;
    }
}

// Create the library
TranslatedLibraryBuilder libraryBuilder = new();
libraryBuilder.AddCommandLineArgument("-D_DEBUG");
libraryBuilder.AddCommandLineArgument("--language=c++");
libraryBuilder.AddCommandLineArgument("--std=c++17");
libraryBuilder.AddCommandLineArgument("-Wno-return-type-c-linkage"); // PxGetFoundation triggers this. There's code to suppress it, but it's only triggered when building for Clang on Linux.
libraryBuilder.AddCommandLineArgument("-Wno-microsoft-include"); // This triggers on a few includes for some reason.

foreach (string includeDirectory in includeDirectories)
{
    libraryBuilder.AddCommandLineArgument($"-I{includeDirectory}");

    foreach (string headerFile in Directory.EnumerateFiles(includeDirectory, "*.h", SearchOption.AllDirectories))
    {
        // Skip PxUnixIntrinsics since it's not relevant on Windows
        if (Path.GetFileName(headerFile) == "PxUnixIntrinsics.h")
        { continue; }

        libraryBuilder.AddFile(headerFile);
    }
}

TranslatedLibrary library = libraryBuilder.Create();

// Start output session
using OutputSession outputSession = new()
{
    AutoRenameConflictingFiles = true,
    BaseOutputDirectory = outputDirectoryPath
};

// Apply transformations
Console.WriteLine("==============================================================================");
Console.WriteLine("Performing library-specific transformations...");
Console.WriteLine("==============================================================================");

BrokenDeclarationExtractor brokenDeclarationExtractor = new();
library = brokenDeclarationExtractor.Transform(library);

library = new RemoveBadPhysXDeclarationsTransformation().Transform(library);
library = new PhysXRemovePaddingFieldsTransformation().Transform(library);
library = new PhysXEnumTransformation().Transform(library);
library = new PhysXFlagsEnumTransformation().Transform(library);

library = new RemoveExplicitBitFieldPaddingFieldsTransformation().Transform(library);
library = new AddBaseVTableAliasTransformation().Transform(library);
library = new ConstOverloadRenameTransformation().Transform(library);
library = new MakeEverythingPublicTransformation().Transform(library);
library = new RemoveRemainingTypedefsTransformation().Transform(library);

CSharpTypeReductionTransformation typeReductionTransformation = new();
int iterations = 0;
do
{
    library = typeReductionTransformation.Transform(library);
    iterations++;
} while (typeReductionTransformation.ConstantArrayTypesCreated > 0);
Console.WriteLine($"Finished reducing types in {iterations} iterations.");

library = new LiftAnonymousUnionFieldsTransformation().Transform(library);
library = new CSharpBuiltinTypeTransformation().Transform(library);
library = new KludgeUnknownClangTypesIntoBuiltinTypesTransformation(emitErrorOnFail: true).Transform(library);
library = new DeduplicateNamesTransformation().Transform(library);
library = new MoveLooseDeclarationsIntoTypesTransformation().Transform(library);

// Perform validation
Console.WriteLine("==============================================================================");
Console.WriteLine("Performing post-translation validation...");
Console.WriteLine("==============================================================================");

library = new CSharpTranslationVerifier().Transform(library);

// Remove final broken declarations
library = brokenDeclarationExtractor.Transform(library);

// Emit the translation
Console.WriteLine("==============================================================================");
Console.WriteLine("Emitting translation...");
Console.WriteLine("==============================================================================");

// Generate module definition
ModuleDefinitionGenerator.Generate(outputSession, "InfectedPhysX.def", library);
InlineReferenceFileGenerator.Generate(outputSession, "InfectedPhysX.cpp", library);

ImmutableArray<TranslationDiagnostic> generationDiagnostics = CSharpLibraryGenerator.Generate
(
    CSharpGenerationOptions.Default with { DumpClangInfo = false },
    outputSession,
    library,
    LibraryTranslationMode.OneFilePerType
);

// Write out diagnostics log
using StreamWriter diagnosticsOutput = outputSession.Open<StreamWriter>("Diagnostics.log");

void OutputDiagnostic(in TranslationDiagnostic diagnostic)
{
    WriteDiagnosticToConsole(diagnostic);
    WriteDiagnosticToWriter(diagnostic, diagnosticsOutput);
}

diagnosticsOutput.WriteLine("==============================================================================");
diagnosticsOutput.WriteLine("Parsing Diagnostics");
diagnosticsOutput.WriteLine("==============================================================================");

foreach (TranslationDiagnostic parsingDiagnostic in library.ParsingDiagnostics)
{ OutputDiagnostic(parsingDiagnostic); }

diagnosticsOutput.WriteLine("==============================================================================");
diagnosticsOutput.WriteLine("Translation Diagnostics");
diagnosticsOutput.WriteLine("==============================================================================");

foreach (TranslatedDeclaration declaration in library.EnumerateRecursively())
{
    if (declaration.Diagnostics.Length > 0)
    {
        diagnosticsOutput.WriteLine($"--------------- {declaration.GetType().Name} {declaration.Name} ---------------");

        foreach (TranslationDiagnostic diagnostic in declaration.Diagnostics)
        { OutputDiagnostic(diagnostic); }
    }
}

if (brokenDeclarationExtractor.BrokenDeclarations.Length > 0)
{
    diagnosticsOutput.WriteLine("==============================================================================");
    diagnosticsOutput.WriteLine("Broken Declarations");
    diagnosticsOutput.WriteLine("==============================================================================");

    foreach (TranslatedDeclaration declaration in brokenDeclarationExtractor.BrokenDeclarations)
    {
        diagnosticsOutput.WriteLine($"=============== {declaration.GetType().Name} {declaration.Name} ===============");

        foreach (TranslationDiagnostic diagnostic in declaration.Diagnostics)
        { OutputDiagnostic(diagnostic); }
    }
}

diagnosticsOutput.WriteLine("==============================================================================");
diagnosticsOutput.WriteLine("Generation Diagnostics");
diagnosticsOutput.WriteLine("==============================================================================");

if (generationDiagnostics.Length == 0)
{ diagnosticsOutput.WriteLine("Generation completed successfully."); }
else
{
    foreach (TranslationDiagnostic diagnostic in generationDiagnostics)
    { OutputDiagnostic(diagnostic); }
}

outputSession.Dispose();

static void WriteDiagnosticToConsole(in TranslationDiagnostic diagnostic)
{
    TextWriter output;
    ConsoleColor oldForegroundColor = Console.ForegroundColor;
    ConsoleColor oldBackgroundColor = Console.BackgroundColor;

    try
    {
        switch (diagnostic.Severity)
        {
            case Severity.Ignored:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                output = Console.Out;
                break;
            case Severity.Note:
                output = Console.Out;
                break;
            case Severity.Warning:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                output = Console.Error;
                break;
            case Severity.Error:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                output = Console.Error;
                break;
            case Severity.Fatal:
            default:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                output = Console.Error;
                break;
        }

        WriteDiagnosticToWriter(diagnostic, output);
    }
    finally
    {
        Console.BackgroundColor = oldBackgroundColor;
        Console.ForegroundColor = oldForegroundColor;
    }
}

static void WriteDiagnosticToWriter(in TranslationDiagnostic diagnostic, TextWriter output)
{
    if (!diagnostic.Location.IsNull)
    {
        string fileName = Path.GetFileName(diagnostic.Location.SourceFile);
        if (diagnostic.Location.Line != 0)
        { output.WriteLine($"{diagnostic.Severity} at {fileName}:{diagnostic.Location.Line}: {diagnostic.Message}"); }
        else
        { output.WriteLine($"{diagnostic.Severity} at {fileName}: {diagnostic.Message}"); }
    }
    else
    { output.WriteLine($"{diagnostic.Severity}: {diagnostic.Message}"); }
}
