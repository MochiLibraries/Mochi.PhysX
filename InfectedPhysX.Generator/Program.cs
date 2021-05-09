#define USE_TOP_LEVEL_STATEMENTS // Workaround for https://github.com/dotnet/roslyn/issues/50591
using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.OutputGeneration;
using Biohazrd.Transformation.Common;
using Biohazrd.Utilities;
using InfectedPhysX.Generator;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Runtime.CompilerServices;

#if !USE_TOP_LEVEL_STATEMENTS
static class Program { static void Main(string[] args) {
#endif

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
        Console.Error.WriteLine($"PhysX include directory '{includeDirectory}' not found.");
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
library = new CSharpTypeReductionTransformation().Transform(library);

library = new CSharpBuiltinTypeTransformation().Transform(library);
library = new LiftAnonymousRecordFieldsTransformation().Transform(library);
library = new KludgeUnknownClangTypesIntoBuiltinTypesTransformation(emitErrorOnFail: true).Transform(library);
library = new WrapNonBlittableTypesWhereNecessaryTransformation().Transform(library);
library = new PhysXNamespaceFixupTransformation().Transform(library);
library = new AddTrampolineMethodOptionsTransformation(MethodImplOptions.AggressiveInlining).Transform(library);
library = new MoveLooseDeclarationsIntoTypesTransformation
(
    // PhysX has a lot of global functions which confuse this transformation.
    // For example, PxJoint.h has PxSetJointGlobalFrame in the global namespace and involves the type physx::PxJoint.
    (c, d) => "Globals"
).Transform(library);
library = new AutoNameUnnamedParametersTransformation().Transform(library);
library = new StripUnreferencedLazyDeclarationsTransformation().Transform(library);
library = new DeduplicateNamesTransformation().Transform(library);
library = new OrganizeOutputFilesByNamespaceTransformation("PhysX").Transform(library);

// Generate module definition
#pragma warning disable CS0618 // Type or member is obsolete
ModuleDefinitionGenerator.Generate(outputSession, "InfectedPhysX.def", library);
InlineReferenceFileGenerator.Generate(outputSession, "InfectedPhysX.cpp", library);
#pragma warning restore CS0618 // Type or member is obsolete

//TODO: Rework things to use Biohazrd's new inline handling:
// Use InlineExportHelper
// Rebuild the DLL
// Use the librarian to identify DLL exports

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

ImmutableArray<TranslationDiagnostic> generationDiagnostics = CSharpLibraryGenerator.Generate
(
    CSharpGenerationOptions.Default with { DumpClangInfo = false },
    outputSession,
    library
);

// Write out diagnostics log
DiagnosticWriter diagnostics = new();
diagnostics.AddFrom(library);
diagnostics.AddFrom(brokenDeclarationExtractor);
diagnostics.AddCategory("Generation Diagnostics", generationDiagnostics, "Generation completed successfully");

using StreamWriter diagnosticsOutput = outputSession.Open<StreamWriter>("Diagnostics.log");
diagnostics.WriteOutDiagnostics(diagnosticsOutput, writeToConsole: true);

outputSession.Dispose();

#if !USE_TOP_LEVEL_STATEMENTS
}}
#endif
