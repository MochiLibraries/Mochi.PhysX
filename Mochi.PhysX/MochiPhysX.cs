using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mochi.PhysX;

public unsafe static class MochiPhysX
{
    //=======================================================================================================
    // Selecting and overriding the native runtime
    //=======================================================================================================
    public enum Variant
    {
        Default,
        Debug,
        Checked,
        Profile,
        Release,
    }

    private static IntPtr NativeRuntimeHandle;

    // This could be public, but it's very hard to use correctly and the developer could currently manually set their own import resolver.
    // Let's wait to expose this based on a demonstrated need so that the API can stay flexible and so if someone thinks/knows they need this they'll feel more inclined to say something.
    /// <summary>Specifies a specific <see cref="NativeLibrary"/> handle to use for the PhysX runtime.</summary>
    /// <remarks>You must call this method before calling any PhysX functions.</remarks>
    private static void UseSpecificRuntime(IntPtr nativeRuntimeHandle)
    {
        if (nativeRuntimeHandle == IntPtr.Zero)
        { throw new ArgumentException("The specified native runtime handle is invalid.", nameof(nativeRuntimeHandle)); }
        else if (NativeRuntimeHandle != IntPtr.Zero)
        { throw new InvalidOperationException("Cannot select a specific runtime after one has already been loaded."); }

        //TODO: We should validate that the native runtime wasn't already loaded
        NativeRuntimeHandle = nativeRuntimeHandle;

        static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
            => NativeRuntimeHandle;

        NativeLibrary.SetDllImportResolver(typeof(MochiPhysX).Assembly, DllImportResolver);
    }

    /// <summary>Specifies a specific variant of the PhysX runtime to use.</summary>
    /// <remarks>
    /// You must call this method before calling any PhysX functions.
    ///
    /// In order to use a variant, you must manually add the appropriate NuGet package reference.
    /// (For example: To use the checked variant on Windows x64, reference the appropriate version of Mochi.PhysX.Native.win-x64-checked.)
    ///
    /// This method expects the native runtime layout file provided by the official MochiPhysX NuGet packages.
    /// </remarks>
    public static void SelectRuntimeVariant(Variant variant)
    {
        string variantPath = "Mochi.PhysX.Native";

        variantPath = variant switch
        {
            Variant.Default => variantPath,
            Variant.Release => variantPath,
            Variant.Profile => Path.Combine("profile", variantPath),
            Variant.Checked => Path.Combine("checked", variantPath),
            Variant.Debug => Path.Combine("debug", variantPath),
            _ => throw new ArgumentException("The specified variant is invalid.", nameof(variant))
        };

        if (NativeLibrary.TryLoad(variantPath, typeof(MochiPhysX).Assembly, DllImportSearchPath.ApplicationDirectory, out IntPtr handle))
        {
            UseSpecificRuntime(handle);

            if (variant != Variant.Default)
            {
                if (!BuildInfo.EndsWith(variant.ToString().ToLowerInvariant()))
                {
                    string message = $"Tried to load the {variant.ToString().ToLowerInvariant()} vairant of the PhysX runtime but got '{BuildInfo}'.";

                    // The release package is not nested within a subdirectory, so if a non-release runtime was manually copied into the application directory it will be loaded instead
                    if (variant == Variant.Release)
                    { message += $" (Note: {nameof(MochiPhysX)}.{nameof(SelectRuntimeVariant)} expects the file layout provided by the official runtime NuGet packages.)"; }

                    throw new InvalidOperationException(message);
                }
            }
        }
        else
        { throw new DllNotFoundException($"Failed to load the {variant.ToString().ToLowerInvariant()} variant of the PhysX runtime '{variantPath}'. Is the appropriate runtime NuGet package installed?"); }
    }

    //=======================================================================================================
    // Getting the build info
    //=======================================================================================================

    [DllImport("Mochi.PhysX.Native", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetMochiPhysXBuildInfo", ExactSpelling = true)]
    private static extern byte* GetMochiPhysXBuildInfo();

    private static byte* BuildInfoPointerCached;
    private static string? BuildInfoCached;
    /// <summary>Returns the build information of the native runtime component</summary>
    /// <remarks>The build info will be a string including the PhysX SDK version and the build variant (IE: debug, checked, profile, release) of Mochi.PhysX.Native.</remarks>
    public static string BuildInfo
    {
        get
        {
            if (BuildInfoCached is not null)
            {
                Debug.Assert(GetMochiPhysXBuildInfo() == BuildInfoPointerCached);
                return BuildInfoCached;
            }

            byte* buildVariant = GetMochiPhysXBuildInfo();
            string result = Marshal.PtrToStringAnsi((IntPtr)buildVariant)!;
            BuildInfoCached = result;
            BuildInfoPointerCached = buildVariant;
            return result;
        }
    }
}
