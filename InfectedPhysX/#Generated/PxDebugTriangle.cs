// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 48)]
public unsafe partial struct PxDebugTriangle
{
    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxDebugTriangle@physx@@QEAA@AEBVPxVec3@1@00AEBI@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxDebugTriangle* @this, PxVec3* p0, PxVec3* p1, PxVec3* p2, uint* c);

    public unsafe void Constructor(PxVec3* p0, PxVec3* p1, PxVec3* p2, uint* c)
    {
        fixed (PxDebugTriangle* @this = &this)
        { Constructor_PInvoke(@this, p0, p1, p2, c); }
    }

    [FieldOffset(0)] public PxVec3 pos0;

    [FieldOffset(12)] public uint color0;

    [FieldOffset(16)] public PxVec3 pos1;

    [FieldOffset(28)] public uint color1;

    [FieldOffset(32)] public PxVec3 pos2;

    [FieldOffset(44)] public uint color2;
}