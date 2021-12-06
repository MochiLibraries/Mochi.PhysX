using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mochi.PhysX;

public unsafe sealed partial class MochiPhysX
{
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
