using Biohazrd;
using Biohazrd.CSharp;
using Biohazrd.Expressions;
using Biohazrd.OutputGeneration;
using Biohazrd.Transformation;
using Biohazrd.Transformation.Common;
using Biohazrd.Utilities;
using Mochi.PhysX.Generator;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage:");
    Console.Error.WriteLine("    Mochi.PhysX.Generator <path-to-physx-sdk-root> <path-to-output> <path-to-mochi-physx-native>");
    return 1;
}

string physXSdkRoot = Path.GetFullPath(args[0]);
string outputDirectoryPath = Path.GetFullPath(args[1]);
string nativeRuntimeRoot = Path.GetFullPath(args[2]);

string inlineExportHelperFileName = Path.Combine(nativeRuntimeRoot, "InlineExportHelper.gen.cpp");

const string canonicalBuildVariant = "checked";

string physXPresetName;
string dotNetRid;
string libraryArchiveFilter;
string nativeRuntimeBuildScript;
string importLibraryName;
if (OperatingSystem.IsWindows())
{
    physXPresetName = "Mochi.PhysX.Windows.x64";
    dotNetRid = "win-x64";
    libraryArchiveFilter = "*.lib";
    nativeRuntimeBuildScript = "build-native.cmd";
    importLibraryName = "Mochi.PhysX.Native.lib";
}
else
{
    Console.Error.WriteLine($"'{RuntimeInformation.OSDescription}' is not supported by this generator.");
    return 1;
}

nativeRuntimeBuildScript = Path.Combine(nativeRuntimeRoot, nativeRuntimeBuildScript);
string nativeRuntimeOutputDirectory = Path.Combine(nativeRuntimeRoot, "..", "bin", "Mochi.PhysX.Native", dotNetRid, canonicalBuildVariant);

if (!Directory.Exists(physXSdkRoot))
{
    Console.Error.WriteLine($"PhysX SDK not found at '{physXSdkRoot}'.");
    return 1;
}

string physXInstallRoot = Path.Combine(physXSdkRoot, "physx", "install", physXPresetName);
string physXBinariesDirectoryPath = Path.Combine(physXInstallRoot, "bin");
{
    string? binarySubdirectory = null;

    foreach (string candidate in Directory.EnumerateDirectories(physXBinariesDirectoryPath))
    {
        if (binarySubdirectory is not null)
        {
            Console.Error.WriteLine($"'{physXBinariesDirectoryPath}' contains more than one subdirectory. Aborting since we're not sure which one should be used!");
            return 1;
        }

        binarySubdirectory = candidate;
    }

    if (binarySubdirectory is null)
    {
        Console.Error.WriteLine($"PhysX binaries not found in '{physXBinariesDirectoryPath}', was it built?");
        return 1;
    }

    physXBinariesDirectoryPath = Path.Combine(physXBinariesDirectoryPath, binarySubdirectory, canonicalBuildVariant);

    if (!Directory.Exists(physXBinariesDirectoryPath))
    {
        Console.Error.WriteLine($"PhysX binaries not found in '{physXBinariesDirectoryPath}', do you need to build it?");
        return 1;
    }
}

string[] includeDirectories =
{
    Path.Combine(physXInstallRoot, "include")
};

foreach (string includeDirectory in includeDirectories)
{
    if (!Directory.Exists(includeDirectory))
    {
        Console.Error.WriteLine($"PhysX include directory '{includeDirectory}' not found.");
        return 1;
    }
}

// Create the library
Console.WriteLine("==============================================================================");
Console.WriteLine("Parsing PhysX headers...");
Console.WriteLine("==============================================================================");
TranslatedLibrary library;
TranslatedLibraryConstantEvaluator constantEvaluator;
{
    TranslatedLibraryBuilder libraryBuilder = new();
    libraryBuilder.AddCommandLineArgument("-D_DEBUG");
    libraryBuilder.AddCommandLineArgument("--language=c++");
    libraryBuilder.AddCommandLineArgument("--std=c++17");
    libraryBuilder.AddCommandLineArgument("-Wno-return-type-c-linkage"); // PxGetFoundation triggers this. There's code to suppress it, but it's only triggered when building for Clang on Linux.
    libraryBuilder.AddCommandLineArgument("-Wno-microsoft-include"); // This triggers on a few includes for some reason.

    HashSet<string> indexedFiles = new()
    {
        // All headers within PhysX are considered in-scope, but we only directly index PxPhysicsAPI.h since it includes all of the public API automatically
        "PxPhysicsAPI.h",
        // ...and a few others that are missing from PxPhysicsAPI.h for some reason
        "PxD6JointCreate.h",
        "PxFileBuf.h",
        "PxRaycastCCD.h",
        "PxImmediateMode.h",
        // Others which are missing but also aren't indexed:
        // PxWindowsDelayLoadHook.h -- Only used when building PhysX as separate DLLs (which we don't.)
        // PxPhysicsSerialization.h -- Not meant to be used directly
        // PxProfileZone.h -- Utility macros for profiling
        // PxConfig.h -- Macros written by CMake, not used by PhysX
        //
        // The following aren't directly documented as not being meant to be used directly, but they're surrounded by `#if !PX_DOXYGEN` suggesting they aren't:
        // PxCollisionDefs.h
        // PxCollisionExt.h
    };

    // These headers aren't useful from a binding perspective
    HashSet<string> explicitlyOutOfScope = new()
    {
        "PxWindowsIntrinsics.h",
        "PxUnixIntrinsics.h",
        "PxXboxOneIntrinsics.h",
        "PxXboxSeriesXIntrinsics.h",
        "PxSwitchIntrinsics.h",
    };

    foreach (string includeDirectory in includeDirectories)
    {
        libraryBuilder.AddCommandLineArgument($"-I{includeDirectory}");

        foreach (string headerFile in Directory.EnumerateFiles(includeDirectory, "*.h", SearchOption.AllDirectories))
        {
            string headerFileName = Path.GetFileName(headerFile);

            if (explicitlyOutOfScope.Contains(headerFileName))
            { continue; }

            libraryBuilder.AddFile(new SourceFile(headerFile) { IndexDirectly = indexedFiles.Contains(headerFileName) });
        }
    }

    library = libraryBuilder.Create();
    constantEvaluator = libraryBuilder.CreateConstantEvaluator();
}

// Start output session
using OutputSession outputSession = new()
{
    AutoRenameConflictingFiles = true,
    ConservativeFileLogging = false, // We don't want this since the files in Mochi.PhysX.Native are outside the main output directory.
    BaseOutputDirectory = outputDirectoryPath
};

// Make a report of unused header files
using (StreamWriter unusedFilesReport = outputSession.Open<StreamWriter>("UnusedFiles.txt"))
{
    foreach (TranslatedFile file in library.Files.Where(f => f.WasNotUsed).OrderBy(f => f.FilePath))
    { unusedFilesReport.WriteLine(Path.GetRelativePath(physXSdkRoot, file.FilePath).Replace('\\', '/')); }
}

// Apply transformations
Console.WriteLine("==============================================================================");
Console.WriteLine("Performing library-specific transformations...");
Console.WriteLine("==============================================================================");

BrokenDeclarationExtractor brokenDeclarationExtractor = new();
library = brokenDeclarationExtractor.Transform(library);

library = new __StripPrivateAndProtectedMembersTransformation().Transform(library); //TODO: Put this in Biohazrd

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
library = new WrapNonBlittableTypesWhereNecessaryTransformation().Transform(library);
library = new PhysXNamespaceFixupTransformation().Transform(library);
library = new AddTrampolineMethodOptionsTransformation(MethodImplOptions.AggressiveInlining).Transform(library);
library = new MoveLooseDeclarationsIntoTypesTransformation
(
    // PhysX has a lot of global functions which confuse the default type placement logic used by this transformation transformation, so just put everything in `Globals`.
    // For example, PxJoint.h has PxSetJointGlobalFrame in the global namespace and involves the type PxJoint in the physx namespace.
    // (The default logic here will try to create a new PxJoint type in the global namespace to house PxSetJoinGlobalFrame.)
    (c, d) => "Globals"
).Transform(library);
library = new PhysXMacrosToConstantsTransformation(constantEvaluator).Transform(library);
library = new AutoNameUnnamedParametersTransformation().Transform(library);
library = new StripUnreferencedLazyDeclarationsTransformation().Transform(library);
library = new DeduplicateNamesTransformation().Transform(library);
library = new OrganizeOutputFilesByNamespaceTransformation("Mochi.PhysX").Transform(library);

// Generate the exports list for the native runtime
using (TextWriter exportsList = OperatingSystem.IsWindows() ? outputSession.Open<CppCodeWriter>(Path.Combine(nativeRuntimeRoot, "Exports.gen.cpp")) : outputSession.Open<StreamWriter>(Path.Combine(nativeRuntimeRoot, "Exports.gen.map")))
{
    // Use a dummy LinkImportsTransformation to enumerate all symbols exported by PhysX's static libraries
    LinkImportsTransformation staticExportLookup = new();
    foreach (string libFilePath in Directory.EnumerateFiles(physXBinariesDirectoryPath, libraryArchiveFilter))
    {
        Console.WriteLine($"Scanning '{Path.GetFileName(libFilePath)}' for exported symbols...");
        staticExportLookup.AddLibrary(libFilePath);
    }

    if (!OperatingSystem.IsWindows())
    {
        exportsList.WriteLine("{");
        exportsList.WriteLine("  global:");
    }

    // Ask the linker to export all symbols which are statically exported by PhysX's static libraries
    void Export(string symbol)
    {
        if (OperatingSystem.IsWindows())
        { exportsList.WriteLine($"#pragma comment(linker, \"/export:{symbol}\")"); }
        else
        { exportsList.WriteLine($"    {symbol};"); }
    }

    foreach (TranslatedDeclaration declaration in library.EnumerateRecursively())
    {
        switch (declaration)
        {
            case TranslatedFunction function:
                if (staticExportLookup.ContainsSymbol(function.MangledName))
                { Export(function.MangledName); }
                break;
            case TranslatedStaticField staticField:
                if (staticExportLookup.ContainsSymbol(staticField.MangledName))
                { Export(staticField.MangledName); }
                break;
        }
    }

    if (!OperatingSystem.IsWindows())
    { exportsList.WriteLine("};"); }
}

// Generate the inline export helper
library = new InlineExportHelper(outputSession, inlineExportHelperFileName)
{
    __ItaniumExportMode = !OperatingSystem.IsWindows()
}.Transform(library);

// Rebuild the native DLL so that the librarian can access the library including the inline-exported functions
Console.WriteLine("Rebuilding native runtime...");
Process nativeRuntimeBuild = Process.Start(new ProcessStartInfo(nativeRuntimeBuildScript)
{
    WorkingDirectory = nativeRuntimeRoot
})!;
nativeRuntimeBuild.WaitForExit();

if (nativeRuntimeBuild.ExitCode != 0)
{
    Console.Error.WriteLine("Failed to rebuild the native runtime!");
    return nativeRuntimeBuild.ExitCode;
}

// Use librarian to identifiy DLL exports
{
    LinkImportsTransformation linkImports = new();

    string nativeRuntimeLibPath = Path.Combine(nativeRuntimeOutputDirectory, importLibraryName);
    if (!File.Exists(nativeRuntimeLibPath))
    {
        Console.Error.WriteLine($"Native runtime archive file could not be found at '{nativeRuntimeLibPath}'.");
        return 1;
    }

    linkImports.AddLibrary(nativeRuntimeLibPath);
    library = linkImports.Transform(library);
}

// Workaround for https://github.com/MochiLibraries/Biohazrd/issues/80
library = new SimpleTransformation()
{
    TransformFunction = (context, function) =>
    {
        if (function.Name is "PxDefaultSimulationFilterShader" && context.ParentDeclaration?.Name is "Globals")
        {
            TransformationResult result = new TranslatedConstant($"{function.Name}MangledName", new StringConstant(function.MangledName)) { Accessibility = AccessModifier.Internal };
            result.Add(new TranslatedConstant($"{function.Name}DllFileName", new StringConstant(function.DllFileName)) { Accessibility = AccessModifier.Internal });
            return result;
        }

        return function;
    }
}.Transform(library);

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
    CSharpGenerationOptions.Default,
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
return 0;
