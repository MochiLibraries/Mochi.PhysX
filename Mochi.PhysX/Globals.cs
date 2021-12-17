using System;
using System.Runtime.InteropServices;

namespace Mochi.PhysX;

unsafe partial class Globals
{
    // This is a workaround for https://github.com/MochiLibraries/Biohazrd/issues/80
    // Ideally we'd be able to tell Biohazrd that PxDefaultSimulationFilter is special and should just be generated this way in the first place.
    private static class PxDefaultSimulationFilterShaderCache
    {
        private static readonly IntPtr LibraryHandle = NativeLibrary.Load(PxDefaultSimulationFilterShaderDllFileName);
        internal static readonly IntPtr Export = NativeLibrary.GetExport(LibraryHandle, PxDefaultSimulationFilterShaderMangledName);
    }

    public static delegate* unmanaged[Cdecl]<PxFilterFlags*, uint, PxFilterData*, uint, PxFilterData*, PxPairFlags*, void*, uint, PxFilterFlags*> PxDefaultSimulationFilter
        => (delegate* unmanaged[Cdecl]<PxFilterFlags*, uint, PxFilterData*, uint, PxFilterData*, PxPairFlags*, void*, uint, PxFilterFlags*>)PxDefaultSimulationFilterShaderCache.Export;
}
