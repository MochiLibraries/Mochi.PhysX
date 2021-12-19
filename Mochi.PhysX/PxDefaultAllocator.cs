using System.Runtime.InteropServices;

namespace Mochi.PhysX;

unsafe partial struct PxDefaultAllocator
{
    // This is a workaround for https://github.com/MochiLibraries/Biohazrd/issues/31
    public PxDefaultAllocator()
    {
        fixed (PxDefaultAllocator* @this = &this)
        { PInvoke(@this); }

        [DllImport("Mochi.PhysX.Native", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__newPxDefaultAllocator", ExactSpelling = true)]
        static extern void PInvoke(PxDefaultAllocator* @this);
    }
}
